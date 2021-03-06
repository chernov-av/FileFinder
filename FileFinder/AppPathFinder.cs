﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        int hour, minute, second;

        Regex reg = new Regex(@"^^(?!p_|t_).*");

        private System.Threading.ManualResetEvent _busy;

        private bool isDoubleClick = false;


        public AppPathFinder()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            UpdateStatus(AppStatus.Ready);
            textBox_path.Text = Properties.Settings.Default.LastPath;
            textBox_file.Text = Properties.Settings.Default.LastFileName; }

        private void InitializeBackgroundWorker()
        {
            backgroundWorkerApp = new BackgroundWorker();
            backgroundWorkerApp.WorkerSupportsCancellation = true;
            backgroundWorkerApp.DoWork +=
                new DoWorkEventHandler(backgroundWorkerApp_DoWork);
            backgroundWorkerApp.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
                    backgroundWorkerApp_RunWorkerCompleted);
       _busy = new System.Threading.ManualResetEvent(false);
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
            if (button_find.Text == "Find")
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
                    button_stop.Enabled = true;
                    button_find.Enabled = false;

                    FileList.Clear();
                    FolderList.Clear();
                    label_filesFound.Text = "0";
                    label_allFiles.Text = "0";
                    FileAmount = 0;

                    timer = new Timer();
                    timer.Tick += new EventHandler(TimerTick);
                    timer.Interval = 1000;
                    label_time.Text = "0:0:0";
                    hour = 0;
                    minute = 0;
                    second = 1;
                    timer.Start();

                    doubleBufferedTreeView1.Nodes.Clear();

                    Properties.Settings.Default.LastPath = textBox_path.Text;
                    Properties.Settings.Default.LastFileName = textBox_file.Text;
                    Properties.Settings.Default.Save();

                    SetWorkerMode(true);
                    backgroundWorkerApp.RunWorkerAsync(Tuple.Create(textBox_path.Text, textBox_file.Text));
                }
            }
        }
        
        private void SetWorkerMode(bool running)
        {
            if (running)
            {
                button_pauseResume.Text = "Pause";
                timer.Start();
                _busy.Set();
            }
            else
            {
                button_pauseResume.Text = "Resume";
                timer.Stop();
                _busy.Reset();
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
                    _busy.WaitOne();
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
                    string[] File = Directory.GetFiles(_path, _fileName).Where(path=>reg.IsMatch(path)).ToList().ToArray();

                    for (int i = 0; i < File.Length; i++)
                    {
                        FileList.Add(File[i]);
                        FolderList.Add(_path);

                        label_filesFound.Invoke((MethodInvoker) delegate
                        {
                            label_filesFound.Text = FileList.Count.ToString();
                        });

                        doubleBufferedTreeView1.Invoke((MethodInvoker) delegate
                        {
                            doubleBufferedTreeView1.BeginUpdate();
                            BuildTree(doubleBufferedTreeView1.Nodes,FileList.ToArray());
                            doubleBufferedTreeView1.EndUpdate();
                        });
                    }
                }
                catch (Exception exc)
                {
                    //no access files
                }
            }
        }

        private void BuildTree(TreeNodeCollection _nodes, string[] _list)
        {
            foreach (string path in _list)
            {
                TreeNodeCollection childs = _nodes;
                string pathAndFile = path.Split(';')[0];
                string[] parts = pathAndFile.Split('\\');
                for (int i = 0; i < parts.Length; i++)
                {
                    childs = FindOrCreateNode(childs, parts[i]).Nodes;
                }
            }
        }

        private TreeNode FindOrCreateNode(TreeNodeCollection _coll, string _name)
        {
            TreeNode[] nodeFound = _coll.Find(_name.ToLower(), false);
            if (nodeFound.Length > 0)
            {
                return nodeFound[0];
            }
            return _coll.Add(_name.ToLower(), _name);
        }

        private void UpdateStatus(string _status)
        {
            toolStripStatusLabel.Text = _status;
            statusStrip.Refresh();
        }

        private void button_pauseResume_Click(object sender, EventArgs e)
        {
            SetWorkerMode(button_pauseResume.Text == "Resume");
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                //if (!e.Node.Text.Contains("."))
                System.Diagnostics.Process.Start(@e.Node.FullPath);
            }
            catch (System.ComponentModel.Win32Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Collapse)
                e.Cancel = true;
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Expand)
                e.Cancel = true;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            isDoubleClick = e.Clicks > 1;
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (button_pauseResume.Text == "Resume")
            {
                button_pauseResume_Click(sender,e);
            }
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
