///////////////////////////////////////////////////////////////////////////
// <copyright file="SerialComm.cs" company="Intel Corporation">
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
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Management;
using System.Linq;
using System.Globalization;

namespace ACAT_Arduino_Windows_App
{
    class SerialComm
    {
        private string serialPortName = null;
        private SerialPort serialCom;
        private Thread serialConnectThread;
        private bool stopSerialConnectThread = false;
        private int numAttempts = 1;
        private bool attemptingToConnect = false;
        private Thread maintainComThread;
        private bool stopMaintainComThread = false;

        private long previousProximitySensorTimestamp = -999;
        private long proximitySensorTimestamp = -999;

        private Thread ACATActuateThread;        
        private bool stopACATActuateThread = false;
        bool actuate_start_received = false;
        bool actuate_end_received = false;
        bool actuate_timeout_received = false;
        public ConcurrentQueue<string> actuateQueue = new ConcurrentQueue<string>();

        string strDataReceived = "";
        string strDataReceivedCleaned = "";
        string strDataReceivedPartial = "";
        // Skip first skipCount chunks of data received after COM port opened
        // Right after COM opened, can receive jumbled / bad data from Arduino
        int skipSerialDataCount = 0;

        public bool isChartData = false;
        public ConcurrentQueue<string> chartQueue = new ConcurrentQueue<string>();

        public delegate void SerialPortOpenedEventDelegate(object sender, string errmsg);
        public event SerialPortOpenedEventDelegate EvtSerialPortOpened;
        public delegate void SerialPortClosedEventDelegate(object sender, string errmsg);
        public event SerialPortClosedEventDelegate EvtSerialPortClosed;
       
        //color | 0 = green | 1 = red
        public delegate void SerialPortStatusMessage(object sender, string message, int color);
        public event SerialPortStatusMessage EvtSerialPortStatusMessage;

        public SensorPositionTracker sensorPositionTracker = new SensorPositionTracker();

        /// <summary>
        /// SerialComm constructor. Instantiate new SerialPort object and set baud
        /// </summary>
        public SerialComm()
        {
            serialCom = new SerialPort();
            serialCom.BaudRate = 9600;
        }

        /// <summary>
        /// Callback when data is received over serial link from Arduino
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        private void serialCom_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort spReceiver = (SerialPort)sender;
                strDataReceived = spReceiver.ReadExisting();

                if (skipSerialDataCount < 500)
                {
                    skipSerialDataCount++;
                    return;
                }

                ParseData();
                
                if (isChartData)
                {
                    string strChartQueueData = String.Copy(strDataReceivedCleaned);
                    chartQueue.Enqueue(strChartQueueData);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("serialCom_DataReceived(): exception" + ex.Message);
            }
        }

        /// <summary>
        /// Attempt to open serial connection. If successful, send
        /// current settings to Arduino and start ACAT actuate thread
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            bool status = false;
            try
            {
                if (serialCom.IsOpen == false)
                {
                    try
                    {
                        serialCom.PortName = serialPortName;
                        serialCom.Open();
                        System.Diagnostics.Trace.WriteLine("Opened " + serialPortName);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Trace.WriteLine("Open() exception: " + e.Message);
                    }

                    //Serial port opened successfully
                    if (serialCom.IsOpen)
                    {
                        //Cleanup if bad close
                        serialCom.DiscardOutBuffer();
                        strDataReceived = "";
                        strDataReceivedCleaned = "";
                        skipSerialDataCount = 0;
                        serialCom.DataReceived += serialCom_DataReceived;
                        status = true;
                        notifySerialPortOpened();

                        // Send current settings to Arduino
                        new Thread(() =>
                        {
                            Thread.Sleep(3000);
                            setArduinoSettings();
                        }).Start();

                        // Start thread that sends ACAT actuate events
                        stopACATActuateThread = false;
                        ACATActuateThread = new Thread(new ThreadStart(ACATActuateThreadFunc));
                        ACATActuateThread.Start();
                    }
                }
                else
                {
                    //port already open
                    System.Diagnostics.Trace.WriteLine("Port already open");
                    status = true;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("Open() exception: " + e.Message);
            }

            return (status);
        }

