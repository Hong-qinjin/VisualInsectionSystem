namespace GetResultControl
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
            this.groupBoxMsg = new System.Windows.Forms.GroupBox();
            this.buttonLang = new System.Windows.Forms.Button();
            this.buttonDeleteMsg = new System.Windows.Forms.Button();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vmProcedureConfigControl1 = new VMControls.Winform.Release.VmProcedureConfigControl();
            this.groupBoxSolution = new System.Windows.Forms.GroupBox();
            this.buttonLoadSolution = new System.Windows.Forms.Button();
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBarSaveAndLoad = new System.Windows.Forms.ProgressBar();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxSolutionPath = new System.Windows.Forms.TextBox();
            this.buttonChooseSoluPath = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonGetProcedureResult = new System.Windows.Forms.Button();
            this.buttonGetImageSourceResultFromCallBack = new System.Windows.Forms.Button();
            this.buttonGetImageSourceResult = new System.Windows.Forms.Button();
            this.buttonAddShape = new System.Windows.Forms.Button();
            this.vmRenderControl1 = new VMControls.Winform.Release.VmRenderControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveShape = new System.Windows.Forms.Button();
            this.buttonExecuteOnce = new System.Windows.Forms.Button();
            this.groupBoxMsg.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxSolution.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
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
            // groupBoxSolution
            // 
            resources.ApplyResources(this.groupBoxSolution, "groupBoxSolution");
            this.groupBoxSolution.Controls.Add(this.buttonLoadSolution);
            this.groupBoxSolution.Controls.Add(this.labelProgress);
            this.groupBoxSolution.Controls.Add(this.progressBarSaveAndLoad);
            this.groupBoxSolution.Controls.Add(this.label9);
            this.groupBoxSolution.Controls.Add(this.textBoxSolutionPath);
            this.groupBoxSolution.Controls.Add(this.buttonChooseSoluPath);
            this.groupBoxSolution.Controls.Add(this.label8);
            this.groupBoxSolution.Controls.Add(this.textBoxPassword);
            this.groupBoxSolution.Controls.Add(this.label11);
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
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.buttonGetProcedureResult);
            this.groupBox2.Controls.Add(this.buttonGetImageSourceResultFromCallBack);
            this.groupBox2.Controls.Add(this.buttonGetImageSourceResult);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // buttonGetProcedureResult
            // 
            resources.ApplyResources(this.buttonGetProcedureResult, "buttonGetProcedureResult");
            this.buttonGetProcedureResult.Name = "buttonGetProcedureResult";
            this.buttonGetProcedureResult.UseVisualStyleBackColor = true;
            this.buttonGetProcedureResult.Click += new System.EventHandler(this.buttonGetProcedureResult_Click);
            // 
            // buttonGetImageSourceResultFromCallBack
            // 
            resources.ApplyResources(this.buttonGetImageSourceResultFromCallBack, "buttonGetImageSourceResultFromCallBack");
            this.buttonGetImageSourceResultFromCallBack.Name = "buttonGetImageSourceResultFromCallBack";
            this.buttonGetImageSourceResultFromCallBack.UseVisualStyleBackColor = true;
            this.buttonGetImageSourceResultFromCallBack.Click += new System.EventHandler(this.buttonGetImageSourceResultFromCallBack_Click);
            // 
            // buttonGetImageSourceResult
            // 
            resources.ApplyResources(this.buttonGetImageSourceResult, "buttonGetImageSourceResult");
            this.buttonGetImageSourceResult.Name = "buttonGetImageSourceResult";
            this.buttonGetImageSourceResult.UseVisualStyleBackColor = true;
            this.buttonGetImageSourceResult.Click += new System.EventHandler(this.buttonGetImageSourceResult_Click);
            // 
            // buttonAddShape
            // 
            resources.ApplyResources(this.buttonAddShape, "buttonAddShape");
            this.buttonAddShape.Name = "buttonAddShape";
            this.buttonAddShape.UseVisualStyleBackColor = true;
            this.buttonAddShape.Click += new System.EventHandler(this.buttonAddShape_Click);
            // 
            // vmRenderControl1
            // 
            resources.ApplyResources(this.vmRenderControl1, "vmRenderControl1");
            this.vmRenderControl1.BackColor = System.Drawing.Color.Black;
            this.vmRenderControl1.CoordinateInfoVisible = true;
            this.vmRenderControl1.ImageSource = null;
            this.vmRenderControl1.ModuleSource = null;
            this.vmRenderControl1.Name = "vmRenderControl1";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.buttonRemoveShape);
            this.groupBox3.Controls.Add(this.buttonExecuteOnce);
            this.groupBox3.Controls.Add(this.buttonAddShape);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // buttonRemoveShape
            // 
            resources.ApplyResources(this.buttonRemoveShape, "buttonRemoveShape");
            this.buttonRemoveShape.Name = "buttonRemoveShape";
            this.buttonRemoveShape.UseVisualStyleBackColor = true;
            this.buttonRemoveShape.Click += new System.EventHandler(this.buttonRemoveShape_Click);
            // 
            // buttonExecuteOnce
            // 
            resources.ApplyResources(this.buttonExecuteOnce, "buttonExecuteOnce");
            this.buttonExecuteOnce.Name = "buttonExecuteOnce";
            this.buttonExecuteOnce.UseVisualStyleBackColor = true;
            this.buttonExecuteOnce.Click += new System.EventHandler(this.buttonExecuteOnce_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.vmRenderControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxSolution);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxMsg);
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBoxMsg.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxSolution.ResumeLayout(false);
            this.groupBoxSolution.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMsg;
        private System.Windows.Forms.Button buttonDeleteMsg;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private VMControls.Winform.Release.VmProcedureConfigControl vmProcedureConfigControl1;
        private System.Windows.Forms.GroupBox groupBoxSolution;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ProgressBar progressBarSaveAndLoad;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxSolutionPath;
        private System.Windows.Forms.Button buttonChooseSoluPath;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonLoadSolution;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonGetImageSourceResult;
        private System.Windows.Forms.Button buttonGetImageSourceResultFromCallBack;
        private System.Windows.Forms.Button buttonGetProcedureResult;
        private VMControls.Winform.Release.VmRenderControl vmRenderControl1;
        private System.Windows.Forms.Button buttonAddShape;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonRemoveShape;
        private System.Windows.Forms.Button buttonExecuteOnce;
        private System.Windows.Forms.Button buttonLang;
    }
}

