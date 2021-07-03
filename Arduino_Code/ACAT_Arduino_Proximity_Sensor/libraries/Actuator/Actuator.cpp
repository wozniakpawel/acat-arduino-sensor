///////////////////////////////////////////////////////////////////////////
// <copyright file="Actuator.cpp" company="Intel Corporation">
//
// Copyright (c) 2019 Intel Corporation 
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
////////////////////////////////////////////////////////////////////////////

#include "Actuator.h"

Actuator::Actuator()
{

}

Actuator::~Actuator()
{

}

float PROX_TRASFORM_EQ_CONSTANT = 600;

TRIGGERING_MODE triggering_mode = THRESHOLDING; //Default mode
// TRIGGERING_MODE triggering_mode = PEAK_DETECTION_ALGO;
APP_STATE CURRENT_STATE = NO_GESTURE;

// THRESHOLDING mode variables
float CURRENT_THRESHOLD = 8.0;

// PEAK DETECTION ALGO mode variables
uint16_t PD_WINDOW_SIZE_SAMPLES = 25;
uint16_t PD_FD_WINDOW_SIZE = 13; //Length of queue holding first derivative values
float PD_PREV_SMOOTHED_VAL = -999999; //Used in peak detection algo. Init value
float PD_NEG_GESTURE_THRESHOLD = -0.5; //Peak detection negative gesture threshold
float PD_POS_GESTURE_THRESHOLD = 1.0; //Peak detection positive gesture threshold
uint16_t PD_GESTURE_TIMEOUT = 249; //Peak Detection gesture timeout (samples)
uint16_t PD_NUM_SAMPLES_IN_GESTURE = 0; //Variable keeping track of number of samples in gesture
float PD_GESTURE_MAXSTD = 100; //Peak detection gesture max standard deviation
float PD_MAX_STDDEV_IN_GESTURE = 10000; //Variable keeping track of max standard deviation in gesture
float PD_MIN_STD_GESTURE_START = 0.02; //Minimum standard deviation to trainistion from possible gesture start state to in gesture stat

int floatSize = sizeof(float);
#define IMPLEMENTATION  LIFO
Queue pd_rv_queue(floatSize, PD_WINDOW_SIZE_SAMPLES, IMPLEMENTATION, true); //Queue holding raw distance transformation values
Queue pd_fd_queue(floatSize, PD_FD_WINDOW_SIZE, IMPLEMENTATION, true); //Queue holding first derivative values
Queue pd_fea_queue(floatSize, PD_WINDOW_SIZE_SAMPLES, IMPLEMENTATION, true); //Queue holding values for feature computation

// Serial communication
const static byte serialInputBufferSize = 128;
char serialInputBuffer[serialInputBufferSize];
boolean newSerialData = false;
char msgStartMarker = '<';
char msgEndMarker = '>';
char newSettingMarker[] = "newSetting,";
char newTriggeringModePdMarker[] = "TriggeringModePeakDetectionAlgo,";
char newTriggeringModeThresholdingMarker[] = "TriggeringModeThresholding,";
char newPdGesStartThreshMarker[] = "PeakDetectionAlgoGestureStartThreshold,";
char newPdGesEndThreshMarker[] = "PeakDetectionAlgoGestureEndThreshold,";
char newThresholdingGesStartThreshMarker[] = "ThresholdingGestureStartThreshold,";


