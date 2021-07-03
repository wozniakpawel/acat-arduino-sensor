///////////////////////////////////////////////////////////////////////////
// <copyright file="FormFeedback.cs" company="Intel Corporation">
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
using System.Threading;

namespace ACAT_Arduino_Windows_App
{
    public partial class FormFeedback : Form
    {
        public static FormFeedback _feedbackFormHandle = null;
        public static Form1 _form1Handle = null;

        //ACAT WindowOverlapWatchdog (same thing ACAT uses except it doesn't attempt to overtake ACAT windows and timer is set to longer interval)
        private WindowOverlapWatchdog _windowOverlapWatchdog;

        /// <summary>
        /// Feedback form constructor. Initialize window overlap watchdog
        /// </summary>
        public FormFeedback()
        {
            InitializeComponent();
            _windowOverlapWatchdog = new WindowOverlapWatchdog(this, false, 30000); // Watchdog timer = 30 sec
            _feedbackFormHandle = this;
        }

        /// <summary>
        /// Connect feedback form events and main form events
        /// </summary>
        /// <param name="_theOtherForm"></param>
        public void setForm1Handles(Form1 _theOtherForm)
        {
            _form1Handle = _theOtherForm;
            _form1Handle.EvtFeedbackThresholdChanged += _form1Handle_EvtFeedbackThresholdChanged;
            _form1Handle.EvtProximityFirmwareFeedbackFormParamsBaseline += _form1Handle_EvtProximityFirmwareFeedbackFormParamsBaseline;
            _form1Handle.EvtProximityFirmwareFeedbackFormGestureDetected += _form1Handle_EvtProximityFirmwareFeedbackFormGestureDetected;
        }

