using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTGP_7_Music_Slot_User
{
    public partial class Form1 : Form
    {
        NameList translateList;
        MusicConfigManager musicConfig;
        string fileName = "";
        public Form1()
        {
            InitializeComponent();
            translateList = new NameList();
            foreach (var entry in translateList.nameInf.Entry)
            {
                courseNameBox.Items.Add(entry.humanName);
            }
            musicModeBox.Items.AddRange(new string[] { "Single Channel", "Multi Channel Water", "Multi Channel Area"});
            musicConfig = new MusicConfigManager();
            dataGrid.DataSource = musicConfig.entries;
            this.Text = "CTGP-7 Music Slots Editor - New";
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                this.Text = "CTGP-7 Music Slots Editor - " + System.IO.Path.GetFileName(fileName);
                musicConfig = new MusicConfigManager(translateList, fileName);
                dataGrid.DataSource = musicConfig.entries;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName == "") {SaveAsToolStripMenuItem_Click(sender, e); return; }
            musicConfig.Write(fileName);
            MessageBox.Show("Saved successfully!");
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
                this.Text = "CTGP-7 Music Slots Editor - " + System.IO.Path.GetFileName(fileName);
                musicConfig.Write(fileName);
                MessageBox.Show("Saved successfully!");
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CTGP-7 Music Slots Info Editor\nVersion: 1.0");
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://mk7.tock.eu/index.php?title=MusicSlotsUser.ini");
        }
        public bool dataIsChanging = false;
        public void updateNameLabel()
        {
            if (musicFileBox.Text == "") nameInfo.Text = "";
            else nameInfo.Text = "Normal: STRM_C" + musicFileBox.Text + "_N.bcstm; Fast: STRM_C" + musicFileBox.Text + "_F.bcstm";
        }
        public void fieldChanged()
        {

            if (dataGrid.SelectedRows.Count == 0) return;
            if (dataGrid.SelectedRows[0].Index == -1) return;
            if (dataIsChanging) return;
            dataIsChanging = true;
            MusicConfigEntry currEntry = musicConfig.entries[dataGrid.SelectedRows[0].Index];
            currEntry.CourseName = courseNameBox.Text;
            currEntry.MusicName = musicFileBox.Text;
            updateNameLabel();
            currEntry.MusicMode = musicModeBox.Text;
            currEntry.NormalBPM = bpmNormalBox.Text;
            currEntry.FastBPM = bpmFastBox.Text;
            currEntry.NormalOffset = offsetNormalBox.Text;
            currEntry.FastOffset = offsetFastBox.Text;
            dataGrid.DataSource = musicConfig.entries;
            dataIsChanging = false;
            dataGrid.Refresh();
        }
        public void rowChanged()
        {
            if (dataGrid.SelectedRows.Count == 0) return;
            if (dataGrid.SelectedRows[0].Index == -1) return;
            if (dataIsChanging) return;
            dataIsChanging = true;
            MusicConfigEntry currEntry = musicConfig.entries[dataGrid.SelectedRows[0].Index];
            courseNameBox.Text = currEntry.CourseName;
            musicFileBox.Text = currEntry.MusicName;
            updateNameLabel();
            musicModeBox.Text = currEntry.MusicMode;
            bpmNormalBox.Text = currEntry.NormalBPM;
            bpmFastBox.Text = currEntry.FastBPM;
            offsetNormalBox.Text = currEntry.NormalOffset;
            offsetFastBox.Text = currEntry.FastOffset;
            dataGrid.DataSource = musicConfig.entries;
            dataIsChanging = false;
        }

        private void DataGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            rowChanged();
        }

        private void CourseNameBox_TextChanged(object sender, EventArgs e)
        {
            fieldChanged();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            musicConfig.entries.Add(new MusicConfigEntry(translateList));
            dataGrid.DataSource = null;
            dataGrid.DataSource = musicConfig.entries;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count == 0) return;
            if (dataGrid.SelectedRows[0].Index == -1) return;
            MusicConfigEntry currEntry = musicConfig.entries[dataGrid.SelectedRows[0].Index];
            musicConfig.entries.Remove(currEntry);
            dataGrid.DataSource = null;
            dataGrid.DataSource = musicConfig.entries;
        }
    }
}
