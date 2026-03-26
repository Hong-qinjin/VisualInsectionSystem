using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VM.Core;                     // 引用 VmProcedure
using VMControls.Interface;        // 引用 IVmModule
using VMControls.Winform.Release;  // 引用 VmFrontendControl

namespace VisualInsectionSystem.SubControls
{
    public partial class FrontControl1 : UserControl
    {
        public FrontControl1()
        {
            InitializeComponent();
        }

        private void vmFrontendControl1_Load(object sender, EventArgs e)
        {
            //// FrontControl1 内只有一个子控件 vmFrontendControl1
            VmFrontendControl vmFrontendControl1 = new VmFrontendControl();
            vmFrontendControl1.Dock = DockStyle.Fill;
            vmFrontendControl1.LoadFrontendSource();           
        }
    }
}
