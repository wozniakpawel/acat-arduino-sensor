///////////////////////////////////////////////////////////////////////////
// <copyright file="Settings.cs" company="Intel Corporation">
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
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ACAT_Arduino_Windows_App
{
    public class Settings
    {
        public static String SettingsXmlFileName = "App_Settings.xml"; // Name of file containing settings to use
        public static String DefaultSettingsXmlFileName = "App_Settings_DEFAULT.xml"; //Name of file containing default settings
        public static ArrayList SettingsList = new ArrayList();
        public static String ArduinoSensorSerialPortDescription = "Arduino";
        public static bool TriggeringModeThresholding = true;
        public static bool TriggeringModePeakDetectionAlgo = false;
        public static float ThresholdingGestureStartThreshold = 10.0F;
        public static float PeakDetectionAlgoGestureStartThreshold = 1.0F;
        public static float PeakDetectionAlgoGestureEndThreshold = -0.5F;
        public static bool ActuateACAT = true;
        public static float RecommendedSensorPositionMin = 5.0F;
        public static float RecommendedSensorPositionMax = 10.0F;
        public static float SensorPostionDetectionWindowNumSamples = 500; // 250 Hz * 2 seconds
        public static float SensorPostionDetectionWindowMaxRestStdDev = 0.03F;
        public static String FeedbackWindowPosition = "BottomRight"; //FeedbackWindowPosition = "BottomRight" | "BottomLeft" | "TopLeft" | "TopRight"

        /// <summary>
        /// Load Settings from XML file and save them in variables in this class
        /// </summary>
        /// <param name="loadDefaultSettings">whether to load default settings or not</param>
        public void Load(bool loadDefaultSettings)
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // This will get the current PROJECT directory
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            //System.Diagnostics.Trace.WriteLine("projectDirectory: " + projectDirectory);
            string SettingsFilePath;
            
            if(loadDefaultSettings == true)
                SettingsFilePath = workingDirectory + "//" + DefaultSettingsXmlFileName;
            else
                SettingsFilePath = workingDirectory + "//" + SettingsXmlFileName;

            XmlDocument doc = new XmlDocument();
            doc.Load(SettingsFilePath);
            SettingsList.Clear();

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                String fieldName = node.Name;
                string text = node.InnerText; //or loop through its children as well
                System.Diagnostics.Trace.WriteLine(fieldName + ": " + text);

                Type type = this.GetType().GetField(fieldName).FieldType;
                //System.Diagnostics.Trace.WriteLine("type: " + type.ToString());
                var field = this.GetType().GetField(fieldName); //Of type FieldInfo

                // Add to list keeping track of settings (for saving)
                SettingsList.Add(field);

                if (type == typeof(string))
                {
                    System.Diagnostics.Trace.WriteLine(fieldName + " is a string");
                    field.SetValue(this, text);
                }

                if (type == typeof(bool))
                {
                    //System.Diagnostics.Trace.WriteLine(fieldName + " is a bool");
                    if (text.Equals("true"))
                    {
                        field.SetValue(this, true);
                    }
                    if (text.Equals("false"))
                    {
                        field.SetValue(this, false);
                    }
                }

                if (type == typeof(int))
                {
                    //System.Diagnostics.Trace.WriteLine(fieldName + " is an int");
                    field.SetValue(this, Convert.ToInt32(text));
                }

                if (type == typeof(float))
                {
                    //System.Diagnostics.Trace.WriteLine(fieldName + " is a float");
                    field.SetValue(this, (float)Convert.ToDouble(text));
                }
            }
        }

        /// <summary>
        /// Save settings saved in variables of this class to XML file
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // This will get the current PROJECT directory
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            //System.Diagnostics.Trace.WriteLine("projectDirectory: " + projectDirectory);
            string SettingsFilePath = workingDirectory + "//" + SettingsXmlFileName;

            XmlDocument doc = new XmlDocument();
            XmlNode rootElement = doc.CreateElement(SettingsXmlFileName.Replace(".xml",""));
            doc.AppendChild(rootElement);

            foreach (FieldInfo field in SettingsList)
            {
                // Create and append a child element for the title of the book.
                XmlElement fieldElement = doc.CreateElement(field.Name);

                Type type = field.FieldType;

                if (type == typeof(string))
                {
                    fieldElement.InnerText = field.GetValue(this).ToString();
                }

                if (type == typeof(bool))
                {
                    if ((bool)field.GetValue(this))
                    {
                        fieldElement.InnerText = "true";
                    }
                    if (!(bool)field.GetValue(this))
                    {
                        fieldElement.InnerText = "false";
                    }
                }

                if (type == typeof(int))
                {
                    fieldElement.InnerText = ((int)field.GetValue(this)).ToString();
                }

                if (type == typeof(float))
                { 
                    fieldElement.InnerText = string.Format("{0:N2}", ((float)field.GetValue(this)));
                }

                doc.DocumentElement.AppendChild(fieldElement);
            }

            doc.Save(SettingsFilePath);
       
            return true;
        }



    }
}