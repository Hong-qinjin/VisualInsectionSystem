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
            this.components = new System.ComponentModel.Container();
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemClearMsg = new System.Windows.Forms.ToolStripMenuItem();
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
            this.groupBoxResult.SuspendLayout();
            this.resultPanel.SuspendLayout();
            this.groupBoxProcedure.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxSolution.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxResult
            // 
            this.groupBoxResult.Controls.Add(this.listBoxResult);
            this.groupBoxResult.Controls.Add(this.labelResultState);
            this.groupBoxResult.ForeColor = System.Drawing.Color.White;
            this.groupBoxResult.Location = new System.Drawing.Point(5, 0);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(861, 139);
            this.groupBoxResult.TabIndex = 9;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "结果";
            this.groupBoxResult.Enter += new System.EventHandler(this.groupBoxResult_Enter);
            // 
            // listBoxResult
            // 
            this.listBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxResult.ForeColor = System.Drawing.Color.White;
            this.listBoxResult.FormattingEnabled = true;
            this.listBoxResult.ItemHeight = 12;
            this.listBoxResult.Location = new System.Drawing.Point(6, 20);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(706, 96);
            this.listBoxResult.TabIndex = 8;
            this.listBoxResult.SelectedIndexChanged += new System.EventHandler(this.listBoxResult_SelectedIndexChanged);
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
            this.labelResultState.Click += new System.EventHandler(this.labelResultState_Click);
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
            this.buttonConfig.BackColor = System.Drawing.Color.White;
            this.buttonConfig.FlatAppearance.BorderSize = 0;
            this.buttonConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConfig.ForeColor = System.Drawing.Color.Black;
            this.buttonConfig.Location = new System.Drawing.Point(973, 7);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(85, 25);
            this.buttonConfig.TabIndex = 6;
            this.buttonConfig.Text = "参数配置";
            this.buttonConfig.UseVisualStyleBackColor = false;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            // 
            // buttonRender
            // 
            this.buttonRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRender.BackColor = System.Drawing.Color.White;
            this.buttonRender.FlatAppearance.BorderSize = 0;
            this.buttonRender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRender.ForeColor = System.Drawing.Color.Black;
            this.buttonRender.Location = new System.Drawing.Point(884, 7);
            this.buttonRender.Name = "buttonRender";
            this.buttonRender.Size = new System.Drawing.Size(82, 25);
            this.buttonRender.TabIndex = 5;
            this.buttonRender.Text = "图像显示";
            this.buttonRender.UseVisualStyleBackColor = false;
            this.buttonRender.Click += new System.EventHandler(this.buttonRender_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择流程";
            // 
            // comboProcedure
            // 
            this.comboProcedure.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProcedure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboProcedure.Font = new System.Drawing.Font("宋体", 9F);
            this.comboProcedure.ForeColor = System.Drawing.Color.Black;
            this.comboProcedure.FormattingEnabled = true;
            this.comboProcedure.Location = new System.Drawing.Point(6, 49);
            this.comboProcedure.Name = "comboProcedure";
            this.comboProcedure.Size = new System.Drawing.Size(121, 20);
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
            this.groupBoxProcedure.Location = new System.Drawing.Point(884, 208);
            this.groupBoxProcedure.Name = "groupBoxProcedure";
            this.groupBoxProcedure.Size = new System.Drawing.Size(392, 96);
            this.groupBoxProcedure.TabIndex = 4;
            this.groupBoxProcedure.TabStop = false;
            this.groupBoxProcedure.Text = "流程操作";
            // 
            // buttonContiRun
            // 
            this.buttonContiRun.BackColor = System.Drawing.Color.White;
            this.buttonContiRun.FlatAppearance.BorderSize = 0;
            this.buttonContiRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonContiRun.ForeColor = System.Drawing.Color.Black;
            this.buttonContiRun.Location = new System.Drawing.Point(262, 31);
            this.buttonContiRun.Name = "buttonContiRun";
            this.buttonContiRun.Size = new System.Drawing.Size(110, 42);
            this.buttonContiRun.TabIndex = 1;
            this.buttonContiRun.Text = "连续运行";
            this.buttonContiRun.UseVisualStyleBackColor = false;
            this.buttonContiRun.Click += new System.EventHandler(this.buttonContiRun_Click);
            // 
            // buttonRunOnce
            // 
            this.buttonRunOnce.BackColor = System.Drawing.Color.White;
            this.buttonRunOnce.FlatAppearance.BorderSize = 0;
            this.buttonRunOnce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRunOnce.ForeColor = System.Drawing.Color.Black;
            this.buttonRunOnce.Location = new System.Drawing.Point(135, 31);
            this.buttonRunOnce.Name = "buttonRunOnce";
            this.buttonRunOnce.Size = new System.Drawing.Size(110, 42);
            this.buttonRunOnce.TabIndex = 0;
            this.buttonRunOnce.Text = "运行一次";
            this.buttonRunOnce.UseVisualStyleBackColor = false;
            this.buttonRunOnce.Click += new System.EventHandler(this.buttonRunOnce_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemClearMsg});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // ToolStripMenuItemClearMsg
            // 
            this.ToolStripMenuItemClearMsg.Name = "ToolStripMenuItemClearMsg";
            this.ToolStripMenuItemClearMsg.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemClearMsg.Text = "清除";
            this.ToolStripMenuItemClearMsg.Click += new System.EventHandler(this.ToolStripMenuItemClearMsg_Click);
            // 
            // infoHeader
            // 
            this.infoHeader.Text = "消息";
            this.infoHeader.Width = 120;
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
            this.infoHeader,
            this.timeStampHeader});
            this.listViewLog.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewLog.ForeColor = System.Drawing.Color.Black;
            this.listViewLog.HideSelection = false;
            this.listViewLog.Location = new System.Drawing.Point(17, 20);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.ShowItemToolTips = true;
            this.listViewLog.Size = new System.Drawing.Size(366, 413);
            this.listViewLog.TabIndex = 0;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            this.listViewLog.SelectedIndexChanged += new System.EventHandler(this.listViewLog_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listViewLog);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(884, 310);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 440);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "日志消息";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // buttonSaveSolu
            // 
            this.buttonSaveSolu.BackColor = System.Drawing.Color.White;
            this.buttonSaveSolu.FlatAppearance.BorderSize = 0;
            this.buttonSaveSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveSolu.ForeColor = System.Drawing.Color.Black;
            this.buttonSaveSolu.Location = new System.Drawing.Point(262, 20);
            this.buttonSaveSolu.Name = "buttonSaveSolu";
            this.buttonSaveSolu.Size = new System.Drawing.Size(110, 41);
            this.buttonSaveSolu.TabIndex = 2;
            this.buttonSaveSolu.Text = "保存方案";
            this.buttonSaveSolu.UseVisualStyleBackColor = false;
            this.buttonSaveSolu.Click += new System.EventHandler(this.buttonSaveSolu_Click);
            // 
            // buttonLoadSolu
            // 
            this.buttonLoadSolu.BackColor = System.Drawing.Color.White;
            this.buttonLoadSolu.FlatAppearance.BorderSize = 0;
            this.buttonLoadSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadSolu.ForeColor = System.Drawing.Color.Black;
            this.buttonLoadSolu.Location = new System.Drawing.Point(135, 20);
            this.buttonLoadSolu.Name = "buttonLoadSolu";
            this.buttonLoadSolu.Size = new System.Drawing.Size(110, 41);
            this.buttonLoadSolu.TabIndex = 1;
            this.buttonLoadSolu.Text = "加载方案";
            this.buttonLoadSolu.UseVisualStyleBackColor = false;
            this.buttonLoadSolu.Click += new System.EventHandler(this.buttonLoadSolu_Click);
            // 
            // buttonSelectSolu
            // 
            this.buttonSelectSolu.BackColor = System.Drawing.Color.White;
            this.buttonSelectSolu.FlatAppearance.BorderSize = 0;
            this.buttonSelectSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectSolu.ForeColor = System.Drawing.Color.Black;
            this.buttonSelectSolu.Location = new System.Drawing.Point(17, 20);
            this.buttonSelectSolu.Name = "buttonSelectSolu";
            this.buttonSelectSolu.Size = new System.Drawing.Size(110, 41);
            this.buttonSelectSolu.TabIndex = 0;
            this.buttonSelectSolu.Text = "选择方案";
            this.buttonSelectSolu.UseVisualStyleBackColor = false;
            this.buttonSelectSolu.Click += new System.EventHandler(this.buttonSelectSolu_Click);
            // 
            // groupBoxSolution
            // 
            this.groupBoxSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSolution.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxSolution.Controls.Add(this.buttonSaveSolu);
            this.groupBoxSolution.Controls.Add(this.buttonLoadSolu);
            this.groupBoxSolution.Controls.Add(this.buttonSelectSolu);
            this.groupBoxSolution.ForeColor = System.Drawing.Color.Black;
            this.groupBoxSolution.Location = new System.Drawing.Point(884, 115);
            this.groupBoxSolution.Name = "groupBoxSolution";
            this.groupBoxSolution.Size = new System.Drawing.Size(392, 87);
            this.groupBoxSolution.TabIndex = 2;
            this.groupBoxSolution.TabStop = false;
            this.groupBoxSolution.Text = "方案操作";
            this.groupBoxSolution.Enter += new System.EventHandler(this.groupBoxSolution_Enter);
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
            this.renderPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.renderPanel_Paint);
            // 
            // buttonChineseOREnglish
            // 
            this.buttonChineseOREnglish.BackColor = System.Drawing.Color.White;
            this.buttonChineseOREnglish.ForeColor = System.Drawing.Color.Black;
            this.buttonChineseOREnglish.Location = new System.Drawing.Point(1064, 7);
            this.buttonChineseOREnglish.Name = "buttonChineseOREnglish";
            this.buttonChineseOREnglish.Size = new System.Drawing.Size(85, 25);
            this.buttonChineseOREnglish.TabIndex = 9;
            this.buttonChineseOREnglish.Text = "中/英";
            this.buttonChineseOREnglish.UseVisualStyleBackColor = false;
            this.buttonChineseOREnglish.Click += new System.EventHandler(this.buttonChineseOREnglish_Click);
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 762);
            this.Controls.Add(this.buttonChineseOREnglish);
            this.Controls.Add(this.groupBoxSolution);
            this.Controls.Add(this.resultPanel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.renderPanel);
            this.Controls.Add(this.buttonConfig);
            this.Controls.Add(this.groupBoxProcedure);
            this.Controls.Add(this.buttonRender);
            this.Font = new System.Drawing.Font("宋体", 9F);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DebugForm";
            this.Text = "调试界面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugForm_FormClosing);
            this.Load += new System.EventHandler(this.DebugForm_Load);
            this.groupBoxResult.ResumeLayout(false);
            this.resultPanel.ResumeLayout(false);
            this.groupBoxProcedure.ResumeLayout(false);
            this.groupBoxProcedure.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBoxSolution.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClearMsg;
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
    }
}