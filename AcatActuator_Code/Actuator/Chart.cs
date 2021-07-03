///////////////////////////////////////////////////////////////////////////
// <copyright file="Chart.cs" company="Intel Corporation">
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;

namespace ACAT_Arduino_Windows_App
{
    public partial class Form1 : Form
    {
        public Thread chartThread;
        public delegate void chartAddDelegate();
        public chartAddDelegate chartAddDel;
        int chartMaxPoints = 350;
        volatile bool stopChartThread = false;

        public ConcurrentQueue<string> chartQueue = new ConcurrentQueue<string>();
        int feaOffset = 2;
        ArrayList feaDisplayIndices = new ArrayList();

        Regex validChartDataRegex = new Regex(@"P,\d+,\d+"); //Data format = "P,timestamp,distance value,feature value,gesture detected"

        /// <summary>
        /// Chart constructor
        /// </summary>
        public void LoadChart()
        {
            if ((chartThread == null)
                     || ((chartThread != null) && (!chartThread.IsAlive)))
            {
                stopChartThread = false;
                chartThread = new Thread(new ThreadStart(ChartThreadFunction));
                chartAddDel += new chartAddDelegate(ChartAdd);
            }
        }

        /// <summary>
        /// Initialize chart
        /// </summary>
        public void InitChart()
        {
            chartSensor.ChartAreas[0].AxisX.Minimum = 1;
            chartSensor.ChartAreas[0].AxisX.Maximum = chartMaxPoints;
            chartSensor.ChartAreas[0].AxisX.IsStartedFromZero = false;
            chartSensor.ChartAreas[0].AxisY.IsStartedFromZero = false;

            chartSensor.Series.Clear();
            feaDisplayIndices.Clear();
            chartSensor.Titles.Clear();

            // Add the title to top graph
            if(Settings.TriggeringModeThresholding == true)
                chartSensor.Titles.Add("Sensor Data");
            else if(Settings.TriggeringModePeakDetectionAlgo == true)
                chartSensor.Titles.Add("Peak Detection Feature");

            chartSensor.Titles[0].DockedToChartArea = chartSensor.ChartAreas[0].Name;
            chartSensor.Titles[0].IsDockedInsideChartArea = false;

            // show proximity value
            int seriesIdx = -1;
            {
                seriesIdx = 0; //0
                feaDisplayIndices.Add(0);
                Series proxSeries = new Series("Proximity");
                proxSeries.ChartType = SeriesChartType.Line;
                proxSeries.XValueType = ChartValueType.Int32;
                proxSeries.BorderWidth = 1;
                proxSeries.Color = Color.Blue;
                proxSeries.ShadowOffset = 1;
                chartSensor.Series.Add(proxSeries);
                chartSensor.Series[seriesIdx].ChartArea = chartSensor.ChartAreas[0].Name;
            }

            // show peak feature
            {
                seriesIdx = 1; //1
                feaDisplayIndices.Add(1);
                Series peakFeaSeries = new Series("PeakFeaVal");
                peakFeaSeries.ChartType = SeriesChartType.Line;
                peakFeaSeries.XValueType = ChartValueType.Int32;
                peakFeaSeries.BorderWidth = 1;
                peakFeaSeries.Color = Color.Red;
                peakFeaSeries.ShadowOffset = 1;
                chartSensor.Series.Add(peakFeaSeries);
                chartSensor.Series[seriesIdx].ChartArea = chartSensor.ChartAreas[0].Name;
            }

            // show gesture detected
            {
                seriesIdx = 2; //2
                feaDisplayIndices.Add(2);
                Series gestureDetectionSeries = new Series("GestureDetection");
                gestureDetectionSeries.ChartType = SeriesChartType.Bubble;
                gestureDetectionSeries.MarkerStyle = MarkerStyle.Circle;
                gestureDetectionSeries.MarkerColor = Color.Yellow;
                gestureDetectionSeries.MarkerSize = 1;
                gestureDetectionSeries.XValueType = ChartValueType.Int32;
                gestureDetectionSeries.BorderWidth = 1;
                gestureDetectionSeries.ShadowOffset = 1;
                chartSensor.Series.Add(gestureDetectionSeries);
                chartSensor.Series[seriesIdx].ChartArea = chartSensor.ChartAreas[0].Name;

                chartSensor.DataManipulator.IsEmptyPointIgnored = true;
                chartSensor.Series[seriesIdx].EmptyPointStyle.Color = Color.Transparent;
                chartSensor.Series[seriesIdx].BackGradientStyle = GradientStyle.TopBottom;
            }

            // show threshold 1
            // Threshold for thresholding mode. Positive gesture threshold for peak detection mode
            {
                seriesIdx=3; //3
                feaDisplayIndices.Add(3); //idx=3
                Series threshold1Series = new Series("Threshold 1");
                threshold1Series.ChartType = SeriesChartType.Line;
                threshold1Series.BorderDashStyle = ChartDashStyle.Dash;
                threshold1Series.XValueType = ChartValueType.Int32;
                threshold1Series.BorderWidth = 4;
                threshold1Series.Color = Color.DarkSeaGreen;
                threshold1Series.ShadowOffset = 1;
                chartSensor.Series.Add(threshold1Series);
                chartSensor.Series[seriesIdx].ChartArea = chartSensor.ChartAreas[0].Name;
            }

            if (chartThread.IsAlive == false)
            {
                chartThread.Start();
            }
        }

