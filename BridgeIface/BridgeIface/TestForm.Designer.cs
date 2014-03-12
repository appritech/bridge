namespace BridgeIface
{
    partial class TestForm
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
            this.components = new System.ComponentModel.Container();
            this.NMEA_String = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sentenceTypeLabel = new System.Windows.Forms.Label();
            this.sentenceTypeDisplay = new System.Windows.Forms.TextBox();
            this.parse_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lastStringEntered = new System.Windows.Forms.TextBox();
            this.label_bowThruster = new System.Windows.Forms.Label();
            this.rpmDemandLabel = new System.Windows.Forms.Label();
            this.rpmDemandDisplay1 = new System.Windows.Forms.TextBox();
            this.pitchDemandDisplay1 = new System.Windows.Forms.TextBox();
            this.pitchDemandLabel = new System.Windows.Forms.Label();
            this.azimuthDemandDisplay1 = new System.Windows.Forms.TextBox();
            this.azimuthDemandLabel = new System.Windows.Forms.Label();
            this.locationDisplay1 = new System.Windows.Forms.TextBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.sentenceStatusDisplay1 = new System.Windows.Forms.TextBox();
            this.sentenceStatusLabel = new System.Windows.Forms.Label();
            this.errorMessage = new System.Windows.Forms.Label();
            this.sentenceStatusDisplay2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.locationDisplay2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.azimuthDemandDisplay2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pitchDemandDisplay2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rpmDemandDisplay2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // NMEA_String
            // 
            this.NMEA_String.Location = new System.Drawing.Point(12, 28);
            this.NMEA_String.Name = "NMEA_String";
            this.NMEA_String.Size = new System.Drawing.Size(360, 20);
            this.NMEA_String.TabIndex = 0;
            this.NMEA_String.Text = "$--TRC,1,9.9,P,5.0,D,0.0,C,R*hh<CR><LF>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Insert NMEA String here";
            // 
            // sentenceTypeLabel
            // 
            this.sentenceTypeLabel.AutoSize = true;
            this.sentenceTypeLabel.Location = new System.Drawing.Point(12, 79);
            this.sentenceTypeLabel.Name = "sentenceTypeLabel";
            this.sentenceTypeLabel.Size = new System.Drawing.Size(83, 13);
            this.sentenceTypeLabel.TabIndex = 4;
            this.sentenceTypeLabel.Text = "Sentence Type:";
            // 
            // sentenceTypeDisplay
            // 
            this.sentenceTypeDisplay.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.sentenceTypeDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sentenceTypeDisplay.Location = new System.Drawing.Point(118, 79);
            this.sentenceTypeDisplay.Name = "sentenceTypeDisplay";
            this.sentenceTypeDisplay.ReadOnly = true;
            this.sentenceTypeDisplay.Size = new System.Drawing.Size(134, 20);
            this.sentenceTypeDisplay.TabIndex = 5;
            // 
            // parse_button
            // 
            this.parse_button.Location = new System.Drawing.Point(378, 26);
            this.parse_button.Name = "parse_button";
            this.parse_button.Size = new System.Drawing.Size(75, 23);
            this.parse_button.TabIndex = 6;
            this.parse_button.Text = "Parse!";
            this.parse_button.UseVisualStyleBackColor = true;
            this.parse_button.Click += new System.EventHandler(this.parse_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Last String Entered:";
            // 
            // lastStringEntered
            // 
            this.lastStringEntered.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lastStringEntered.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lastStringEntered.Location = new System.Drawing.Point(118, 53);
            this.lastStringEntered.Name = "lastStringEntered";
            this.lastStringEntered.ReadOnly = true;
            this.lastStringEntered.Size = new System.Drawing.Size(254, 20);
            this.lastStringEntered.TabIndex = 8;
            // 
            // label_bowThruster
            // 
            this.label_bowThruster.AutoSize = true;
            this.label_bowThruster.Location = new System.Drawing.Point(12, 288);
            this.label_bowThruster.Name = "label_bowThruster";
            this.label_bowThruster.Size = new System.Drawing.Size(70, 13);
            this.label_bowThruster.TabIndex = 9;
            this.label_bowThruster.Text = "Bow Thruster";
            // 
            // rpmDemandLabel
            // 
            this.rpmDemandLabel.AutoSize = true;
            this.rpmDemandLabel.Location = new System.Drawing.Point(12, 315);
            this.rpmDemandLabel.Name = "rpmDemandLabel";
            this.rpmDemandLabel.Size = new System.Drawing.Size(77, 13);
            this.rpmDemandLabel.TabIndex = 11;
            this.rpmDemandLabel.Text = "RMP Demand:";
            // 
            // rpmDemandDisplay1
            // 
            this.rpmDemandDisplay1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.rpmDemandDisplay1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rpmDemandDisplay1.Location = new System.Drawing.Point(105, 308);
            this.rpmDemandDisplay1.Name = "rpmDemandDisplay1";
            this.rpmDemandDisplay1.ReadOnly = true;
            this.rpmDemandDisplay1.Size = new System.Drawing.Size(147, 20);
            this.rpmDemandDisplay1.TabIndex = 12;
            // 
            // pitchDemandDisplay1
            // 
            this.pitchDemandDisplay1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.pitchDemandDisplay1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pitchDemandDisplay1.Location = new System.Drawing.Point(105, 334);
            this.pitchDemandDisplay1.Name = "pitchDemandDisplay1";
            this.pitchDemandDisplay1.ReadOnly = true;
            this.pitchDemandDisplay1.Size = new System.Drawing.Size(147, 20);
            this.pitchDemandDisplay1.TabIndex = 14;
            // 
            // pitchDemandLabel
            // 
            this.pitchDemandLabel.AutoSize = true;
            this.pitchDemandLabel.Location = new System.Drawing.Point(12, 341);
            this.pitchDemandLabel.Name = "pitchDemandLabel";
            this.pitchDemandLabel.Size = new System.Drawing.Size(77, 13);
            this.pitchDemandLabel.TabIndex = 13;
            this.pitchDemandLabel.Text = "Pitch Demand:";
            // 
            // azimuthDemandDisplay1
            // 
            this.azimuthDemandDisplay1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.azimuthDemandDisplay1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.azimuthDemandDisplay1.Location = new System.Drawing.Point(105, 360);
            this.azimuthDemandDisplay1.Name = "azimuthDemandDisplay1";
            this.azimuthDemandDisplay1.ReadOnly = true;
            this.azimuthDemandDisplay1.Size = new System.Drawing.Size(147, 20);
            this.azimuthDemandDisplay1.TabIndex = 16;
            // 
            // azimuthDemandLabel
            // 
            this.azimuthDemandLabel.AutoSize = true;
            this.azimuthDemandLabel.Location = new System.Drawing.Point(12, 367);
            this.azimuthDemandLabel.Name = "azimuthDemandLabel";
            this.azimuthDemandLabel.Size = new System.Drawing.Size(90, 13);
            this.azimuthDemandLabel.TabIndex = 15;
            this.azimuthDemandLabel.Text = "Azimuth Demand:";
            // 
            // locationDisplay1
            // 
            this.locationDisplay1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.locationDisplay1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.locationDisplay1.Location = new System.Drawing.Point(105, 386);
            this.locationDisplay1.Name = "locationDisplay1";
            this.locationDisplay1.ReadOnly = true;
            this.locationDisplay1.Size = new System.Drawing.Size(147, 20);
            this.locationDisplay1.TabIndex = 18;
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(12, 393);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(48, 13);
            this.locationLabel.TabIndex = 17;
            this.locationLabel.Text = "Location";
            // 
            // sentenceStatusDisplay1
            // 
            this.sentenceStatusDisplay1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.sentenceStatusDisplay1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sentenceStatusDisplay1.Location = new System.Drawing.Point(105, 412);
            this.sentenceStatusDisplay1.Name = "sentenceStatusDisplay1";
            this.sentenceStatusDisplay1.ReadOnly = true;
            this.sentenceStatusDisplay1.Size = new System.Drawing.Size(147, 20);
            this.sentenceStatusDisplay1.TabIndex = 20;
            // 
            // sentenceStatusLabel
            // 
            this.sentenceStatusLabel.AutoSize = true;
            this.sentenceStatusLabel.Location = new System.Drawing.Point(12, 419);
            this.sentenceStatusLabel.Name = "sentenceStatusLabel";
            this.sentenceStatusLabel.Size = new System.Drawing.Size(37, 13);
            this.sentenceStatusLabel.TabIndex = 19;
            this.sentenceStatusLabel.Text = "Status";
            // 
            // errorMessage
            // 
            this.errorMessage.AutoSize = true;
            this.errorMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.errorMessage.Location = new System.Drawing.Point(15, 111);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(0, 13);
            this.errorMessage.TabIndex = 21;
            // 
            // sentenceStatusDisplay2
            // 
            this.sentenceStatusDisplay2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.sentenceStatusDisplay2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sentenceStatusDisplay2.Location = new System.Drawing.Point(382, 412);
            this.sentenceStatusDisplay2.Name = "sentenceStatusDisplay2";
            this.sentenceStatusDisplay2.ReadOnly = true;
            this.sentenceStatusDisplay2.Size = new System.Drawing.Size(147, 20);
            this.sentenceStatusDisplay2.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 419);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Status";
            // 
            // locationDisplay2
            // 
            this.locationDisplay2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.locationDisplay2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.locationDisplay2.Location = new System.Drawing.Point(382, 386);
            this.locationDisplay2.Name = "locationDisplay2";
            this.locationDisplay2.ReadOnly = true;
            this.locationDisplay2.Size = new System.Drawing.Size(147, 20);
            this.locationDisplay2.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(289, 393);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Location";
            // 
            // azimuthDemandDisplay2
            // 
            this.azimuthDemandDisplay2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.azimuthDemandDisplay2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.azimuthDemandDisplay2.Location = new System.Drawing.Point(382, 360);
            this.azimuthDemandDisplay2.Name = "azimuthDemandDisplay2";
            this.azimuthDemandDisplay2.ReadOnly = true;
            this.azimuthDemandDisplay2.Size = new System.Drawing.Size(147, 20);
            this.azimuthDemandDisplay2.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(289, 367);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Azimuth Demand:";
            // 
            // pitchDemandDisplay2
            // 
            this.pitchDemandDisplay2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.pitchDemandDisplay2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pitchDemandDisplay2.Location = new System.Drawing.Point(382, 334);
            this.pitchDemandDisplay2.Name = "pitchDemandDisplay2";
            this.pitchDemandDisplay2.ReadOnly = true;
            this.pitchDemandDisplay2.Size = new System.Drawing.Size(147, 20);
            this.pitchDemandDisplay2.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(289, 341);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Pitch Demand:";
            // 
            // rpmDemandDisplay2
            // 
            this.rpmDemandDisplay2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.rpmDemandDisplay2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rpmDemandDisplay2.Location = new System.Drawing.Point(382, 308);
            this.rpmDemandDisplay2.Name = "rpmDemandDisplay2";
            this.rpmDemandDisplay2.ReadOnly = true;
            this.rpmDemandDisplay2.Size = new System.Drawing.Size(147, 20);
            this.rpmDemandDisplay2.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(289, 315);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "RMP Demand:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(289, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Stern Thruster";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TestForm
            // 
            this.AcceptButton = this.parse_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.sentenceStatusDisplay2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.locationDisplay2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.azimuthDemandDisplay2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pitchDemandDisplay2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.rpmDemandDisplay2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.errorMessage);
            this.Controls.Add(this.sentenceStatusDisplay1);
            this.Controls.Add(this.sentenceStatusLabel);
            this.Controls.Add(this.locationDisplay1);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.azimuthDemandDisplay1);
            this.Controls.Add(this.azimuthDemandLabel);
            this.Controls.Add(this.pitchDemandDisplay1);
            this.Controls.Add(this.pitchDemandLabel);
            this.Controls.Add(this.rpmDemandDisplay1);
            this.Controls.Add(this.rpmDemandLabel);
            this.Controls.Add(this.label_bowThruster);
            this.Controls.Add(this.lastStringEntered);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.parse_button);
            this.Controls.Add(this.sentenceTypeDisplay);
            this.Controls.Add(this.sentenceTypeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NMEA_String);
            this.Name = "TestForm";
            this.Text = "Bridge Interface Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NMEA_String;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label sentenceTypeLabel;
        private System.Windows.Forms.TextBox sentenceTypeDisplay;
        private System.Windows.Forms.Button parse_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox lastStringEntered;
        private System.Windows.Forms.Label label_bowThruster;
        private System.Windows.Forms.Label rpmDemandLabel;
        private System.Windows.Forms.TextBox rpmDemandDisplay1;
        private System.Windows.Forms.TextBox pitchDemandDisplay1;
        private System.Windows.Forms.Label pitchDemandLabel;
        private System.Windows.Forms.TextBox azimuthDemandDisplay1;
        private System.Windows.Forms.Label azimuthDemandLabel;
        private System.Windows.Forms.TextBox locationDisplay1;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.TextBox sentenceStatusDisplay1;
        private System.Windows.Forms.Label sentenceStatusLabel;
        private System.Windows.Forms.Label errorMessage;
        private System.Windows.Forms.TextBox sentenceStatusDisplay2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox locationDisplay2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox azimuthDemandDisplay2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox pitchDemandDisplay2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox rpmDemandDisplay2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer1;
    }
}

