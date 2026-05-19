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
    public partial class DbgRenderControl : UserControl
    {
        public DbgRenderControl()
        {
            InitializeComponent();
        }
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
    }
}
