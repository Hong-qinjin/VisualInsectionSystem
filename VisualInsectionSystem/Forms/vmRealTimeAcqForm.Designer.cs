namespace VisualInsectionSystem.Forms
{
    partial class vmRealTimeAcqForm
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
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.vmRealTimeAcqControl1 = new VMControls.Winform.Release.VmRealTimeAcqControl();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.vmRealTimeAcqControl1);
            this.groupBox8.Location = new System.Drawing.Point(218, 7);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(364, 437);
            this.groupBox8.TabIndex = 12;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "全局相机控件VmRealTimeAcqControl";
            // 
            // vmRealTimeAcqControl1
            // 
            this.vmRealTimeAcqControl1.BackColor = System.Drawing.Color.Black;
            this.vmRealTimeAcqControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmRealTimeAcqControl1.IsShowButton = true;
            this.vmRealTimeAcqControl1.Location = new System.Drawing.Point(3, 17);
            this.vmRealTimeAcqControl1.ModuleSource = null;
            this.vmRealTimeAcqControl1.Name = "vmRealTimeAcqControl1";
            this.vmRealTimeAcqControl1.Size = new System.Drawing.Size(358, 417);
            this.vmRealTimeAcqControl1.TabIndex = 0;
            // 
            // vmRealTimeAcqForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 461);
            this.Controls.Add(this.groupBox8);
            this.Name = "vmRealTimeAcqForm";
            this.Text = "vmRealTimeAcqForm";
            this.groupBox8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox8;
        private VMControls.Winform.Release.VmRealTimeAcqControl vmRealTimeAcqControl1;
    }
}