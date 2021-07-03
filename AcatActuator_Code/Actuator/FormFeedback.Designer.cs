///////////////////////////////////////////////////////////////////////////
// <copyright file="FormFeedback.Designer.cs" company="Intel Corporation">
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
    partial class FormFeedback
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
            this.labelCurrentThresholdValue = new System.Windows.Forms.Label();
            this.labelCurrentSensorPositionStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSensorBaselineValue = new System.Windows.Forms.Label();
            this.labelLowerSensorPositionValue = new System.Windows.Forms.Label();
            this.labelUpperSensorPositionValue = new System.Windows.Forms.Label();
            this.labelFeedbackSuggestedThreshold = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // labelCurrentThresholdValue
            // 
            this.labelCurrentThresholdValue.AutoSize = true;
            this.labelCurrentThresholdValue.BackColor = System.Drawing.SystemColors.HighlightText;
            this.labelCurrentThresholdValue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelCurrentThresholdValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentThresholdValue.ForeColor = System.Drawing.Color.Blue;
            this.labelCurrentThresholdValue.Location = new System.Drawing.Point(84, 63);
            this.labelCurrentThresholdValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCurrentThresholdValue.Name = "labelCurrentThresholdValue";
            this.labelCurrentThresholdValue.Size = new System.Drawing.Size(17, 18);
            this.labelCurrentThresholdValue.TabIndex = 0;
            this.labelCurrentThresholdValue.Text = "?";
            this.labelCurrentThresholdValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCurrentSensorPositionStatus
            // 
            this.labelCurrentSensorPositionStatus.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.labelCurrentSensorPositionStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCurrentSensorPositionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentSensorPositionStatus.Location = new System.Drawing.Point(148, 3);
            this.labelCurrentSensorPositionStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCurrentSensorPositionStatus.Name = "labelCurrentSensorPositionStatus";
            this.labelCurrentSensorPositionStatus.Size = new System.Drawing.Size(284, 24);
            this.labelCurrentSensorPositionStatus.TabIndex = 1;
            this.labelCurrentSensorPositionStatus.Text = "Not Detected";
            this.labelCurrentSensorPositionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sensor Position";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Threshold:";
            // 
            // labelSensorBaselineValue
            // 
            this.labelSensorBaselineValue.AutoSize = true;
            this.labelSensorBaselineValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSensorBaselineValue.ForeColor = System.Drawing.Color.Blue;
            this.labelSensorBaselineValue.Location = new System.Drawing.Point(108, 33);
            this.labelSensorBaselineValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSensorBaselineValue.Name = "labelSensorBaselineValue";
            this.labelSensorBaselineValue.Size = new System.Drawing.Size(17, 18);
            this.labelSensorBaselineValue.TabIndex = 4;
            this.labelSensorBaselineValue.Text = "?";
            // 
            // labelLowerSensorPositionValue
            // 
            this.labelLowerSensorPositionValue.AutoSize = true;
            this.labelLowerSensorPositionValue.BackColor = System.Drawing.SystemColors.Window;
            this.labelLowerSensorPositionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLowerSensorPositionValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelLowerSensorPositionValue.Location = new System.Drawing.Point(299, 32);
            this.labelLowerSensorPositionValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLowerSensorPositionValue.Name = "labelLowerSensorPositionValue";
            this.labelLowerSensorPositionValue.Size = new System.Drawing.Size(17, 18);
            this.labelLowerSensorPositionValue.TabIndex = 5;
            this.labelLowerSensorPositionValue.Text = "?";
            this.labelLowerSensorPositionValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelUpperSensorPositionValue
            // 
            this.labelUpperSensorPositionValue.AutoSize = true;
            this.labelUpperSensorPositionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpperSensorPositionValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelUpperSensorPositionValue.Location = new System.Drawing.Point(349, 32);
            this.labelUpperSensorPositionValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUpperSensorPositionValue.Name = "labelUpperSensorPositionValue";
            this.labelUpperSensorPositionValue.Size = new System.Drawing.Size(17, 18);
            this.labelUpperSensorPositionValue.TabIndex = 6;
            this.labelUpperSensorPositionValue.Text = "?";
            this.labelUpperSensorPositionValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFeedbackSuggestedThreshold
            // 
            this.labelFeedbackSuggestedThreshold.AutoSize = true;
            this.labelFeedbackSuggestedThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFeedbackSuggestedThreshold.ForeColor = System.Drawing.Color.Green;
            this.labelFeedbackSuggestedThreshold.Location = new System.Drawing.Point(197, 62);
            this.labelFeedbackSuggestedThreshold.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFeedbackSuggestedThreshold.Name = "labelFeedbackSuggestedThreshold";
            this.labelFeedbackSuggestedThreshold.Size = new System.Drawing.Size(17, 24);
            this.labelFeedbackSuggestedThreshold.TabIndex = 7;
            this.labelFeedbackSuggestedThreshold.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(152, 34);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Recommended Range:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(336, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 34);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Current Value:";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(13, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 2);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // FormFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(437, 90);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelFeedbackSuggestedThreshold);
            this.Controls.Add(this.labelUpperSensorPositionValue);
            this.Controls.Add(this.labelLowerSensorPositionValue);
            this.Controls.Add(this.labelSensorBaselineValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelCurrentSensorPositionStatus);
            this.Controls.Add(this.labelCurrentThresholdValue);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormFeedback";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFeedback_FormClosing_1);
            this.Load += new System.EventHandler(this.FormFeedback_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCurrentThresholdValue;
        private System.Windows.Forms.Label labelCurrentSensorPositionStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSensorBaselineValue;
        private System.Windows.Forms.Label labelLowerSensorPositionValue;
        private System.Windows.Forms.Label labelUpperSensorPositionValue;
        private System.Windows.Forms.Label labelFeedbackSuggestedThreshold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}