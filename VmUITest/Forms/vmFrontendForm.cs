using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Interface;

namespace VisualInsectionSystem.Forms
{
    public partial class vmFrontendForm : Form
    {
        bool mSolutionIsLoad = false;
        bool mFrontedLoad = false;

        public vmFrontendForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ListBox.CheckForIllegalCrossThreadCalls = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

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

        private void buttonLoadFrontendData_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            try
            {
                if (mSolutionIsLoad == false)
                {
                    strMsg = "Solution Not loaded yet !";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                if (vmFrontendControl1 == null) return;
                vmFrontendControl1.LoadFrontendSource();
                mFrontedLoad = true;
            }
            catch (VmException ex)
            {
                strMsg = "Load Frontend Data Fail, Error Code: " + Convert.ToString(ex.errorCode, 16);
                MessageBox.Show(strMsg);
                return;
            }
        }

        private void buttonExecuteOnce_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            try
            {
                if (mSolutionIsLoad == false)
                {
                    strMsg = "Solution Not loaded yet !";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                var m_VmSol = VmSolution.Instance[""] as IVMRun;
                if (null == m_VmSol)
                {
                    strMsg = "VmSolution doesn't exist !";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                m_VmSol.Run();
            }
            catch (VmException ex)
            {
                strMsg = "VmSolution run failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            strMsg = "Process run success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        private void buttonChangeSize1_Click(object sender, EventArgs e)
        {
            if (vmFrontendControl1 == null) return;
            if (vmFrontendControl1.Dock != DockStyle.None)
            {
                vmFrontendControl1.Dock = DockStyle.None;
            }
            vmFrontendControl1.Height = vmFrontendControl1.Height - 100;
            vmFrontendControl1.Width = vmFrontendControl1.Width - 100;
            vmFrontendControl1.AutoChangeSize();
        }

        private void buttonChangeSize2_Click(object sender, EventArgs e)
        {
            if (vmFrontendControl1 == null) return;
            if (vmFrontendControl1.Dock != DockStyle.None)
            {
                vmFrontendControl1.Dock = DockStyle.None;
            }
            vmFrontendControl1.Height = vmFrontendControl1.Height + 100;
            vmFrontendControl1.Width = vmFrontendControl1.Width + 100;
            vmFrontendControl1.AutoChangeSize();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            VmSolution.Instance?.Dispose();
        }

        private void vmFrontendControl1_SizeChanged(object sender, EventArgs e)
        {
            if ((null != vmFrontendControl1) && mFrontedLoad)
            {
                vmFrontendControl1.AutoChangeSize();
            }
        }

        private void buttonDeleteMsg_Click(object sender, EventArgs e)
        {
            listBoxMsg.Items.Clear();
        }
    }
}

