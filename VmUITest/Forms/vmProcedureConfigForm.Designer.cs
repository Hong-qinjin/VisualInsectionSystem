namespace VisualInsectionSystem.Forms
{
    partial class vmProcedureConfigForm
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
            this.vmProcedureConfigControl1 = new VMControls.Winform.Release.VmProcedureConfigControl();
            this.SuspendLayout();
            // 
            // vmProcedureConfigControl1
            // 
            this.vmProcedureConfigControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmProcedureConfigControl1.Location = new System.Drawing.Point(0, 0);
            this.vmProcedureConfigControl1.Margin = new System.Windows.Forms.Padding(2);
            this.vmProcedureConfigControl1.Name = "vmProcedureConfigControl1";
            this.vmProcedureConfigControl1.Size = new System.Drawing.Size(944, 461);
            this.vmProcedureConfigControl1.TabIndex = 1;
// TODO: “”的代码生成失败，原因是出现异常“无效的基元类型: System.IntPtr。请考虑使用 CodeObjectCreateExpression。”。
            // 
            // vmProcedureConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 461);
            this.Controls.Add(this.vmProcedureConfigControl1);
            this.Name = "vmProcedureConfigForm";
            this.Text = "vmProcedureConfigForm";
            this.Load += new System.EventHandler(this.vmProcedureConfigForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private VMControls.Winform.Release.VmProcedureConfigControl vmProcedureConfigControl1;
    }
}