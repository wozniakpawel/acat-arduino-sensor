///////////////////////////////////////////////////////////////////////////
// <copyright file="Form1.Designer.cs" company="Intel Corporation">
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

namespace ACAT_Arduino_Windows_App
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBoxCOMPort = new System.Windows.Forms.GroupBox();
            this.labelComPort = new System.Windows.Forms.Label();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.labelStatusMessage = new System.Windows.Forms.Label();
            this.chartSensor = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonChartStart = new System.Windows.Forms.Button();
            this.textBoxMaxChartPoints = new System.Windows.Forms.TextBox();
            this.labelMaxChartPoints = new System.Windows.Forms.Label();
            this.panelArduino = new System.Windows.Forms.Panel();
            this.buttonSetDefaults = new System.Windows.Forms.Button();
            this.buttonArduinoSet = new System.Windows.Forms.Button();
            this.panelArduinoControl = new System.Windows.Forms.Panel();
            this.panelPeakAlgoFeedback = new System.Windows.Forms.Panel();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.textBoxRecommendedPositionMax = new System.Windows.Forms.TextBox();
            this.textBoxRecommendedPositionMin = new System.Windows.Forms.TextBox();
            this.labelRecommendedSensorPosition1 = new System.Windows.Forms.Label();
            this.labelRecommendedSensorPosition2 = new System.Windows.Forms.Label();
            this.labelFeedbackWindowSettings = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelThresholdingThreshold = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelThresholdingThreshold = new System.Windows.Forms.Label();
            this.textBoxThresholdingThreshold = new System.Windows.Forms.TextBox();
            this.labelThresholdingSettings = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxActuateACAT = new System.Windows.Forms.CheckBox();
            this.panelTriggeringMode = new System.Windows.Forms.Panel();
            this.radioButtonThresholding = new System.Windows.Forms.RadioButton();
            this.labelTriggeringMode = new System.Windows.Forms.Label();
            this.radioButtonPeakDetectionAlgo = new System.Windows.Forms.RadioButton();
            this.labelArduinoControl = new System.Windows.Forms.Label();
            this.panelPDAlgoSettings = new System.Windows.Forms.Panel();
            this.panelPeakAlgoLabelGestureEnd = new System.Windows.Forms.Panel();
            this.labelPeakAlgoGestureEndThreshold2 = new System.Windows.Forms.Label();
            this.labelPeakAlgoGestureEndThreshold1 = new System.Windows.Forms.Label();
            this.textBoxPeakAlgoGestureEndThreshold = new System.Windows.Forms.TextBox();
            this.panelPeakAlgoLabelGestureStart = new System.Windows.Forms.Panel();
            this.labelPeakAlgoGestureStart2 = new System.Windows.Forms.Label();
            this.labelPeakAlgoGestureStartThreshold1 = new System.Windows.Forms.Label();
            this.labelPeakDetectionAlgoThresholds = new System.Windows.Forms.Label();
            this.textBoxPeakAlgoGestureStartThreshold = new System.Windows.Forms.TextBox();
            this.labelPeakDetectionAlgoSettings = new System.Windows.Forms.Label();
            this.groupBoxCOMPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSensor)).BeginInit();
            this.panelArduino.SuspendLayout();
            this.panelArduinoControl.SuspendLayout();
            this.panelPeakAlgoFeedback.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelThresholdingThreshold.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelTriggeringMode.SuspendLayout();
            this.panelPDAlgoSettings.SuspendLayout();
            this.panelPeakAlgoLabelGestureEnd.SuspendLayout();
            this.panelPeakAlgoLabelGestureStart.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCOMPort
            // 
            this.groupBoxCOMPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBoxCOMPort.Controls.Add(this.labelComPort);
            this.groupBoxCOMPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCOMPort.Location = new System.Drawing.Point(13, 11);
            this.groupBoxCOMPort.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxCOMPort.Name = "groupBoxCOMPort";
            this.groupBoxCOMPort.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxCOMPort.Size = new System.Drawing.Size(82, 61);
            this.groupBoxCOMPort.TabIndex = 1;
            this.groupBoxCOMPort.TabStop = false;
            this.groupBoxCOMPort.Text = "COM Port";
            // 
            // labelComPort
            // 
            this.labelComPort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelComPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelComPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelComPort.ForeColor = System.Drawing.Color.Black;
            this.labelComPort.Location = new System.Drawing.Point(3, 22);
            this.labelComPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelComPort.Name = "labelComPort";
            this.labelComPort.Size = new System.Drawing.Size(74, 29);
            this.labelComPort.TabIndex = 30;
            this.labelComPort.Text = "Text";
            this.labelComPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.HideSelection = false;
            this.richTextBoxMessage.Location = new System.Drawing.Point(11, 369);
            this.richTextBoxMessage.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Size = new System.Drawing.Size(313, 203);
            this.richTextBoxMessage.TabIndex = 2;
            this.richTextBoxMessage.Text = "";
            // 
            // labelStatusMessage
            // 
            this.labelStatusMessage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelStatusMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatusMessage.ForeColor = System.Drawing.Color.Red;
            this.labelStatusMessage.Location = new System.Drawing.Point(339, 10);
            this.labelStatusMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStatusMessage.Name = "labelStatusMessage";
            this.labelStatusMessage.Size = new System.Drawing.Size(452, 63);
            this.labelStatusMessage.TabIndex = 6;
            this.labelStatusMessage.Text = "Text";
            this.labelStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chartSensor
            // 
            chartArea3.CursorX.IsUserEnabled = true;
            chartArea3.CursorX.IsUserSelectionEnabled = true;
            chartArea3.Name = "ChartArea1";
            chartArea4.Name = "ChartArea2";
            chartArea4.Visible = false;
            this.chartSensor.ChartAreas.Add(chartArea3);
            this.chartSensor.ChartAreas.Add(chartArea4);
            legend2.DockedToChartArea = "ChartArea1";
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            legend2.Enabled = false;
            legend2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend1";
            this.chartSensor.Legends.Add(legend2);
            this.chartSensor.Location = new System.Drawing.Point(330, 77);
            this.chartSensor.Margin = new System.Windows.Forms.Padding(2);
            this.chartSensor.Name = "chartSensor";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Blue;
            series2.IsValueShownAsLabel = true;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.chartSensor.Series.Add(series2);
            this.chartSensor.Size = new System.Drawing.Size(476, 369);
            this.chartSensor.TabIndex = 7;
            // 
            // buttonChartStart
            // 
            this.buttonChartStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonChartStart.Location = new System.Drawing.Point(328, 451);
            this.buttonChartStart.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChartStart.Name = "buttonChartStart";
            this.buttonChartStart.Size = new System.Drawing.Size(94, 21);
            this.buttonChartStart.TabIndex = 8;
            this.buttonChartStart.Text = "Stop Graph";
            this.buttonChartStart.UseVisualStyleBackColor = true;
            this.buttonChartStart.Click += new System.EventHandler(this.buttonShowChart_Click);
            // 
            // textBoxMaxChartPoints
            // 
            this.textBoxMaxChartPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMaxChartPoints.Location = new System.Drawing.Point(460, 453);
            this.textBoxMaxChartPoints.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMaxChartPoints.Name = "textBoxMaxChartPoints";
            this.textBoxMaxChartPoints.Size = new System.Drawing.Size(37, 19);
            this.textBoxMaxChartPoints.TabIndex = 13;
            this.textBoxMaxChartPoints.Text = "350";
            this.textBoxMaxChartPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMaxChartPoints.Leave += new System.EventHandler(this.textBoxMaxChartPoints_Leave);
            // 
            // labelMaxChartPoints
            // 
            this.labelMaxChartPoints.AutoSize = true;
            this.labelMaxChartPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxChartPoints.Location = new System.Drawing.Point(427, 454);
            this.labelMaxChartPoints.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMaxChartPoints.Name = "labelMaxChartPoints";
            this.labelMaxChartPoints.Size = new System.Drawing.Size(36, 13);
            this.labelMaxChartPoints.TabIndex = 12;
            this.labelMaxChartPoints.Text = "Points";
            // 
            // panelArduino
            // 
            this.panelArduino.Controls.Add(this.buttonSetDefaults);
            this.panelArduino.Controls.Add(this.buttonArduinoSet);
            this.panelArduino.Controls.Add(this.panelArduinoControl);
            this.panelArduino.Location = new System.Drawing.Point(11, 76);
            this.panelArduino.Margin = new System.Windows.Forms.Padding(2);
            this.panelArduino.Name = "panelArduino";
            this.panelArduino.Size = new System.Drawing.Size(313, 289);
            this.panelArduino.TabIndex = 29;
            // 
            // buttonSetDefaults
            // 
            this.buttonSetDefaults.Location = new System.Drawing.Point(2, 264);
            this.buttonSetDefaults.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSetDefaults.Name = "buttonSetDefaults";
            this.buttonSetDefaults.Size = new System.Drawing.Size(56, 20);
            this.buttonSetDefaults.TabIndex = 11;
            this.buttonSetDefaults.Text = "Defaults";
            this.buttonSetDefaults.UseVisualStyleBackColor = true;
            this.buttonSetDefaults.Click += new System.EventHandler(this.buttonSetDefaults_Click);
            // 
            // buttonArduinoSet
            // 
            this.buttonArduinoSet.Location = new System.Drawing.Point(249, 264);
            this.buttonArduinoSet.Margin = new System.Windows.Forms.Padding(2);
            this.buttonArduinoSet.Name = "buttonArduinoSet";
            this.buttonArduinoSet.Size = new System.Drawing.Size(56, 20);
            this.buttonArduinoSet.TabIndex = 10;
            this.buttonArduinoSet.Text = "Set";
            this.buttonArduinoSet.UseVisualStyleBackColor = true;
            this.buttonArduinoSet.Click += new System.EventHandler(this.buttonArduinoSet_Click);
            // 
            // panelArduinoControl
            // 
            this.panelArduinoControl.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelArduinoControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelArduinoControl.Controls.Add(this.panelPeakAlgoFeedback);
            this.panelArduinoControl.Controls.Add(this.panel2);
            this.panelArduinoControl.Controls.Add(this.panel1);
            this.panelArduinoControl.Controls.Add(this.panelTriggeringMode);
            this.panelArduinoControl.Controls.Add(this.labelArduinoControl);
            this.panelArduinoControl.Controls.Add(this.panelPDAlgoSettings);
            this.panelArduinoControl.Location = new System.Drawing.Point(2, 2);
            this.panelArduinoControl.Margin = new System.Windows.Forms.Padding(2);
            this.panelArduinoControl.Name = "panelArduinoControl";
            this.panelArduinoControl.Size = new System.Drawing.Size(303, 258);
            this.panelArduinoControl.TabIndex = 1;
            // 
            // panelPeakAlgoFeedback
            // 
            this.panelPeakAlgoFeedback.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelPeakAlgoFeedback.Controls.Add(this.label50);
            this.panelPeakAlgoFeedback.Controls.Add(this.label49);
            this.panelPeakAlgoFeedback.Controls.Add(this.textBoxRecommendedPositionMax);
            this.panelPeakAlgoFeedback.Controls.Add(this.textBoxRecommendedPositionMin);
            this.panelPeakAlgoFeedback.Controls.Add(this.labelRecommendedSensorPosition1);
            this.panelPeakAlgoFeedback.Controls.Add(this.labelRecommendedSensorPosition2);
            this.panelPeakAlgoFeedback.Controls.Add(this.labelFeedbackWindowSettings);
            this.panelPeakAlgoFeedback.Location = new System.Drawing.Point(1, 199);
            this.panelPeakAlgoFeedback.Margin = new System.Windows.Forms.Padding(2);
            this.panelPeakAlgoFeedback.Name = "panelPeakAlgoFeedback";
            this.panelPeakAlgoFeedback.Size = new System.Drawing.Size(296, 56);
            this.panelPeakAlgoFeedback.TabIndex = 33;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.Location = new System.Drawing.Point(219, 15);
            this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(26, 13);
            this.label50.TabIndex = 32;
            this.label50.Text = "max";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.Location = new System.Drawing.Point(169, 15);
            this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(23, 13);
            this.label49.TabIndex = 32;
            this.label49.Text = "min";
            // 
            // textBoxRecommendedPositionMax
            // 
            this.textBoxRecommendedPositionMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRecommendedPositionMax.Location = new System.Drawing.Point(210, 31);
            this.textBoxRecommendedPositionMax.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRecommendedPositionMax.Name = "textBoxRecommendedPositionMax";
            this.textBoxRecommendedPositionMax.Size = new System.Drawing.Size(44, 19);
            this.textBoxRecommendedPositionMax.TabIndex = 33;
            // 
            // textBoxRecommendedPositionMin
            // 
            this.textBoxRecommendedPositionMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRecommendedPositionMin.Location = new System.Drawing.Point(161, 31);
            this.textBoxRecommendedPositionMin.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRecommendedPositionMin.Name = "textBoxRecommendedPositionMin";
            this.textBoxRecommendedPositionMin.Size = new System.Drawing.Size(44, 19);
            this.textBoxRecommendedPositionMin.TabIndex = 32;
            // 
            // labelRecommendedSensorPosition1
            // 
            this.labelRecommendedSensorPosition1.AutoSize = true;
            this.labelRecommendedSensorPosition1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRecommendedSensorPosition1.Location = new System.Drawing.Point(1, 21);
            this.labelRecommendedSensorPosition1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRecommendedSensorPosition1.Name = "labelRecommendedSensorPosition1";
            this.labelRecommendedSensorPosition1.Size = new System.Drawing.Size(157, 13);
            this.labelRecommendedSensorPosition1.TabIndex = 32;
            this.labelRecommendedSensorPosition1.Text = "Recommended Baseline Range";
            // 
            // labelRecommendedSensorPosition2
            // 
            this.labelRecommendedSensorPosition2.AutoSize = true;
            this.labelRecommendedSensorPosition2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRecommendedSensorPosition2.Location = new System.Drawing.Point(58, 37);
            this.labelRecommendedSensorPosition2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRecommendedSensorPosition2.Name = "labelRecommendedSensorPosition2";
            this.labelRecommendedSensorPosition2.Size = new System.Drawing.Size(98, 13);
            this.labelRecommendedSensorPosition2.TabIndex = 31;
            this.labelRecommendedSensorPosition2.Text = "For Sensor Position";
            // 
            // labelFeedbackWindowSettings
            // 
            this.labelFeedbackWindowSettings.AutoSize = true;
            this.labelFeedbackWindowSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFeedbackWindowSettings.Location = new System.Drawing.Point(1, 2);
            this.labelFeedbackWindowSettings.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFeedbackWindowSettings.Name = "labelFeedbackWindowSettings";
            this.labelFeedbackWindowSettings.Size = new System.Drawing.Size(162, 13);
            this.labelFeedbackWindowSettings.TabIndex = 31;
            this.labelFeedbackWindowSettings.Text = "Feedback Window Settings";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.panelThresholdingThreshold);
            this.panel2.Controls.Add(this.labelThresholdingThreshold);
            this.panel2.Controls.Add(this.textBoxThresholdingThreshold);
            this.panel2.Controls.Add(this.labelThresholdingSettings);
            this.panel2.Location = new System.Drawing.Point(1, 143);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 52);
            this.panel2.TabIndex = 32;
            // 
            // panelThresholdingThreshold
            // 
            this.panelThresholdingThreshold.Controls.Add(this.label4);
            this.panelThresholdingThreshold.Controls.Add(this.label5);
            this.panelThresholdingThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelThresholdingThreshold.Location = new System.Drawing.Point(99, 18);
            this.panelThresholdingThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.panelThresholdingThreshold.Name = "panelThresholdingThreshold";
            this.panelThresholdingThreshold.Size = new System.Drawing.Size(43, 26);
            this.panelThresholdingThreshold.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 9);
            this.label4.TabIndex = 30;
            this.label4.Text = "start";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1, 3);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 9);
            this.label5.TabIndex = 29;
            this.label5.Text = "gesture";
            // 
            // labelThresholdingThreshold
            // 
            this.labelThresholdingThreshold.AutoSize = true;
            this.labelThresholdingThreshold.Location = new System.Drawing.Point(3, 24);
            this.labelThresholdingThreshold.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelThresholdingThreshold.Name = "labelThresholdingThreshold";
            this.labelThresholdingThreshold.Size = new System.Drawing.Size(94, 13);
            this.labelThresholdingThreshold.TabIndex = 19;
            this.labelThresholdingThreshold.Text = "Current Threshold:";
            // 
            // textBoxThresholdingThreshold
            // 
            this.textBoxThresholdingThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxThresholdingThreshold.Location = new System.Drawing.Point(146, 22);
            this.textBoxThresholdingThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxThresholdingThreshold.Name = "textBoxThresholdingThreshold";
            this.textBoxThresholdingThreshold.Size = new System.Drawing.Size(45, 19);
            this.textBoxThresholdingThreshold.TabIndex = 18;
            this.textBoxThresholdingThreshold.Enter += new System.EventHandler(this.textBoxThresholdingStartThreshold_Enter);
            // 
            // labelThresholdingSettings
            // 
            this.labelThresholdingSettings.AutoSize = true;
            this.labelThresholdingSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelThresholdingSettings.Location = new System.Drawing.Point(2, 0);
            this.labelThresholdingSettings.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelThresholdingSettings.Name = "labelThresholdingSettings";
            this.labelThresholdingSettings.Size = new System.Drawing.Size(130, 13);
            this.labelThresholdingSettings.TabIndex = 1;
            this.labelThresholdingSettings.Text = "Thresholding Settings";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.checkBoxActuateACAT);
            this.panel1.Location = new System.Drawing.Point(201, 143);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(94, 52);
            this.panel1.TabIndex = 32;
            // 
            // checkBoxActuateACAT
            // 
            this.checkBoxActuateACAT.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxActuateACAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxActuateACAT.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxActuateACAT.Location = new System.Drawing.Point(2, 4);
            this.checkBoxActuateACAT.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxActuateACAT.Name = "checkBoxActuateACAT";
            this.checkBoxActuateACAT.Size = new System.Drawing.Size(91, 36);
            this.checkBoxActuateACAT.TabIndex = 0;
            this.checkBoxActuateACAT.Text = "Actuate ACAT";
            this.checkBoxActuateACAT.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxActuateACAT.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.checkBoxActuateACAT.UseVisualStyleBackColor = true;
            this.checkBoxActuateACAT.CheckedChanged += new System.EventHandler(this.checkBoxActuateACAT_CheckedChanged);
            // 
            // panelTriggeringMode
            // 
            this.panelTriggeringMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelTriggeringMode.Controls.Add(this.radioButtonThresholding);
            this.panelTriggeringMode.Controls.Add(this.labelTriggeringMode);
            this.panelTriggeringMode.Controls.Add(this.radioButtonPeakDetectionAlgo);
            this.panelTriggeringMode.Location = new System.Drawing.Point(2, 31);
            this.panelTriggeringMode.Margin = new System.Windows.Forms.Padding(2);
            this.panelTriggeringMode.Name = "panelTriggeringMode";
            this.panelTriggeringMode.Size = new System.Drawing.Size(294, 48);
            this.panelTriggeringMode.TabIndex = 9;
            // 
            // radioButtonThresholding
            // 
            this.radioButtonThresholding.AutoSize = true;
            this.radioButtonThresholding.Location = new System.Drawing.Point(156, 20);
            this.radioButtonThresholding.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonThresholding.Name = "radioButtonThresholding";
            this.radioButtonThresholding.Size = new System.Drawing.Size(86, 17);
            this.radioButtonThresholding.TabIndex = 11;
            this.radioButtonThresholding.TabStop = true;
            this.radioButtonThresholding.Text = "Thresholding";
            this.radioButtonThresholding.UseVisualStyleBackColor = true;
            this.radioButtonThresholding.CheckedChanged += new System.EventHandler(this.radioButtonThresholding_CheckedChanged);
            // 
            // labelTriggeringMode
            // 
            this.labelTriggeringMode.AutoSize = true;
            this.labelTriggeringMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTriggeringMode.Location = new System.Drawing.Point(3, 2);
            this.labelTriggeringMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTriggeringMode.Name = "labelTriggeringMode";
            this.labelTriggeringMode.Size = new System.Drawing.Size(99, 13);
            this.labelTriggeringMode.TabIndex = 9;
            this.labelTriggeringMode.Text = "Triggering Mode";
            // 
            // radioButtonPeakDetectionAlgo
            // 
            this.radioButtonPeakDetectionAlgo.AutoSize = true;
            this.radioButtonPeakDetectionAlgo.Location = new System.Drawing.Point(7, 20);
            this.radioButtonPeakDetectionAlgo.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonPeakDetectionAlgo.Name = "radioButtonPeakDetectionAlgo";
            this.radioButtonPeakDetectionAlgo.Size = new System.Drawing.Size(145, 17);
            this.radioButtonPeakDetectionAlgo.TabIndex = 10;
            this.radioButtonPeakDetectionAlgo.TabStop = true;
            this.radioButtonPeakDetectionAlgo.Text = "Peak Detection Algorithm";
            this.radioButtonPeakDetectionAlgo.UseVisualStyleBackColor = true;
            this.radioButtonPeakDetectionAlgo.CheckedChanged += new System.EventHandler(this.radioButtonPeakDetectionAlgo_CheckedChanged);
            // 
            // labelArduinoControl
            // 
            this.labelArduinoControl.AutoSize = true;
            this.labelArduinoControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelArduinoControl.Location = new System.Drawing.Point(2, 2);
            this.labelArduinoControl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelArduinoControl.Name = "labelArduinoControl";
            this.labelArduinoControl.Size = new System.Drawing.Size(167, 17);
            this.labelArduinoControl.TabIndex = 8;
            this.labelArduinoControl.Text = "Arduino Control Panel";
            // 
            // panelPDAlgoSettings
            // 
            this.panelPDAlgoSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelPDAlgoSettings.Controls.Add(this.panelPeakAlgoLabelGestureEnd);
            this.panelPDAlgoSettings.Controls.Add(this.textBoxPeakAlgoGestureEndThreshold);
            this.panelPDAlgoSettings.Controls.Add(this.panelPeakAlgoLabelGestureStart);
            this.panelPDAlgoSettings.Controls.Add(this.labelPeakDetectionAlgoThresholds);
            this.panelPDAlgoSettings.Controls.Add(this.textBoxPeakAlgoGestureStartThreshold);
            this.panelPDAlgoSettings.Controls.Add(this.labelPeakDetectionAlgoSettings);
            this.panelPDAlgoSettings.Location = new System.Drawing.Point(2, 94);
            this.panelPDAlgoSettings.Margin = new System.Windows.Forms.Padding(2);
            this.panelPDAlgoSettings.Name = "panelPDAlgoSettings";
            this.panelPDAlgoSettings.Size = new System.Drawing.Size(294, 52);
            this.panelPDAlgoSettings.TabIndex = 0;
            // 
            // panelPeakAlgoLabelGestureEnd
            // 
            this.panelPeakAlgoLabelGestureEnd.Controls.Add(this.labelPeakAlgoGestureEndThreshold2);
            this.panelPeakAlgoLabelGestureEnd.Controls.Add(this.labelPeakAlgoGestureEndThreshold1);
            this.panelPeakAlgoLabelGestureEnd.Location = new System.Drawing.Point(200, 17);
            this.panelPeakAlgoLabelGestureEnd.Margin = new System.Windows.Forms.Padding(2);
            this.panelPeakAlgoLabelGestureEnd.Name = "panelPeakAlgoLabelGestureEnd";
            this.panelPeakAlgoLabelGestureEnd.Size = new System.Drawing.Size(42, 26);
            this.panelPeakAlgoLabelGestureEnd.TabIndex = 31;
            // 
            // labelPeakAlgoGestureEndThreshold2
            // 
            this.labelPeakAlgoGestureEndThreshold2.AutoSize = true;
            this.labelPeakAlgoGestureEndThreshold2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeakAlgoGestureEndThreshold2.Location = new System.Drawing.Point(14, 10);
            this.labelPeakAlgoGestureEndThreshold2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPeakAlgoGestureEndThreshold2.Name = "labelPeakAlgoGestureEndThreshold2";
            this.labelPeakAlgoGestureEndThreshold2.Size = new System.Drawing.Size(20, 9);
            this.labelPeakAlgoGestureEndThreshold2.TabIndex = 30;
            this.labelPeakAlgoGestureEndThreshold2.Text = "end";
            // 
            // labelPeakAlgoGestureEndThreshold1
            // 
            this.labelPeakAlgoGestureEndThreshold1.AutoSize = true;
            this.labelPeakAlgoGestureEndThreshold1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeakAlgoGestureEndThreshold1.Location = new System.Drawing.Point(2, 2);
            this.labelPeakAlgoGestureEndThreshold1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPeakAlgoGestureEndThreshold1.Name = "labelPeakAlgoGestureEndThreshold1";
            this.labelPeakAlgoGestureEndThreshold1.Size = new System.Drawing.Size(37, 9);
            this.labelPeakAlgoGestureEndThreshold1.TabIndex = 29;
            this.labelPeakAlgoGestureEndThreshold1.Text = "gesture";
            // 
            // textBoxPeakAlgoGestureEndThreshold
            // 
            this.textBoxPeakAlgoGestureEndThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPeakAlgoGestureEndThreshold.Location = new System.Drawing.Point(245, 20);
            this.textBoxPeakAlgoGestureEndThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPeakAlgoGestureEndThreshold.Name = "textBoxPeakAlgoGestureEndThreshold";
            this.textBoxPeakAlgoGestureEndThreshold.Size = new System.Drawing.Size(42, 19);
            this.textBoxPeakAlgoGestureEndThreshold.TabIndex = 20;
            this.textBoxPeakAlgoGestureEndThreshold.Enter += new System.EventHandler(this.textBoxPeakAlgoGestureEndThreshold_Enter);
            // 
            // panelPeakAlgoLabelGestureStart
            // 
            this.panelPeakAlgoLabelGestureStart.Controls.Add(this.labelPeakAlgoGestureStart2);
            this.panelPeakAlgoLabelGestureStart.Controls.Add(this.labelPeakAlgoGestureStartThreshold1);
            this.panelPeakAlgoLabelGestureStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelPeakAlgoLabelGestureStart.Location = new System.Drawing.Point(101, 17);
            this.panelPeakAlgoLabelGestureStart.Margin = new System.Windows.Forms.Padding(2);
            this.panelPeakAlgoLabelGestureStart.Name = "panelPeakAlgoLabelGestureStart";
            this.panelPeakAlgoLabelGestureStart.Size = new System.Drawing.Size(42, 26);
            this.panelPeakAlgoLabelGestureStart.TabIndex = 29;
            // 
            // labelPeakAlgoGestureStart2
            // 
            this.labelPeakAlgoGestureStart2.AutoSize = true;
            this.labelPeakAlgoGestureStart2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeakAlgoGestureStart2.Location = new System.Drawing.Point(9, 11);
            this.labelPeakAlgoGestureStart2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPeakAlgoGestureStart2.Name = "labelPeakAlgoGestureStart2";
            this.labelPeakAlgoGestureStart2.Size = new System.Drawing.Size(25, 9);
            this.labelPeakAlgoGestureStart2.TabIndex = 30;
            this.labelPeakAlgoGestureStart2.Text = "start";
            // 
            // labelPeakAlgoGestureStartThreshold1
            // 
            this.labelPeakAlgoGestureStartThreshold1.AutoSize = true;
            this.labelPeakAlgoGestureStartThreshold1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeakAlgoGestureStartThreshold1.Location = new System.Drawing.Point(1, 3);
            this.labelPeakAlgoGestureStartThreshold1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPeakAlgoGestureStartThreshold1.Name = "labelPeakAlgoGestureStartThreshold1";
            this.labelPeakAlgoGestureStartThreshold1.Size = new System.Drawing.Size(37, 9);
            this.labelPeakAlgoGestureStartThreshold1.TabIndex = 29;
            this.labelPeakAlgoGestureStartThreshold1.Text = "gesture";
            // 
            // labelPeakDetectionAlgoThresholds
            // 
            this.labelPeakDetectionAlgoThresholds.AutoSize = true;
            this.labelPeakDetectionAlgoThresholds.Location = new System.Drawing.Point(3, 22);
            this.labelPeakDetectionAlgoThresholds.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPeakDetectionAlgoThresholds.Name = "labelPeakDetectionAlgoThresholds";
            this.labelPeakDetectionAlgoThresholds.Size = new System.Drawing.Size(99, 13);
            this.labelPeakDetectionAlgoThresholds.TabIndex = 19;
            this.labelPeakDetectionAlgoThresholds.Text = "Current Thresholds:";
            // 
            // textBoxPeakAlgoGestureStartThreshold
            // 
            this.textBoxPeakAlgoGestureStartThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPeakAlgoGestureStartThreshold.Location = new System.Drawing.Point(148, 20);
            this.textBoxPeakAlgoGestureStartThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPeakAlgoGestureStartThreshold.Name = "textBoxPeakAlgoGestureStartThreshold";
            this.textBoxPeakAlgoGestureStartThreshold.Size = new System.Drawing.Size(45, 19);
            this.textBoxPeakAlgoGestureStartThreshold.TabIndex = 18;
            this.textBoxPeakAlgoGestureStartThreshold.Enter += new System.EventHandler(this.textBoxPeakAlgoGestureStartThreshold_Enter);
            // 
            // labelPeakDetectionAlgoSettings
            // 
            this.labelPeakDetectionAlgoSettings.AutoSize = true;
            this.labelPeakDetectionAlgoSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeakDetectionAlgoSettings.Location = new System.Drawing.Point(2, 0);
            this.labelPeakDetectionAlgoSettings.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPeakDetectionAlgoSettings.Name = "labelPeakDetectionAlgoSettings";
            this.labelPeakDetectionAlgoSettings.Size = new System.Drawing.Size(201, 13);
            this.labelPeakDetectionAlgoSettings.TabIndex = 1;
            this.labelPeakDetectionAlgoSettings.Text = "Peak Detection Algorithm Settings";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 580);
            this.Controls.Add(this.panelArduino);
            this.Controls.Add(this.labelStatusMessage);
            this.Controls.Add(this.groupBoxCOMPort);
            this.Controls.Add(this.richTextBoxMessage);
            this.Controls.Add(this.textBoxMaxChartPoints);
            this.Controls.Add(this.labelMaxChartPoints);
            this.Controls.Add(this.buttonChartStart);
            this.Controls.Add(this.chartSensor);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "ACAT_Arduino_Windows_App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxCOMPort.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSensor)).EndInit();
            this.panelArduino.ResumeLayout(false);
            this.panelArduinoControl.ResumeLayout(false);
            this.panelArduinoControl.PerformLayout();
            this.panelPeakAlgoFeedback.ResumeLayout(false);
            this.panelPeakAlgoFeedback.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelThresholdingThreshold.ResumeLayout(false);
            this.panelThresholdingThreshold.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panelTriggeringMode.ResumeLayout(false);
            this.panelTriggeringMode.PerformLayout();
            this.panelPDAlgoSettings.ResumeLayout(false);
            this.panelPDAlgoSettings.PerformLayout();
            this.panelPeakAlgoLabelGestureEnd.ResumeLayout(false);
            this.panelPeakAlgoLabelGestureEnd.PerformLayout();
            this.panelPeakAlgoLabelGestureStart.ResumeLayout(false);
            this.panelPeakAlgoLabelGestureStart.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxCOMPort;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.Label labelStatusMessage;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSensor;
        private System.Windows.Forms.Button buttonChartStart;
        private System.Windows.Forms.TextBox textBoxMaxChartPoints;
        private System.Windows.Forms.Label labelMaxChartPoints;
        private System.Windows.Forms.Panel panelArduino;
        private System.Windows.Forms.Panel panelArduinoControl;
        private System.Windows.Forms.Label labelArduinoControl;
        private System.Windows.Forms.Button buttonArduinoSet;
        private System.Windows.Forms.RadioButton radioButtonThresholding;
        private System.Windows.Forms.RadioButton radioButtonPeakDetectionAlgo;
        private System.Windows.Forms.Label labelTriggeringMode;
        private System.Windows.Forms.Panel panelTriggeringMode;
        private System.Windows.Forms.CheckBox checkBoxActuateACAT;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelPDAlgoSettings;
        private System.Windows.Forms.Panel panelPeakAlgoLabelGestureEnd;
        private System.Windows.Forms.Label labelPeakAlgoGestureEndThreshold2;
        private System.Windows.Forms.Label labelPeakAlgoGestureEndThreshold1;
        private System.Windows.Forms.TextBox textBoxPeakAlgoGestureEndThreshold;
        private System.Windows.Forms.Panel panelPeakAlgoLabelGestureStart;
        private System.Windows.Forms.Label labelPeakAlgoGestureStart2;
        private System.Windows.Forms.Label labelPeakAlgoGestureStartThreshold1;
        private System.Windows.Forms.Label labelPeakDetectionAlgoThresholds;
        private System.Windows.Forms.TextBox textBoxPeakAlgoGestureStartThreshold;
        private System.Windows.Forms.Label labelPeakDetectionAlgoSettings;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelThresholdingThreshold;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelThresholdingThreshold;
        private System.Windows.Forms.TextBox textBoxThresholdingThreshold;
        private System.Windows.Forms.Label labelThresholdingSettings;
        private System.Windows.Forms.Button buttonSetDefaults;
        private System.Windows.Forms.Label labelComPort;
        private System.Windows.Forms.Panel panelPeakAlgoFeedback;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox textBoxRecommendedPositionMax;
        private System.Windows.Forms.TextBox textBoxRecommendedPositionMin;
        private System.Windows.Forms.Label labelRecommendedSensorPosition1;
        private System.Windows.Forms.Label labelRecommendedSensorPosition2;
        private System.Windows.Forms.Label labelFeedbackWindowSettings;
    }
}

