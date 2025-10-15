namespace GroupControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vmRenderControl1 = new VMControls.Winform.Release.VmRenderControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.vmSingleModuleSetConfigControl1 = new VMControls.Winform.Release.VmSingleModuleSetConfigControl();
            this.groupBoxMsg = new System.Windows.Forms.GroupBox();
            this.buttonDeleteMsg = new System.Windows.Forms.Button();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonExecuteOnce = new System.Windows.Forms.Button();
            this.buttonExportGroup = new System.Windows.Forms.Button();
            this.buttonChooseGroPath = new System.Windows.Forms.Button();
            this.textBoxGroPath = new System.Windows.Forms.TextBox();
            this.buttonImportGroup = new System.Windows.Forms.Button();
            this.buttonLang = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxMsg.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.vmRenderControl1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
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
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.vmSingleModuleSetConfigControl1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // vmSingleModuleSetConfigControl1
            // 
            resources.ApplyResources(this.vmSingleModuleSetConfigControl1, "vmSingleModuleSetConfigControl1");
            this.vmSingleModuleSetConfigControl1.ModuleSource = null;
            this.vmSingleModuleSetConfigControl1.Name = "vmSingleModuleSetConfigControl1";
            // 
            // groupBoxMsg
            // 
            this.groupBoxMsg.Controls.Add(this.buttonLang);
            this.groupBoxMsg.Controls.Add(this.buttonDeleteMsg);
            this.groupBoxMsg.Controls.Add(this.listBoxMsg);
            resources.ApplyResources(this.groupBoxMsg, "groupBoxMsg");
            this.groupBoxMsg.Name = "groupBoxMsg";
            this.groupBoxMsg.TabStop = false;
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
            this.listBoxMsg.FormattingEnabled = true;
            resources.ApplyResources(this.listBoxMsg, "listBoxMsg");
            this.listBoxMsg.Name = "listBoxMsg";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonExecuteOnce);
            this.groupBox3.Controls.Add(this.buttonExportGroup);
            this.groupBox3.Controls.Add(this.buttonChooseGroPath);
            this.groupBox3.Controls.Add(this.textBoxGroPath);
            this.groupBox3.Controls.Add(this.buttonImportGroup);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // buttonExecuteOnce
            // 
            resources.ApplyResources(this.buttonExecuteOnce, "buttonExecuteOnce");
            this.buttonExecuteOnce.Name = "buttonExecuteOnce";
            this.buttonExecuteOnce.UseVisualStyleBackColor = true;
            this.buttonExecuteOnce.Click += new System.EventHandler(this.buttonExecuteOnce_Click);
            // 
            // buttonExportGroup
            // 
            resources.ApplyResources(this.buttonExportGroup, "buttonExportGroup");
            this.buttonExportGroup.Name = "buttonExportGroup";
            this.buttonExportGroup.UseVisualStyleBackColor = true;
            this.buttonExportGroup.Click += new System.EventHandler(this.buttonExportGroup_Click);
            // 
            // buttonChooseGroPath
            // 
            resources.ApplyResources(this.buttonChooseGroPath, "buttonChooseGroPath");
            this.buttonChooseGroPath.Name = "buttonChooseGroPath";
            this.buttonChooseGroPath.UseVisualStyleBackColor = true;
            this.buttonChooseGroPath.Click += new System.EventHandler(this.buttonChooseGroPath_Click);
            // 
            // textBoxGroPath
            // 
            resources.ApplyResources(this.textBoxGroPath, "textBoxGroPath");
            this.textBoxGroPath.Name = "textBoxGroPath";
            // 
            // buttonImportGroup
            // 
            resources.ApplyResources(this.buttonImportGroup, "buttonImportGroup");
            this.buttonImportGroup.Name = "buttonImportGroup";
            this.buttonImportGroup.UseVisualStyleBackColor = true;
            this.buttonImportGroup.Click += new System.EventHandler(this.buttonImportGroup_Click);
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
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBoxMsg);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBoxMsg.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBoxMsg;
        private System.Windows.Forms.Button buttonDeleteMsg;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonExportGroup;
        private System.Windows.Forms.Button buttonChooseGroPath;
        private System.Windows.Forms.TextBox textBoxGroPath;
        private System.Windows.Forms.Button buttonImportGroup;
        private System.Windows.Forms.Button buttonExecuteOnce;
        private VMControls.Winform.Release.VmRenderControl vmRenderControl1;
        private VMControls.Winform.Release.VmSingleModuleSetConfigControl vmSingleModuleSetConfigControl1;
        private System.Windows.Forms.Button buttonLang;
    }
}
