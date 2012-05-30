namespace VideoPanelControl
{
    partial class TrainingVideoPanel
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TrainingVideoPlayPanel = new System.Windows.Forms.Panel();
            this.SubFrame = new System.Windows.Forms.TextBox();
            this.RepeatCommand = new System.Windows.Forms.Button();
            this.scoringCommand = new System.Windows.Forms.Button();
            this.SlowMoCommand = new System.Windows.Forms.Button();
            this.nextPlayName = new System.Windows.Forms.TextBox();
            this.previousPlayName = new System.Windows.Forms.TextBox();
            this.scoreFeedback = new System.Windows.Forms.TextBox();
            this.nextVideoCommand = new System.Windows.Forms.Button();
            this.currentPlayName = new System.Windows.Forms.TextBox();
            this.PreviousFrameCommand = new System.Windows.Forms.Button();
            this.TrainingVideoPlayPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrainingVideoPlayPanel
            // 
            this.TrainingVideoPlayPanel.BackgroundImage = global::VideoPanelControl.Properties.Resources.TapRapperLogo;
            this.TrainingVideoPlayPanel.Controls.Add(this.SubFrame);
            this.TrainingVideoPlayPanel.Controls.Add(this.RepeatCommand);
            this.TrainingVideoPlayPanel.Controls.Add(this.scoringCommand);
            this.TrainingVideoPlayPanel.Controls.Add(this.SlowMoCommand);
            this.TrainingVideoPlayPanel.Controls.Add(this.nextPlayName);
            this.TrainingVideoPlayPanel.Controls.Add(this.previousPlayName);
            this.TrainingVideoPlayPanel.Controls.Add(this.scoreFeedback);
            this.TrainingVideoPlayPanel.Controls.Add(this.nextVideoCommand);
            this.TrainingVideoPlayPanel.Controls.Add(this.currentPlayName);
            this.TrainingVideoPlayPanel.Controls.Add(this.PreviousFrameCommand);
            this.TrainingVideoPlayPanel.Location = new System.Drawing.Point(0, 3);
            this.TrainingVideoPlayPanel.Name = "TrainingVideoPlayPanel";
            this.TrainingVideoPlayPanel.Size = new System.Drawing.Size(1207, 747);
            this.TrainingVideoPlayPanel.TabIndex = 0;
            // 
            // SubFrame
            // 
            this.SubFrame.BackColor = System.Drawing.Color.Black;
            this.SubFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubFrame.ForeColor = System.Drawing.Color.Red;
            this.SubFrame.Location = new System.Drawing.Point(405, 272);
            this.SubFrame.Multiline = true;
            this.SubFrame.Name = "SubFrame";
            this.SubFrame.Size = new System.Drawing.Size(419, 93);
            this.SubFrame.TabIndex = 12;
            // 
            // RepeatCommand
            // 
            this.RepeatCommand.BackColor = System.Drawing.Color.Gray;
            this.RepeatCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RepeatCommand.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.RepeatCommand.FlatAppearance.BorderSize = 5;
            this.RepeatCommand.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.RepeatCommand.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.RepeatCommand.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RepeatCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RepeatCommand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RepeatCommand.Image = global::VideoPanelControl.Properties.Resources.PlayFore;
            this.RepeatCommand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RepeatCommand.Location = new System.Drawing.Point(260, 407);
            this.RepeatCommand.Name = "RepeatCommand";
            this.RepeatCommand.Size = new System.Drawing.Size(204, 93);
            this.RepeatCommand.TabIndex = 0;
            this.RepeatCommand.Text = "Play  ";
            this.RepeatCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RepeatCommand.UseVisualStyleBackColor = false;
            this.RepeatCommand.Click += new System.EventHandler(this.RepeatCommand_Click);
            // 
            // scoringCommand
            // 
            this.scoringCommand.BackColor = System.Drawing.Color.Gray;
            this.scoringCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scoringCommand.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.scoringCommand.FlatAppearance.BorderSize = 5;
            this.scoringCommand.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.scoringCommand.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.scoringCommand.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.scoringCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoringCommand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.scoringCommand.Image = global::VideoPanelControl.Properties.Resources.ScoringFore;
            this.scoringCommand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.scoringCommand.Location = new System.Drawing.Point(845, 408);
            this.scoringCommand.Name = "scoringCommand";
            this.scoringCommand.Size = new System.Drawing.Size(204, 92);
            this.scoringCommand.TabIndex = 7;
            this.scoringCommand.Text = "Scoring   ";
            this.scoringCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.scoringCommand.UseVisualStyleBackColor = false;
            this.scoringCommand.Click += new System.EventHandler(this.scoringCommand_Click);
            // 
            // SlowMoCommand
            // 
            this.SlowMoCommand.BackColor = System.Drawing.Color.Gray;
            this.SlowMoCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SlowMoCommand.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.SlowMoCommand.FlatAppearance.BorderSize = 5;
            this.SlowMoCommand.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.SlowMoCommand.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.SlowMoCommand.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SlowMoCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SlowMoCommand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SlowMoCommand.Image = global::VideoPanelControl.Properties.Resources.SlowMotionFore1;
            this.SlowMoCommand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SlowMoCommand.Location = new System.Drawing.Point(547, 407);
            this.SlowMoCommand.Name = "SlowMoCommand";
            this.SlowMoCommand.Size = new System.Drawing.Size(204, 93);
            this.SlowMoCommand.TabIndex = 2;
            this.SlowMoCommand.Text = "Slow Mo";
            this.SlowMoCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SlowMoCommand.UseVisualStyleBackColor = false;
            this.SlowMoCommand.Click += new System.EventHandler(this.SlowMoCommand_Click);
            // 
            // nextPlayName
            // 
            this.nextPlayName.BackColor = System.Drawing.Color.Black;
            this.nextPlayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextPlayName.ForeColor = System.Drawing.Color.Red;
            this.nextPlayName.Location = new System.Drawing.Point(934, 144);
            this.nextPlayName.Multiline = true;
            this.nextPlayName.Name = "nextPlayName";
            this.nextPlayName.Size = new System.Drawing.Size(219, 111);
            this.nextPlayName.TabIndex = 11;
            this.nextPlayName.Text = "Next";
            this.nextPlayName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // previousPlayName
            // 
            this.previousPlayName.BackColor = System.Drawing.Color.Black;
            this.previousPlayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previousPlayName.ForeColor = System.Drawing.Color.Red;
            this.previousPlayName.Location = new System.Drawing.Point(45, 144);
            this.previousPlayName.Multiline = true;
            this.previousPlayName.Name = "previousPlayName";
            this.previousPlayName.Size = new System.Drawing.Size(204, 121);
            this.previousPlayName.TabIndex = 10;
            this.previousPlayName.Text = "Previous";
            this.previousPlayName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // scoreFeedback
            // 
            this.scoreFeedback.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.scoreFeedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreFeedback.ForeColor = System.Drawing.Color.Yellow;
            this.scoreFeedback.Location = new System.Drawing.Point(289, 528);
            this.scoreFeedback.Multiline = true;
            this.scoreFeedback.Name = "scoreFeedback";
            this.scoreFeedback.Size = new System.Drawing.Size(628, 177);
            this.scoreFeedback.TabIndex = 6;
            this.scoreFeedback.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nextVideoCommand
            // 
            this.nextVideoCommand.BackColor = System.Drawing.Color.Gray;
            this.nextVideoCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextVideoCommand.Image = global::VideoPanelControl.Properties.Resources.SkipFore;
            this.nextVideoCommand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.nextVideoCommand.Location = new System.Drawing.Point(934, 272);
            this.nextVideoCommand.Name = "nextVideoCommand";
            this.nextVideoCommand.Size = new System.Drawing.Size(207, 92);
            this.nextVideoCommand.TabIndex = 5;
            this.nextVideoCommand.Text = "Skip  ";
            this.nextVideoCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nextVideoCommand.UseVisualStyleBackColor = false;
            this.nextVideoCommand.Click += new System.EventHandler(this.nextVideoCommand_Click);
            // 
            // currentPlayName
            // 
            this.currentPlayName.BackColor = System.Drawing.Color.Black;
            this.currentPlayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentPlayName.ForeColor = System.Drawing.Color.Red;
            this.currentPlayName.Location = new System.Drawing.Point(405, 16);
            this.currentPlayName.Multiline = true;
            this.currentPlayName.Name = "currentPlayName";
            this.currentPlayName.Size = new System.Drawing.Size(419, 239);
            this.currentPlayName.TabIndex = 4;
            this.currentPlayName.Text = "Test";
            this.currentPlayName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PreviousFrameCommand
            // 
            this.PreviousFrameCommand.BackColor = System.Drawing.Color.Gray;
            this.PreviousFrameCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PreviousFrameCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousFrameCommand.Image = global::VideoPanelControl.Properties.Resources.BackFore;
            this.PreviousFrameCommand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PreviousFrameCommand.Location = new System.Drawing.Point(45, 272);
            this.PreviousFrameCommand.Name = "PreviousFrameCommand";
            this.PreviousFrameCommand.Size = new System.Drawing.Size(204, 93);
            this.PreviousFrameCommand.TabIndex = 3;
            this.PreviousFrameCommand.Text = "Back  ";
            this.PreviousFrameCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PreviousFrameCommand.UseVisualStyleBackColor = false;
            this.PreviousFrameCommand.Click += new System.EventHandler(this.PreviousFrameCommand_Click);
            // 
            // TrainingVideoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TrainingVideoPlayPanel);
            this.Name = "TrainingVideoPanel";
            this.Size = new System.Drawing.Size(1210, 711);
            this.TrainingVideoPlayPanel.ResumeLayout(false);
            this.TrainingVideoPlayPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TrainingVideoPlayPanel;
        private System.Windows.Forms.Button SlowMoCommand;
        private System.Windows.Forms.Button RepeatCommand;
        private System.Windows.Forms.Button PreviousFrameCommand;
        private System.Windows.Forms.TextBox currentPlayName;
        private System.Windows.Forms.Button nextVideoCommand;
        private System.Windows.Forms.TextBox scoreFeedback;
        private System.Windows.Forms.TextBox nextPlayName;
        private System.Windows.Forms.TextBox previousPlayName;
        private System.Windows.Forms.Button scoringCommand;
        private System.Windows.Forms.TextBox SubFrame;
    }
}
