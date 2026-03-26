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
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.vmFrontendControl1 = new VMControls.Winform.Release.VmFrontendControl();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.vmFrontendControl1);
            this.groupBox7.Location = new System.Drawing.Point(169, 32);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(463, 387);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "前端运行控件VmFronttendControl";
            // 
            // vmFrontendControl1
            // 
            this.vmFrontendControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmFrontendControl1.Location = new System.Drawing.Point(3, 17);
            this.vmFrontendControl1.Name = "vmFrontendControl1";
            this.vmFrontendControl1.Size = new System.Drawing.Size(457, 367);
            this.vmFrontendControl1.TabIndex = 0;
            // 
            // vmFrontendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 461);
            this.Controls.Add(this.groupBox7);
            this.Name = "vmFrontendForm";
            this.Text = "vmFrontendForm";
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private VMControls.Winform.Release.VmFrontendControl vmFrontendControl1;
    }
}