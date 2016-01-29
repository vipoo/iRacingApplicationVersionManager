namespace iRacingApplicationVersionManger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.versionSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.downloadButton = new System.Windows.Forms.Button();
            this.progressPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.isRunningWarningPanel = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.alreadyRuninngWarning = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.currentVersion = new System.Windows.Forms.TextBox();
            this.openApplication = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.installButton = new System.Windows.Forms.Button();
            this.progressPanel.SuspendLayout();
            this.isRunningWarningPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // versionSelector
            // 
            this.versionSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionSelector.Enabled = false;
            this.versionSelector.FormattingEnabled = true;
            this.versionSelector.Items.AddRange(new object[] {
            "1.0.0.1 - Today",
            "1.0.0.10 - Yesterday",
            "1.0.2.2- Whatever"});
            this.versionSelector.Location = new System.Drawing.Point(15, 215);
            this.versionSelector.Name = "versionSelector";
            this.versionSelector.Size = new System.Drawing.Size(366, 32);
            this.versionSelector.TabIndex = 3;
            this.versionSelector.SelectedIndexChanged += new System.EventHandler(this.versionSelector_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select a version to install";
            // 
            // downloadButton
            // 
            this.downloadButton.Enabled = false;
            this.downloadButton.Location = new System.Drawing.Point(387, 214);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(160, 33);
            this.downloadButton.TabIndex = 5;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressPanel
            // 
            this.progressPanel.Controls.Add(this.label2);
            this.progressPanel.Controls.Add(this.progressBar1);
            this.progressPanel.Location = new System.Drawing.Point(15, 254);
            this.progressPanel.Name = "progressPanel";
            this.progressPanel.Size = new System.Drawing.Size(620, 94);
            this.progressPanel.TabIndex = 8;
            this.progressPanel.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "Downloading:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 42);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(620, 27);
            this.progressBar1.TabIndex = 7;
            // 
            // isRunningWarningPanel
            // 
            this.isRunningWarningPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.isRunningWarningPanel.Controls.Add(this.button2);
            this.isRunningWarningPanel.Controls.Add(this.alreadyRuninngWarning);
            this.isRunningWarningPanel.Location = new System.Drawing.Point(86, 321);
            this.isRunningWarningPanel.Name = "isRunningWarningPanel";
            this.isRunningWarningPanel.Size = new System.Drawing.Size(505, 152);
            this.isRunningWarningPanel.TabIndex = 11;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(66, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(378, 51);
            this.button2.TabIndex = 11;
            this.button2.Text = "Close iRacing Replay Director Application";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // alreadyRuninngWarning
            // 
            this.alreadyRuninngWarning.ForeColor = System.Drawing.Color.DarkRed;
            this.alreadyRuninngWarning.Location = new System.Drawing.Point(6, 4);
            this.alreadyRuninngWarning.Name = "alreadyRuninngWarning";
            this.alreadyRuninngWarning.Size = new System.Drawing.Size(490, 78);
            this.alreadyRuninngWarning.TabIndex = 10;
            this.alreadyRuninngWarning.Text = "\"iRacing Replay Director\" is currently running.  You need to close it, before att" +
    "empting to install a different version.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "Current Version:";
            // 
            // currentVersion
            // 
            this.currentVersion.Location = new System.Drawing.Point(166, 133);
            this.currentVersion.Name = "currentVersion";
            this.currentVersion.ReadOnly = true;
            this.currentVersion.Size = new System.Drawing.Size(178, 29);
            this.currentVersion.TabIndex = 13;
            // 
            // openApplication
            // 
            this.openApplication.Enabled = false;
            this.openApplication.Location = new System.Drawing.Point(359, 132);
            this.openApplication.Name = "openApplication";
            this.openApplication.Size = new System.Drawing.Size(94, 33);
            this.openApplication.TabIndex = 14;
            this.openApplication.Text = "Open";
            this.openApplication.UseVisualStyleBackColor = true;
            this.openApplication.Click += new System.EventHandler(this.openApplication_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(164, 24);
            this.label4.TabIndex = 15;
            this.label4.Text = "Application Name:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(181, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(335, 29);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "iRacing Replay Director";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(522, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(113, 96);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Location = new System.Drawing.Point(12, 117);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(623, 10);
            this.panel1.TabIndex = 18;
            // 
            // installButton
            // 
            this.installButton.Enabled = false;
            this.installButton.Location = new System.Drawing.Point(553, 214);
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(83, 33);
            this.installButton.TabIndex = 19;
            this.installButton.Text = "Install";
            this.installButton.UseVisualStyleBackColor = true;
            this.installButton.Click += new System.EventHandler(this.installButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 343);
            this.Controls.Add(this.installButton);
            this.Controls.Add(this.isRunningWarningPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.openApplication);
            this.Controls.Add(this.currentVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressPanel);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.versionSelector);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Application Version Manager";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.progressPanel.ResumeLayout(false);
            this.progressPanel.PerformLayout();
            this.isRunningWarningPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox versionSelector;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel progressPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel isRunningWarningPanel;
        private System.Windows.Forms.Label alreadyRuninngWarning;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox currentVersion;
        private System.Windows.Forms.Button openApplication;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button installButton;
    }
}

