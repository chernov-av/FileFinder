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

namespace FileFinder
{
    public partial class AppPathFinder : Form
    {
        private List<string> FileList = new List<string>();
        private List<string> FolderList = new List<string>();

        public AppPathFinder()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            this.UpdateStatus(AppStatus.Ready);

        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorkerApp = new BackgroundWorker();
            backgroundWorkerApp.WorkerReportsProgress = true;
            backgroundWorkerApp.WorkerSupportsCancellation = true;
            backgroundWorkerApp.DoWork +=
                new DoWorkEventHandler(backgroundWorkerApp_DoWork);
            backgroundWorkerApp.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
                    backgroundWorkerApp_RunWorkerCompleted);
            backgroundWorkerApp.ProgressChanged +=
                new ProgressChangedEventHandler(
                    backgroundWorkerApp_ProgressChanged);
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

        private void button_find_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_path.Text))
            {
                this.UpdateStatus(AppStatus.PathError);
                return;
            }
            else if (string.IsNullOrWhiteSpace(textBox_file.Text))
            {
                this.UpdateStatus(AppStatus.FileNameError);
                return;
            }
            else
            {
                this.button_find.Enabled = false;
                this.button_stop.Enabled = true;
                this.backgroundWorkerApp.RunWorkerAsync(Tuple.Create(textBox_path.Text, textBox_file.Text));
            }
        }

        private void backgroundWorkerApp_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            this.FindFile(e.Argument as Tuple<string,string>, worker, e);
            e.Result = 1;
        }

        private void FindFile(Tuple<string,string> _args, BackgroundWorker _worker, DoWorkEventArgs e)
        {
            string _path = _args.Item1;
            string _fileName = _args.Item2;
            if (_worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                    _worker.ReportProgress(100, _path);
                    statusStrip.Invoke((MethodInvoker)delegate {
                        toolStripStatusLabel.Text = _path;
                    });
                    string[] SubDir = Directory.GetDirectories(_path);
                    for (int i = 0; i < SubDir.Length; i++)
                    {
                        FindFile(Tuple.Create(SubDir[i], _fileName), _worker, e);
                    }
                    string[] File = Directory.GetFiles(_path, _fileName);

                    for (int i = 0; i < File.Length; i++)
                    {
                        this.FileList.Add(File[i]);
                        this.FolderList.Add(_path);
                        MessageBox.Show(File[i] + '\n' + _path);
                    }
                }
                catch (Exception exc)
                {
                  //  MessageBox.Show(exc.Message);
                 //return;
                }
            }
        }

        private void UpdateStatus(string _status)
        {
            this.toolStripStatusLabel.Text = _status;
            this.statusStrip.Refresh();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            this.backgroundWorkerApp.CancelAsync();
            this.button_stop.Enabled = false;
        }

        private void backgroundWorkerApp_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // this.UpdateStatus(e.UserState.ToString());
           // toolStripStatusLabel.Text = e.UserState as String;
           // label1.Text = e.UserState.ToString();
          //  statusStrip.Refresh();
        }

        private void backgroundWorkerApp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                this.UpdateStatus(AppStatus.Canceled);
            }
            else
            {
                this.UpdateStatus(AppStatus.Done);
            }

            this.button_find.Enabled = true;
            this.button_stop.Enabled = false;
        }
    }
}
