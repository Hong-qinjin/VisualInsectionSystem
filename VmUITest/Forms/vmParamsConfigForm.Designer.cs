namespace VisualInsectionSystem.Forms
{
    partial class vmParamsConfigForm
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.vmParamsConfigControl1 = new VMControls.Winform.Release.VmParamsConfigControl();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.vmParamsConfigWithRenderControl1 = new VMControls.Winform.Release.VmParamsConfigWithRenderControl();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.AutoSize = true;
            this.groupBox5.Controls.Add(this.vmParamsConfigControl1);
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(411, 452);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "参数配置控件VmParamsConfigControl";
            // 
            // vmParamsConfigControl1
            // 
            this.vmParamsConfigControl1.BackColor = System.Drawing.Color.White;
            this.vmParamsConfigControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmParamsConfigControl1.Location = new System.Drawing.Point(3, 17);
            this.vmParamsConfigControl1.ModuleSource = null;
            this.vmParamsConfigControl1.Name = "vmParamsConfigControl1";
            this.vmParamsConfigControl1.ParamsConfig = null;
            this.vmParamsConfigControl1.Size = new System.Drawing.Size(405, 432);
            this.vmParamsConfigControl1.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.AutoSize = true;
            this.groupBox6.Controls.Add(this.vmParamsConfigWithRenderControl1);
            this.groupBox6.Location = new System.Drawing.Point(417, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(525, 452);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "参数配置带渲染控件VmParamsConfigWithRenderControl";
            // 
            // vmParamsConfigWithRenderControl1
            // 
            this.vmParamsConfigWithRenderControl1.BackColor = System.Drawing.Color.Black;
            this.vmParamsConfigWithRenderControl1.CoordinateInfoVisible = true;
            this.vmParamsConfigWithRenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmParamsConfigWithRenderControl1.ImageSource = null;
            this.vmParamsConfigWithRenderControl1.Location = new System.Drawing.Point(3, 17);
            this.vmParamsConfigWithRenderControl1.ModuleSource = null;
            this.vmParamsConfigWithRenderControl1.MultiImageButtonVisible = false;
            this.vmParamsConfigWithRenderControl1.Name = "vmParamsConfigWithRenderControl1";
            this.vmParamsConfigWithRenderControl1.ParamsConfig = null;
            this.vmParamsConfigWithRenderControl1.ROIVisible = true;
            this.vmParamsConfigWithRenderControl1.Size = new System.Drawing.Size(519, 432);
            this.vmParamsConfigWithRenderControl1.TabIndex = 0;
            // 
            // vmParamsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 461);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Name = "vmParamsConfigForm";
            this.Text = "vmParamsConfigForm";
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private VMControls.Winform.Release.VmParamsConfigControl vmParamsConfigControl1;
        private System.Windows.Forms.GroupBox groupBox6;
        private VMControls.Winform.Release.VmParamsConfigWithRenderControl vmParamsConfigWithRenderControl1;
    }
}