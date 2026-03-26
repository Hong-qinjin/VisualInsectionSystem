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
using ImageSourceModuleCs;
using IMVSCircleFindModuCs;
using System.Threading;
using System.Globalization;

namespace CircleFind
{
    public partial class Form1 : Form
    {
        bool mSolutionIsLoad = false;

        public Form1()
        {
            InitializeComponent();
        }

        /****************************************************************************
         * @fn           选择方案路径
         * @fn           Select solution's path
         ****************************************************************************/
        private void buttonChooseSoluPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VM Sol File|*.sol*";
            DialogResult openFileRes = openFileDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                textBoxSolutionPath.Text = openFileDialog.FileName;
            }
        }

        /****************************************************************************
         * @fn           加载方案
         * @fn           Load solution
         ****************************************************************************/
        private void buttonLoadSolution_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            int nProgress = 0;
            progressBarSaveAndLoad.Value = nProgress;
            labelProgress.Text = nProgress.ToString();
            labelProgress.Refresh();

            try
            {
                this.Enabled = false;
                VmSolution.Load(textBoxSolutionPath.Text, textBoxPassword.Text);
                mSolutionIsLoad = true;

                IMVSCircleFindModuTool circleFindModule = (IMVSCircleFindModuTool)VmSolution.Instance["Flow1.Circle Search1"];
                vmRenderControl1.ModuleSource = circleFindModule;
            }
            catch (VmException ex)
            {
                strMsg = "LoadSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }
            finally
            {
                this.Enabled = true;
            }

            strMsg = "LoadSolution success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;

            nProgress = 100;
            labelProgress.Text = nProgress.ToString();
            labelProgress.Refresh();
            progressBarSaveAndLoad.Value = Convert.ToInt32(nProgress);
        }

        /****************************************************************************
         * @fn           保存方案
         * @fn           Save solution
         ****************************************************************************/
        private void buttonSaveSolution_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            int nProgress = 0;
            progressBarSaveAndLoad.Value = nProgress;
            labelProgress.Text = nProgress.ToString();
            labelProgress.Refresh();

            if (mSolutionIsLoad == true)
            {
                try
                {
                    this.Enabled = false;
                    VmSolution.Save();
                }
                catch (VmException ex)
                {
                    strMsg = "SaveSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                finally
                {
                    this.Enabled = true;
                }

                strMsg = "SaveSolution success";
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;

                nProgress = 100;
                labelProgress.Text = nProgress.ToString();
                labelProgress.Refresh();
                progressBarSaveAndLoad.Value = Convert.ToInt32(nProgress);
            }
            else
            {
                strMsg = "No solution file.";
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

            try
            {
                VmProcedure process = (VmProcedure)VmSolution.Instance["Flow1"];
                if (null == process) return;

                process.Run();
            }
            catch (VmException ex)
            {
                strMsg = "ExecuteOnce failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            strMsg = "ExecuteOnce success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        /****************************************************************************
         * @fn           获取结果
         * @fn           Get result
         ****************************************************************************/
        private void buttonGetResult_Click(object sender, EventArgs e)
        {
            IMVSCircleFindModuTool circleFindModule = (IMVSCircleFindModuTool)VmSolution.Instance["Flow1.Circle Search1"];
            if (null == circleFindModule) return;

            var nCenterX = circleFindModule.ModuResult.OutputCircle.CenterPoint.X;
            var nCenterY = circleFindModule.ModuResult.OutputCircle.CenterPoint.Y;
            var nModuleStatus = (VmSolution.Instance["Flow1.Circle Search1.Outputs.ModuStatus.Value"] as Array)?.GetValue(0);

            string strMsg = "ModuleStatus: " + Convert.ToString(nModuleStatus);
            listBoxMsg.Items.Add(strMsg);
            strMsg = "CenterX: " + Convert.ToString(nCenterX);
            listBoxMsg.Items.Add(strMsg);
            strMsg = "CenterY: " + Convert.ToString(nCenterY);
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        /****************************************************************************
         * @fn           绑定参数配置控件
         * @fn           Binding param
         ****************************************************************************/
        private void buttonParam_Click(object sender, EventArgs e)
        {
            IMVSCircleFindModuTool circleFindModule = (IMVSCircleFindModuTool)VmSolution.Instance["Flow1.Circle Search1"];
            if (null == circleFindModule) return;
            vmParamsConfigWithRenderControl1.ModuleSource = circleFindModule;

            string strMsg = "Binding param control success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
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
            VmSolution.Instance?.Dispose();
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