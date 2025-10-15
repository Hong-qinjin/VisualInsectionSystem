namespace OCRDemoCs
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonLoadSolu = new System.Windows.Forms.Button();
            this.buttonSelectSolu = new System.Windows.Forms.Button();
            this.buttonRunOnce = new System.Windows.Forms.Button();
            this.buttonContiRun = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSaveSolu = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboProcedure = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.logTimeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logMessageHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.logClear = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listBoxResult = new System.Windows.Forms.ListBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.buttonRender = new System.Windows.Forms.Button();
            this.buttonConfig = new System.Windows.Forms.Button();
            this.buttonChineseOREnglish = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStripLog.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLoadSolu
            // 
            this.buttonLoadSolu.BackColor = System.Drawing.SystemColors.GrayText;
            this.buttonLoadSolu.FlatAppearance.BorderSize = 0;
            this.buttonLoadSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadSolu.ForeColor = System.Drawing.Color.White;
            this.buttonLoadSolu.Location = new System.Drawing.Point(134, 32);
            this.buttonLoadSolu.Name = "buttonLoadSolu";
            this.buttonLoadSolu.Size = new System.Drawing.Size(110, 41);
            this.buttonLoadSolu.TabIndex = 3;
            this.buttonLoadSolu.Text = "加载方案";
            this.buttonLoadSolu.UseVisualStyleBackColor = false;
            this.buttonLoadSolu.Click += new System.EventHandler(this.buttonLoadSolu_Click);
            // 
            // buttonSelectSolu
            // 
            this.buttonSelectSolu.BackColor = System.Drawing.Color.Gray;
            this.buttonSelectSolu.FlatAppearance.BorderSize = 0;
            this.buttonSelectSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectSolu.ForeColor = System.Drawing.Color.White;
            this.buttonSelectSolu.Location = new System.Drawing.Point(6, 32);
            this.buttonSelectSolu.Name = "buttonSelectSolu";
            this.buttonSelectSolu.Size = new System.Drawing.Size(110, 41);
            this.buttonSelectSolu.TabIndex = 0;
            this.buttonSelectSolu.Text = "选择方案";
            this.buttonSelectSolu.UseVisualStyleBackColor = false;
            this.buttonSelectSolu.Click += new System.EventHandler(this.buttonSelectSolu_Click);
            // 
            // buttonRunOnce
            // 
            this.buttonRunOnce.BackColor = System.Drawing.SystemColors.GrayText;
            this.buttonRunOnce.FlatAppearance.BorderSize = 0;
            this.buttonRunOnce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRunOnce.ForeColor = System.Drawing.Color.White;
            this.buttonRunOnce.Location = new System.Drawing.Point(134, 29);
            this.buttonRunOnce.Name = "buttonRunOnce";
            this.buttonRunOnce.Size = new System.Drawing.Size(110, 41);
            this.buttonRunOnce.TabIndex = 6;
            this.buttonRunOnce.Text = "单次运行";
            this.buttonRunOnce.UseVisualStyleBackColor = false;
            this.buttonRunOnce.Click += new System.EventHandler(this.buttonRunOnce_Click);
            // 
            // buttonContiRun
            // 
            this.buttonContiRun.BackColor = System.Drawing.SystemColors.GrayText;
            this.buttonContiRun.FlatAppearance.BorderSize = 0;
            this.buttonContiRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonContiRun.ForeColor = System.Drawing.Color.White;
            this.buttonContiRun.Location = new System.Drawing.Point(261, 29);
            this.buttonContiRun.Name = "buttonContiRun";
            this.buttonContiRun.Size = new System.Drawing.Size(110, 41);
            this.buttonContiRun.TabIndex = 7;
            this.buttonContiRun.Text = "连续运行";
            this.buttonContiRun.UseVisualStyleBackColor = false;
            this.buttonContiRun.Click += new System.EventHandler(this.buttonContiRun_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(2, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(913, 600);
            this.panel1.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonSaveSolu);
            this.groupBox1.Controls.Add(this.buttonSelectSolu);
            this.groupBox1.Controls.Add(this.buttonLoadSolu);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(921, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 100);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "方案操作";
            // 
            // buttonSaveSolu
            // 
            this.buttonSaveSolu.BackColor = System.Drawing.SystemColors.GrayText;
            this.buttonSaveSolu.FlatAppearance.BorderSize = 0;
            this.buttonSaveSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveSolu.ForeColor = System.Drawing.Color.White;
            this.buttonSaveSolu.Location = new System.Drawing.Point(261, 32);
            this.buttonSaveSolu.Name = "buttonSaveSolu";
            this.buttonSaveSolu.Size = new System.Drawing.Size(110, 41);
            this.buttonSaveSolu.TabIndex = 8;
            this.buttonSaveSolu.Text = "保存方案";
            this.buttonSaveSolu.UseVisualStyleBackColor = false;
            this.buttonSaveSolu.Click += new System.EventHandler(this.buttonSaveSolu_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboProcedure);
            this.groupBox2.Controls.Add(this.buttonRunOnce);
            this.groupBox2.Controls.Add(this.buttonContiRun);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(921, 159);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 100);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "流程操作";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "选择流程";
            // 
            // comboProcedure
            // 
            this.comboProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProcedure.FormattingEnabled = true;
            this.comboProcedure.Location = new System.Drawing.Point(17, 40);
            this.comboProcedure.Name = "comboProcedure";
            this.comboProcedure.Size = new System.Drawing.Size(99, 20);
            this.comboProcedure.TabIndex = 8;
            this.comboProcedure.SelectedIndexChanged += new System.EventHandler(this.comboBoxAllProcedure_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.listViewLog);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(921, 265);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(380, 513);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "日志消息";
            // 
            // listViewLog
            // 
            this.listViewLog.AutoArrange = false;
            this.listViewLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.logTimeHeader,
            this.logMessageHeader});
            this.listViewLog.ContextMenuStrip = this.contextMenuStripLog;
            this.listViewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewLog.ForeColor = System.Drawing.Color.White;
            this.listViewLog.Location = new System.Drawing.Point(3, 17);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.ShowItemToolTips = true;
            this.listViewLog.Size = new System.Drawing.Size(374, 493);
            this.listViewLog.TabIndex = 0;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // logTimeHeader
            // 
            this.logTimeHeader.Text = "时间";
            this.logTimeHeader.Width = 160;
            // 
            // logMessageHeader
            // 
            this.logMessageHeader.Text = "消息";
            this.logMessageHeader.Width = 250;
            // 
            // contextMenuStripLog
            // 
            this.contextMenuStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logClear});
            this.contextMenuStripLog.Name = "contextMenuStripLog";
            this.contextMenuStripLog.Size = new System.Drawing.Size(101, 26);
            // 
            // logClear
            // 
            this.logClear.Name = "logClear";
            this.logClear.Size = new System.Drawing.Size(100, 22);
            this.logClear.Text = "清除";
            this.logClear.Click += new System.EventHandler(this.logClear_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Location = new System.Drawing.Point(5, 611);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(913, 170);
            this.panel2.TabIndex = 14;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listBoxResult);
            this.groupBox4.Controls.Add(this.labelResult);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(913, 170);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "结果";
            // 
            // listBoxResult
            // 
            this.listBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxResult.ForeColor = System.Drawing.Color.White;
            this.listBoxResult.FormattingEnabled = true;
            this.listBoxResult.ItemHeight = 12;
            this.listBoxResult.Location = new System.Drawing.Point(3, 20);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(726, 132);
            this.listBoxResult.TabIndex = 2;
            // 
            // labelResult
            // 
            this.labelResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelResult.Font = new System.Drawing.Font("宋体", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelResult.ForeColor = System.Drawing.Color.White;
            this.labelResult.Location = new System.Drawing.Point(776, 48);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(100, 80);
            this.labelResult.TabIndex = 1;
            this.labelResult.Text = "OK";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonRender
            // 
            this.buttonRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRender.BackColor = System.Drawing.Color.Gray;
            this.buttonRender.FlatAppearance.BorderSize = 0;
            this.buttonRender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRender.ForeColor = System.Drawing.Color.White;
            this.buttonRender.Location = new System.Drawing.Point(927, 12);
            this.buttonRender.Name = "buttonRender";
            this.buttonRender.Size = new System.Drawing.Size(85, 25);
            this.buttonRender.TabIndex = 9;
            this.buttonRender.Text = "图像显示";
            this.buttonRender.UseVisualStyleBackColor = false;
            this.buttonRender.Click += new System.EventHandler(this.buttonRender_Click);
            // 
            // buttonConfig
            // 
            this.buttonConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfig.BackColor = System.Drawing.Color.Gray;
            this.buttonConfig.FlatAppearance.BorderSize = 0;
            this.buttonConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConfig.ForeColor = System.Drawing.Color.White;
            this.buttonConfig.Location = new System.Drawing.Point(1013, 12);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(85, 25);
            this.buttonConfig.TabIndex = 17;
            this.buttonConfig.Text = "参数配置";
            this.buttonConfig.UseVisualStyleBackColor = false;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            // 
            // buttonChineseOREnglish
            // 
            this.buttonChineseOREnglish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChineseOREnglish.BackColor = System.Drawing.Color.Gray;
            this.buttonChineseOREnglish.FlatAppearance.BorderSize = 0;
            this.buttonChineseOREnglish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChineseOREnglish.ForeColor = System.Drawing.Color.White;
            this.buttonChineseOREnglish.Location = new System.Drawing.Point(1104, 12);
            this.buttonChineseOREnglish.Name = "buttonChineseOREnglish";
            this.buttonChineseOREnglish.Size = new System.Drawing.Size(85, 25);
            this.buttonChineseOREnglish.TabIndex = 18;
            this.buttonChineseOREnglish.Text = "中文/英文";
            this.buttonChineseOREnglish.UseVisualStyleBackColor = false;
            this.buttonChineseOREnglish.Click += new System.EventHandler(this.buttonChineseOREnglish_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1322, 781);
            this.Controls.Add(this.buttonChineseOREnglish);
            this.Controls.Add(this.buttonConfig);
            this.Controls.Add(this.buttonRender);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "OCRDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.contextMenuStripLog.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonLoadSolu;
        private System.Windows.Forms.Button buttonSelectSolu;
        private System.Windows.Forms.Button buttonRunOnce;
        private System.Windows.Forms.Button buttonContiRun;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSaveSolu;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboProcedure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.ColumnHeader logTimeHeader;
        private System.Windows.Forms.ColumnHeader logMessageHeader;
        private System.Windows.Forms.ListBox listBoxResult;
        private System.Windows.Forms.Button buttonRender;
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLog;
        private System.Windows.Forms.ToolStripMenuItem logClear;
        private System.Windows.Forms.Button buttonChineseOREnglish;
    }
}

