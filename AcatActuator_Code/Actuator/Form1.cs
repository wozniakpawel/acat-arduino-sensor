///////////////////////////////////////////////////////////////////////////
// <copyright file="Form1.cs" company="Intel Corporation">
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
using System.Windows.Forms;

namespace ACAT_Arduino_Windows_App
{
    public partial class Form1 : Form
    {
        string version = "v1.00";

        public static Form1 _mainForm;
        public bool _isClosing = false;

        public FormFeedback _formFeedback;

        public delegate void FeedbackThresholdChangedEventDelegate(object sender, string thresholdValue);
        public event FeedbackThresholdChangedEventDelegate EvtFeedbackThresholdChanged;

        public delegate void ProximityFirmwareFeedbackFormParamsBaselineEventDelegate(object sender, float baselineValue, int sensorPositionStatus, string lower, string upper, string suggestedThreshold);
        public event ProximityFirmwareFeedbackFormParamsBaselineEventDelegate EvtProximityFirmwareFeedbackFormParamsBaseline;

        public delegate void ProximityFirmwareFeedbackFormGestureDetectedEventDelegate(object sender, int gestureDetected);
        public event ProximityFirmwareFeedbackFormGestureDetectedEventDelegate EvtProximityFirmwareFeedbackFormGestureDetected;


        /// <summary>
        /// Main form constructor. Load settings from XML file and instantiate feedback form
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            this.Text += " " + version;

            AcatActuator.serialport.EvtSerialPortOpened += serialport_EvtSerialPortOpened;
            AcatActuator.serialport.EvtSerialPortClosed += serialport_EvtSerialPortClosed;
            AcatActuator.serialport.EvtSerialPortStatusMessage += serialport_EvtSerialPortStatusMessage;

            // Load settings from XML file and populate form
            load_settings_populate_form(false);

            _formFeedback = new FormFeedback();
            _formFeedback.setForm1Handles(this);
            _formFeedback.Show();
            _formFeedback.ShowInTaskbar = false;

            _mainForm = this;
        }


        /// <summary>
        /// Populate main form and feedback form with settings
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Form1_Load");

            groupBoxCOMPort.Enabled = true;

            // Only change Set button color to red if user changed parameter
            buttonArduinoSet.BackColor = DefaultBackColor;

            //Disable graphing by default
            AcatActuator.serialport.isChartData = false;
            //richTextBoxMessage.AppendText("Hiding Chart\n");
            buttonChartStart.Text = "Start Graph";
            textBoxMaxChartPoints.Enabled = true;
            buttonChartStart.BackColor = DefaultBackColor;
            if (chartThread != null && chartThread.IsAlive == true)
            {
                stopChartThread = true;
            }

            labelComPort.Text = "";
            labelStatusMessage.Text = "";

            // Fill feedback window with correct values
            if (radioButtonPeakDetectionAlgo.Checked == true)
                notifyFeedbackThresholdChanged(textBoxPeakAlgoGestureStartThreshold.Text);
            else if (radioButtonThresholding.Checked == true)
                notifyFeedbackThresholdChanged(textBoxThresholdingThreshold.Text);
            notifyFeedbackFormParamsBaseline(0, -1, null);

