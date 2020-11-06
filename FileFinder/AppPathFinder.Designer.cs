namespace FileFinder
{
    partial class AppPathFinder
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_find = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.label_path = new System.Windows.Forms.Label();
            this.textBox_file = new System.Windows.Forms.TextBox();
            this.label_file = new System.Windows.Forms.Label();
            this.button_setPath = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.button_stop = new System.Windows.Forms.Button();
            this.backgroundWorkerApp = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_find
            // 
            this.button_find.Location = new System.Drawing.Point(168, 84);
            this.button_find.Name = "button_find";
            this.button_find.Size = new System.Drawing.Size(98, 23);
            this.button_find.TabIndex = 0;
            this.button_find.Text = "Find";
            this.button_find.UseVisualStyleBackColor = true;
            this.button_find.Click += new System.EventHandler(this.button_find_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(52, 9);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(214, 20);
            this.textBox_path.TabIndex = 1;
            this.textBox_path.Text = "D:\\";
            // 
            // label_path
            // 
            this.label_path.AutoSize = true;
            this.label_path.Location = new System.Drawing.Point(14, 12);
            this.label_path.Name = "label_path";
            this.label_path.Size = new System.Drawing.Size(32, 13);
            this.label_path.TabIndex = 2;
            this.label_path.Text = "Path:";
            // 
            // textBox_file
            // 
            this.textBox_file.Location = new System.Drawing.Point(52, 45);
            this.textBox_file.Name = "textBox_file";
            this.textBox_file.Size = new System.Drawing.Size(214, 20);
            this.textBox_file.TabIndex = 1;
            // 
            // label_file
            // 
            this.label_file.AutoSize = true;
            this.label_file.Location = new System.Drawing.Point(14, 48);
            this.label_file.Name = "label_file";
            this.label_file.Size = new System.Drawing.Size(26, 13);
            this.label_file.TabIndex = 2;
            this.label_file.Text = "File:";
            // 
            // button_setPath
            // 
            this.button_setPath.Location = new System.Drawing.Point(272, 7);
            this.button_setPath.Name = "button_setPath";
            this.button_setPath.Size = new System.Drawing.Size(40, 23);
            this.button_setPath.TabIndex = 3;
            this.button_setPath.Text = "Set";
            this.button_setPath.UseVisualStyleBackColor = true;
            this.button_setPath.Click += new System.EventHandler(this.button_setPath_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 23);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(293, 278);
            this.treeView1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_stop);
            this.panel1.Controls.Add(this.button_find);
            this.panel1.Controls.Add(this.textBox_path);
            this.panel1.Controls.Add(this.button_setPath);
            this.panel1.Controls.Add(this.textBox_file);
            this.panel1.Controls.Add(this.label_file);
            this.panel1.Controls.Add(this.label_path);
            this.panel1.Location = new System.Drawing.Point(319, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 155);
            this.panel1.TabIndex = 5;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 302);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(658, 22);
            this.statusStrip.TabIndex = 6;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(168, 113);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(98, 23);
            this.button_stop.TabIndex = 0;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // backgroundWorkerApp
            // 
            this.backgroundWorkerApp.WorkerReportsProgress = true;
            this.backgroundWorkerApp.WorkerSupportsCancellation = true;
            this.backgroundWorkerApp.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerApp_ProgressChanged);
            this.backgroundWorkerApp.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerApp_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(371, 223);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            // 
            // AppPathFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 324);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeView1);
            this.Name = "AppPathFinder";
            this.Text = "FileFinder";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_find;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Label label_path;
        private System.Windows.Forms.TextBox textBox_file;
        private System.Windows.Forms.Label label_file;
        private System.Windows.Forms.Button button_setPath;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Button button_stop;
        private System.ComponentModel.BackgroundWorker backgroundWorkerApp;
        private System.Windows.Forms.Label label1;
    }
}

