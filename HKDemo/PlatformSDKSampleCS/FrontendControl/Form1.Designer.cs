namespace FrontendControl
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBoxControl = new System.Windows.Forms.GroupBox();
            this.buttonExecuteOnce = new System.Windows.Forms.Button();
            this.buttonChangeSize2 = new System.Windows.Forms.Button();
            this.buttonChangeSize1 = new System.Windows.Forms.Button();
            this.buttonLoadFrontendData = new System.Windows.Forms.Button();
            this.groupBoxMsg = new System.Windows.Forms.GroupBox();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.buttonDeleteMsg = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vmFrontendControl1 = new VMControls.Winform.Release.VmFrontendControl();
            this.buttonLang = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxSolution.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxControl.SuspendLayout();
            this.groupBoxMsg.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxSolution);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxMsg);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
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
            resources.ApplyResources(this.groupBoxSolution, "groupBoxSolution");
            this.groupBoxSolution.Name = "groupBoxSolution";
            this.groupBoxSolution.TabStop = false;
            // 
            // buttonLoadSolution
            // 
            resources.ApplyResources(this.buttonLoadSolution, "buttonLoadSolution");
            this.buttonLoadSolution.Name = "buttonLoadSolution";
            this.buttonLoadSolution.UseVisualStyleBackColor = true;
            this.buttonLoadSolution.Click += new System.EventHandler(this.buttonLoadSolution_Click);
            // 
            // buttonChooseSoluPath
            // 
            resources.ApplyResources(this.buttonChooseSoluPath, "buttonChooseSoluPath");
            this.buttonChooseSoluPath.Name = "buttonChooseSoluPath";
            this.buttonChooseSoluPath.UseVisualStyleBackColor = true;
            this.buttonChooseSoluPath.Click += new System.EventHandler(this.buttonChooseSoluPath_Click);
            // 
            // progressBarSaveAndLoad
            // 
            resources.ApplyResources(this.progressBarSaveAndLoad, "progressBarSaveAndLoad");
            this.progressBarSaveAndLoad.Name = "progressBarSaveAndLoad";
            // 
            // textBoxPassword
            // 
            resources.ApplyResources(this.textBoxPassword, "textBoxPassword");
            this.textBoxPassword.Name = "textBoxPassword";
            // 
            // textBoxSolutionPath
            // 
            resources.ApplyResources(this.textBoxSolutionPath, "textBoxSolutionPath");
            this.textBoxSolutionPath.Name = "textBoxSolutionPath";
            // 
            // labelProgress
            // 
            resources.ApplyResources(this.labelProgress, "labelProgress");
            this.labelProgress.Name = "labelProgress";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBoxControl);
            this.groupBox2.Controls.Add(this.buttonChangeSize2);
            this.groupBox2.Controls.Add(this.buttonChangeSize1);
            this.groupBox2.Controls.Add(this.buttonLoadFrontendData);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBoxControl
            // 
            this.groupBoxControl.Controls.Add(this.buttonExecuteOnce);
            resources.ApplyResources(this.groupBoxControl, "groupBoxControl");
            this.groupBoxControl.Name = "groupBoxControl";
            this.groupBoxControl.TabStop = false;
            // 
            // buttonExecuteOnce
            // 
            resources.ApplyResources(this.buttonExecuteOnce, "buttonExecuteOnce");
            this.buttonExecuteOnce.Name = "buttonExecuteOnce";
            this.buttonExecuteOnce.UseVisualStyleBackColor = true;
            this.buttonExecuteOnce.Click += new System.EventHandler(this.buttonExecuteOnce_Click);
            // 
            // buttonChangeSize2
            // 
            resources.ApplyResources(this.buttonChangeSize2, "buttonChangeSize2");
            this.buttonChangeSize2.Name = "buttonChangeSize2";
            this.buttonChangeSize2.UseVisualStyleBackColor = true;
            this.buttonChangeSize2.Click += new System.EventHandler(this.buttonChangeSize2_Click);
            // 
            // buttonChangeSize1
            // 
            resources.ApplyResources(this.buttonChangeSize1, "buttonChangeSize1");
            this.buttonChangeSize1.Name = "buttonChangeSize1";
            this.buttonChangeSize1.UseVisualStyleBackColor = true;
            this.buttonChangeSize1.Click += new System.EventHandler(this.buttonChangeSize1_Click);
            // 
            // buttonLoadFrontendData
            // 
            resources.ApplyResources(this.buttonLoadFrontendData, "buttonLoadFrontendData");
            this.buttonLoadFrontendData.Name = "buttonLoadFrontendData";
            this.buttonLoadFrontendData.UseVisualStyleBackColor = true;
            this.buttonLoadFrontendData.Click += new System.EventHandler(this.buttonLoadFrontendData_Click);
            // 
            // groupBoxMsg
            // 
            resources.ApplyResources(this.groupBoxMsg, "groupBoxMsg");
            this.groupBoxMsg.Controls.Add(this.buttonLang);
            this.groupBoxMsg.Controls.Add(this.listBoxMsg);
            this.groupBoxMsg.Controls.Add(this.buttonDeleteMsg);
            this.groupBoxMsg.Name = "groupBoxMsg";
            this.groupBoxMsg.TabStop = false;
            // 
            // listBoxMsg
            // 
            resources.ApplyResources(this.listBoxMsg, "listBoxMsg");
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.Name = "listBoxMsg";
            // 
            // buttonDeleteMsg
            // 
            resources.ApplyResources(this.buttonDeleteMsg, "buttonDeleteMsg");
            this.buttonDeleteMsg.Name = "buttonDeleteMsg";
            this.buttonDeleteMsg.UseVisualStyleBackColor = true;
            this.buttonDeleteMsg.Click += new System.EventHandler(this.buttonDeleteMsg_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.vmFrontendControl1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // vmFrontendControl1
            // 
            resources.ApplyResources(this.vmFrontendControl1, "vmFrontendControl1");
            this.vmFrontendControl1.Name = "vmFrontendControl1";
            // 
            // buttonLang
            // 
            resources.ApplyResources(this.buttonLang, "buttonLang");
            this.buttonLang.Name = "buttonLang";
            this.buttonLang.UseVisualStyleBackColor = true;
            this.buttonLang.Click += new System.EventHandler(this.buttonLang_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxSolution.ResumeLayout(false);
            this.groupBoxSolution.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBoxControl.ResumeLayout(false);
            this.groupBoxMsg.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSolution;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonLoadSolution;
        private System.Windows.Forms.Button buttonChooseSoluPath;
        private System.Windows.Forms.ProgressBar progressBarSaveAndLoad;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxSolutionPath;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.Button buttonDeleteMsg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonChangeSize2;
        private System.Windows.Forms.Button buttonChangeSize1;
        private System.Windows.Forms.Button buttonLoadFrontendData;
        private System.Windows.Forms.GroupBox groupBoxControl;
        private System.Windows.Forms.Button buttonExecuteOnce;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VMControls.Winform.Release.VmFrontendControl vmFrontendControl1;
        private System.Windows.Forms.Button buttonLang;
    }
}
