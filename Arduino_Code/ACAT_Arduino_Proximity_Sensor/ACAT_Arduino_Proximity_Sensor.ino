///////////////////////////////////////////////////////////////////////////
// <copyright file="ACAT_Arduino_Proximity_Sensor.ino" company="Intel Corporation">
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

#include <Wire.h>
#include <VCNL4010.h>
#include <Actuator.h>

VCNL4010 vcnl4010; // VCNL4010 proximity sensor
Actuator actuator; // ACAT actuator

void setup() {
  Serial.begin(9600);
  
  // DEFAULT
  // Init VCNL4010 sensor with 200mA LED current and 250Hz sampling rate
  if (vcnl4010.init(VCNL4010::VCNL4010_IR_LED_CUR_VAL_200MA, 
  VCNL4010::VCNL4010_PROX_RATE_VAL_250) == VCNL4010_ERROR){
    Serial.println("Error initializing VCNL4010 sensor");
    while (1);
  }
  Serial.println("Successfully initialized VCNL4010 sensor");

  pinMode(LED_BUILTIN, OUTPUT);
}

void loop() {
   uint16_t proxVal = 0;
   if(vcnl4010.acquireProxSample(&proxVal) == VCNL4010_NO_ERROR){
      // Serial.print("P,"); Serial.println(proxVal);

      // Process proximity value and actuate accordingly
      unsigned long timestamp = millis();
      actuator.processProxVal(timestamp,proxVal);
   }
   else{
    Serial.print("Error reading proximity sensor value");
   }

    // Process commands sent to Arduino over serial
     actuator.processSerialInput();
}
