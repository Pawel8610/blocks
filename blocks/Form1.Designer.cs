namespace blocks
{
    partial class Notes
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
            this.button1New = new System.Windows.Forms.Button();
            this.button1Find = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.performBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteOldBackupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRestoringPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox1DisableSound = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.inputLicenseKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1New
            // 
            this.button1New.AllowDrop = true;
            this.button1New.Location = new System.Drawing.Point(12, 27);
            this.button1New.Name = "button1New";
            this.button1New.Size = new System.Drawing.Size(75, 23);
            this.button1New.TabIndex = 0;
            this.button1New.Text = "New note";
            this.button1New.UseVisualStyleBackColor = true;
            this.button1New.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1Find
            // 
            this.button1Find.Location = new System.Drawing.Point(1039, 29);
            this.button1Find.Name = "button1Find";
            this.button1Find.Size = new System.Drawing.Size(78, 23);
            this.button1Find.TabIndex = 1;
            this.button1Find.Text = "Refresh";
            this.button1Find.UseVisualStyleBackColor = true;
            this.button1Find.Click += new System.EventHandler(this.button1Find_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.readMeToolStripMenuItem,
            this.aboutMeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1230, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.performBackupToolStripMenuItem,
            this.deleteOldBackupsToolStripMenuItem,
            this.openRestoringPointToolStripMenuItem,
            this.inputLicenseKeyToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // performBackupToolStripMenuItem
            // 
            this.performBackupToolStripMenuItem.Name = "performBackupToolStripMenuItem";
            this.performBackupToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.performBackupToolStripMenuItem.Text = "Perform Backup";
            this.performBackupToolStripMenuItem.Click += new System.EventHandler(this.performBackupToolStripMenuItem_Click);
            // 
            // deleteOldBackupsToolStripMenuItem
            // 
            this.deleteOldBackupsToolStripMenuItem.Name = "deleteOldBackupsToolStripMenuItem";
            this.deleteOldBackupsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.deleteOldBackupsToolStripMenuItem.Text = "Delete old backups";
            this.deleteOldBackupsToolStripMenuItem.Click += new System.EventHandler(this.deleteOldBackupsToolStripMenuItem_Click);
            // 
            // openRestoringPointToolStripMenuItem
            // 
            this.openRestoringPointToolStripMenuItem.Name = "openRestoringPointToolStripMenuItem";
            this.openRestoringPointToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openRestoringPointToolStripMenuItem.Text = "Open restoring point";
            this.openRestoringPointToolStripMenuItem.Click += new System.EventHandler(this.openRestoringPointToolStripMenuItem_Click);
            // 
            // readMeToolStripMenuItem
            // 
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            this.readMeToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.readMeToolStripMenuItem.Text = "Read Me";
            this.readMeToolStripMenuItem.Click += new System.EventHandler(this.readMeToolStripMenuItem_Click);
            // 
            // aboutMeToolStripMenuItem
            // 
            this.aboutMeToolStripMenuItem.Name = "aboutMeToolStripMenuItem";
            this.aboutMeToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.aboutMeToolStripMenuItem.Text = "About Me";
            this.aboutMeToolStripMenuItem.Click += new System.EventHandler(this.aboutMeToolStripMenuItem_Click);
            // 
            // checkBox1DisableSound
            // 
            this.checkBox1DisableSound.AutoSize = true;
            this.checkBox1DisableSound.Location = new System.Drawing.Point(1123, 33);
            this.checkBox1DisableSound.Name = "checkBox1DisableSound";
            this.checkBox1DisableSound.Size = new System.Drawing.Size(95, 17);
            this.checkBox1DisableSound.TabIndex = 5;
            this.checkBox1DisableSound.Text = "Disable Sound";
            this.checkBox1DisableSound.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Location = new System.Drawing.Point(0, 56);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1231, 495);
            this.tabControl1.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(538, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add Tab";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(619, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Remove Tab";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(432, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 8;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(720, 27);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(117, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Change Tab Name";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // inputLicenseKeyToolStripMenuItem
            // 
            this.inputLicenseKeyToolStripMenuItem.Name = "inputLicenseKeyToolStripMenuItem";
            this.inputLicenseKeyToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.inputLicenseKeyToolStripMenuItem.Text = "Input License Key";
            this.inputLicenseKeyToolStripMenuItem.Click += new System.EventHandler(this.inputLicenseKeyToolStripMenuItem_Click);
            // 
            // Notes
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1230, 553);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.checkBox1DisableSound);
            this.Controls.Add(this.button1Find);
            this.Controls.Add(this.button1New);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Notes";
            this.Text = "Notes";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Notes_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Notes_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1New;
        private System.Windows.Forms.Button button1Find;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutMeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readMeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem performBackupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteOldBackupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRestoringPointToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox1DisableSound;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem inputLicenseKeyToolStripMenuItem;
    }
}