        /// <summary>
        /// Attempt to close serial connection
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            bool status = false;
            try
            {
                if (serialCom.IsOpen == true)
                {
                    serialCom.DataReceived -= serialCom_DataReceived;
                    System.Diagnostics.Trace.WriteLine("Close() trying...");

                    //Timeout if serialCom.Close() doesn't return 
                    var task = Task.Run(() => serialCom.Close());
                    if (task.Wait(1500))
                    {
                        System.Diagnostics.Trace.WriteLine("Close() Successfully returned from serialCom.Close()");
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine("Close() Timed out from serialCom.Close()");
                    }
                    Thread.Sleep(500);
                    
                    stopACATActuateThread = true;
                    notifySerialPortClosed();
                    status = true;
                }
                else
                    System.Diagnostics.Trace.WriteLine("serialCom is already closed");
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("Close() exception: " + e.Message);
            }

            return (status);
        }

        /// <summary>
        /// Send settings to Arduino over serial connection
        /// Commands enclosed by '<' '>' and in format <newSetting,settingName,value>
        /// </summary>
        /// <returns></returns>
        public bool setArduinoSettings()
        {
            bool status = true;
            try
            {
                lock (serialCom)
                {
                    if (serialCom.IsOpen)
                    {

                        // Send all settings to Arduino

                        // Commands enclosed by <> and in format
                        // <newSetting,settingName,value>

                        // Triggering Mode
                        if (Settings.TriggeringModePeakDetectionAlgo == true)
                        {
                            //System.Diagnostics.Trace.WriteLine("[SerialComm] Sending test string");
                            serialCom.WriteLine("<newSetting,TriggeringModePeakDetectionAlgo,1>");
                            Thread.Sleep(10);
                        }
                        else
                        {
                            serialCom.WriteLine("<newSetting,TriggeringModePeakDetectionAlgo,0>");
                            Thread.Sleep(10);
                        }
                        if (Settings.TriggeringModeThresholding == true)
                        {
                            serialCom.WriteLine("<newSetting,TriggeringModeThresholding,1>");
                            Thread.Sleep(10);
                        }
                        else
                        {
                            serialCom.WriteLine("<newSetting,TriggeringModeThresholding,0>");
                            Thread.Sleep(10);
                        }

                        // Peak Detection Algo start and end thresholds
                        serialCom.WriteLine("<newSetting,PeakDetectionAlgoGestureStartThreshold,"+
                            Settings.PeakDetectionAlgoGestureStartThreshold+">");
                        Thread.Sleep(10);
                        serialCom.WriteLine("<newSetting,PeakDetectionAlgoGestureEndThreshold," +
                            Settings.PeakDetectionAlgoGestureEndThreshold + ">");
                        Thread.Sleep(10);

                        // Thresholding threshold
                        serialCom.WriteLine("<newSetting,ThresholdingGestureStartThreshold," +
                            Settings.ThresholdingGestureStartThreshold + ">");
                        Thread.Sleep(10);
                    } 
                }
            }
            catch (Exception e)
            {
                status = false;
                System.Diagnostics.Trace.WriteLine("setArduinoSettings() exception: " + e.Message);
            }
            return (status);
        }

        /// <summary>
        /// For shifting focus away from AcatActuator app if it's selected
        /// and gesture occurs
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        private string GetActiveProcessFileName()
        {
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p.ProcessName;
        }

        /// <summary>
        /// For sending F12 key UP/DOWN events (to actuate ACAT)
        /// </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const int VK_F12 = 0x7B;

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Send F12 key down event to actuate ACAT
        /// If AcatActuator is focused window, shift focus to Desktop
        /// so ACAT receives F12 key event
        /// </summary>
        private void sendF12KeyDownEvent()
        {
            System.Diagnostics.Trace.WriteLine("sendF12KeyDownEvent()");
            // If foreground window selected is current app (AcatActuator), change
            // focus to Desktop (explorer). F12 doesn't work to trigger ACAT when AcatActuator focused
            if (GetActiveProcessFileName().Equals("AcatActuator")) 
            {
                //System.Diagnostics.Trace.WriteLine("Shifting focus to explorer (desktop)...");
                Process[] processes = Process.GetProcessesByName("explorer"); 
                SetForegroundWindow(processes[0].MainWindowHandle);
            }
            //Send F12 key DOWN event (to actuate ACAT)
            keybd_event((byte)VK_F12, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
        }

        /// <summary>
        /// Send F12 key up event to finish ACAT actuation
        /// If AcatActuator is focused window, shift focus to Desktop
        /// so ACAT receives F12 key event
        /// </summary>
        private void sendF12KeyUpEvent()
        {
            System.Diagnostics.Trace.WriteLine("sendF12KeyUpEvent()");
            if (GetActiveProcessFileName().Equals("AcatActuator")) 
            {
                //System.Diagnostics.Trace.WriteLine("Shifting focus to explorer (desktop)...");
                Process[] processes = Process.GetProcessesByName("explorer");
                SetForegroundWindow(processes[0].MainWindowHandle);
            }
            //Send F12 key UP event (to actuate ACAT)
            keybd_event((byte) VK_F12, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// ACAT actuate thread. Receives gesture start / end messages
        /// and sends F12 key down / up events accordingly
        /// </summary>
        private void ACATActuateThreadFunc()
        {
            System.Diagnostics.Trace.WriteLine("Started ACATActuateThreadFunc()");
            string strActuateEvent = null;
            while (stopACATActuateThread == false) 
            {
                actuateQueue.TryDequeue(out strActuateEvent);
                if (!String.IsNullOrEmpty(strActuateEvent))
                {
                    if (strActuateEvent.Equals("actuate_start"))
                    {
                        //System.Diagnostics.Trace.WriteLine("sendACATActuateEventsThread - actuate_start");
                        sendF12KeyDownEvent();
                    }
                    if (strActuateEvent.Equals("actuate_end"))
                    {
                        //System.Diagnostics.Trace.WriteLine("sendACATActuateEventsThread - actuate_end");
                        sendF12KeyUpEvent();
                    }
                }
            }
        }

        /// <summary>
        /// Function to parse and clean serial data received from Arduino
        /// </summary>
        private void ParseData()
        {
            string strInData;
            strDataReceivedCleaned = "";
            if (!String.IsNullOrWhiteSpace(strDataReceivedPartial)) // != "")
            {
                strInData = strDataReceivedPartial + strDataReceived;
                strDataReceivedPartial = "";
            }
            else
                strInData = strDataReceived;

            char[] delimiters = new char[] { '\n', '\r', '$' };
            string[] samples = strInData.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            bool isIncompleteLastSample = true;
            if (strInData.EndsWith("\n") || strInData.EndsWith("\r") || strInData.EndsWith("$"))
                isIncompleteLastSample = false;

            int sampleNum = 0;
            foreach (string onesample in samples)
            {
                sampleNum++;
                if (isIncompleteLastSample && (sampleNum == samples.Length))
                {
                    strDataReceivedPartial = onesample;
                    continue;
                }

                if (onesample.Contains("Error"))
                {
                    foreach (string errSamp in samples)
                    {
                        System.Diagnostics.Trace.WriteLine(onesample);
                    }
                }

                // ACTUATE_START / ACTUATE_END
                if (onesample.Contains("ACTUATE_START"))
                {
                    actuate_start_received = true;
                    if (Settings.ActuateACAT)
                    {
                        //Send F12 key DOWN event (to actuate ACAT)
                        actuateQueue.Enqueue("actuate_start");
                    }
                    //System.Diagnostics.Trace.WriteLine("After sendF12KeyDownEvent()");
                }

                if (onesample.Contains("ACTUATE_END"))
                {
                    actuate_end_received = true;
                    if (Settings.ActuateACAT)
                    {
                        //Send F12 key UP event (to actuate ACAT)
                        actuateQueue.Enqueue("actuate_end");
                    }
                    //System.Diagnostics.Trace.WriteLine("After sendF12KeyUpEvent()");
                }

                if (onesample.Contains("ACTUATE_TIMEOUT"))
                {
                    actuate_timeout_received = true;
                    if (Settings.ActuateACAT)
                    {
                        //Send F12 key UP event (to actuate ACAT)
                        actuateQueue.Enqueue("actuate_end");
                    }
                    //System.Diagnostics.Trace.WriteLine("After sendF12KeyUpEvent()");
                }

                if ( onesample.StartsWith("P,"))
                {
                    char[] delimiters2 = new char[] { '\r', ',', ' ' };
                    string[] features = onesample.Split(delimiters2, StringSplitOptions.RemoveEmptyEntries);
                    DateTime TimeStamp = DateTime.Now;
                    proximitySensorTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    if ( (features[0] == "P") )
                    {
                        strDataReceivedCleaned = onesample;

                        // If ACTUATE_START / ACTUATE_END messages received, append 1 or 2 to the proximity data accordingly

                        if (actuate_start_received)
                        {
                            //System.Diagnostics.Trace.WriteLine("Appending 1 to strDataReceivedCleaned");
                            strDataReceivedCleaned += ",1";
                            EvtSerialPortStatusMessage(this,"Gesture Detected", 0);
                            actuate_start_received = false;
                        }

                        if (actuate_end_received)
                        {
                            //System.Diagnostics.Trace.WriteLine("Appending 2 to strDataReceivedCleaned");
                            strDataReceivedCleaned += ",2";
                            EvtSerialPortStatusMessage(this, "", 0);
                            actuate_end_received = false;
                        }

                        if (actuate_timeout_received)
                        {
                            //System.Diagnostics.Trace.WriteLine("Appending 3 to strDataReceivedCleaned");
                            strDataReceivedCleaned += ",3";
                            EvtSerialPortStatusMessage(this, "Gesture Timeout", 0);
                            actuate_timeout_received = false;
                        }

                        //System.Diagnostics.Trace.WriteLine("features[2]:" + features[2]);
                        sensorPositionTracker.trackBaselineForSensorPositionFeedback(float.Parse(features[2], CultureInfo.InvariantCulture.NumberFormat));
                    }
                    else
                    {
                        strDataReceivedPartial = onesample;
                    }
                }

            }
        }

        /// <summary>
        /// Serial port opened event
        /// </summary>
        private void notifySerialPortOpened()
        {
            if (EvtSerialPortOpened != null)
            {
                EvtSerialPortOpened(this, serialPortName);
            }
        }

        /// <summary>
        /// Serial port closed event
        /// </summary>
        private void notifySerialPortClosed()
        {
            if (EvtSerialPortClosed != null)
            {
                EvtSerialPortClosed(this, numAttempts.ToString());
            }
        }

        /// <summary>
        /// Initializes serial auto connect mechanisms
        /// </summary>
        public void Init()
        {
            startSerialConnectThread();
            startMaintainComThread(); //Maintain COM thread always running
        }

        /// <summary>
        /// Start thread that tries to auto connect to Arduino
        /// </summary>
        public void startSerialConnectThread()
        {
            stopSerialConnectThread = false;
            attemptingToConnect = true;
            serialConnectThread = new Thread(new ThreadStart(SerialConnectThreadFunction));
            serialConnectThread.Start();
        }

        /// <summary>
        /// Serial auto connect function
        /// </summary>
        private void SerialConnectThreadFunction()
        {
            System.Diagnostics.Trace.WriteLine("SerialConnectThreadFunction start");
            while (!stopSerialConnectThread)
            {
                // Stop thread if main form closed
                if (Form1._mainForm == null || Form1._mainForm._isClosing == true)
                {
                    stopSerialConnectThread = true;
                    break;
                }

                numAttempts++;
                serialPortName = null;
                // Look for Arduino sensor in COM ports
                serialPortName = FindSerialPortName();

                //found com port
                if (!String.IsNullOrEmpty(serialPortName))
                {
                    //Open port and start sending/receiving data
                    System.Diagnostics.Trace.WriteLine("Opening Com port " + serialPortName);
                    if (Open() == true)
                    {
                        System.Diagnostics.Trace.WriteLine("Port " + serialPortName + " opened");
                        stopSerialConnectThread = true;
                        numAttempts = 1;
                        previousProximitySensorTimestamp = -999;
                        attemptingToConnect = false;
                    }
                }
                else
                {
                    attemptingToConnect = true;
                    System.Diagnostics.Trace.WriteLine("Still searching for Arduino sensor:" + numAttempts);
                    notifySerialPortClosed();
                    Thread.Sleep(1000); //sleep 1 sec before retrying
                }
            }
        }

        /// <summary>
        /// Look for Arduino com port based on string in settings XML file
        /// </summary>
        /// <returns></returns>
        private string FindSerialPortName()
        {
            String serialPortDescription = Settings.ArduinoSensorSerialPortDescription;
            string comPortname = "";
            try
            {
                using (var searcher = new ManagementObjectSearcher
                ("SELECT * FROM WIN32_SerialPort"))
                {
                    string[] portnames = SerialPort.GetPortNames();
                    var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                    var tList = (from n in portnames
                                 join p in ports on n equals p["DeviceID"].ToString()
                                 select n + " - " + p["Caption"]).ToList();

                    System.Diagnostics.Trace.WriteLine("FindSerialPortName | Found the following COM devices");
                    foreach (string s in tList)
                    {
                        System.Diagnostics.Trace.WriteLine("FindSerialPortName | " + s);

                        if (s.Contains(serialPortDescription))
                        {
                            comPortname = s.Split('(', ')')[1];
                            System.Diagnostics.Trace.WriteLine("Found comport: " + s + "*" + comPortname + "*");
                            break;
                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                System.Diagnostics.Trace.WriteLine("Could not find " + serialPortDescription);
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            System.Diagnostics.Trace.WriteLine("Returning comport: " + comPortname);
            return (comPortname);
        }

        /// <summary>
        /// Start thread that checks if we're receving data from Arduino
        /// If not, start serial auto connect thread
        /// </summary>
        private void startMaintainComThread()
        {
            stopMaintainComThread = false;
            maintainComThread = new Thread(new ThreadStart(MaintainComThreadFunction));
            maintainComThread.Start();
        }

        /// <summary>
        /// Check if we're receiving data from Arduino every so often
        /// If not, start serial auto connect thread
        /// </summary>
        private void MaintainComThreadFunction()
        {
            System.Diagnostics.Trace.WriteLine("MaintainComThreadFunction start");
            while (!stopMaintainComThread)
            {
                // Stop thread if Form1 closed
                if (Form1._mainForm == null || Form1._mainForm._isClosing == true)
                {
                    stopMaintainComThread = true;
                    break;
                }

                //System.Diagnostics.Trace.WriteLine("Checking if COM is alive...");
                //Every 3 sec check if COM connection alive (getting proximity data). If not, start auto connect thread again
                Thread.Sleep(3000);

                //Don't execute rest of thread body if attempting to connect
                if (attemptingToConnect == true)
                    continue;

                if ((previousProximitySensorTimestamp != -999) && (previousProximitySensorTimestamp >= proximitySensorTimestamp)) //previousProximitySensorTimestamp = -999 on first connect
                {
                    //System.Diagnostics.Trace.WriteLine("COM is not alive. Starting serial connect thread...");
                    Close();
                    notifySerialPortClosed();
                    startSerialConnectThread();
                }
                else
                {
                    //System.Diagnostics.Trace.WriteLine("COM is alive");
                    previousProximitySensorTimestamp = proximitySensorTimestamp;
                }
            }
        }

    }
}
