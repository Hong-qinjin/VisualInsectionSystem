namespace VisualInsectionSystem.Forms
{
    partial class vmFrontendForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(vmFrontendForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxMsg = new System.Windows.Forms.GroupBox();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.buttonDeleteMsg = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBoxControl = new System.Windows.Forms.GroupBox();
            this.buttonExecuteOnce = new System.Windows.Forms.Button();
            this.buttonChangeSize2 = new System.Windows.Forms.Button();
            this.buttonChangeSize1 = new System.Windows.Forms.Button();
            this.buttonLoadFrontendData = new System.Windows.Forms.Button();
            this.groupBoxSolution = new System.Windows.Forms.GroupBox();
            this.buttonLoadSolution = new System.Windows.Forms.Button();
            this.buttonChooseSoluPath = new System.Windows.Forms.Button();
            this.progressBarSaveAndLoad = new System.Windows.Forms.ProgressBar();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxSolutionPath = new System.Windows.Forms.TextBox();
            this.labelProgress = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vmFrontendControl1 = new VMControls.Winform.Release.VmFrontendControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxMsg.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxControl.SuspendLayout();
            this.groupBoxSolution.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxMsg);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxSolution);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1193, 520);
            this.splitContainer1.SplitterDistance = 460;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBoxMsg
            // 
            this.groupBoxMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxMsg.Controls.Add(this.listBoxMsg);
            this.groupBoxMsg.Controls.Add(this.buttonDeleteMsg);
            this.groupBoxMsg.Location = new System.Drawing.Point(21, 276);
            this.groupBoxMsg.Name = "groupBoxMsg";
            this.groupBoxMsg.Size = new System.Drawing.Size(419, 220);
            this.groupBoxMsg.TabIndex = 8;
            this.groupBoxMsg.TabStop = false;
            this.groupBoxMsg.Text = "消息显示区";
            // 
            // listBoxMsg
            // 
            this.listBoxMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.ItemHeight = 12;
            this.listBoxMsg.Location = new System.Drawing.Point(22, 56);
            this.listBoxMsg.Name = "listBoxMsg";
            this.listBoxMsg.Size = new System.Drawing.Size(386, 136);
            this.listBoxMsg.TabIndex = 3;
            // 
            // buttonDeleteMsg
            // 
            this.buttonDeleteMsg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonDeleteMsg.Location = new System.Drawing.Point(304, 20);
            this.buttonDeleteMsg.Name = "buttonDeleteMsg";
            this.buttonDeleteMsg.Size = new System.Drawing.Size(105, 30);
            this.buttonDeleteMsg.TabIndex = 2;
            this.buttonDeleteMsg.Text = "清空消息";
            this.buttonDeleteMsg.UseVisualStyleBackColor = true;
            this.buttonDeleteMsg.Click += new System.EventHandler(this.buttonDeleteMsg_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBoxControl);
            this.groupBox2.Controls.Add(this.buttonChangeSize2);
            this.groupBox2.Controls.Add(this.buttonChangeSize1);
            this.groupBox2.Controls.Add(this.buttonLoadFrontendData);
            this.groupBox2.Location = new System.Drawing.Point(21, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(419, 116);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "运行界面控制区";
            // 
            // groupBoxControl
            // 
            this.groupBoxControl.Controls.Add(this.buttonExecuteOnce);
            this.groupBoxControl.Location = new System.Drawing.Point(329, 29);
            this.groupBoxControl.Name = "groupBoxControl";
            this.groupBoxControl.Size = new System.Drawing.Size(77, 66);
            this.groupBoxControl.TabIndex = 32;
            this.groupBoxControl.TabStop = false;
            this.groupBoxControl.Text = "单次执行";
            // 
            // buttonExecuteOnce
            // 
            this.buttonExecuteOnce.Image = ((System.Drawing.Image)(resources.GetObject("buttonExecuteOnce.Image")));
            this.buttonExecuteOnce.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonExecuteOnce.Location = new System.Drawing.Point(18, 20);
            this.buttonExecuteOnce.Name = "buttonExecuteOnce";
            this.buttonExecuteOnce.Size = new System.Drawing.Size(38, 36);
            this.buttonExecuteOnce.TabIndex = 18;
            this.buttonExecuteOnce.UseVisualStyleBackColor = true;
            // 
            // buttonChangeSize2
            // 
            this.buttonChangeSize2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonChangeSize2.Location = new System.Drawing.Point(147, 73);
            this.buttonChangeSize2.Name = "buttonChangeSize2";
            this.buttonChangeSize2.Size = new System.Drawing.Size(98, 28);
            this.buttonChangeSize2.TabIndex = 7;
            this.buttonChangeSize2.Text = "窗口放大";
            this.buttonChangeSize2.UseVisualStyleBackColor = true;
            this.buttonChangeSize2.Click += new System.EventHandler(this.buttonChangeSize2_Click);
            // 
            // buttonChangeSize1
            // 
            this.buttonChangeSize1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonChangeSize1.Location = new System.Drawing.Point(22, 73);
            this.buttonChangeSize1.Name = "buttonChangeSize1";
            this.buttonChangeSize1.Size = new System.Drawing.Size(98, 28);
            this.buttonChangeSize1.TabIndex = 7;
            this.buttonChangeSize1.Text = "窗口缩小";
            this.buttonChangeSize1.UseVisualStyleBackColor = true;
            this.buttonChangeSize1.Click += new System.EventHandler(this.buttonChangeSize1_Click);
            // 
            // buttonLoadFrontendData
            // 
            this.buttonLoadFrontendData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonLoadFrontendData.Location = new System.Drawing.Point(22, 29);
            this.buttonLoadFrontendData.Name = "buttonLoadFrontendData";
            this.buttonLoadFrontendData.Size = new System.Drawing.Size(98, 28);
            this.buttonLoadFrontendData.TabIndex = 7;
            this.buttonLoadFrontendData.Text = "加载界面数据";
            this.buttonLoadFrontendData.UseVisualStyleBackColor = true;
            this.buttonLoadFrontendData.Click += new System.EventHandler(this.buttonLoadFrontendData_Click);
            // 
            // groupBoxSolution
            // 
            this.groupBoxSolution.Controls.Add(this.buttonLoadSolution);
            this.groupBoxSolution.Controls.Add(this.buttonChooseSoluPath);
            this.groupBoxSolution.Controls.Add(this.progressBarSaveAndLoad);
            this.groupBoxSolution.Controls.Add(this.textBoxPassword);
            this.groupBoxSolution.Controls.Add(this.textBoxSolutionPath);
            this.groupBoxSolution.Controls.Add(this.labelProgress);
            this.groupBoxSolution.Controls.Add(this.label3);
            this.groupBoxSolution.Controls.Add(this.label2);
            this.groupBoxSolution.Controls.Add(this.label1);
            this.groupBoxSolution.Location = new System.Drawing.Point(21, 25);
            this.groupBoxSolution.Name = "groupBoxSolution";
            this.groupBoxSolution.Size = new System.Drawing.Size(419, 123);
            this.groupBoxSolution.TabIndex = 6;
            this.groupBoxSolution.TabStop = false;
            this.groupBoxSolution.Text = "方案操作接口";
            // 
            // buttonLoadSolution
            // 
            this.buttonLoadSolution.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonLoadSolution.Location = new System.Drawing.Point(310, 83);
            this.buttonLoadSolution.Name = "buttonLoadSolution";
            this.buttonLoadSolution.Size = new System.Drawing.Size(98, 28);
            this.buttonLoadSolution.TabIndex = 7;
            this.buttonLoadSolution.Text = "加载方案";
            this.buttonLoadSolution.UseVisualStyleBackColor = true;
            this.buttonLoadSolution.Click += new System.EventHandler(this.buttonLoadSolution_Click);
            // 
            // buttonChooseSoluPath
            // 
            this.buttonChooseSoluPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonChooseSoluPath.Location = new System.Drawing.Point(310, 20);
            this.buttonChooseSoluPath.Name = "buttonChooseSoluPath";
            this.buttonChooseSoluPath.Size = new System.Drawing.Size(98, 28);
            this.buttonChooseSoluPath.TabIndex = 7;
            this.buttonChooseSoluPath.Text = "选择方案路径";
            this.buttonChooseSoluPath.UseVisualStyleBackColor = true;
            this.buttonChooseSoluPath.Click += new System.EventHandler(this.buttonChooseSoluPath_Click);
            // 
            // progressBarSaveAndLoad
            // 
            this.progressBarSaveAndLoad.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.progressBarSaveAndLoad.Location = new System.Drawing.Point(97, 85);
            this.progressBarSaveAndLoad.Name = "progressBarSaveAndLoad";
            this.progressBarSaveAndLoad.Size = new System.Drawing.Size(198, 25);
            this.progressBarSaveAndLoad.TabIndex = 6;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(96, 54);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(199, 21);
            this.textBoxPassword.TabIndex = 5;
            // 
            // textBoxSolutionPath
            // 
            this.textBoxSolutionPath.Location = new System.Drawing.Point(96, 23);
            this.textBoxSolutionPath.Name = "textBoxSolutionPath";
            this.textBoxSolutionPath.Size = new System.Drawing.Size(199, 21);
            this.textBoxSolutionPath.TabIndex = 4;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelProgress.Location = new System.Drawing.Point(70, 90);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(11, 12);
            this.labelProgress.TabIndex = 3;
            this.labelProgress.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(20, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "进度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(20, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "方案密码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(20, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "方案路径：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.vmFrontendControl1);
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(719, 520);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "运行界面渲染区";
            // 
            // vmFrontendControl1
            // 
            this.vmFrontendControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vmFrontendControl1.Location = new System.Drawing.Point(4, 16);
            this.vmFrontendControl1.Name = "vmFrontendControl1";
            this.vmFrontendControl1.Size = new System.Drawing.Size(712, 492);
            this.vmFrontendControl1.TabIndex = 0;
            // 
            // vmFrontendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 520);
            this.Controls.Add(this.splitContainer1);
            this.Name = "vmFrontendForm";
            this.Text = "vmFrontendForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxMsg.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBoxControl.ResumeLayout(false);
            this.groupBoxSolution.ResumeLayout(false);
            this.groupBoxSolution.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxMsg;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.Button buttonDeleteMsg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBoxControl;
        private System.Windows.Forms.Button buttonExecuteOnce;
        private System.Windows.Forms.Button buttonChangeSize2;
        private System.Windows.Forms.Button buttonChangeSize1;
        private System.Windows.Forms.Button buttonLoadFrontendData;
        private System.Windows.Forms.GroupBox groupBoxSolution;
        private System.Windows.Forms.Button buttonLoadSolution;
        private System.Windows.Forms.Button buttonChooseSoluPath;
        private System.Windows.Forms.ProgressBar progressBarSaveAndLoad;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxSolutionPath;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private VMControls.Winform.Release.VmFrontendControl vmFrontendControl1;
    }
}