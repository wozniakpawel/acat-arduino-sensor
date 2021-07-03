///////////////////////////////////////////////////////////////////////////
// <copyright file="VCNL4010.cpp" company="Intel Corporation">
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

#include "VCNL4010.h"

VCNL4010::VCNL4010()
{

}

VCNL4010::~VCNL4010()
{

}

//TODO: Expose other sensor initialization parameters?
int VCNL4010::init(int VCNL4010_PROX_RATE_VAL, int VCNL4010_IR_LED_CUR_VAL)
{
    Wire.begin();

    Serial.println("VCNL4010 init");
    uint8_t rev = 0x00;
    readReg(VCNL4010_REV_REG_1, &rev);
    // rev = read8(VCNL4010_REV_REG_1);
    Serial.print("Product ID Rev: "); Serial.println(rev);
    if(rev != VCNL4010_REV_VAL)
        return VCNL4010_ERROR;

    //Disable everything in command REG #0
    if(writeReg(VCNL4010_CMD_REG_0, 0) == VCNL4010_ERROR)
        return VCNL4010_ERROR;

    //Write proximity measurement rate to REG #2
    if(writeReg(VCNL4010_PROX_RATE_REG_2, (VCNL4010_PROX_RATE_VAL & VCNL4010_PROX_RATE_MASK) << VCNL4010_PROX_RATE_SHIFT)== VCNL4010_ERROR)
        return VCNL4010_ERROR;

    //Write IR LED current value to REG #3
    if(writeReg(VCNL4010_IR_LED_CUR_REG_3, (VCNL4010_IR_LED_CUR_VAL & VCNL4010_IR_LED_CUR_MASK) << VCNL4010_IR_LED_CUR_SHIFT) == VCNL4010_ERROR)
        return VCNL4010_ERROR;

    //Write continuous conversion mode, ambient light measurement rate, auto offset compensation, 
    //averaging function values to ambient light parameter REG #4 
    //(Hardcoded values = Default values from data sheet with exception of ambient light measurement rate) 
    uint8_t param_val = 0x00;
    param_val = (VCNL4010_ALS_PARAM_CONT_CONV_DIS & 1) << VCNL4010_ALS_PARAM_CONT_CONV_SHIFT;
    param_val |= (VCNL4010_ALS_PARAM_RATE_10 & VCNL4010_ALS_PARAM_RATE_MASK) << VCNL4010_ALS_PARAM_RATE_SHIFT;
    param_val |= (VCNL4010_ALS_PARAM_COMPENSATE_EN & 1) << VCNL4010_ALS_PARAM_COMPENSATE_SHIFT;
    param_val |= (VCNL4010_ALS_PARAM_AVERAGE_32 & VCNL4010_ALS_PARAM_AVERAGE_MASK) << VCNL4010_ALS_PARAM_AVERAGE_SHIFT;
    if(writeReg(VCNL4010_ALS_PARAM_REG_4, param_val) == VCNL4010_ERROR)
        return VCNL4010_ERROR;

    //Write values to interrupt control REG #9. Currently, no interrupts generated when data is ready
    uint8_t int_val = 0x00;
    int_val = (VCNL4010_INT_CTL_COUNT_EXCEED_1 & VCNL4010_INT_CTL_COUNT_EXCEED_MASK) << VCNL4010_INT_CTL_COUNT_EXCEED_SHIFT;
    int_val |= (VCNL4010_INT_CTL_PROX_RDY_DIS & 1) << VCNL4010_INT_CTL_PROX_RDY_SHIFT;
    int_val |= (VCNL4010_INT_CTL_ALS_RDY_DIS & 1) << VCNL4010_INT_CTL_ALS_RDY_SHIFT;
    int_val |= (VCNL4010_INT_CTL_THRES_DIS & 1) << VCNL4010_INT_CTL_THRES_SHIFT;   
    int_val |= (VCNL4010_INT_CTL_THRES_SEL_PROX & 1) << VCNL4010_INT_CTL_THRES_SEL_SHIFT;
    if(writeReg(VCNL4010_INT_CTL_REG_9, param_val) == VCNL4010_ERROR)
        return VCNL4010_ERROR;

    //Clear interrupt status REG #14
    uint8_t int_clear_val = 0x00;
    int_clear_val = (VCNL4010_INT_STAT_PROX_RDY & 1) << VCNL4010_INT_STAT_PROX_RDY_SHIFT;
    int_clear_val |= (VCNL4010_INT_STAT_ALS_RDY & 1) << VCNL4010_INT_STAT_ALS_RDY_SHIFT;
    int_clear_val |= (VCNL4010_INT_STAT_THRES_L & 1) << VCNL4010_INT_STAT_THRES_L_SHIFT;
    int_clear_val |= (VCNL4010_INT_STAT_THRES_H & 1) << VCNL4010_INT_STAT_THRES_H_SHIFT;
    if(writeReg(VCNL4010_INT_STAT_REG_14, int_clear_val) == VCNL4010_ERROR)
        return VCNL4010_ERROR;

    //Write modulation delay time, proximity frequency, and modulation dead time to proximity modulator timing
    //adjustment REG #15
    //(Hardcoded values = Default values from data sheet)
    uint8_t prox_mod_val = 0x00;
    prox_mod_val = (VCNL4010_PROX_MOD_DELAY_TIME_VAL & VCNL4010_PROX_MOD_DELAY_TIME_MASK) << VCNL4010_PROX_MOD_DELAY_TIME_SHIFT;
    prox_mod_val |= (VCNL4010_PROX_MOD_FREQ_VAL_391KHZ & VCNL4010_PROX_MOD_FREQ_MASK) << VCNL4010_PROX_MOD_FREQ_SHIFT;
    prox_mod_val |= (VCNL4010_PROX_MOD_DEAD_TIME_VAL & VCNL4010_PROX_MOD_DEAD_TIME_MASK) << VCNL4010_PROX_MOD_DEAD_TIME_SHIFT;
    if(writeReg(VCNL4010_PROX_MOD_REG_15, prox_mod_val) == VCNL4010_ERROR)
        return VCNL4010_ERROR;

    //Enable on-demand measurement for ambient light and proximity in command REG #0
    //Disable periodic and self timed measurement
    uint8_t command_val = 0x00;
    command_val = (VCNL4010_CMD_ALS_OD_VAL & 1) << VCNL4010_CMD_ALS_OD_SHIFT;       //enable on-demand LIGHT
    command_val |= (VCNL4010_CMD_PROX_OD_VAL & 1) << VCNL4010_CMD_PROX_OD_SHIFT;    //enable on-demand PROXIMITY
    command_val |= (VCNL4010_CMD_ALS_DIS & 1) << VCNL4010_CMD_ALS_EN_SHIFT;
    command_val |= (VCNL4010_CMD_PROX_DIS & 1) << VCNL4010_CMD_PROX_EN_SHIFT;   
    command_val |= (VCNL4010_CMD_SELFTIMED_DIS & 1) << VCNL4010_CMD_SELFTIMED_EN_SHIFT;
    if(writeReg(VCNL4010_CMD_REG_0, command_val) == VCNL4010_ERROR)
        return VCNL4010_ERROR;

    return VCNL4010_NO_ERROR;
}

