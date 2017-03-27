namespace MapInjector {
    partial class Main {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.mapList = new System.Windows.Forms.CheckedListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_open = new System.Windows.Forms.Button();
            this.button_remove = new System.Windows.Forms.Button();
            this.button_options = new System.Windows.Forms.Button();
            this.label_loadedMaps = new System.Windows.Forms.Label();
            this.mapImage = new System.Windows.Forms.PictureBox();
            this.label_imageName = new System.Windows.Forms.Label();
            this.label_actions = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapImage)).BeginInit();
            this.SuspendLayout();
            // 
            // mapList
            // 
            this.mapList.FormattingEnabled = true;
            this.mapList.Location = new System.Drawing.Point(12, 105);
            this.mapList.Name = "mapList";
            this.mapList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mapList.ScrollAlwaysVisible = true;
            this.mapList.Size = new System.Drawing.Size(400, 259);
            this.mapList.TabIndex = 0;
            this.mapList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.mapList_ItemCheck);
            this.mapList.SelectedValueChanged += new System.EventHandler(this.mapList_SelectedValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createBackupToolStripMenuItem,
            this.loadFromBackupToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.fileToolStripMenuItem.Text = "Backups...";
            // 
            // createBackupToolStripMenuItem
            // 
            this.createBackupToolStripMenuItem.Name = "createBackupToolStripMenuItem";
            this.createBackupToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.createBackupToolStripMenuItem.Text = "Create Backup";
            this.createBackupToolStripMenuItem.Click += new System.EventHandler(this.createBackupToolStripMenuItem_Click);
            // 
            // loadFromBackupToolStripMenuItem
            // 
            this.loadFromBackupToolStripMenuItem.Name = "loadFromBackupToolStripMenuItem";
            this.loadFromBackupToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.loadFromBackupToolStripMenuItem.Text = "Load from Backup";
            this.loadFromBackupToolStripMenuItem.Click += new System.EventHandler(this.loadFromBackupToolStripMenuItem_Click);
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(108, 55);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(90, 30);
            this.button_open.TabIndex = 4;
            this.button_open.Text = "Open...";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // button_remove
            // 
            this.button_remove.Location = new System.Drawing.Point(204, 55);
            this.button_remove.Name = "button_remove";
            this.button_remove.Size = new System.Drawing.Size(90, 30);
            this.button_remove.TabIndex = 5;
            this.button_remove.Text = "Remove";
            this.button_remove.UseVisualStyleBackColor = true;
            this.button_remove.Click += new System.EventHandler(this.button_remove_Click);
            // 
            // button_options
            // 
            this.button_options.Location = new System.Drawing.Point(300, 55);
            this.button_options.Name = "button_options";
            this.button_options.Size = new System.Drawing.Size(90, 30);
            this.button_options.TabIndex = 6;
            this.button_options.Text = "Options...";
            this.button_options.UseVisualStyleBackColor = true;
            this.button_options.Click += new System.EventHandler(this.button_options_Click);
            // 
            // label_loadedMaps
            // 
            this.label_loadedMaps.AutoSize = true;
            this.label_loadedMaps.Location = new System.Drawing.Point(340, 88);
            this.label_loadedMaps.Name = "label_loadedMaps";
            this.label_loadedMaps.Size = new System.Drawing.Size(72, 13);
            this.label_loadedMaps.TabIndex = 7;
            this.label_loadedMaps.Text = "Loaded Maps";
            // 
            // mapImage
            // 
            this.mapImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapImage.Location = new System.Drawing.Point(433, 44);
            this.mapImage.Name = "mapImage";
            this.mapImage.Size = new System.Drawing.Size(339, 320);
            this.mapImage.TabIndex = 8;
            this.mapImage.TabStop = false;
            // 
            // label_imageName
            // 
            this.label_imageName.AutoSize = true;
            this.label_imageName.Location = new System.Drawing.Point(430, 28);
            this.label_imageName.Name = "label_imageName";
            this.label_imageName.Size = new System.Drawing.Size(73, 13);
            this.label_imageName.TabIndex = 9;
            this.label_imageName.Text = "Selected Map";
            // 
            // label_actions
            // 
            this.label_actions.AutoSize = true;
            this.label_actions.Location = new System.Drawing.Point(9, 39);
            this.label_actions.Name = "label_actions";
            this.label_actions.Size = new System.Drawing.Size(42, 13);
            this.label_actions.TabIndex = 10;
            this.label_actions.Text = "Actions";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 30);
            this.button1.TabIndex = 11;
            this.button1.Text = "Add...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 388);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_actions);
            this.Controls.Add(this.label_imageName);
            this.Controls.Add(this.mapImage);
            this.Controls.Add(this.label_loadedMaps);
            this.Controls.Add(this.button_options);
            this.Controls.Add(this.button_remove);
            this.Controls.Add(this.button_open);
            this.Controls.Add(this.mapList);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "The Bond of Stone Map Manager";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox mapList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createBackupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromBackupToolStripMenuItem;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Button button_remove;
        private System.Windows.Forms.Button button_options;
        private System.Windows.Forms.Label label_loadedMaps;
        private System.Windows.Forms.PictureBox mapImage;
        private System.Windows.Forms.Label label_imageName;
        private System.Windows.Forms.Label label_actions;
        private System.Windows.Forms.Button button1;
    }
}

