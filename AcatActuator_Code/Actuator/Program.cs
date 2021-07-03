///////////////////////////////////////////////////////////////////////////
// <copyright file="Program.cs" company="Intel Corporation">
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
using System.Windows.Forms;

namespace ACAT_Arduino_Windows_App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Diagnostics.Process[] pname = System.Diagnostics.Process.GetProcessesByName("AcatActuator");
            if (pname.Length > 1){

                //Kill AcatActuator process if it is already open
                foreach (var process in System.Diagnostics.Process.GetProcessesByName("AcatActuator"))
                {
                    TimeSpan runtime = DateTime.Now - process.StartTime;
                    if(runtime.Seconds > 1)
                        process.Kill();
                }

                System.Threading.Thread.Sleep(1000);

                //Run App
                AcatActuator acatActuator = new AcatActuator();
                acatActuator.Init();
                Application.Run();
                
            }  
            else{

                //Run App
                AcatActuator acatActuator = new AcatActuator();
                acatActuator.Init();
                Application.Run();
            }
        }

    }
}
