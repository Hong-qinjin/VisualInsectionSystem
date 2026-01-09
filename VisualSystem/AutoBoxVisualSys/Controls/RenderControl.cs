using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VMControls.Interface;

namespace VisualInsectionSystem
{
    public partial class RenderControl : UserControl
    {
        public RenderControl()
        {
            InitializeComponent();
        }

        private VMControls.Winform.Release.VmRenderControl vmRenderControl1;
        private IVmModule _moduleSoure;
        public IVmModule ModuleSource
        {
            get { return _moduleSoure; }
            set
            {
                _moduleSoure = value;
                vmRenderControl1.ModuleSource = _moduleSoure;
            }
        }
        private void RenderControl_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.vmRenderControl1 = new VMControls.Winform.Release.VmRenderControl();
            this.SuspendLayout();
            // 
            // vmRenderControl1
            // 
            this.vmRenderControl1.BackColor = System.Drawing.Color.Black;
            this.vmRenderControl1.CoordinateInfoVisible = true;
            this.vmRenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmRenderControl1.ImageSource = null;
            this.vmRenderControl1.Location = new System.Drawing.Point(0, 0);
            this.vmRenderControl1.ModuleSource = null;
            this.vmRenderControl1.Name = "vmRenderControl1";
            this.vmRenderControl1.Size = new System.Drawing.Size(716, 524);
            this.vmRenderControl1.TabIndex = 1;
            // 
            // RenderControl
            // 
            this.Controls.Add(this.vmRenderControl1);
            this.Name = "RenderControl";
            this.Size = new System.Drawing.Size(716, 524);
            this.ResumeLayout(false);

        }
    }
}