int Actuator::processProxVal(uint64_t timestamp, uint16_t prox_val)
{
    float distance_val = transform_raw_prox_val(prox_val);
    // Serial.print("Prox transform value: "); Serial.println(distance_val);


    // THRESHOLDING MODE
    if(triggering_mode == THRESHOLDING){

        if(distance_val < CURRENT_THRESHOLD){
            if(CURRENT_STATE == NO_GESTURE){
                Serial.println("ACTUATE_START");
                digitalWrite(LED_BUILTIN, HIGH); // LED ON
            }
            CURRENT_STATE = IN_GESTURE;
        }
        else{
            if(CURRENT_STATE == IN_GESTURE){
                Serial.println("ACTUATE_END");
                digitalWrite(LED_BUILTIN, LOW); // LED OFF
            }
            CURRENT_STATE = NO_GESTURE;
        }

        //Print data in format "P,timestamp,value,0"
        char s[50];
        sprintf(s, "P,%lu,",timestamp); 
        Serial.print(s);
        Serial.print(distance_val);
        Serial.print(",");
        Serial.println(0);
    }


    // PEAK DETECTION ALGORITHM MODE
    if(triggering_mode == PEAK_DETECTION_ALGO){

        insert_into_queue(&pd_rv_queue, distance_val);

        // For debugging
        //Print data in format "P,timestamp,value,pd_fea1"
        // char s[50];
        // sprintf(s, "P,%lu,",timestamp);
        // Serial.print(s);
        // Serial.print(distance_val);
        // Serial.print("\n");

        // Only process values when queue has PD_WINDOW_SIZE_SAMPLES elements
        if(pd_rv_queue.getCount() != PD_WINDOW_SIZE_SAMPLES){
            return -1;
        }

        float pd_rv_vals[PD_WINDOW_SIZE_SAMPLES];
        memset(pd_rv_vals, 0, (PD_WINDOW_SIZE_SAMPLES*sizeof(float)) );
        pd_rv_queue.peekAll(pd_rv_vals);
        // Serial.println("pd_rv_vals: ");
        // for(int n = 0; n < PD_WINDOW_SIZE_SAMPLES; n++){
        //     Serial.print(pd_rv_vals[n]);
        //     Serial.print(" ");
        // }
        // Serial.println("\n\n");


        //Get smoothed value
        //Calculate running average of samples in the window (last PD_WINDOW_SIZE_SAMPLES)
        float sum = 0;
        int16_t i = 0;
        while(i < PD_WINDOW_SIZE_SAMPLES)
        {
            sum += pd_rv_vals[i];
            i++;
        }
        float running_average = sum / ((float) PD_WINDOW_SIZE_SAMPLES);


        //For state machine later
        //Per smoothed sample, compute first derivative as FD(n) = SS(n) - SS(n-1) where SS(n) is the nth smoothed sensor value
        //Save the value
        float first_derivative = 0;
        if(PD_PREV_SMOOTHED_VAL != -999999)
        {
            //Not the first sample. Otherwise leave first_derivative as 0
            first_derivative = running_average - PD_PREV_SMOOTHED_VAL;
        }
        PD_PREV_SMOOTHED_VAL = running_average;
        insert_into_queue(&pd_fd_queue, first_derivative);


        int num_pd_fea_queue_vals = pd_fea_queue.getCount();
        if(num_pd_fea_queue_vals < PD_WINDOW_SIZE_SAMPLES){
            //Just insert into queue holding values for feature computation
            //Don't continue (wait until full queue)
            insert_into_queue(&pd_fea_queue, running_average);
            return -1;
        }

        //Get smoothed values for feature computation
        float smoothed_values[PD_WINDOW_SIZE_SAMPLES];
        memset(smoothed_values, 0, (PD_WINDOW_SIZE_SAMPLES*sizeof(float)) );
        pd_fea_queue.peekAll(smoothed_values);
                  
        
        //Compute peak detection feature from the smoothed data
        //sum of the differences of the current sample with all the samples in the window
        float pd_fea1 = 0; 
        i = 0;
        while(i < PD_WINDOW_SIZE_SAMPLES)
        {
            pd_fea1 += (running_average - smoothed_values[i]);
            i++;
        }
        pd_fea1 = -pd_fea1;
    
        //Compute standard deviation from the smoothed data
        i = 0;
        float mean = 0; 
        while(i < PD_WINDOW_SIZE_SAMPLES)
        {
            mean +=  smoothed_values[i];
            i++;
        }
        mean += running_average; //Include current sample in std dev computation
        mean = mean / ((float) (PD_WINDOW_SIZE_SAMPLES + 1));
        i = 0;
        float variance = 0;
        while(i < PD_WINDOW_SIZE_SAMPLES)
        {
            variance += pow( ( smoothed_values[i] - mean), 2 );
            i++;
        }
        variance += pow( ( running_average - mean), 2); //Include current sample in std dev computation
        variance = variance / ((float) (PD_WINDOW_SIZE_SAMPLES + 1));
        float std_dev = sqrt(variance);


        //Add smoothed value to queue
        insert_into_queue(&pd_fea_queue, running_average);


        //Print data in format "P,timestamp,value,pd_fea1"
        char s[50];
        sprintf(s, "P,%lu,",timestamp);
        Serial.print(s);
        Serial.print(distance_val);
        Serial.print(",");
        Serial.println(pd_fea1);


        ///////////////// STATE MACHINE /////////////////
        if(CURRENT_STATE == NO_GESTURE)
        {
            //If feature crosses positive threshold and all values in the first derivative 
            //queue are negative, go to possible gesture start state
        
            if(pd_fea1 > PD_POS_GESTURE_THRESHOLD) 
            {        
                int16_t num_fd_vals = pd_fd_queue.getCount();
                if(num_fd_vals == PD_FD_WINDOW_SIZE)
                {
                    //Check all first derivative values in queue are negative (distance to sensor decreasing)
                    float fd_values[PD_FD_WINDOW_SIZE]; 
                    memset(fd_values, 0, (PD_FD_WINDOW_SIZE*sizeof(float)));
                    pd_fd_queue.peekAll(fd_values);
                    
                    bool all_fd_vals_negative = true;
                    uint16_t m = 0;
                    while(m < PD_FD_WINDOW_SIZE)
                    {
                        if(fd_values[m] >= 0)
                            all_fd_vals_negative = false;
                        m++;
                    }
                        
                    if(all_fd_vals_negative == true)
                    {
                        //Go to possible gesture start state
                        //Reset counter keeping track of number of samples since gesture start
                        PD_NUM_SAMPLES_IN_GESTURE = 1;
                        //Reset variable keeping track of max standard deviation in gesture
                        PD_MAX_STDDEV_IN_GESTURE = std_dev;
                    
                        // Serial.println("POSSIBLE_GESTURE_START"); //FOR DEBUGGING
                        CURRENT_STATE = POSSIBLE_GESTURE_START;
                    
                        //If std dev is greater than PD_MIN_STD_GESTURE_START, go directly to in gesture state
                        if(std_dev > PD_MIN_STD_GESTURE_START)
                        {
                            //Real gesture. Go to in gesture state
                            Serial.println("ACTUATE_START");
                            digitalWrite(LED_BUILTIN, HIGH); // LED ON
                            CURRENT_STATE = IN_GESTURE;
                        }
                    }                            
                }
            }
        }
        else if(CURRENT_STATE == POSSIBLE_GESTURE_START)
        {
            //Increment counter keeping track of number of samples in gesture
            PD_NUM_SAMPLES_IN_GESTURE++;
            if(std_dev > PD_MIN_STD_GESTURE_START)
            {
                //Real gesture. Go to in gesture state
                Serial.println("ACTUATE_START");
                digitalWrite(LED_BUILTIN, HIGH); // LED ON
                CURRENT_STATE = IN_GESTURE;
            }
        
            if(std_dev >= PD_MAX_STDDEV_IN_GESTURE)
            {
                //Standard deviation should always be increasing in this state until hitting PD_MIN_STD_GESTURE_START
                //Otherwise exit and go back to no gesture state
                PD_MAX_STDDEV_IN_GESTURE = std_dev;
            }
            else
            {
                //Abandon gesture. Standard dev has decreased
                PD_MAX_STDDEV_IN_GESTURE = 0;
                PD_NUM_SAMPLES_IN_GESTURE = 0;
                // Serial.println("ABANDON_GESTURE"); //FOR DEBUGGING
                CURRENT_STATE = NO_GESTURE;
            }
        }
        else if(CURRENT_STATE == IN_GESTURE)
        {
            //Increment counter keeping track of number of samples in gesture and keep track of max std dev in gesture
            PD_NUM_SAMPLES_IN_GESTURE++;
            if(std_dev > PD_MAX_STDDEV_IN_GESTURE)
                PD_MAX_STDDEV_IN_GESTURE = std_dev;
        
            if(pd_fea1 <  PD_NEG_GESTURE_THRESHOLD)
            {   
                //Gesture over
                Serial.println("ACTUATE_END");
                digitalWrite(LED_BUILTIN, LOW); // LED OFF
                CURRENT_STATE = NO_GESTURE;
            }
        
            //Timeout has been reached or exceeded and the maximum std dev in the gesture is less than the defined maximum std dev of a typical gesture
            else if( (PD_NUM_SAMPLES_IN_GESTURE >= PD_GESTURE_TIMEOUT) && (PD_MAX_STDDEV_IN_GESTURE < PD_GESTURE_MAXSTD) )
            {
                Serial.println("ACTUATE_TIMEOUT");
                digitalWrite(LED_BUILTIN, LOW); // LED OFF
                CURRENT_STATE = NO_GESTURE;
            }
        
        }
    }
}

