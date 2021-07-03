///////////////////////////////////////////////////////////////////////////
// <copyright file="VCNL4010.h" company="Intel Corporation">
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

#define VCNL4010_NO_ERROR                   0
#define VCNL4010_ERROR                      -1
#define VCNL4010_I2C_ADDRESS                0x13
#define VCNL4010_REV_VAL                    0x21    // Product ID Revision Register 1 //Read only
#define VCNL4010_ALS_CONVERSION_TIME_MS     100     // standard mode (bit 7 of reg4 = 0)
#define VCNL4010_PROX_CONVERSION_TIME_MS    2       // if we have to use milliseconds, round up to 1
#define VCNL4010_MAX_LUX_COUNT              65536
#define VCNL4010_MIN_LUX_COUNT              1
#define VCNL4010_AMBIENTREADY               0x40
#define VCNL4010_PROXIMITYREADY             0x20

class VCNL4010
{

public:

  // Register definitions
  typedef enum {
    VCNL4010_CMD_REG_0 = 0x80,
    VCNL4010_REV_REG_1 = 0x81,
    VCNL4010_PROX_RATE_REG_2 = 0x82,
    VCNL4010_IR_LED_CUR_REG_3 = 0x83,
    VCNL4010_ALS_PARAM_REG_4 = 0x84,
    VCNL4010_ALS_DATA_REG_5 = 0x85,    //high byte
    VCNL4010_ALS_DATA_REG_6 = 0x86,    //low byte
    VCNL4010_PROX_DATA_REG_7 = 0x87,                //high byte
    VCNL4010_PROX_DATA_REG_8 = 0x88,                //low byte
    VCNL4010_INT_CTL_REG_9  = 0x89,
    VCNL4010_L_THRES_REG_10 = 0x8A,    //high byte    
    VCNL4010_L_THRES_REG_11 = 0x8B,    //low byte
    VCNL4010_H_THRES_REG_12 = 0x8C,                //high byte
    VCNL4010_H_THRES_REG_13 = 0x8D,                //low byte
    VCNL4010_INT_STAT_REG_14 = 0x8E,
    VCNL4010_PROX_MOD_REG_15 = 0x8F,
    VCNL4010_LEVEL_REG_16 = 0x90    //Mfgr use only
  } reg_t;

  // Command Register 0
  typedef enum {
    VCNL4010_CMD_LOCK_SHIFT = 0x7,     //Read only
    VCNL4010_CMD_LOCK_VAL = 0x1,
    VCNL4010_CMD_ALS_DATA_RDY_SHIFT = 0x6,     //Read only
    VCNL4010_CMD_ALS_DATA_RDY_VAL = 0x1,
    VCNL4010_CMD_PROX_DATA_RDY_SHIFT = 0x5,    //Read only
    VCNL4010_CMD_PROX_DATA_RDY_VAL = 0x1,
    VCNL4010_CMD_ALS_OD_SHIFT = 0x4,
    VCNL4010_CMD_ALS_OD_VAL = 0x1,
    VCNL4010_CMD_ALS_OD_DIS = 0x0,
    VCNL4010_CMD_PROX_OD_SHIFT = 0x3,
    VCNL4010_CMD_PROX_OD_VAL = 0x1,
    VCNL4010_CMD_PROX_OD_DIS = 0x0,
    VCNL4010_CMD_ALS_EN_SHIFT = 0x2,
    VCNL4010_CMD_ALS_EN_VAL = 0x1,
    VCNL4010_CMD_ALS_DIS = 0x0,
    VCNL4010_CMD_PROX_EN_SHIFT = 0x1,
    VCNL4010_CMD_PROX_EN_VAL = 0x1,
    VCNL4010_CMD_PROX_DIS  = 0x0,
    VCNL4010_CMD_SELFTIMED_EN_SHIFT = 0x0,
    VCNL4010_CMD_SELFTIMED_EN_VAL = 0x1,
    VCNL4010_CMD_SELFTIMED_DIS = 0x0
  } cmd_reg_0_t;

  //Proximity Rate Register 2
  typedef enum {
    VCNL4010_PROX_RATE_SHIFT = 0x0,
    VCNL4010_PROX_RATE_MASK = 0x7,
    VCNL4010_PROX_RATE_VAL_2 = 0x0,    //All rates listed as rounded up to nearest whole number unit measurements/s
    VCNL4010_PROX_RATE_VAL_4 = 0x1,
    VCNL4010_PROX_RATE_VAL_8 = 0x2,
    VCNL4010_PROX_RATE_VAL_16 = 0x3,
    VCNL4010_PROX_RATE_VAL_31 = 0x4,
    VCNL4010_PROX_RATE_VAL_63 = 0x5,
    VCNL4010_PROX_RATE_VAL_125 = 0x6,
    VCNL4010_PROX_RATE_VAL_250 = 0x7
  } prox_rate_reg_2_t;

