using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapInjector {
    public partial class OptionsForm : Form {

        Main mainForm;

        private DirectoryInfo mapDir;
        private DirectoryInfo backupDir;
        private DirectoryInfo imageEditorDir;

        private bool useDevMaps;
        private bool useCustomMaps;

        public OptionsForm(Main parent) {
            InitializeComponent();

            mainForm = parent;
        }

        private void OptionsForm_Load(object sender, EventArgs e) {
            //Load the settings
            string preferencesPath = Path.Combine(Directory.GetCurrentDirectory(), "preferences.txt");

            if (!File.Exists(preferencesPath))
                CreatePreferencesFile(preferencesPath);

            using (StreamReader r = new StreamReader(preferencesPath)) {
                string line;
                string[] splitLine;
                int iter = 0;
                while ((line = r.ReadLine()) != null) {
                    splitLine = line.Split('=');

                    switch (iter) {
                        case 0:
                            mapDir = new DirectoryInfo(splitLine[1]);
                            break;

                        case 1:
                            backupDir = new DirectoryInfo(splitLine[1]);
                            break;

                        case 2:
                            imageEditorDir = new DirectoryInfo(splitLine[1]);
                            break;
                    }

                    iter++;
                }
            }

            textBox_mapDir.Text = mapDir.ToString();
            textBox_backupDir.Text = mapDir.ToString();
            textBox_imageEditorDir.Text = imageEditorDir.ToString();
        }

        void CreatePreferencesFile(string path) {
            using (StreamWriter w = new StreamWriter(path)) {
                //default the map directory to this directory
                w.WriteLine("mapsdirectory=" + Directory.GetCurrentDirectory());
                //default the backups directory to this directory
                w.WriteLine("backupdirectory=" + Directory.GetCurrentDirectory());
                //default the image editor to ms paint
                w.WriteLine("defaultimageeditor=C:\\Windows\\System32\\mspaint.exe");
            }
        }

        private void button_changeMapsDir_Click(object sender, EventArgs e) {
            using (var fbd = new FolderBrowserDialog()) {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath) && fbd.SelectedPath != backupDir.ToString())
                    mapDir = new DirectoryInfo(fbd.SelectedPath);
                else if(fbd.SelectedPath == backupDir.ToString()) {
                    MessageBox.Show("Maps directory path cannot be the same as your backup directory!", "Warning", MessageBoxButtons.OK);
                }
            }

            textBox_mapDir.Text = mapDir.ToString();
        }

        private void button_changeBackupDir_Click(object sender, EventArgs e) {
            using (var fbd = new FolderBrowserDialog()) {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath) && fbd.SelectedPath != mapDir.ToString())
                    backupDir = new DirectoryInfo(fbd.SelectedPath);
                else if (fbd.SelectedPath == mapDir.ToString()) {
                    MessageBox.Show("Backup directory path cannot be the same as your maps directory!", "Warning", MessageBoxButtons.OK);
                }
            }

            textBox_backupDir.Text = backupDir.ToString();
        }

        private void button_changeImageEditor_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Executable Files|*.exe";
            DialogResult res = ofd.ShowDialog(this);

            if (res == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName.ToString())) {
                imageEditorDir = new DirectoryInfo(ofd.FileName);
                textBox_imageEditorDir.Text = ofd.FileName;
            }
        }

        private void button_saveOptions_Click(object sender, EventArgs e) {
            string preferencesPath = Path.Combine(Directory.GetCurrentDirectory(), "preferences.txt");

            if (!File.Exists(preferencesPath))
                CreatePreferencesFile(preferencesPath);

            using (StreamWriter w = new StreamWriter(preferencesPath, false)) {
                //save mapsDir as the new maps directory
                w.WriteLine("mapsdirectory=" + mapDir.ToString());
                //save backupDir as the new backup directory
                w.WriteLine("backupdirectory=" + backupDir.ToString());
                //set the image editor to the new path
                w.WriteLine("defaultimageeditor=" + imageEditorDir.ToString());
            }

            mainForm.LoadSettings();
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
