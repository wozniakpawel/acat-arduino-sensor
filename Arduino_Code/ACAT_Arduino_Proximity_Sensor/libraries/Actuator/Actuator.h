///////////////////////////////////////////////////////////////////////////
// <copyright file="Actuator.h" company="Intel Corporation">
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

#if ARDUINO >= 100
 #include "Arduino.h"
#else
 #include "WProgram.h"
#endif

#include <Wire.h>
#include "cppQueue.h"

/**
 * Triggering mode of the Arduino sensor
 * THRESHOLDING = Thresholding mode. Trigger when value passes certain threshold
 * PEAK_DETECTION_ALGO = "Peak detection" algorithm. Trigger when there is
 * movement in from of the sensor
 */
typedef enum
{
    THRESHOLDING, PEAK_DETECTION_ALGO
} TRIGGERING_MODE;

typedef enum {NO_GESTURE, POSSIBLE_GESTURE_START, IN_GESTURE
} APP_STATE;

class Actuator
{

public:

  /**
   * @brief      Constructs the Actuator object
   */
  Actuator();

  /**
   * @brief      Destroys the Actuator object
   */
  ~Actuator();

  /**
   * @brief      Processes proximity sensor values and sends actuate start / end messages
   *
   * @param[in]  timestamp  The timestamp of the proximity value
   * @param[in]  prox_val   The proximity value to process
   *
   */
  int processProxVal(uint64_t timestamp, uint16_t prox_val);

  /**
   * @brief      Insert new value into queue
   *
   * @param      queue  The queue to insert values into
   * @param[in]  val    The value to insert into queue
   *
   */
  int insert_into_queue(Queue *queue, float val);

  /**
   * @brief      Transform non-linear proximity value vs. distance relationship into linear one
   *
   * @param[in]  prox_val  The proximity value to transform
   *
   * @return     Returns transformed value
   */
  float transform_raw_prox_val(uint16_t prox_val);

  /**
   * @brief      Process the serial input to the Arduino sensor
   */
  void processSerialInput();

};


