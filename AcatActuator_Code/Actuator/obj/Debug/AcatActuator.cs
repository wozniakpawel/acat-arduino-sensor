///////////////////////////////////////////////////////////////////////////
// <copyright file="AcatActuator.cs" company="Intel Corporation">
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

namespace ACAT_Arduino_Windows_App
{
    class AcatActuator
    {
        public static Form1 _form;
        public static SerialComm serialport = null;

        /// <summary>
        /// AcatActuator constructor
        /// </summary>
        public AcatActuator()
        {

        }

        /// <summary>
        /// Initialize main form and serial communication to Arduino
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            System.Diagnostics.Trace.WriteLine("AcatActuator Init()");
           
            // Initialize Serial Comm
            serialport = new SerialComm();

            // Initialize Form
            System.Diagnostics.Trace.WriteLine("Starting Form");
            _form = new Form1();
            _form.Closed += FormOnClosed;
            _form.Show();

            System.Diagnostics.Trace.WriteLine("Form Initialized");

            return true;
        }

        /// <summary>
        /// Callback when main form is closed. Close serial port if open
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="eventArgs">event args</param>
        private void FormOnClosed(object sender, EventArgs eventArgs)
        {
            System.Diagnostics.Trace.WriteLine("FormOnCLosed");

            serialport.Close();
        }

    }
}
