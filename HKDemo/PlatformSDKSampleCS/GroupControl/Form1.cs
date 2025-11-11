using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VM.Core;
using VM.PlatformSDKCS;
using IMVSGroupCs;
using IMVSCircleFindModuCs;
using System.Threading;
using System.Globalization;

namespace GroupControl
{
    public partial class Form1 : Form
    {
        IMVSGroupTool mGroupTool = null;

        public Form1()
        {
            InitializeComponent();
        }

        /****************************************************************************
         * @fn           选择方案路径
         * @fn           Select solution's path
         ****************************************************************************/
        private void buttonChooseGroPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VM Gro File|*.gro*";
            DialogResult openFileRes = openFileDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                textBoxGroPath.Text = openFileDialog.FileName;
            }
        }

        /****************************************************************************
         * @fn           导入Group
         * @fn           Import group
         ****************************************************************************/
        private void buttonImportGroup_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            try
            {
                mGroupTool = IMVSGroupTool.LoadIndependentGroup(textBoxGroPath.Text);
                vmSingleModuleSetConfigControl1.ModuleSource = mGroupTool;
            }
            catch (VmException ex)
            {
                strMsg = "LoadGroupFromFile failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            strMsg = "LoadGroupFromFile success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        /****************************************************************************
         * @fn           导出Group
         * @fn           Export group
         ****************************************************************************/
        private void buttonExportGroup_Click(object sender, EventArgs e)
        {
            string strMsg = null;

            if ("" == textBoxGroPath.Text)
            {
                strMsg = "Please enter a valid group path!";
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            if(null != mGroupTool)
            {
                try
                {
                    mGroupTool.SaveAs(textBoxGroPath.Text);
                }
                catch (VmException ex)
                {
                    strMsg = "SaveGroupToFile failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }

                strMsg = "SaveGroupToFile success";
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
            }
            else
            {
                strMsg = "There is no group!";
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
            }
        }

        /****************************************************************************
         * @fn           执行一次
         * @fn           Execute once
         ****************************************************************************/
        private void buttonExecuteOnce_Click(object sender, EventArgs e)
        {
            string strMsg = null;

            if(null != mGroupTool)
            {
                try
                {
                    IMVSCircleFindModuTool circleFindModule = (IMVSCircleFindModuTool)mGroupTool["Circle Search1"];
                    vmRenderControl1.ModuleSource = circleFindModule;

                    mGroupTool.Run();
                }
                catch (VmException ex)
                {
                    strMsg = "Run failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }

                strMsg = "Run success";
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
            }
            else
            {
                strMsg = "There is no group!";
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
            }
        }

        /****************************************************************************
         * @fn           清空消息
         * @fn           Clear the contents of the List Box
         ****************************************************************************/
        private void buttonDeleteMsg_Click(object sender, EventArgs e)
        {
            listBoxMsg.Items.Clear();
        }

        /****************************************************************************
         * @fn           退出
         * @fn           Quit
         ****************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mGroupTool?.DestroyGroup();
        }

        /****************************************************************************
         * @fn           切换语言
         * @fn           Switch language
        ****************************************************************************/
        private void buttonLang_Click(object sender, EventArgs e)
        {
            // 仅切换Demo界面语言，控件语言通过配置文件切换
            // Only switch the language of demo interface,and switch the language of control through the configuration file
            int currentLCID = Thread.CurrentThread.CurrentUICulture.LCID;
            currentLCID = (2052 == currentLCID) ? 1033 : 2052;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLCID);

            ApplyLangResource(this);
        }

        private void ApplyLangResource(Control control)
        {
            foreach (Control ct in control.Controls)
            {
                resourceManager.ApplyResources(ct, ct.Name, Thread.CurrentThread.CurrentUICulture);
                if (ct.HasChildren)
                {
                    ApplyLangResource(ct);
                }
            }
        }

        private ComponentResourceManager resourceManager = new ComponentResourceManager(typeof(Form1));
    }
}