namespace VisualInsectionSystem.SubControls
{
    partial class FrontControl1
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.vmFrontendControl1 = new VMControls.Winform.Release.VmFrontendControl();
            this.SuspendLayout();
            // 
            // vmFrontendControl1
            // 
            this.vmFrontendControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmFrontendControl1.Location = new System.Drawing.Point(0, 0);
            this.vmFrontendControl1.Name = "vmFrontendControl1";
            this.vmFrontendControl1.Size = new System.Drawing.Size(907, 560);
            this.vmFrontendControl1.TabIndex = 0;
            this.vmFrontendControl1.Load += new System.EventHandler(this.vmFrontendControl1_Load);
            // 
            // FrontControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vmFrontendControl1);
            this.Name = "FrontControl1";
            this.Size = new System.Drawing.Size(907, 560);
            this.ResumeLayout(false);

        }

        #endregion

        private VMControls.Winform.Release.VmFrontendControl vmFrontendControl1;
    }
}