        /// <summary>
        /// Called when there needs to be an update to feedback form components
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="curBaseline">new sensor position</param>
        /// <param name="sensorPositionStatus">status of sensor position 
        /// | 0 = good | 1 = move sensor closer | 2 = move sensor farther |
        /// | -1 = no data from sensor | -2 = Connceted and calculating sensor position</param>
        /// <param name="lower">lower recommended sensor position value</param>
        /// <param name="upper">upper recommended sensor position value</param>
        /// <param name="suggestedThreshold">suggested threshold</param>
        void _form1Handle_EvtProximityFirmwareFeedbackFormParamsBaseline(object sender, float curBaseline, int sensorPositionStatus, string lower, string upper, string suggestedThreshold)
        {
            if ((this.IsDisposed || !this.IsHandleCreated))
            {
                return;
            }
            try
            {
                this.Invoke(new Action(() =>
                {
                    labelSensorBaselineValue.Text = string.Format("{0:N1}", curBaseline);
                    labelLowerSensorPositionValue.Text = lower;
                    labelUpperSensorPositionValue.Text = upper;

                    if (sensorPositionStatus == 0) // good
                    {
                        labelCurrentSensorPositionStatus.Text = "Good";
                        labelCurrentSensorPositionStatus.BackColor = Color.Green;
                    }
                    else if (sensorPositionStatus == 1) // move closer
                    {
                        labelCurrentSensorPositionStatus.Text = "Move Sensor Closer to Body";
                        labelCurrentSensorPositionStatus.BackColor = Color.Red;
                    }
                    else if (sensorPositionStatus == 2) // move farther
                    {
                        labelCurrentSensorPositionStatus.Text = "Move Sensor Away from Body";
                        labelCurrentSensorPositionStatus.BackColor = Color.Red;
                    }
                    else if (sensorPositionStatus == -1) // No data
                    {
                        labelCurrentSensorPositionStatus.Text = "Not receiving data";
                        labelCurrentSensorPositionStatus.BackColor = Color.LightGray;
                    }
                    else if (sensorPositionStatus == -2) // Connected. But finding sensor baseline
                    {
                        labelCurrentSensorPositionStatus.Text = "Connected. Finding sensor position";
                        labelCurrentSensorPositionStatus.BackColor = Color.LightGreen;
                    }
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("_form1Handle_EvtProximityFirmwareFeedbackFormParamsBaseline: " + ex.Message);
            }
        }

        /// <summary>
        /// Called when gesture detected and need to update UI
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="gestureDetected">status of gesture detected
        /// 0 = gesture done | 1 = gesture detected</param>
        void _form1Handle_EvtProximityFirmwareFeedbackFormGestureDetected(object sender, int gestureDetected)
        {
            if ((this.IsDisposed || !this.IsHandleCreated))
            {
                return;
            }
            try
            {
                this.Invoke(new Action(() =>
                {
                    if (gestureDetected == 1)
                    {
                        labelFeedbackSuggestedThreshold.Text = "Gesture Detected";
                    }
                    else if (gestureDetected == 0)
                    {
                        labelFeedbackSuggestedThreshold.Text = "";
                    }
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("_form1Handle_EvtProximityFirmwareFeedbackFormGestureDetected " + ex.Message);
            }
        }

        /// <summary>
        /// Feedback form load. Sets window position
        /// FeedbackWindowPosition = "BottomRight" | "BottomLeft" | "TopLeft" | "TopRight"
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void FormFeedback_Load(object sender, EventArgs e)
        {
            String feedbackWindowPos = Settings.FeedbackWindowPosition;
            WindowPosition wp = WindowPosition.BottomRight; // Default
            if (feedbackWindowPos.Contains("BottomRight"))
            {
                wp = WindowPosition.BottomRight;
            }
            else if (feedbackWindowPos.Contains("BottomLeft"))
            {
                wp = WindowPosition.BottomLeft;
            }
            else if (feedbackWindowPos.Contains("TopLeft"))
            {
                wp = WindowPosition.TopLeft;
            }
            else if (feedbackWindowPos.Contains("TopRight"))
            {
                wp = WindowPosition.TopRight;
            }
            SetWindowPosition(this, wp);
        }

        /// <summary>
        /// Set threshold box to new value
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="thresholdValue">new threshold value</param>
        void _form1Handle_EvtFeedbackThresholdChanged(object sender, string thresholdValue)
        {
            this.Invoke(new Action(() => { labelCurrentThresholdValue.Text = thresholdValue; }));
        }

        /// <summary>
        /// Feedback form is closing. Dispose of window overlap watchdog and main form
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void FormFeedback_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_windowOverlapWatchdog != null)
                {
                    _windowOverlapWatchdog.Dispose();
                }

                //If feedback form closed, also close main form
                if (_form1Handle != null && (_form1Handle._isClosing == false))
                {
                    _form1Handle.Close();
                    _form1Handle = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("FormClosing: exception" + ex.Message);
            }
        }

        /// <summary>
        /// Detect resolution change / monitor plug in and adjust feedback window position accordingly
        /// </summary>
        private const int WM_DISPLAYCHANGE = 0x7E;
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            switch (m.Msg)
            {
                case WM_DISPLAYCHANGE:
                    new Thread(() =>
                    {
                        this.Invoke(new Action(() => {
                            Thread.Sleep(5000);
                            Thread.CurrentThread.IsBackground = true;

                            String feedbackWindowPos = Settings.FeedbackWindowPosition;
                            WindowPosition wp = WindowPosition.BottomRight; // Default
                            if (feedbackWindowPos.Contains("BottomRight"))
                            {
                                wp = WindowPosition.BottomRight;
                            }
                            else if (feedbackWindowPos.Contains("BottomLeft"))
                            {
                                wp = WindowPosition.BottomLeft;
                            }
                            else if (feedbackWindowPos.Contains("TopLeft"))
                            {
                                wp = WindowPosition.TopLeft;
                            }
                            else if (feedbackWindowPos.Contains("TopRight"))
                            {
                                wp = WindowPosition.TopRight;
                            }
                            SetWindowPosition(this, wp);
                        }));
                    }).Start();
                    break;
            }
            base.WndProc(ref m);
        }


        /// <summary>
        /// Window position
        /// </summary>
        public enum WindowPosition
        {
            TopRight,
            TopLeft,
            BottomRight,
            BottomLeft,
            CenterScreen,
        }

        /// <summary>
        /// Set the window position and notify when the position has changed
        /// </summary>
        /// <param name="form">form to change</param>
        /// <param name="position">new window position</param>
        static public void SetWindowPosition(Form form, WindowPosition position)
        {
            Rectangle r = Screen.PrimaryScreen.WorkingArea;
            form.StartPosition = FormStartPosition.Manual;

            switch (position)
            {
                case WindowPosition.TopRight:
                    form.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - form.Width, 0);
                    break;

                case WindowPosition.TopLeft:
                    form.Location = new Point(0, 0);
                    break;

                case WindowPosition.BottomRight:
                    form.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - form.Width, Screen.PrimaryScreen.WorkingArea.Height - form.Height);
                    break;

                case WindowPosition.BottomLeft:
                    form.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height - form.Height);
                    break;

                case WindowPosition.CenterScreen:
                    form.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - form.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - form.Height) / 2);
                    break;
            }
        }


    }

}
