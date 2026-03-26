using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionControlCs_WPF
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SolutionControlCS_WPF.App app = new SolutionControlCS_WPF.App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception ex)
            {
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);
                if (null != vmEx)
                {
                    string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);
                    System.Windows.MessageBox.Show(strMsg);
                }
                else
                {
                    return;
                }
            }

        }
        
    }
}
