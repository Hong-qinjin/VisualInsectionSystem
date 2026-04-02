namespace VisualInsectionSystem
{
    partial class DebugForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            this.groupBoxResult = new System.Windows.Forms.GroupBox();
            this.listBoxResult = new System.Windows.Forms.ListBox();
            this.labelResultState = new System.Windows.Forms.Label();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.buttonConfig = new System.Windows.Forms.Button();
            this.buttonRender = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboProcedure = new System.Windows.Forms.ComboBox();
            this.groupBoxProcedure = new System.Windows.Forms.GroupBox();
            this.buttonContiRun = new System.Windows.Forms.Button();
            this.buttonRunOnce = new System.Windows.Forms.Button();
            this.infoHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timeStampHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewLog = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSaveSolu = new System.Windows.Forms.Button();
            this.buttonLoadSolu = new System.Windows.Forms.Button();
            this.buttonSelectSolu = new System.Windows.Forms.Button();
            this.groupBoxSolution = new System.Windows.Forms.GroupBox();
            this.renderPanel = new System.Windows.Forms.Panel();
            this.buttonChineseOREnglish = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxResult.SuspendLayout();
            this.resultPanel.SuspendLayout();
            this.groupBoxProcedure.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxSolution.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxResult
            // 
            this.groupBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxResult.Controls.Add(this.listBoxResult);
            this.groupBoxResult.Controls.Add(this.labelResultState);
            this.groupBoxResult.ForeColor = System.Drawing.Color.White;
            this.groupBoxResult.Location = new System.Drawing.Point(5, 0);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(861, 139);
            this.groupBoxResult.TabIndex = 9;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "结果";
            // 
            // listBoxResult
            // 
            this.listBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxResult.ForeColor = System.Drawing.Color.White;
            this.listBoxResult.FormattingEnabled = true;
            this.listBoxResult.ItemHeight = 12;
            this.listBoxResult.Location = new System.Drawing.Point(0, 20);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(706, 96);
            this.listBoxResult.TabIndex = 8;
            // 
            // labelResultState
            // 
            this.labelResultState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelResultState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelResultState.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelResultState.ForeColor = System.Drawing.Color.White;
            this.labelResultState.Location = new System.Drawing.Point(735, 20);
            this.labelResultState.Name = "labelResultState";
            this.labelResultState.Size = new System.Drawing.Size(100, 96);
            this.labelResultState.TabIndex = 7;
            this.labelResultState.Text = "OK";
            this.labelResultState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // resultPanel
            // 
            this.resultPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.resultPanel.Controls.Add(this.groupBoxResult);
            this.resultPanel.Location = new System.Drawing.Point(12, 608);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(868, 142);
            this.resultPanel.TabIndex = 8;
            // 
            // buttonConfig
            // 
            this.buttonConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfig.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonConfig.FlatAppearance.BorderSize = 0;
            this.buttonConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConfig.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonConfig.ForeColor = System.Drawing.SystemColors.Desktop;
            this.buttonConfig.Location = new System.Drawing.Point(133, 20);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(100, 40);
            this.buttonConfig.TabIndex = 6;
            this.buttonConfig.Text = "参数配置";
            this.buttonConfig.UseVisualStyleBackColor = false;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            // 
            // buttonRender
            // 
            this.buttonRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRender.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonRender.FlatAppearance.BorderSize = 0;
            this.buttonRender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRender.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonRender.ForeColor = System.Drawing.SystemColors.Desktop;
            this.buttonRender.Location = new System.Drawing.Point(15, 20);
            this.buttonRender.Name = "buttonRender";
            this.buttonRender.Size = new System.Drawing.Size(100, 40);
            this.buttonRender.TabIndex = 5;
            this.buttonRender.Text = "图像显示";
            this.buttonRender.UseVisualStyleBackColor = false;
            this.buttonRender.Click += new System.EventHandler(this.buttonRender_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择流程";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboProcedure
            // 
            this.comboProcedure.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProcedure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboProcedure.Font = new System.Drawing.Font("宋体", 9F);
            this.comboProcedure.ForeColor = System.Drawing.Color.Black;
            this.comboProcedure.FormattingEnabled = true;
            this.comboProcedure.Location = new System.Drawing.Point(135, 29);
            this.comboProcedure.Name = "comboProcedure";
            this.comboProcedure.Size = new System.Drawing.Size(237, 20);
            this.comboProcedure.TabIndex = 0;
            this.comboProcedure.SelectedIndexChanged += new System.EventHandler(this.comboProcedure_SelectedIndexChanged);
            // 
            // groupBoxProcedure
            // 
            this.groupBoxProcedure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProcedure.Controls.Add(this.label1);
            this.groupBoxProcedure.Controls.Add(this.buttonContiRun);
            this.groupBoxProcedure.Controls.Add(this.comboProcedure);
            this.groupBoxProcedure.Controls.Add(this.buttonRunOnce);
            this.groupBoxProcedure.ForeColor = System.Drawing.Color.Black;
            this.groupBoxProcedure.Location = new System.Drawing.Point(884, 242);
            this.groupBoxProcedure.Name = "groupBoxProcedure";
            this.groupBoxProcedure.Size = new System.Drawing.Size(392, 116);
            this.groupBoxProcedure.TabIndex = 4;
            this.groupBoxProcedure.TabStop = false;
            this.groupBoxProcedure.Text = "流程操作";
            // 
            // buttonContiRun
            // 
            this.buttonContiRun.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonContiRun.FlatAppearance.BorderSize = 0;
            this.buttonContiRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonContiRun.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonContiRun.ForeColor = System.Drawing.SystemColors.Desktop;
            this.buttonContiRun.Location = new System.Drawing.Point(135, 70);
            this.buttonContiRun.Name = "buttonContiRun";
            this.buttonContiRun.Size = new System.Drawing.Size(100, 40);
            this.buttonContiRun.TabIndex = 1;
            this.buttonContiRun.Text = "连续运行";
            this.buttonContiRun.UseVisualStyleBackColor = false;
            this.buttonContiRun.Click += new System.EventHandler(this.buttonContiRun_Click);
            // 
            // buttonRunOnce
            // 
            this.buttonRunOnce.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonRunOnce.FlatAppearance.BorderSize = 0;
            this.buttonRunOnce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRunOnce.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonRunOnce.ForeColor = System.Drawing.SystemColors.Desktop;
            this.buttonRunOnce.Location = new System.Drawing.Point(17, 70);
            this.buttonRunOnce.Name = "buttonRunOnce";
            this.buttonRunOnce.Size = new System.Drawing.Size(100, 40);
            this.buttonRunOnce.TabIndex = 0;
            this.buttonRunOnce.Text = "运行一次";
            this.buttonRunOnce.UseVisualStyleBackColor = false;
            this.buttonRunOnce.Click += new System.EventHandler(this.buttonRunOnce_Click);
            // 
            // infoHeader
            // 
            this.infoHeader.Text = "消息";
            this.infoHeader.Width = 267;
            // 
            // timeStampHeader
            // 
            this.timeStampHeader.Text = "时间";
            this.timeStampHeader.Width = 98;
            // 
            // listViewLog
            // 
            this.listViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLog.BackColor = System.Drawing.SystemColors.Control;
            this.listViewLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timeStampHeader,
            this.infoHeader});
            this.listViewLog.ForeColor = System.Drawing.Color.Black;
            this.listViewLog.HideSelection = false;
            this.listViewLog.Location = new System.Drawing.Point(16, 20);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.ShowItemToolTips = true;
            this.listViewLog.Size = new System.Drawing.Size(366, 359);
            this.listViewLog.TabIndex = 0;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listViewLog);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(884, 364);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 386);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "日志消息";
            // 
            // buttonSaveSolu
            // 
            this.buttonSaveSolu.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonSaveSolu.FlatAppearance.BorderSize = 0;
            this.buttonSaveSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveSolu.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonSaveSolu.ForeColor = System.Drawing.SystemColors.Desktop;
            this.buttonSaveSolu.Location = new System.Drawing.Point(262, 20);
            this.buttonSaveSolu.Name = "buttonSaveSolu";
            this.buttonSaveSolu.Size = new System.Drawing.Size(100, 40);
            this.buttonSaveSolu.TabIndex = 2;
            this.buttonSaveSolu.Text = "保存方案";
            this.buttonSaveSolu.UseVisualStyleBackColor = false;
            this.buttonSaveSolu.Click += new System.EventHandler(this.buttonSaveSolu_Click);
            // 
            // buttonLoadSolu
            // 
            this.buttonLoadSolu.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonLoadSolu.FlatAppearance.BorderSize = 0;
            this.buttonLoadSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadSolu.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonLoadSolu.ForeColor = System.Drawing.SystemColors.Desktop;
            this.buttonLoadSolu.Location = new System.Drawing.Point(135, 20);
            this.buttonLoadSolu.Name = "buttonLoadSolu";
            this.buttonLoadSolu.Size = new System.Drawing.Size(100, 40);
            this.buttonLoadSolu.TabIndex = 1;
            this.buttonLoadSolu.Text = "加载方案";
            this.buttonLoadSolu.UseVisualStyleBackColor = false;
            this.buttonLoadSolu.Click += new System.EventHandler(this.buttonLoadSolu_Click);
            // 
            // buttonSelectSolu
            // 
            this.buttonSelectSolu.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonSelectSolu.FlatAppearance.BorderSize = 0;
            this.buttonSelectSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectSolu.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonSelectSolu.ForeColor = System.Drawing.SystemColors.Desktop;
            this.buttonSelectSolu.Location = new System.Drawing.Point(17, 20);
            this.buttonSelectSolu.Name = "buttonSelectSolu";
            this.buttonSelectSolu.Size = new System.Drawing.Size(100, 40);
            this.buttonSelectSolu.TabIndex = 0;
            this.buttonSelectSolu.Text = "选择方案";
            this.buttonSelectSolu.UseVisualStyleBackColor = false;
            this.buttonSelectSolu.Click += new System.EventHandler(this.buttonSelectSolu_Click);
            // 
            // groupBoxSolution
            // 
            this.groupBoxSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSolution.BackColor = System.Drawing.SystemColors.Window;
            this.groupBoxSolution.Controls.Add(this.buttonSaveSolu);
            this.groupBoxSolution.Controls.Add(this.buttonLoadSolu);
            this.groupBoxSolution.Controls.Add(this.buttonSelectSolu);
            this.groupBoxSolution.ForeColor = System.Drawing.Color.Black;
            this.groupBoxSolution.Location = new System.Drawing.Point(884, 154);
            this.groupBoxSolution.Name = "groupBoxSolution";
            this.groupBoxSolution.Size = new System.Drawing.Size(392, 82);
            this.groupBoxSolution.TabIndex = 2;
            this.groupBoxSolution.TabStop = false;
            this.groupBoxSolution.Text = "方案操作";
            // 
            // renderPanel
            // 
            this.renderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.renderPanel.Location = new System.Drawing.Point(12, 7);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(868, 598);
            this.renderPanel.TabIndex = 0;
            // 
            // buttonChineseOREnglish
            // 
            this.buttonChineseOREnglish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChineseOREnglish.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonChineseOREnglish.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonChineseOREnglish.ForeColor = System.Drawing.SystemColors.Desktop;
            this.buttonChineseOREnglish.Location = new System.Drawing.Point(260, 20);
            this.buttonChineseOREnglish.Name = "buttonChineseOREnglish";
            this.buttonChineseOREnglish.Size = new System.Drawing.Size(100, 40);
            this.buttonChineseOREnglish.TabIndex = 9;
            this.buttonChineseOREnglish.Text = "中/英";
            this.buttonChineseOREnglish.UseVisualStyleBackColor = false;
            this.buttonChineseOREnglish.Click += new System.EventHandler(this.buttonChineseOREnglish_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.Menu;
            this.button4.Font = new System.Drawing.Font("宋体", 10F);
            this.button4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.button4.Location = new System.Drawing.Point(14, 82);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 40);
            this.button4.TabIndex = 20;
            this.button4.Text = "锁  定";
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Menu;
            this.button5.Font = new System.Drawing.Font("宋体", 10F);
            this.button5.ForeColor = System.Drawing.SystemColors.Desktop;
            this.button5.Location = new System.Drawing.Point(133, 82);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 40);
            this.button5.TabIndex = 25;
            this.button5.Text = "解  锁";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.buttonChineseOREnglish);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.buttonRender);
            this.groupBox1.Controls.Add(this.buttonConfig);
            this.groupBox1.Location = new System.Drawing.Point(886, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 141);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "页面控制";
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1278, 762);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxSolution);
            this.Controls.Add(this.resultPanel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.renderPanel);
            this.Controls.Add(this.groupBoxProcedure);
            this.Font = new System.Drawing.Font("宋体", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DebugForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "调试界面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugForm_FormClosing);
            this.Load += new System.EventHandler(this.DebugForm_Load);
            this.groupBoxResult.ResumeLayout(false);
            this.resultPanel.ResumeLayout(false);
            this.groupBoxProcedure.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBoxSolution.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.ListBox listBoxResult;
        private System.Windows.Forms.Label labelResultState;
        private System.Windows.Forms.Panel resultPanel;
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Button buttonRender;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboProcedure;
        private System.Windows.Forms.GroupBox groupBoxProcedure;
        private System.Windows.Forms.Button buttonContiRun;
        private System.Windows.Forms.Button buttonRunOnce;
        private System.Windows.Forms.ColumnHeader infoHeader;
        private System.Windows.Forms.ColumnHeader timeStampHeader;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonSaveSolu;
        private System.Windows.Forms.Button buttonLoadSolu;
        private System.Windows.Forms.Button buttonSelectSolu;
        private System.Windows.Forms.GroupBox groupBoxSolution;
        private System.Windows.Forms.Panel renderPanel;
        private System.Windows.Forms.Button buttonChineseOREnglish;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}