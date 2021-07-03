///////////////////////////////////////////////////////////////////////////
// <copyright file="SensorPositionTracker.cs" company="Intel Corporation">
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


using System;
using System.Collections.Generic;

namespace ACAT_Arduino_Windows_App
{
    class SensorPositionTracker
    {
        float proxSumBaseline;
        float proxSumSquareBaseline;
        List<float[]> proxSampleBufferBaseline = new List<float[]>();
        float curBaseline;
        int curBufferLenBaseline = 0;

        /// <summary>
        /// SensorPositionTracker constructor
        /// </summary>
        public SensorPositionTracker()
        {}

        /// <summary>
        /// Track the baseline / sensor position when there is no motion
        /// and give recommendation on sensor position
        /// </summary>
        /// <param name="proxValue">new proximity value</param>
        public void trackBaselineForSensorPositionFeedback(float proxValue)
        {
            float curStdevBaseline = getProxStdDev_Baseline(proxValue);
            float curMeanBaseline = getProxMean_Baseline();

            float recommendedPositionMin = Settings.RecommendedSensorPositionMin;
            float recommendedPositionmax = Settings.RecommendedSensorPositionMax;

            //--------Calibrate-----------
            bool isNoMotion = dg_calibrate(curMeanBaseline, curStdevBaseline);

            if (isNoMotion)
            {
                int sensorPositionStatus = -2; //not known / finding baseline
                //check for Baseline within recommended
                if ((curMeanBaseline >= recommendedPositionMin) && (curMeanBaseline <= recommendedPositionmax))
                {
                    sensorPositionStatus = 0; //good
                }
                else if (curMeanBaseline < recommendedPositionMin)
                {
                    sensorPositionStatus = 2;
                }
                else
                {
                    sensorPositionStatus = 1;
                }

                string suggestedThreshold = "";
                //if (curMeanBaseline <= 6.5F)
                //    suggestedThreshold = "Around 1.5 (Suggested)";
                //else if ((curMeanBaseline > 6.5F) && (curMeanBaseline <= 8.0F))
                //    suggestedThreshold = "Around 1.0 (Suggested)";
                //else
                //    suggestedThreshold = "Around 0.5 (Suggested)";

                Form1._mainForm.notifyFeedbackFormParamsBaseline(curMeanBaseline, sensorPositionStatus, suggestedThreshold);

                ResetBaselineBuffer(); //restart
            }
        }

        /// <summary>
        /// Function to get standard deviation of sensor position baseline
        /// </summary>
        /// <param name="prox">new proximity value</param>
        /// <returns></returns>
        public float getProxStdDev_Baseline(float prox)
        {
            float[] reading;
            float filterLenBaseline = Settings.SensorPostionDetectionWindowNumSamples;

            proxSumBaseline += prox;
            proxSumSquareBaseline += (prox * prox);

            curBufferLenBaseline++;
            if (curBufferLenBaseline < filterLenBaseline)
            {
                reading = new float[] { prox, 0, 0 };
                proxSampleBufferBaseline.Add(reading);
                return (0);
            }

            if (curBufferLenBaseline > filterLenBaseline)
            {
                float removeProx = proxSampleBufferBaseline[0][0];
                proxSumBaseline -= removeProx;
                proxSumSquareBaseline -= (removeProx * removeProx);
                proxSampleBufferBaseline.RemoveAt(0);
            }

            double avg = (double)(proxSumBaseline) / ((double)filterLenBaseline);
            double savg = ((double)proxSumSquareBaseline) / ((double)filterLenBaseline);
            double avgavg = avg * avg;
            float stddev = (float)Math.Sqrt(savg - avgavg);
            reading = new float[] { prox, stddev, (float)avg };
            proxSampleBufferBaseline.Add(reading);

            return stddev;
        }

        /// <summary>
        /// Function to get mean of sensor position baseline
        /// </summary>
        /// <returns></returns>
        public float getProxMean_Baseline()
        {
            float filterLenBaseline = Settings.SensorPostionDetectionWindowNumSamples;
            // shouldbe called only after getProxStdDev to get the latest value;
            if (curBufferLenBaseline < filterLenBaseline)
            {
                return (0);
            }
            float mean = (float)(proxSumBaseline / (double)filterLenBaseline);
            return mean;
        }

        /// <summary>
        /// Function to get whether there is motion or no motion in front of the sensor
        /// using mean and standard deviation
        /// </summary>
        /// <param name="curMean">current mean of sensor baseline</param>
        /// <param name="curStdev">current standard deviation of sensor baseline</param>
        /// <returns></returns>
        private bool dg_calibrate(float curMean, float curStdev)
        {
            bool isNoMotion = false;
            float filterLenBaseline = Settings.SensorPostionDetectionWindowNumSamples;
            float trackCalibrationMaxStdev = Settings.SensorPostionDetectionWindowMaxRestStdDev;

            if (curBufferLenBaseline < filterLenBaseline)
                return (false);

            //--------Calibrate-----------
            if (curStdev <= trackCalibrationMaxStdev) //nomotion in original domain
            {
                curBaseline = curMean;
                isNoMotion = true;
            }

            return (isNoMotion);
        }

        /// <summary>
        /// Reset sensor position tracking parameters
        /// </summary>
        private void ResetBaselineBuffer()
        {
            curBufferLenBaseline = 0;
            proxSumBaseline = 0;
            proxSumSquareBaseline = 0;
            proxSampleBufferBaseline.Clear();
        }
    }
}