  // IR LED Current Register 3
  typedef enum {
    VCNL4010_IR_LED_CUR_SHIFT = 0x0,
    VCNL4010_IR_LED_CUR_MASK = 0x3F,
    VCNL4010_IR_LED_CUR_VAL_0MA = 0x00,    //10mA stepping 
    VCNL4010_IR_LED_CUR_VAL_10MA = 0x01,
    VCNL4010_IR_LED_CUR_VAL_20MA = 0x02,
    VCNL4010_IR_LED_CUR_VAL_30MA = 0x03,
    VCNL4010_IR_LED_CUR_VAL_40MA = 0x04,
    VCNL4010_IR_LED_CUR_VAL_50MA = 0x05,
    VCNL4010_IR_LED_CUR_VAL_60MA = 0x06,
    VCNL4010_IR_LED_CUR_VAL_70MA = 0x07,
    VCNL4010_IR_LED_CUR_VAL_80MA = 0x08,
    VCNL4010_IR_LED_CUR_VAL_90MA = 0x09,
    VCNL4010_IR_LED_CUR_VAL_100MA = 0x0A,
    VCNL4010_IR_LED_CUR_VAL_110MA = 0x0B,
    VCNL4010_IR_LED_CUR_VAL_120MA = 0x0C,
    VCNL4010_IR_LED_CUR_VAL_130MA = 0x0D,
    VCNL4010_IR_LED_CUR_VAL_140MA = 0x0E,
    VCNL4010_IR_LED_CUR_VAL_150MA = 0x0F,
    VCNL4010_IR_LED_CUR_VAL_160MA = 0x10,
    VCNL4010_IR_LED_CUR_VAL_170MA = 0x11,
    VCNL4010_IR_LED_CUR_VAL_180MA = 0x12,
    VCNL4010_IR_LED_CUR_VAL_190MA = 0x13,
    VCNL4010_IR_LED_CUR_VAL_200MA = 0x14
    } ir_led_current_reg_3_t;

  // AmbientLightSensor Param Register 4
  typedef enum {
    VCNL4010_ALS_PARAM_CONT_CONV_SHIFT = 0x7,
    VCNL4010_ALS_PARAM_CONT_CONV_EN = 0x1,
    VCNL4010_ALS_PARAM_CONT_CONV_DIS = 0x0,
    VCNL4010_ALS_PARAM_RATE_SHIFT = 0x4,
    VCNL4010_ALS_PARAM_RATE_MASK = 0x7,
    VCNL4010_ALS_PARAM_RATE_1 = 0x0, //samples per second
    VCNL4010_ALS_PARAM_RATE_2 = 0x1,
    VCNL4010_ALS_PARAM_RATE_3 = 0x2,
    VCNL4010_ALS_PARAM_RATE_4 = 0x3,
    VCNL4010_ALS_PARAM_RATE_5 = 0x4,
    VCNL4010_ALS_PARAM_RATE_6 = 0x5,
    VCNL4010_ALS_PARAM_RATE_8 = 0x6,
    VCNL4010_ALS_PARAM_RATE_10 = 0x7,
    VCNL4010_ALS_PARAM_COMPENSATE_SHIFT = 0x3,
    VCNL4010_ALS_PARAM_COMPENSATE_EN = 0x1,
    VCNL4010_ALS_PARAM_COMPENSATE_DIS = 0x0,
    VCNL4010_ALS_PARAM_AVERAGE_SHIFT = 0x0,
    VCNL4010_ALS_PARAM_AVERAGE_MASK = 0x7,
    VCNL4010_ALS_PARAM_AVERAGE_1 = 0x0,     //number of single conversions done during one measurement cycle.
    VCNL4010_ALS_PARAM_AVERAGE_2 = 0x1,
    VCNL4010_ALS_PARAM_AVERAGE_4 = 0x2,
    VCNL4010_ALS_PARAM_AVERAGE_8  = 0x3,
    VCNL4010_ALS_PARAM_AVERAGE_16 = 0x4,
    VCNL4010_ALS_PARAM_AVERAGE_32 = 0x5,
    VCNL4010_ALS_PARAM_AVERAGE_64 = 0x6,
    VCNL4010_ALS_PARAM_AVERAGE_128 = 0x7
  } als_param_reg_4_t;