int VCNL4010::readReg(reg_t reg, uint8_t *val)
{
    uint8_t data;

    Wire.beginTransmission(VCNL4010_I2C_ADDRESS);
#if ARDUINO >= 100
     Wire.write((uint8_t)reg);
#else
     Wire.send((uint8_t)reg);
#endif
    Wire.endTransmission();

    delayMicroseconds(170);  // delay required

    Wire.requestFrom((uint8_t)VCNL4010_I2C_ADDRESS, (uint8_t)1);

#if ARDUINO >= 100
    *val = Wire.read();
#else
    *val = Wire.receive();
#endif

    return VCNL4010_NO_ERROR;
}

int VCNL4010::writeReg(reg_t reg, char val)
{
    Wire.beginTransmission(VCNL4010_I2C_ADDRESS);
#if ARDUINO >= 100
    Wire.write(reg);
    if(Wire.write(val) != sizeof(val))
        return VCNL4010_ERROR;
#else
    Wire.send(address);
    if(Wire.write(val) != sizeof(val))
        return VCNL4010_ERROR; 
#endif
    if(Wire.endTransmission() != 0)
        return VCNL4010_ERROR;

    return VCNL4010_NO_ERROR;
}

int VCNL4010::acquireProxSample(uint16_t *sample)
{
    //Enable on-demand measurement for ambient light and proximity in command REG #0
    //Disable periodic and self timed measurement
    uint8_t command_val = 0x00;
    command_val = (VCNL4010_CMD_ALS_OD_VAL & 1) << VCNL4010_CMD_ALS_OD_SHIFT;       //enable on-demand LIGHT
    command_val |= (VCNL4010_CMD_PROX_OD_VAL & 1) << VCNL4010_CMD_PROX_OD_SHIFT;    //only enable on-demand PROXIMITY
    command_val |= (VCNL4010_CMD_ALS_DIS & 1) << VCNL4010_CMD_ALS_EN_SHIFT;
    command_val |= (VCNL4010_CMD_PROX_DIS & 1) << VCNL4010_CMD_PROX_EN_SHIFT;   
    command_val |= (VCNL4010_CMD_SELFTIMED_DIS & 1) << VCNL4010_CMD_SELFTIMED_EN_SHIFT;
    writeReg(VCNL4010_CMD_REG_0, command_val);

    while(1){
        uint8_t res = 0x00;
        readReg(VCNL4010_CMD_REG_0, &res);
        if(res & VCNL4010_PROXIMITYREADY){
            //Read Proximity Result Registers #7 (high byte) and #8 (low byte)
            uint8_t highByte[1];
            uint8_t lowByte[1];
            if (readReg(VCNL4010_PROX_DATA_REG_7, highByte) == VCNL4010_ERROR) {
                return VCNL4010_ERROR;
            }
            if (readReg(VCNL4010_PROX_DATA_REG_8, lowByte) == VCNL4010_ERROR) {
                return VCNL4010_ERROR;
            }
            *sample = (highByte[0] << 8) | lowByte[0];
            return VCNL4010_NO_ERROR;
        }

        delay(1);
    }

    return VCNL4010_ERROR;
}