            // Intitalize serialport autoconnect threads
            AcatActuator.serialport.Init();

        }

        /// <summary>
        /// Callback when main form is closed. Also close feedback form,
        /// any remaining threads, and serialport
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _isClosing = true;
                System.Diagnostics.Trace.WriteLine("Form1_FormClosing");

                //Close feedback form
                if (_formFeedback != null)
                {
                    _formFeedback.Close();
                    _formFeedback = null;
                }

                if ((chartThread != null) && (chartThread.IsAlive == true))
                {
                    stopChartThread = true;
                    //chartThread.Join();
                }

                AcatActuator.serialport.Close();

                //remove events
                EvtProximityFirmwareFeedbackFormParamsBaseline = null;
                EvtFeedbackThresholdChanged = null;
                EvtProximityFirmwareFeedbackFormGestureDetected = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Form1_FormClosing: exception" + ex.Message);
            }
        }


        /// <summary>
        /// Load settings and populate form
        /// </summary>
        /// <param name="loadDefaultSettings">Load default settings or not</param>
        private void load_settings_populate_form(bool loadDefaultSettings)
        {
            Settings AppSettings = new Settings();
            AppSettings.Load(loadDefaultSettings);

            // Triggering Mode
            if (Settings.TriggeringModePeakDetectionAlgo)
            {
                radioButtonPeakDetectionAlgo.Checked = true;
            }
            if (Settings.TriggeringModeThresholding)
            {
                radioButtonThresholding.Checked = true;
            }

            // Peak Detection Algo
            textBoxPeakAlgoGestureStartThreshold.Text = string.Format("{0:N2}", (Settings.PeakDetectionAlgoGestureStartThreshold));
            textBoxPeakAlgoGestureEndThreshold.Text = string.Format("{0:N2}", (Settings.PeakDetectionAlgoGestureEndThreshold));

            // Thresholding
            textBoxThresholdingThreshold.Text = string.Format("{0:N2}", (Settings.ThresholdingGestureStartThreshold));

            // Actuate ACAT
            if (Settings.ActuateACAT)
            {
                checkBoxActuateACAT.Checked = true;
            }

            // Feedback Window Recommended Sensor Postion
            textBoxRecommendedPositionMin.Text = string.Format("{0:N2}", (Settings.RecommendedSensorPositionMin));
            textBoxRecommendedPositionMax.Text = string.Format("{0:N2}", (Settings.RecommendedSensorPositionMax));
        }


        /// <summary>
        /// When serial port opened. Change UI components accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        void serialport_EvtSerialPortOpened(object sender, string msg)
        {
            try
            {
                this.Invoke(new Action(() => {
                    labelComPort.Text = msg;
                    labelComPort.BackColor = Color.Green;

                    string time = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    richTextBoxMessage.AppendText("\n" + time + "    COM port connected\n");

                    labelStatusMessage.Text = "";

                    notifyFeedbackFormParamsBaseline(0, -2, null);
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("serialport_EvtSerialPortOpened exception:" + ex.Message);
            }
        }

        /// <summary>
        /// When serial port closed. Change UI components accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        void serialport_EvtSerialPortClosed(object sender, string msg)
        {
            try
            {
                this.Invoke(new Action(() => {
                    labelComPort.Text = "None";
                    labelComPort.BackColor = Color.Red;

                    string time = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    richTextBoxMessage.AppendText("\n" + time + "    COM port not connected. Trying to automatically connect. Attempt # " + msg + "\n");

                    labelStatusMessage.Text = "COM port not connected\nTrying to find sensor\nAttempt # " + msg;
                    labelStatusMessage.ForeColor = Color.Red;

                    notifyFeedbackFormParamsBaseline(0, -1, null);
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("serialport_EvtSerialPortClosed exception:" + ex.Message);
            }

        }


        /// <summary>
        /// Update UI with status message
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        /// <param name="color">0 = green | 1 = red</param>
        void serialport_EvtSerialPortStatusMessage(object sender, string msg, int color)
        {
            try
            {
                this.Invoke(new Action(() => {

                    string time = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    string text = "";
                    if (msg.Equals("Gesture Detected"))
                    {
                        richTextBoxMessage.AppendText("\n" + time + "    " + msg);
                        text = msg;
                        notifyFeedbackFormGestureDetected(1);
                    }
                    else if (msg.Equals("Gesture Timeout"))
                    {
                        richTextBoxMessage.AppendText("\n\n" + time + "    " + msg + "\n");
                        notifyFeedbackFormGestureDetected(0);
                    }
                    else
                    {
                        richTextBoxMessage.AppendText("\n" + msg);
                        notifyFeedbackFormGestureDetected(0);
                    }

                    labelStatusMessage.Text = text;
                    if (color == 0)
                    {
                        labelStatusMessage.ForeColor = Color.Green;
                    }
                    if (color == 1)
                    {
                        labelStatusMessage.ForeColor = Color.Red;
                    }
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("serialport_EvtSerialPortStatusMessage exception:" + ex.Message);
            }
        }

        /// <summary>
        /// When "Show Chart" button pressed
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void buttonShowChart_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke(new Action(() => { buttonGraphSet_function(); }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("buttonShowChart_Click exception:" + ex.Message);
            }
        }

        /// <summary>
        /// Update UI accordingly when "Show Chart" button pressed
        /// </summary>
        private void buttonGraphSet_function()
        {
            if (buttonChartStart.Text == "Start Graph")
            {
                // Start graphing
                textBoxMaxChartPoints.Enabled = false;
                feaDisplayIndices.Clear();
                LoadChart();
                InitChart();
                buttonChartStart.BackColor = Color.Green;
                buttonChartStart.Text = "Stop Graph";
                AcatActuator.serialport.isChartData = true;
            }
            else
            {
                // Stop / hide graphing
                AcatActuator.serialport.isChartData = false;
                richTextBoxMessage.AppendText("Hiding Chart\n");
                buttonChartStart.Text = "Start Graph";
                textBoxMaxChartPoints.Enabled = true;
                buttonChartStart.BackColor = DefaultBackColor;
                if (chartThread != null && chartThread.IsAlive == true)
                {
                    stopChartThread = true;
                }
            }
        }

        /// <summary>
        /// Load Default settings from DEFAULT XML file and send to Arduino
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void buttonSetDefaults_Click(object sender, EventArgs e)
        {
            load_settings_populate_form(true);
            textBoxMaxChartPoints.Text = "350";
            chartMaxPoints = Convert.ToInt32(textBoxMaxChartPoints.Text);
            this.Invoke(new Action(() => { buttonArduinoSet_function(); }));
        }


        /// <summary>
        /// Check # of point to graph set to a valid number
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void textBoxMaxChartPoints_Leave(object sender, EventArgs e)
        {
            int n;
            chartMaxPoints = -1;
            if (int.TryParse(textBoxMaxChartPoints.Text, out n))
            {
                chartMaxPoints = Convert.ToInt32(textBoxMaxChartPoints.Text);
            }

            if ((chartMaxPoints < 100) || (chartMaxPoints > 10000))
            {
                richTextBoxMessage.AppendText("\nMax Points displayed on the chart should be between 1-10000\n");
                richTextBoxMessage.AppendText("Resetting to default: 350\n");
                textBoxMaxChartPoints.Text = "350";
                textBoxMaxChartPoints.Focus();
                textBoxMaxChartPoints.SelectionStart = textBoxMaxChartPoints.Text.Length;
            }
            else
            {
                richTextBoxMessage.AppendText("\nMax Number of points to display set to " + chartMaxPoints + " samples\n");
            }
        }


        /// <summary>
        /// When main form is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">event args</param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("FormClosed()");
            _mainForm = null;
        }


        /// <summary>
        /// When "Set" button is pressed
        /// Save settings to XML file and send settings to Arduino
        /// </summary>
        private void buttonArduinoSet_function()
        {
            // Save settings to XML file
            Settings AppSettings = new Settings();
            
            // Triggering Mode
            if (radioButtonPeakDetectionAlgo.Checked == true)
            {
                Settings.TriggeringModePeakDetectionAlgo = true;
            }
            else
            {
                Settings.TriggeringModePeakDetectionAlgo = false;
            }
            if (radioButtonThresholding.Checked == true)
            {
                Settings.TriggeringModeThresholding = true;
            }
            else
            {
                Settings.TriggeringModeThresholding = false;
            }


            // Peak Detection Algo

            // Must be between [0 - 40.0]
            decimal newDecimalVal; bool validInput = false;
            validInput = decimal.TryParse(textBoxPeakAlgoGestureStartThreshold.Text, out newDecimalVal);
            if (!validInput || newDecimalVal < 0 || newDecimalVal > 40)
            {
                richTextBoxMessage.AppendText("\nERROR - Peak detection algorithm gesture start threshold must be a number between [0 - 40]");
                textBoxPeakAlgoGestureStartThreshold.Text = "1.00"; // Set to default
                return;
            }
            Settings.PeakDetectionAlgoGestureStartThreshold = (float) newDecimalVal;

            // Must be between [-5.0 - 0]
            validInput = decimal.TryParse(textBoxPeakAlgoGestureEndThreshold.Text, out newDecimalVal);
            if (!validInput || newDecimalVal < -5 || newDecimalVal > 0)
            {
                richTextBoxMessage.AppendText("\nERROR - Peak detection algorithm gesture end threshold must be a number between [-5 - 0]");
                textBoxPeakAlgoGestureEndThreshold.Text = "-0.50"; // Set to default
                return;
            }
            Settings.PeakDetectionAlgoGestureEndThreshold = (float)newDecimalVal;


            // Thresholding

            // Mest be between [2.0 - 13.0]
            validInput = decimal.TryParse(textBoxThresholdingThreshold.Text, out newDecimalVal);
            if (!validInput || newDecimalVal < 2 || newDecimalVal > 13)
            {
                richTextBoxMessage.AppendText("\nERROR - Thresholding gesture start must be a number between [2 - 13]");
                textBoxThresholdingThreshold.Text = "8.00"; // Set to default
                return;
            }
            Settings.ThresholdingGestureStartThreshold = (float)newDecimalVal;


            // Actuate ACAT
            if (checkBoxActuateACAT.Checked == true)
            {
                Settings.ActuateACAT = true;
            }
            else
            {
                Settings.ActuateACAT = false;
            }

            // Feedback Window Recommended Sensor Postion

            // Must be between [0 - 100.0]
            validInput = decimal.TryParse(textBoxRecommendedPositionMin.Text, out newDecimalVal);
            if (!validInput || newDecimalVal < 0 || newDecimalVal > 100)
            {
                richTextBoxMessage.AppendText("\nERROR - Recommended sensor position min must be a number between [0 - 100]");
                textBoxRecommendedPositionMin.Text = "5.00"; // Set to default
                return;
            }
            Settings.RecommendedSensorPositionMin = (float)newDecimalVal;

            // Must be between [0 - 100.0]
            validInput = decimal.TryParse(textBoxRecommendedPositionMax.Text, out newDecimalVal);
            if (!validInput || newDecimalVal < 0 || newDecimalVal > 40)
            {
                richTextBoxMessage.AppendText("\nERROR - Recommended sensor position min must be a number between [0 - 100]");
                textBoxRecommendedPositionMax.Text = "10.00"; // Set to default
                return;
            }
            Settings.RecommendedSensorPositionMax = (float)newDecimalVal;

            AppSettings.Save();


            // Send settings to Arduino
            AcatActuator.serialport.setArduinoSettings();


            // If graphing enabled, restart chart
            if (buttonChartStart.Text == "Stop Graph")
            {
                textBoxMaxChartPoints.Enabled = false;
                feaDisplayIndices.Clear();
                LoadChart();
                InitChart();
                AcatActuator.serialport.isChartData = true;
            }

            if(radioButtonPeakDetectionAlgo.Checked == true)
                notifyFeedbackThresholdChanged(textBoxPeakAlgoGestureStartThreshold.Text);
            else if(radioButtonThresholding.Checked == true)
                notifyFeedbackThresholdChanged(textBoxThresholdingThreshold.Text);
            notifyFeedbackFormParamsBaseline(0, -1, null);

            buttonArduinoSet.BackColor = DefaultBackColor;

            richTextBoxMessage.AppendText("\nGesture Detection Parameters Set\n");
        }

        /// <summary>
        /// Called when "Set" button is pressed
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void buttonArduinoSet_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke(new Action(() => { buttonArduinoSet_function(); }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("buttonArduinoSet_Click exception:" + ex.Message);
            }
        }

        /// <summary>
        /// Change "Set" button to red when UI component changed 
        /// to let user know they need to press that button to
        /// save the settings
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void radioButtonPeakDetectionAlgo_CheckedChanged(object sender, EventArgs e)
        {
            buttonArduinoSet.BackColor = Color.Red;
        }

        /// <summary>
        /// Change "Set" button to red when UI component changed 
        /// to let user know they need to press that button to
        /// save the setti
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void radioButtonThresholding_CheckedChanged(object sender, EventArgs e)
        {
            buttonArduinoSet.BackColor = Color.Red;
        }

        /// <summary>
        /// Change "Set" button to red when UI component changed 
        /// to let user know they need to press that button to
        /// save the settings
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void checkBoxActuateACAT_CheckedChanged(object sender, EventArgs e)
        {
            buttonArduinoSet.BackColor = Color.Red;
        }

        /// <summary>
        /// Change "Set" button to red when UI component changed 
        /// to let user know they need to press that button to
        /// save the settings
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void textBoxThresholdingStartThreshold_Enter(object sender, EventArgs e)
        {
            buttonArduinoSet.BackColor = Color.Red;
        }

        /// <summary>
        /// Change "Set" button to red when UI component changed 
        /// to let user know they need to press that button to
        /// save the settings
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void textBoxPeakAlgoGestureStartThreshold_Enter(object sender, EventArgs e)
        {
            buttonArduinoSet.BackColor = Color.Red;
        }

        /// <summary>
        /// Change "Set" button to red when UI component changed 
        /// to let user know they need to press that button to
        /// save the settings
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void textBoxPeakAlgoGestureEndThreshold_Enter(object sender, EventArgs e)
        {
            buttonArduinoSet.BackColor = Color.Red;
        }

        /// <summary>
        /// When threshold changed in UI
        /// </summary>
        /// <param name="thresholdValue">new threshold value</param>
        private void notifyFeedbackThresholdChanged(string thresholdValue)
        {
            if (EvtFeedbackThresholdChanged != null)
            {
                EvtFeedbackThresholdChanged(this, thresholdValue);
            }
        }

        /// <summary>
        /// When feedback form UI needs to be updated
        /// </summary>
        /// <param name="baseline">new sensor position baseline</param>
        /// <param name="sensorPositionStatus">status of sensor position 
        /// | 0 = good | 1 = move sensor closer | 2 = move sensor farther |
        /// | -1 = no data from sensor | -2 = Connected and calculating sensor position</param>
        /// <param name="suggestedThreshold">new suggested threshold</param>
        public void notifyFeedbackFormParamsBaseline(float baseline, int sensorPositionStatus, string suggestedThreshold)
        {
            if ((!_formFeedback.IsHandleCreated) || (!_formFeedback.IsDisposed))
            {
                if (EvtProximityFirmwareFeedbackFormParamsBaseline != null)
                {
                    EvtProximityFirmwareFeedbackFormParamsBaseline(this, baseline, sensorPositionStatus, textBoxRecommendedPositionMin.Text, textBoxRecommendedPositionMax.Text, suggestedThreshold);
                }
            }
        }

        /// <summary>
        /// When feedback form needs to be notified of gesture detected event
        /// </summary>
        /// <param name="gestureDetected">status of gesture detected
        /// 0 = gesture done | 1 = gesture detected</param>
        public void notifyFeedbackFormGestureDetected(int gestureDetected)
        {
            if ((!_formFeedback.IsHandleCreated) || (!_formFeedback.IsDisposed))
            {
                if (EvtProximityFirmwareFeedbackFormGestureDetected != null)
                {
                    EvtProximityFirmwareFeedbackFormGestureDetected(this, gestureDetected);
                }
            }
        }

    }
}