int Actuator::insert_into_queue(Queue *queue, float val){

    // Serial.print("queue.getCount(): "); Serial.println(queue->getCount());

    // Automatically overwrites previous records when queue is full
    queue->push(&val);
}

float Actuator::transform_raw_prox_val(uint16_t prox_val) 
{
    float a = -999;

    //Function generated by Vishay Semiconductors
    //Transform non-linear proximity value vs. distance relationship into linear one
    //Turns raw proximity value into some number that has more linear relationship with distance
    a = (1/sqrt((float)prox_val)) * PROX_TRASFORM_EQ_CONSTANT;

    return a;
}

void Actuator::processSerialInput()
{
    static boolean recvInProgress = false;
    static byte ndx = 0;

    char rc;
    char *msgIndex;
    char *msgIndexEnd;
    char newVal[20];
 
    while (Serial.available() > 0 && newSerialData == false) {
        rc = Serial.read();

        if (recvInProgress == true) {
            if (rc != msgEndMarker) {
                serialInputBuffer[ndx] = rc;
                ndx++;
                if (ndx >= serialInputBufferSize) {
                    ndx = serialInputBufferSize - 1;
                }
            }
            else {
                serialInputBuffer[ndx] = '\0'; // terminate the string
                recvInProgress = false;
                ndx = 0;
                newSerialData = true;
            }
        }

        else if (rc == msgStartMarker) {
            recvInProgress = true;
        }
    }

    if(newSerialData){

        char messageBuffer[serialInputBufferSize];
        strncpy(messageBuffer, serialInputBuffer, serialInputBufferSize);

        //Serial.print("Arduino echo - ");
        //Serial.println(serialInputBuffer);

        // Look for "newSetting," marker
        msgIndex = strstr(messageBuffer, newSettingMarker);
        if(msgIndex != NULL){

            // Serial.println("Received newSetting, string");

            // Look for message markers for each changeable parameter
            msgIndex = strstr(messageBuffer,newTriggeringModePdMarker);
            if(msgIndex != NULL){
                msgIndex = msgIndex + strlen(newTriggeringModePdMarker);
                int numChars = strlen(messageBuffer) - (strlen(newTriggeringModePdMarker)+
                    strlen(newSettingMarker));
                memset(newVal,0,sizeof(newVal));
                memcpy(newVal, msgIndex, numChars);
                // Serial.print("numChars: ");
                // Serial.println(numChars);

                // Currently Only 2 modes (PEAK_DETECTION_ALGO and THRESHOLDING)
                int triggeringModePd = atoi(newVal);
                if(triggeringModePd == 0){
                    triggering_mode = THRESHOLDING;
                    // Serial.println("THRESHOLDING");
                }
                if(triggeringModePd == 1){
                    triggering_mode = PEAK_DETECTION_ALGO;
                    // Serial.println("PEAK_DETECTION_ALGO");
                }

                newSerialData = false;
                return;
            }

            msgIndex = strstr(messageBuffer,newTriggeringModeThresholdingMarker);
            if(msgIndex != NULL){
                msgIndex = msgIndex + strlen(newTriggeringModeThresholdingMarker);
                int numChars = strlen(messageBuffer) - (strlen(newTriggeringModeThresholdingMarker)+
                    strlen(newSettingMarker));
                memset(newVal,0,sizeof(newVal));
                memcpy(newVal, msgIndex, numChars);

                // Currently Only 2 modes (PEAK_DETECTION_ALGO and THRESHOLDING)
                int triggeringModeThresh = atoi(newVal);
                if(triggeringModeThresh == 0){
                    triggering_mode = PEAK_DETECTION_ALGO;
                    // Serial.println("PEAK_DETECTION_ALGO");
                }
                if(triggeringModeThresh == 1){
                    triggering_mode = THRESHOLDING;
                    // Serial.println("THRESHOLDING");
                }

                newSerialData = false;
                return;
            }

            msgIndex = strstr(messageBuffer,newPdGesStartThreshMarker);
            if(msgIndex != NULL){
                msgIndex = msgIndex + strlen(newPdGesStartThreshMarker);
                int numChars = strlen(messageBuffer) - (strlen(newPdGesStartThreshMarker)+
                    strlen(newSettingMarker));
                memset(newVal,0,sizeof(newVal));
                memcpy(newVal, msgIndex, numChars);

                PD_POS_GESTURE_THRESHOLD = (float) atof(newVal);
                // Serial.println(PD_POS_GESTURE_THRESHOLD);

                newSerialData = false;
                return;
            }

            msgIndex = strstr(messageBuffer,newPdGesEndThreshMarker);
            if(msgIndex != NULL){
                msgIndex = msgIndex + strlen(newPdGesEndThreshMarker);
                int numChars = strlen(messageBuffer) - (strlen(newPdGesEndThreshMarker)+
                    strlen(newSettingMarker));
                memset(newVal,0,sizeof(newVal));
                memcpy(newVal, msgIndex, numChars);

                PD_NEG_GESTURE_THRESHOLD = (float) atof(newVal);
                // Serial.println(PD_NEG_GESTURE_THRESHOLD);

                newSerialData = false;
                return;
            }

            msgIndex = strstr(messageBuffer,newThresholdingGesStartThreshMarker);
            if(msgIndex != NULL){
                msgIndex = msgIndex + strlen(newThresholdingGesStartThreshMarker);
                int numChars = strlen(messageBuffer) - (strlen(newThresholdingGesStartThreshMarker)+
                    strlen(newSettingMarker));
                memset(newVal,0,sizeof(newVal));
                memcpy(newVal, msgIndex, numChars);

                CURRENT_THRESHOLD = (float) atof(newVal);
                // Serial.println(CURRENT_THRESHOLD);

                newSerialData = false;
                return;
            }

        }

        newSerialData = false;
    }
}