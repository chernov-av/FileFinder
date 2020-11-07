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

        private int FileAmount = 0;
        private Timer timer;
        int hour = 0, minute = 0, second = 0;

        public AppPathFinder()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            UpdateStatus(AppStatus.Ready);
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorkerApp = new BackgroundWorker();
            backgroundWorkerApp.WorkerSupportsCancellation = true;
            backgroundWorkerApp.DoWork +=
                new DoWorkEventHandler(backgroundWorkerApp_DoWork);
            backgroundWorkerApp.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
                    backgroundWorkerApp_RunWorkerCompleted);
        }

        private void button_setPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textBox_path.Text = folderBrowserDialog.SelectedPath;
                toolStripStatusLabel.Text = AppStatus.PathIsSelected;
            }
        }

        private void button_find_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_path.Text))
            {
                UpdateStatus(AppStatus.PathError);
                return;
            }
            else if (string.IsNullOrWhiteSpace(textBox_file.Text))
            {
                UpdateStatus(AppStatus.FileNameError);
                return;
            }
            else
            {
                button_find.Enabled = false;
                button_stop.Enabled = true;
                timer = new Timer();
                timer.Tick+=new EventHandler(TimerTick);
                timer.Interval = 1000;
                hour = 0;
                minute = 0;
                second = 1;
                timer.Start();
                backgroundWorkerApp.RunWorkerAsync(Tuple.Create(textBox_path.Text, textBox_file.Text));
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            label_time.Text = hour.ToString() + ":" + minute.ToString() + ":" + second.ToString();
            label_time.Refresh();
            if (second == 59)
            {
                second = 0;
                minute++;
            }
            else { second++; }
            if (minute == 59)
            {
                minute = 0;
                hour++;
            }
        }

        private void backgroundWorkerApp_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            FindFile(e.Argument as Tuple<string,string>, worker, e);
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
                    //Update toolStrip and show current folder
                    statusStrip.Invoke((MethodInvoker) delegate 
                    {
                        if (_path.Length > 100)
                        {
                            toolStripStatusLabel.Text = "..."+_path.Substring(_path.Length - 100);
                        }
                        else
                        {
                            toolStripStatusLabel.Text = _path;
                        }
                    });

                    string[] SubDir = Directory.GetDirectories(_path);

                    for (int i = 0; i < SubDir.Length; i++)
                    {
                        FindFile(Tuple.Create(SubDir[i], _fileName), _worker, e);
                    }

                    //Count all files and update label
                    string[] AllFiles = Directory.GetFiles(_path);
                    FileAmount += AllFiles.Length;

                    label_allFiles.Invoke((MethodInvoker) delegate
                    {
                        label_allFiles.Text = FileAmount.ToString();
                    });

                    //Find files and update tree and file amount
                    string[] File = Directory.GetFiles(_path, _fileName);

                    for (int i = 0; i < File.Length; i++)
                    {
                        FileList.Add(File[i]);
                        FolderList.Add(_path);

                        label_filesFound.Invoke((MethodInvoker) delegate
                        {
                            label_filesFound.Text = this.FileList.Count.ToString();
                        });
                    }
                }
                catch (Exception exc)
                {}
            }
        }

        private void UpdateStatus(string _status)
        {
            toolStripStatusLabel.Text = _status;
            statusStrip.Refresh();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            backgroundWorkerApp.CancelAsync();
            button_stop.Enabled = false;
        }

        private void backgroundWorkerApp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                UpdateStatus(AppStatus.Canceled);
            }
            else
            {
                UpdateStatus(AppStatus.Done);
            }

            button_find.Enabled = true;
            button_stop.Enabled = false;
            timer.Stop();
        }
    }
}