  // Interrupt Control Register 9
  typedef enum {
    VCNL4010_INT_CTL_COUNT_EXCEED_SHIFT = 0x5,
    VCNL4010_INT_CTL_COUNT_EXCEED_MASK  = 0x7,
    VCNL4010_INT_CTL_COUNT_EXCEED_1 = 0x0,     //number of consecutive measurements needed above/blow the threshold
    VCNL4010_INT_CTL_COUNT_EXCEED_2 = 0x1,
    VCNL4010_INT_CTL_COUNT_EXCEED_4 = 0x2,
    VCNL4010_INT_CTL_COUNT_EXCEED_8 = 0x3,
    VCNL4010_INT_CTL_COUNT_EXCEED_16 = 0x4,
    VCNL4010_INT_CTL_COUNT_EXCEED_32 = 0x5,
    VCNL4010_INT_CTL_COUNT_EXCEED_64 = 0x6,
    VCNL4010_INT_CTL_COUNT_EXCEED_128 = 0x7,
    VCNL4010_INT_CTL_PROX_RDY_SHIFT = 0x3,
    VCNL4010_INT_CTL_PROX_RDY_EN = 0x1,
    VCNL4010_INT_CTL_PROX_RDY_DIS = 0x0,
    VCNL4010_INT_CTL_ALS_RDY_SHIFT = 0x2,
    VCNL4010_INT_CTL_ALS_RDY_EN = 0x1,
    VCNL4010_INT_CTL_ALS_RDY_DIS = 0x0,
    VCNL4010_INT_CTL_THRES_SHIFT = 0x1,
    VCNL4010_INT_CTL_THRES_EN  = 0x1,
    VCNL4010_INT_CTL_THRES_DIS = 0x0,
    VCNL4010_INT_CTL_THRES_SEL_SHIFT = 0x0,
    VCNL4010_INT_CTL_THRES_SEL_ALS = 0x1,
    VCNL4010_INT_CTL_THRES_SEL_PROX = 0x0
  } int_control_reg_9_t;

  // Interrupt Status Register 14
  typedef enum {
    VCNL4010_INT_STAT_PROX_RDY_SHIFT = 0x3,
    VCNL4010_INT_STAT_PROX_RDY = 0x1,
    VCNL4010_INT_STAT_PROX_NOT_RDY = 0x0,
    VCNL4010_INT_STAT_ALS_RDY_SHIFT = 0x2,
    VCNL4010_INT_STAT_ALS_RDY = 0x1,
    VCNL4010_INT_STAT_ALS_NOT_RDY = 0x0,
    VCNL4010_INT_STAT_THRES_L_SHIFT = 0x1,
    VCNL4010_INT_STAT_THRES_L = 0x1,
    VCNL4010_INT_STAT_THRES_L_NOT = 0x0,
    VCNL4010_INT_STAT_THRES_H_SHIFT = 0x0,
    VCNL4010_INT_STAT_THRES_H = 0x1,
    VCNL4010_INT_STAT_THRES_H_NOT = 0x0
  } int_status_reg_14_t;

  // Proximity Modulator Timing Register 15 
  typedef enum {
    VCNL4010_PROX_MOD_DELAY_TIME_SHIFT = 0x5,
    VCNL4010_PROX_MOD_DELAY_TIME_MASK = 0x7,
    VCNL4010_PROX_MOD_DELAY_TIME_VAL = 0x0,
    VCNL4010_PROX_MOD_FREQ_SHIFT = 0x3,
    VCNL4010_PROX_MOD_FREQ_MASK = 0x3,
    VCNL4010_PROX_MOD_FREQ_VAL_391KHZ = 0x0,
    VCNL4010_PROX_MOD_FREQ_VAL_781KHZ = 0x1,
    VCNL4010_PROX_MOD_DEAD_TIME_SHIFT = 0x0,
    VCNL4010_PROX_MOD_DEAD_TIME_MASK = 0x7,
    VCNL4010_PROX_MOD_DEAD_TIME_VAL = 0x1
  } prox_mod_timing_reg_15_t;


  /**
   * @brief      Constructs the VCNL4010 object
   */
  VCNL4010();

  /**
   * @brief      Destroys the VCNL4010 object
   */
  ~VCNL4010();

  /**
   * @brief      Initialize VCNL4010 sensor
   *
   * @param[in]  VCNL4010_PROX_RATE_VAL   The VCNL4010 proximity rate value
   * @param[in]  VCNL4010_IR_LED_CUR_VAL  The VCNL4010 IR LED current value
   *
   * @return     Returns VCNL4010_NO_ERROR if successful. Returns VCNL4010_ERROR if unsuccessful
   */
  int init(int VCNL4010_PROX_RATE_VAL, int VCNL4010_IR_LED_CUR_VAL);

  /**
   * @brief      Read register of VCNL4010
   *
   * @param[in]  reg   The register to read
   * @param      val   Where result of read operation is stored
   *
   * @return     Returns VCNL4010_NO_ERROR if successful. Returns VCNL4010_ERROR if unsuccessful
   */
  int readReg(reg_t reg, uint8_t *val);

  /**
   * @brief      Write to a register of VCNL4010
   *
   * @param[in]  reg   The register to write to
   * @param[in]  val   The value to write
   *
   * @return     Returns VCNL4010_NO_ERROR if successful. Returns VCNL4010_ERROR if unsuccessful
   */
  int writeReg(reg_t reg, char val);

  /**
   * @brief      Acquire proximity sensor reading from VCNL4010
   *
   * @param      sample  Where result of acquisition is stored
   *
   * @return     Returns VCNL4010_NO_ERROR if successful. Returns VCNL4010_ERROR if unsuccessful
   */
  int acquireProxSample(uint16_t *sample);
};


