namespace ProcessControl
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBoxSolution = new System.Windows.Forms.GroupBox();
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBarSaveAndLoad = new System.Windows.Forms.ProgressBar();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxSolutionPath = new System.Windows.Forms.TextBox();
            this.buttonChooseSoluPath = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonLoadSolution = new System.Windows.Forms.Button();
            this.buttonSaveSolution = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxProcessName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonExportProcess = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxProcessPath = new System.Windows.Forms.TextBox();
            this.buttonChoosePrcPath = new System.Windows.Forms.Button();
            this.buttonImportProcess = new System.Windows.Forms.Button();
            this.buttonDeleteProcess = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vmProcedureConfigControl1 = new VMControls.Winform.Release.VmProcedureConfigControl();
            this.groupBoxMsg = new System.Windows.Forms.GroupBox();
            this.buttonLang = new System.Windows.Forms.Button();
            this.buttonDeleteMsg = new System.Windows.Forms.Button();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonShieldProcess = new System.Windows.Forms.Button();
            this.groupBoxTimeInterval = new System.Windows.Forms.GroupBox();
            this.textBoxTimeInterval = new System.Windows.Forms.TextBox();
            this.buttonSetTimeInterval = new System.Windows.Forms.Button();
            this.groupBoxControl = new System.Windows.Forms.GroupBox();
            this.buttonStopExecute = new System.Windows.Forms.Button();
            this.buttonExecuteOnce = new System.Windows.Forms.Button();
            this.buttonContinuExecute = new System.Windows.Forms.Button();
            this.groupBoxSolution.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxMsg.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBoxTimeInterval.SuspendLayout();
            this.groupBoxControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSolution
            // 
            resources.ApplyResources(this.groupBoxSolution, "groupBoxSolution");
            this.groupBoxSolution.Controls.Add(this.labelProgress);
            this.groupBoxSolution.Controls.Add(this.progressBarSaveAndLoad);
            this.groupBoxSolution.Controls.Add(this.label9);
            this.groupBoxSolution.Controls.Add(this.textBoxSolutionPath);
            this.groupBoxSolution.Controls.Add(this.buttonChooseSoluPath);
            this.groupBoxSolution.Controls.Add(this.label8);
            this.groupBoxSolution.Controls.Add(this.textBoxPassword);
            this.groupBoxSolution.Controls.Add(this.buttonLoadSolution);
            this.groupBoxSolution.Controls.Add(this.buttonSaveSolution);
            this.groupBoxSolution.Controls.Add(this.label11);
            this.groupBoxSolution.Name = "groupBoxSolution";
            this.groupBoxSolution.TabStop = false;
            // 
            // labelProgress
            // 
            resources.ApplyResources(this.labelProgress, "labelProgress");
            this.labelProgress.Name = "labelProgress";
            // 
            // progressBarSaveAndLoad
            // 
            resources.ApplyResources(this.progressBarSaveAndLoad, "progressBarSaveAndLoad");
            this.progressBarSaveAndLoad.Name = "progressBarSaveAndLoad";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // textBoxSolutionPath
            // 
            resources.ApplyResources(this.textBoxSolutionPath, "textBoxSolutionPath");
            this.textBoxSolutionPath.Name = "textBoxSolutionPath";
            // 
            // buttonChooseSoluPath
            // 
            resources.ApplyResources(this.buttonChooseSoluPath, "buttonChooseSoluPath");
            this.buttonChooseSoluPath.Name = "buttonChooseSoluPath";
            this.buttonChooseSoluPath.UseVisualStyleBackColor = true;
            this.buttonChooseSoluPath.Click += new System.EventHandler(this.buttonChooseSoluPath_Click);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // textBoxPassword
            // 
            resources.ApplyResources(this.textBoxPassword, "textBoxPassword");
            this.textBoxPassword.Name = "textBoxPassword";
            // 
            // buttonLoadSolution
            // 
            resources.ApplyResources(this.buttonLoadSolution, "buttonLoadSolution");
            this.buttonLoadSolution.Name = "buttonLoadSolution";
            this.buttonLoadSolution.UseVisualStyleBackColor = true;
            this.buttonLoadSolution.Click += new System.EventHandler(this.buttonLoadSolution_Click);
            // 
            // buttonSaveSolution
            // 
            resources.ApplyResources(this.buttonSaveSolution, "buttonSaveSolution");
            this.buttonSaveSolution.Name = "buttonSaveSolution";
            this.buttonSaveSolution.UseVisualStyleBackColor = true;
            this.buttonSaveSolution.Click += new System.EventHandler(this.buttonSaveSolution_Click);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.textBoxProcessName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonExportProcess);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxProcessPath);
            this.groupBox2.Controls.Add(this.buttonChoosePrcPath);
            this.groupBox2.Controls.Add(this.buttonImportProcess);
            this.groupBox2.Controls.Add(this.buttonDeleteProcess);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // textBoxProcessName
            // 
            resources.ApplyResources(this.textBoxProcessName, "textBoxProcessName");
            this.textBoxProcessName.Name = "textBoxProcessName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // buttonExportProcess
            // 
            resources.ApplyResources(this.buttonExportProcess, "buttonExportProcess");
            this.buttonExportProcess.Name = "buttonExportProcess";
            this.buttonExportProcess.UseVisualStyleBackColor = true;
            this.buttonExportProcess.Click += new System.EventHandler(this.buttonExportProcess_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBoxProcessPath
            // 
            resources.ApplyResources(this.textBoxProcessPath, "textBoxProcessPath");
            this.textBoxProcessPath.Name = "textBoxProcessPath";
            // 
            // buttonChoosePrcPath
            // 
            resources.ApplyResources(this.buttonChoosePrcPath, "buttonChoosePrcPath");
            this.buttonChoosePrcPath.Name = "buttonChoosePrcPath";
            this.buttonChoosePrcPath.UseVisualStyleBackColor = true;
            this.buttonChoosePrcPath.Click += new System.EventHandler(this.buttonChoosePrcPath_Click);
            // 
            // buttonImportProcess
            // 
            resources.ApplyResources(this.buttonImportProcess, "buttonImportProcess");
            this.buttonImportProcess.Name = "buttonImportProcess";
            this.buttonImportProcess.UseVisualStyleBackColor = true;
            this.buttonImportProcess.Click += new System.EventHandler(this.buttonImportProcess_Click);
            // 
            // buttonDeleteProcess
            // 
            resources.ApplyResources(this.buttonDeleteProcess, "buttonDeleteProcess");
            this.buttonDeleteProcess.Name = "buttonDeleteProcess";
            this.buttonDeleteProcess.UseVisualStyleBackColor = true;
            this.buttonDeleteProcess.Click += new System.EventHandler(this.buttonDeleteProcess_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.vmProcedureConfigControl1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // vmProcedureConfigControl1
            // 
            resources.ApplyResources(this.vmProcedureConfigControl1, "vmProcedureConfigControl1");
            this.vmProcedureConfigControl1.Name = "vmProcedureConfigControl1";
// TODO: “”的代码生成失败，原因是出现异常“无效的基元类型: System.IntPtr。请考虑使用 CodeObjectCreateExpression。”。
            // 
            // groupBoxMsg
            // 
            resources.ApplyResources(this.groupBoxMsg, "groupBoxMsg");
            this.groupBoxMsg.Controls.Add(this.buttonLang);
            this.groupBoxMsg.Controls.Add(this.buttonDeleteMsg);
            this.groupBoxMsg.Controls.Add(this.listBoxMsg);
            this.groupBoxMsg.Name = "groupBoxMsg";
            this.groupBoxMsg.TabStop = false;
            // 
            // buttonLang
            // 
            resources.ApplyResources(this.buttonLang, "buttonLang");
            this.buttonLang.Name = "buttonLang";
            this.buttonLang.UseVisualStyleBackColor = true;
            this.buttonLang.Click += new System.EventHandler(this.buttonLang_Click);
            // 
            // buttonDeleteMsg
            // 
            resources.ApplyResources(this.buttonDeleteMsg, "buttonDeleteMsg");
            this.buttonDeleteMsg.Name = "buttonDeleteMsg";
            this.buttonDeleteMsg.UseVisualStyleBackColor = true;
            this.buttonDeleteMsg.Click += new System.EventHandler(this.buttonDeleteMsg_Click);
            // 
            // listBoxMsg
            // 
            resources.ApplyResources(this.listBoxMsg, "listBoxMsg");
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.Name = "listBoxMsg";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.groupBoxTimeInterval);
            this.groupBox3.Controls.Add(this.groupBoxControl);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.buttonShieldProcess);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // buttonShieldProcess
            // 
            resources.ApplyResources(this.buttonShieldProcess, "buttonShieldProcess");
            this.buttonShieldProcess.Name = "buttonShieldProcess";
            this.buttonShieldProcess.UseVisualStyleBackColor = true;
            this.buttonShieldProcess.Click += new System.EventHandler(this.buttonShieldProcess_Click);
            // 
            // groupBoxTimeInterval
            // 
            resources.ApplyResources(this.groupBoxTimeInterval, "groupBoxTimeInterval");
            this.groupBoxTimeInterval.Controls.Add(this.textBoxTimeInterval);
            this.groupBoxTimeInterval.Controls.Add(this.buttonSetTimeInterval);
            this.groupBoxTimeInterval.Name = "groupBoxTimeInterval";
            this.groupBoxTimeInterval.TabStop = false;
            // 
            // textBoxTimeInterval
            // 
            resources.ApplyResources(this.textBoxTimeInterval, "textBoxTimeInterval");
            this.textBoxTimeInterval.Name = "textBoxTimeInterval";
            // 
            // buttonSetTimeInterval
            // 
            resources.ApplyResources(this.buttonSetTimeInterval, "buttonSetTimeInterval");
            this.buttonSetTimeInterval.Name = "buttonSetTimeInterval";
            this.buttonSetTimeInterval.UseVisualStyleBackColor = true;
            this.buttonSetTimeInterval.Click += new System.EventHandler(this.buttonSetTimeInterval_Click);
            // 
            // groupBoxControl
            // 
            resources.ApplyResources(this.groupBoxControl, "groupBoxControl");
            this.groupBoxControl.Controls.Add(this.buttonStopExecute);
            this.groupBoxControl.Controls.Add(this.buttonExecuteOnce);
            this.groupBoxControl.Controls.Add(this.buttonContinuExecute);
            this.groupBoxControl.Name = "groupBoxControl";
            this.groupBoxControl.TabStop = false;
            // 
            // buttonStopExecute
            // 
            resources.ApplyResources(this.buttonStopExecute, "buttonStopExecute");
            this.buttonStopExecute.Name = "buttonStopExecute";
            this.buttonStopExecute.UseVisualStyleBackColor = true;
            this.buttonStopExecute.Click += new System.EventHandler(this.buttonStopExecute_Click);
            // 
            // buttonExecuteOnce
            // 
            resources.ApplyResources(this.buttonExecuteOnce, "buttonExecuteOnce");
            this.buttonExecuteOnce.Name = "buttonExecuteOnce";
            this.buttonExecuteOnce.UseVisualStyleBackColor = true;
            this.buttonExecuteOnce.Click += new System.EventHandler(this.buttonExecuteOnce_Click);
            // 
            // buttonContinuExecute
            // 
            resources.ApplyResources(this.buttonContinuExecute, "buttonContinuExecute");
            this.buttonContinuExecute.Name = "buttonContinuExecute";
            this.buttonContinuExecute.UseVisualStyleBackColor = true;
            this.buttonContinuExecute.Click += new System.EventHandler(this.buttonContinuExecute_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBoxMsg);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxSolution);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxSolution.ResumeLayout(false);
            this.groupBoxSolution.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBoxMsg.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBoxTimeInterval.ResumeLayout(false);
            this.groupBoxTimeInterval.PerformLayout();
            this.groupBoxControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSolution;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ProgressBar progressBarSaveAndLoad;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxSolutionPath;
        private System.Windows.Forms.Button buttonChooseSoluPath;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonLoadSolution;
        private System.Windows.Forms.Button buttonSaveSolution;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxProcessPath;
        private System.Windows.Forms.Button buttonChoosePrcPath;
        private System.Windows.Forms.Button buttonImportProcess;
        private System.Windows.Forms.Button buttonDeleteProcess;
        private System.Windows.Forms.Button buttonExportProcess;
        private System.Windows.Forms.GroupBox groupBox1;
        private VMControls.Winform.Release.VmProcedureConfigControl vmProcedureConfigControl1;
        private System.Windows.Forms.GroupBox groupBoxMsg;
        private System.Windows.Forms.Button buttonDeleteMsg;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.GroupBox groupBox3;
        internal System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonShieldProcess;
        internal System.Windows.Forms.GroupBox groupBoxTimeInterval;
        private System.Windows.Forms.TextBox textBoxTimeInterval;
        private System.Windows.Forms.Button buttonSetTimeInterval;
        private System.Windows.Forms.GroupBox groupBoxControl;
        private System.Windows.Forms.Button buttonStopExecute;
        private System.Windows.Forms.Button buttonExecuteOnce;
        private System.Windows.Forms.Button buttonContinuExecute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxProcessName;
        private System.Windows.Forms.Button buttonLang;
    }
}
