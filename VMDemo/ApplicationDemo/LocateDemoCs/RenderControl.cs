using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VMControls.Interface;

namespace LocateDemoCs
{
    public partial class RenderControl : UserControl
    {
        public RenderControl()
        {
            InitializeComponent();
        }

        private IVmModule _moduleSoure;
        public IVmModule ModuleSource
        {
            get { return _moduleSoure; }
            set
            {
                vmRenderControl1.ModuleSource = value;
                _moduleSoure = value;
            }
        }

        private void vmRenderControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
