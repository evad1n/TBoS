namespace MapInjector {
    partial class OptionsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.textBox_mapDir = new System.Windows.Forms.TextBox();
            this.label_mapDir = new System.Windows.Forms.Label();
            this.button_changeMapsDir = new System.Windows.Forms.Button();
            this.label_backupDir = new System.Windows.Forms.Label();
            this.textBox_backupDir = new System.Windows.Forms.TextBox();
            this.button_changeBackupDir = new System.Windows.Forms.Button();
            this.label_imageEditor = new System.Windows.Forms.Label();
            this.textBox_imageEditorDir = new System.Windows.Forms.TextBox();
            this.button_changeImageEditor = new System.Windows.Forms.Button();
            this.button_saveOptions = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_mapDir
            // 
            this.textBox_mapDir.Location = new System.Drawing.Point(17, 25);
            this.textBox_mapDir.Name = "textBox_mapDir";
            this.textBox_mapDir.Size = new System.Drawing.Size(195, 20);
            this.textBox_mapDir.TabIndex = 2;
            // 
            // label_mapDir
            // 
            this.label_mapDir.AutoSize = true;
            this.label_mapDir.Location = new System.Drawing.Point(12, 9);
            this.label_mapDir.Name = "label_mapDir";
            this.label_mapDir.Size = new System.Drawing.Size(78, 13);
            this.label_mapDir.TabIndex = 3;
            this.label_mapDir.Text = "Maps Directory";
            // 
            // button_changeMapsDir
            // 
            this.button_changeMapsDir.Location = new System.Drawing.Point(216, 23);
            this.button_changeMapsDir.Name = "button_changeMapsDir";
            this.button_changeMapsDir.Size = new System.Drawing.Size(75, 23);
            this.button_changeMapsDir.TabIndex = 4;
            this.button_changeMapsDir.Text = "Change";
            this.button_changeMapsDir.UseVisualStyleBackColor = true;
            this.button_changeMapsDir.Click += new System.EventHandler(this.button_changeMapsDir_Click);
            // 
            // label_backupDir
            // 
            this.label_backupDir.AutoSize = true;
            this.label_backupDir.Location = new System.Drawing.Point(12, 60);
            this.label_backupDir.Name = "label_backupDir";
            this.label_backupDir.Size = new System.Drawing.Size(89, 13);
            this.label_backupDir.TabIndex = 5;
            this.label_backupDir.Text = "Backup Directory";
            // 
            // textBox_backupDir
            // 
            this.textBox_backupDir.Location = new System.Drawing.Point(15, 76);
            this.textBox_backupDir.Name = "textBox_backupDir";
            this.textBox_backupDir.Size = new System.Drawing.Size(195, 20);
            this.textBox_backupDir.TabIndex = 6;
            // 
            // button_changeBackupDir
            // 
            this.button_changeBackupDir.Location = new System.Drawing.Point(216, 74);
            this.button_changeBackupDir.Name = "button_changeBackupDir";
            this.button_changeBackupDir.Size = new System.Drawing.Size(75, 23);
            this.button_changeBackupDir.TabIndex = 7;
            this.button_changeBackupDir.Text = "Change";
            this.button_changeBackupDir.UseVisualStyleBackColor = true;
            this.button_changeBackupDir.Click += new System.EventHandler(this.button_changeBackupDir_Click);
            // 
            // label_imageEditor
            // 
            this.label_imageEditor.AutoSize = true;
            this.label_imageEditor.Location = new System.Drawing.Point(14, 111);
            this.label_imageEditor.Name = "label_imageEditor";
            this.label_imageEditor.Size = new System.Drawing.Size(66, 13);
            this.label_imageEditor.TabIndex = 8;
            this.label_imageEditor.Text = "Image Editor";
            // 
            // textBox_imageEditorDir
            // 
            this.textBox_imageEditorDir.Location = new System.Drawing.Point(15, 127);
            this.textBox_imageEditorDir.Name = "textBox_imageEditorDir";
            this.textBox_imageEditorDir.Size = new System.Drawing.Size(195, 20);
            this.textBox_imageEditorDir.TabIndex = 9;
            // 
            // button_changeImageEditor
            // 
            this.button_changeImageEditor.Location = new System.Drawing.Point(216, 125);
            this.button_changeImageEditor.Name = "button_changeImageEditor";
            this.button_changeImageEditor.Size = new System.Drawing.Size(75, 23);
            this.button_changeImageEditor.TabIndex = 10;
            this.button_changeImageEditor.Text = "Change";
            this.button_changeImageEditor.UseVisualStyleBackColor = true;
            this.button_changeImageEditor.Click += new System.EventHandler(this.button_changeImageEditor_Click);
            // 
            // button_saveOptions
            // 
            this.button_saveOptions.Location = new System.Drawing.Point(54, 167);
            this.button_saveOptions.Name = "button_saveOptions";
            this.button_saveOptions.Size = new System.Drawing.Size(75, 23);
            this.button_saveOptions.TabIndex = 11;
            this.button_saveOptions.Text = "Save";
            this.button_saveOptions.UseVisualStyleBackColor = true;
            this.button_saveOptions.Click += new System.EventHandler(this.button_saveOptions_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(169, 167);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 12;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 201);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_saveOptions);
            this.Controls.Add(this.button_changeImageEditor);
            this.Controls.Add(this.textBox_imageEditorDir);
            this.Controls.Add(this.label_imageEditor);
            this.Controls.Add(this.button_changeBackupDir);
            this.Controls.Add(this.textBox_backupDir);
            this.Controls.Add(this.label_backupDir);
            this.Controls.Add(this.button_changeMapsDir);
            this.Controls.Add(this.label_mapDir);
            this.Controls.Add(this.textBox_mapDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_mapDir;
        private System.Windows.Forms.Label label_mapDir;
        private System.Windows.Forms.Button button_changeMapsDir;
        private System.Windows.Forms.Label label_backupDir;
        private System.Windows.Forms.TextBox textBox_backupDir;
        private System.Windows.Forms.Button button_changeBackupDir;
        private System.Windows.Forms.Label label_imageEditor;
        private System.Windows.Forms.TextBox textBox_imageEditorDir;
        private System.Windows.Forms.Button button_changeImageEditor;
        private System.Windows.Forms.Button button_saveOptions;
        private System.Windows.Forms.Button button_cancel;
    }
}