        /// <summary>
        /// Chart data processing function
        /// </summary>
        private void ChartThreadFunction()
        {
            try
            {
                while (!stopChartThread)
                {
                    try
                    {
                        if (chartSensor.IsDisposed == true)
                        {
                            System.Diagnostics.Trace.WriteLine("chartSensor already disposed");
                            stopChartThread = true;
                            break;
                        }
                        chartSensor.Invoke(chartAddDel);
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("ChartThreadFunction() exception: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("ChartThreadFunction() exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Grab data from chart queue and parse it
        /// </summary>
        public void ChartAdd()
        {
            string strChartData;
            while (AcatActuator.serialport.chartQueue.TryDequeue(out strChartData))
            {
                if (!String.IsNullOrEmpty(strChartData))
                {
                    //System.Diagnostics.Trace.WriteLine("chartQueue parse data: " + strChartData);

                    // Check data is in correct format. Can be messed up right after COM opened / closed
                    Match m = validChartDataRegex.Match(strChartData);
                    if (!m.Success)
                    {
                        System.Diagnostics.Trace.WriteLine("validChartDataRegex.Match == false");
                        continue;
                    }

                    // Parse data to add to chart
                    char[] delimiters2 = new char[] { '\n' };
                    string[] samples = strChartData.Split(delimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string onesample in samples)
                    {
                        char[] delimiters = new char[] { ',', ' ' };
                        string[] features = onesample.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        ArrayList feaList = new ArrayList();
                        int i = 0;
                        foreach(string feature in features)
                        {
                            if (feaOffset + i < features.Length)
                            {
                                feaList.Add(features[feaOffset + i]);
                            }
                            i++;
                        }

                        ChartAddPoints(feaList);
                    }
                }
            }
        }

        /// <summary>
        /// Add data to chart
        /// </summary>
        /// <param name="feaList">data (features) to add to chart</param>
        public void ChartAddPoints(ArrayList feaList)
        {
            // 0 = Proximity Value
            // 1 = Peak algo feature
            // 2 = Gesture Start / End
            // 3 = Gesture start threshold

            double proxVal = Convert.ToDouble(feaList[0]);
            double feaVal = Convert.ToDouble(feaList[1]);

            {
                int i = 0; //proximity
                //Only show in Thresholding mode
                if(Settings.TriggeringModeThresholding == true)
                {
                    if (chartSensor.Series[i].Points.Count() >= chartMaxPoints)
                    {
                        chartSensor.Series[i].Points.RemoveAt(0);
                    }
                    chartSensor.Series[i].Points.AddY(proxVal);
                }
            }

            {
                int i = 1; //peak feature
                //Only show in Peak Detection Mode
                if (Settings.TriggeringModePeakDetectionAlgo == true)
                {
                    if (chartSensor.Series[i].Points.Count() >= chartMaxPoints)
                    {
                        chartSensor.Series[i].Points.RemoveAt(0);
                    }
                    chartSensor.Series[i].Points.AddY(feaVal);
                }
            }

            {
                int i = 2; //gesture start/end

                int reading = -999;
                if (feaList.Count > 2 && feaList[i].ToString().All(char.IsDigit))
                    reading = Convert.ToInt32(feaList[i]);

                //System.Diagnostics.Trace.WriteLine("reading = "+reading);

                if (chartSensor.Series[i].Points.Count() >= chartMaxPoints)
                {
                    chartSensor.Series[i].Points.RemoveAt(0);
                }
                if (reading == 1)
                {
                    //gesture start
                    if (Settings.TriggeringModeThresholding == true)
                        chartSensor.Series[i].Points.AddY(proxVal);
                    if (Settings.TriggeringModePeakDetectionAlgo == true)
                        chartSensor.Series[i].Points.AddY(feaVal);
                    chartSensor.Series[i].Points[chartSensor.Series[i].Points.Count - 1].MarkerColor = Color.Green;
                }
                else if ((reading == 2))
                {
                    //gesture end
                    if (Settings.TriggeringModeThresholding == true)
                        chartSensor.Series[i].Points.AddY(proxVal);
                    else if (Settings.TriggeringModePeakDetectionAlgo == true)
                        chartSensor.Series[i].Points.AddY(feaVal);
                    chartSensor.Series[i].Points[chartSensor.Series[i].Points.Count - 1].MarkerColor = Color.Black;
                }
                else if ((reading == 3)) //gesture en due to timeout
                {
                    //gesture end
                    if (Settings.TriggeringModeThresholding == true)
                        chartSensor.Series[i].Points.AddY(proxVal);
                    else if (Settings.TriggeringModePeakDetectionAlgo == true)
                        chartSensor.Series[i].Points.AddY(feaVal);
                    chartSensor.Series[i].Points[chartSensor.Series[i].Points.Count - 1].MarkerColor = Color.BlueViolet;
                }
                else
                {
                    if (Settings.TriggeringModeThresholding == true)
                        chartSensor.Series[i].Points.AddY(proxVal);
                    else if (Settings.TriggeringModePeakDetectionAlgo == true)
                        chartSensor.Series[i].Points.AddY(feaVal);
                    chartSensor.Series[i].Points[chartSensor.Series[i].Points.Count - 1].IsEmpty = true;
                    chartSensor.Series[i].Points[chartSensor.Series[i].Points.Count - 1].MarkerColor = Color.Transparent;
                    chartSensor.Series[i].Points[chartSensor.Series[i].Points.Count - 1].MarkerSize = 0;
                }
            }

            {
                int i = 3; // gesture start thresholds

                if(Settings.TriggeringModeThresholding == true)
                {
                    // Add thresholding gesture start threshold
                    if (chartSensor.Series[i].Points.Count() >= chartMaxPoints)
                    {
                        chartSensor.Series[i].Points.RemoveAt(0);
                    }
                    chartSensor.Series[i].Points.AddY(Settings.ThresholdingGestureStartThreshold);
                }
                else if (Settings.TriggeringModePeakDetectionAlgo == true)
                {
                    // Add peak detection gesture start positive threshold
                    if (chartSensor.Series[i].Points.Count() >= chartMaxPoints)
                    {
                        chartSensor.Series[i].Points.RemoveAt(0);
                    }
                    chartSensor.Series[i].Points.AddY(Settings.PeakDetectionAlgoGestureStartThreshold);
                }
            }

            chartSensor.ChartAreas[0].RecalculateAxesScale();
            chartSensor.Invalidate();
        } 
    }
}
