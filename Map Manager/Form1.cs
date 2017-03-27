using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace MapInjector {
    public partial class Main : Form {

        bool allMapsSelected;
        private bool suppressCheckedChanged;

        private DirectoryInfo mapDir;
        private DirectoryInfo backupDir;
        private DirectoryInfo imageEditorDir;

        public Main() {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e) {
            LoadSettings();
            
        }

        void CreatePreferencesFile(string path) {
            using(StreamWriter w = new StreamWriter(path)) {
                //default the map directory to this directory
                w.WriteLine("mapsdirectory=" + Directory.GetCurrentDirectory());
                //default the backups directory to this directory
                w.WriteLine("backupdirectory=" + Directory.GetCurrentDirectory());
                //default using developer maps to true
                w.WriteLine("usedevelopermaps=true");
                //default using custom maps to true
                w.WriteLine("usecustommaps=true");
                //default the image editor to ms paint
                w.WriteLine("defaultimageeditor=C:\\Windows\\System32\\mspaint.exe");
            }
        }

        private void button_options_Click(object sender, EventArgs e) {
            //SAVE ANY UNSAVED CHANGES TO MAP METADATA BEFORE LOADING THE OPTIONS MENU

            Form options = new OptionsForm(this);
            options.ShowDialog(this);
        }

        //Opens the selected map file in the map list with the specified image editor (defaults to mspaint)
        private void button_open_Click(object sender, EventArgs e) {
            try {
                Process imageEditor = new Process();
                imageEditor.StartInfo.FileName = imageEditorDir.ToString();
                imageEditor.StartInfo.Arguments = Path.Combine(mapDir.ToString(), mapList.SelectedItem.ToString());
                imageEditor.Start();
            } catch (NullReferenceException) {
                //no map selected
            }
        }

        private void checkBox_SelectAllToggle_CheckedChanged(object sender, EventArgs e) {
            allMapsSelected = !allMapsSelected;

            if (suppressCheckedChanged) return;

            if (allMapsSelected) {
                for(int i = 0; i < mapList.Items.Count; i++) {
                    mapList.SetItemChecked(i, true);
                }
            } else {
                for (int i = 0; i < mapList.Items.Count; i++) {
                    mapList.SetItemChecked(i, false);
                }
            }
        }

        private void mapList_ItemCheck(object sender, ItemCheckEventArgs e) {

        }

        private void mapList_SelectedValueChanged(object sender, EventArgs e) {
            if(mapList.SelectedItem != null) {
                label_imageName.Text = mapList.SelectedItem.ToString();
                Image i = Image.FromFile(Path.Combine(mapDir.ToString(), mapList.SelectedItem.ToString()));
                mapImage.Image = ResizeImage((Bitmap)i, mapImage.Width - 10, mapImage.Height - 10);
                i.Dispose();
            }
        }

        private Bitmap ResizeImage(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);
            double ratio = (double)b.Width / (double)b.Height;

            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                if ((int)((double)nHeight * ratio) <= nWidth)
                    g.DrawImage(b, 0, 0, (int)((double)nHeight * ratio), nHeight);
                else
                    g.DrawImage(b, 0, 0, nWidth, (int)((double)nWidth / ratio));
            }
            return result;
        }

        private void button_remove_Click(object sender, EventArgs e) {
            try {
                var selected = mapList.Items[mapList.SelectedIndex];
                
                if (mapList.Items[mapList.SelectedIndex].ToString() == "starter.png") {
                    MessageBox.Show(this, "Cannot delete starter chunk.", "Error", MessageBoxButtons.OK);
                    return;
                }

                if (mapList.Items.Count == 2) {
                    MessageBox.Show(this, "Must have at least 1 chunk in addition to the starter chunk.", "Error", MessageBoxButtons.OK);
                    return;
                }

                var confirmResult = MessageBox.Show(this, "Are you sure you want to delete " + selected.ToString() + "?\nThis action cannot be undone.", "Confirm Map Deletion", MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes) {
                    mapImage.Image.Dispose();
                    string path = Path.Combine(mapDir.FullName, mapList.Items[mapList.SelectedIndex].ToString());
                    mapList.Items.Clear();

                    File.Delete(path);

                    RefreshMaps();
                    mapImage.Image = null;
                }
            } catch(ArgumentOutOfRangeException){
                //There was no item selected.
            }
        }

        public void LoadSettings() {
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

                RefreshMaps();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "PNG Files | *.png";
            if(ofd.ShowDialog() == DialogResult.OK) {
                string[] files = ofd.SafeFileNames;
                for(int i = 0; i < files.Length; i++)
                    File.Copy(ofd.FileNames[i], Path.Combine(mapDir.FullName, files[i]), true);
            }

            ofd.Dispose();

            RefreshMaps();
        }

        void RefreshMaps() {
            //Repopulate the mapList
            mapList.Items.Clear();

            foreach (FileInfo f in mapDir.GetFiles()) {
                if (f.Extension == ".png")
                    mapList.Items.Add(f.Name);
            }
        }

        private void createBackupToolStripMenuItem_Click(object sender, EventArgs e) {
            var confirmResult = MessageBox.Show(this, "Creating a backup will overwrite the contents of the backup directory\nwith the contents of the maps directory. Continue?.", "Confirm Backup Creation", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes) {
                //Delete contents of backup dir
                foreach(FileInfo f in backupDir.GetFiles()) {
                    File.Delete(f.FullName);
                }
                //Copy contents of maps dir to backup dir
                foreach (FileInfo f in mapDir.GetFiles()) {
                    File.Copy(f.FullName, Path.Combine(backupDir.FullName, f.Name));
                }
            }

            RefreshMaps();
        }

        private void loadFromBackupToolStripMenuItem_Click(object sender, EventArgs e) {
            var confirmResult = MessageBox.Show(this, "Are you sure you want to load from a backup?\nThis will overwrite the current maps directory.", "Confirm Load from Backup", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes) {
                //delete contents of map directory
                foreach (FileInfo f in mapDir.GetFiles()) {
                    File.Delete(f.FullName);
                }
                //copy contents of backup directory to maps directory
                foreach (FileInfo f in backupDir.GetFiles()) {
                    File.Copy(f.FullName, Path.Combine(mapDir.FullName, f.Name));
                }
            }

            RefreshMaps();
        }
    }
}
