using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileFinder
{
    public partial class AppPathFinder : Form
    {
        public AppPathFinder()
        {
            InitializeComponent();
        }

        private void button_setPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this.textBox_path.Text = folderBrowserDialog.SelectedPath;
                this.toolStripStatusLabel.Text = AppStatus.PathIsSelected;
            }
        }

        private void PathFinder_Load(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = AppStatus.Ready;
        }

        private void button_find_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_path.Text))
            {
                this.toolStripStatusLabel.Text = AppStatus.PathError;
                return;
            }
            else if (string.IsNullOrWhiteSpace(textBox_file.Text))
            {
                this.toolStripStatusLabel.Text = AppStatus.FileNameError;
                return;
            }
            else
            {
                PathFinder pathFinder = new PathFinder(this, textBox_path.Text, textBox_file.Text);
                pathFinder.Find();
            }
        }

        public void UpdateStatus(string _Status)
        {
            this.toolStripStatusLabel.Text = _Status;
        }
    }
}
