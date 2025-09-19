using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualInsectionSystem
{
    public partial class Form1 : Form
    {     
        /// <summary>
        /// 设置form1为主窗口
        /// </summary>
        public static Form1 form1 = null;
        /// <summary>
        /// 子窗口集合
        /// </summary>
        public static List<Form> subForms = new List<Form>();

        public Form1()
        {
            InitializeComponent();
            //设置form1为主窗口
            form1 = this;
            //初始化子窗口集合
            //subForms.Add(new CameraForm());
            //subForms.Add(new CommunicationForm());
            subForms.Add(new DebugForm());
            //subForms.Add(new AlarmForm());
            //subForms.Add(new LogForm());
            //subForms.Add(new UserForm());
            //subForms.Add(new HelpForm());
            //初始化菜单栏
            //InitMenuStrip();
            ////初始化工具栏
            //InitToolBar();
            ////初始化状态栏
            //InitStatusBar();
            ////初始化窗体
            //InitForm();
        }
    

        

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 相机ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 通讯ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        private void 报警ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }     
        private void 打开调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                //打开调试页面
                DebugForm debugForm = new DebugForm();
                debugForm.Show();
            }
            catch (Exception ex)
            {
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);
                if (null != vmEx)
                {
                    string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);
                    MessageBox.Show(strMsg);
                }
                else
                {
                    return;
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            //UI

        }
    }
}
