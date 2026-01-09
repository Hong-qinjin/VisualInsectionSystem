using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualInsectionSystem
{
    public partial class MainViewControl : UserControl
    {
        private VMControls.Winform.Release.VmGlobalToolControl vmGlobalToolControl1;
        private VMControls.Winform.Release.VmMainViewConfigControl vmMainViewConfigControl1;

        public MainViewControl()
        {
            InitializeComponent();
        }

        private void MainViewControl_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.vmGlobalToolControl1 = new VMControls.Winform.Release.VmGlobalToolControl();
            this.vmMainViewConfigControl1 = new VMControls.Winform.Release.VmMainViewConfigControl();
            this.SuspendLayout();
            // 
            // vmGlobalToolControl1
            // 
            this.vmGlobalToolControl1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.vmGlobalToolControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.vmGlobalToolControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.vmGlobalToolControl1.Location = new System.Drawing.Point(0, 0);
            this.vmGlobalToolControl1.Name = "vmGlobalToolControl1";
            this.vmGlobalToolControl1.Size = new System.Drawing.Size(754, 39);
            this.vmGlobalToolControl1.TabIndex = 1;
            // 
            // vmMainViewConfigControl1
            // 
            this.vmMainViewConfigControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmMainViewConfigControl1.Location = new System.Drawing.Point(0, 39);
            this.vmMainViewConfigControl1.Margin = new System.Windows.Forms.Padding(2);
            this.vmMainViewConfigControl1.Name = "vmMainViewConfigControl1";
            this.vmMainViewConfigControl1.Size = new System.Drawing.Size(754, 501);
            this.vmMainViewConfigControl1.TabIndex = 2;
// TODO: “”的代码生成失败，原因是出现异常“无效的基元类型: System.IntPtr。请考虑使用 CodeObjectCreateExpression。”。
            // 
            // MainViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.vmMainViewConfigControl1);
            this.Controls.Add(this.vmGlobalToolControl1);
            this.Name = "MainViewControl";
            this.Size = new System.Drawing.Size(754, 540);
            this.ResumeLayout(false);

        }
    }
}
