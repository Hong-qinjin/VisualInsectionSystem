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
using Microsoft.WindowsAPICodePack.Dialogs;
using VM.Core;
using VM.PlatformSDKCS;

namespace DeepLearningDemoCs
{
    public partial class MainForm : Form
    {

        private RenderControl renderControl;
        private MainViewControl mainViewControl;
        public bool mSolutionIsLoad = false;  //mSolutionIsLoad = false, indicates that the solution is not loaded
        public VmProcedure vmProcedure = null;
        public ProcessInfoList vmProcessInfoList = new ProcessInfoList();
        public string vmSolutionPath = null;//Solution Path
        private string logPath = Application.StartupPath + "/Log/Message";//Log Path
        private Timer LoadSolutionIndicateTimer = new Timer();
        private string cultureName = "zh-CN";

        public MainForm()
        {
            InitializeComponent();
            renderControl = new RenderControl();
            mainViewControl = new MainViewControl();
            renderControl.Dock = DockStyle.Fill;
            mainViewControl.Dock = DockStyle.Fill;
            buttonRender.BackColor = Color.Orange;
            buttonConfig.BackColor = Color.Gray;
            renderPanel.Controls.Add(mainViewControl);
            LoadSolutionIndicateTimer.Interval = 300;
            LoadSolutionIndicateTimer.Tick += LoadSolutionIndicateTimer_Tick;
            VmSolution.OnWorkStatusEvent += VmSolution_OnWorkStatusEvent;//Registration callback for the procedure working status
            VmSolution.OnProcessStatusStartEvent += VmSolution_OnProcessStatusStartEvent;   // Registration callback for the start of the procedure continuous run
            VmSolution.OnProcessStatusStopEvent += VmSolution_OnProcessStatusStopEvent; // Registration callback for the stop of the procedure continuous run
            cultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }

        /// <summary>
        /// Callback function for the start of the procedure continuous run
        /// </summary>
        /// <param name="statusInfo"></param>
        private void VmSolution_OnProcessStatusStartEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_START_CONTINUOUSLY_INFO statusInfo)
        {
            this.Invoke(new Action(() =>
            {
                if (statusInfo.nStatus == 0)
                {
                    string strMessage = null;

                    if ("zh-CN" == cultureName)
                    {
                        var resourceManager = new ResourceManager("DeepLearningDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiStop.Text", new CultureInfo("zh-CN"));
                    }
                    else
                    {
                        var resourceManager = new ResourceManager("DeepLearningDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiStop.Text", new CultureInfo("en-US"));
                    }

                    //Disable button
                    buttonSelectSolu.Enabled = false;
                    buttonRunOnce.Enabled = false;
                    buttonLoadSolu.Enabled = false;
                    buttonSaveSolu.Enabled = false;
                    comboProcedure.Enabled = false;

                    strMessage = "Start continuous run!";
                    LogFunction(strMessage);
                }
            }));
        }

        /// <summary>
        /// Callback function for the stop of the procedure continuous run
        /// </summary>
        /// <param name="statusInfo"></param>
        private void VmSolution_OnProcessStatusStopEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_STOP_INFO statusInfo)
        {
            this.Invoke(new Action(() =>
            {
                if (statusInfo.nStopAction == 1)
                {
                    string strMessage = null;

                    if ("zh-CN" == cultureName)
                    {
                        var resourceManager = new ResourceManager("DeepLearningDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiRun.Text", new CultureInfo("zh-CN"));
                    }
                    else
                    {
                        var resourceManager = new ResourceManager("DeepLearningDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiRun.Text", new CultureInfo("en-US"));
                    }

                    //Enable button
                    buttonSelectSolu.Enabled = true;
                    buttonRunOnce.Enabled = true;
                    buttonLoadSolu.Enabled = true;
                    buttonSaveSolu.Enabled = true;
                    comboProcedure.Enabled = true;

                    strMessage = "End Run!";
                    LogFunction(strMessage);
                }
            }));
        }

        /// <summary>
        /// The button for loading the solution flashes, prompting you to load the solution after selecting the path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSolutionIndicateTimer_Tick(object sender, EventArgs e)
        {
            if (!mSolutionIsLoad)
            {
                if (buttonLoadSolu.BackColor == Color.DimGray)
                {
                    buttonLoadSolu.BackColor = Color.Orange;
                }
                else
                {
                    buttonLoadSolu.BackColor = Color.DimGray;
                }
            }
            if (mSolutionIsLoad)
            {
                buttonLoadSolu.BackColor = Color.DimGray;
            }
        }

        /// <summary>
        /// Load Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(renderControl);
        }

        /// <summary>
        /// Callback function for the procedure working status
        /// </summary>
        /// <param name="workStatusInfo"></param>
        private void VmSolution_OnWorkStatusEvent(ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            if (workStatusInfo.nWorkStatus == 0)//When the process is running, the nWorkStatus is 1
            {
                try
                {
                    switch (workStatusInfo.nProcessID)
                    {
                        case 10000:
                            if (vmProcessInfoList.nNum==0) return;
                            VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance[vmProcessInfoList.astProcessInfo[0].strProcessName];
                            if (vmProcedure == null) return;
                            List<VmDynamicIODefine.IoNameInfo> ioNameInfos = vmProcedure.ModuResult.GetAllOutputNameInfo();
                            foreach (var item in ioNameInfos)
                            {
                                if (item.Name == "out"&&item.TypeName!= IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
                                {
                                    Task.Run(() =>
                                    {
                                        UpdateResult("The result argument (out) is not string format！");

                                    });
                                    return;
                                }
                            }
                            var vmResult = vmProcedure.ModuResult.GetOutputString("out").astStringVal[0].strValue;

                            Task.Run(() =>
                            {
                                UpdateResult(vmResult);
                                LogFunction("Process running time：" + vmProcedure.ProcessTime.ToString() + "ms");
                            });
                            break;
                        default:
                            break;
                    }
                }
                catch (VmException ex)
                {
                    LogFunction("Failed to get results, Error code: 0x" + Convert.ToString(ex.errorCode, 16));
                    return;
                }
                catch (Exception ex)
                {
                    LogFunction("Failed to get results: " + ex.ToString());
                    return;
                }
            }
        }

        /// <summary>
        /// Update Results
        /// </summary>
        /// <param name="result"></param>
        public void UpdateResult(string result)
        {
            try
            {
                string[] str = result.Split(',');
                if (str[0] == "OK")
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        labelResult.Text = "OK";
                        labelResult.BackColor = Color.FromArgb(255, 0, 192, 0);
                    }));
                }
                else
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        labelResult.Text = "NG";
                        labelResult.BackColor = Color.FromArgb(255, 255, 0, 0);
                    }));
                }
                this.BeginInvoke(new Action(() =>
                {
                    listBoxResult.Items.Add("Results: " + result.ToString());
                    listBoxResult.TopIndex = listBoxResult.Items.Count - 1;
                }));
            }
            catch (Exception ex)
            {
                LogFunction("Failed to update results:" + ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Display picture page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRender_Click(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(renderControl);
            buttonRender.BackColor = Color.Orange;
            buttonConfig.BackColor = Color.Gray;
        }

        /// <summary>
        /// Configure parameter page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfig_Click(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(mainViewControl);
            buttonRender.BackColor = Color.Gray;
            buttonConfig.BackColor = Color.Orange;
        }

        /// <summary>
        /// Select solution path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectSolu_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VM Sol File|*.sol*";
            DialogResult openFileRes = openFileDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                vmSolutionPath = openFileDialog.FileName;
                mSolutionIsLoad = false;
                LoadSolutionIndicateTimer.Enabled = true;
                LogFunction(vmSolutionPath);
            }
        }

        /// <summary>
        /// Load Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadSolu_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            LoadSolutionIndicateTimer.Enabled = false;
            buttonLoadSolu.BackColor = Color.Orange;
            buttonLoadSolu.Enabled = false;

            // Disable button
            buttonSelectSolu.Enabled = false;
            buttonRunOnce.Enabled = false;
            buttonContiRun.Enabled = false;
            buttonSaveSolu.Enabled = false;
            comboProcedure.Enabled = false;
            buttonRender.Enabled = false;
            buttonConfig.Enabled = false;
            try
            {
                if (vmSolutionPath != null && File.Exists(vmSolutionPath))
                {
                    VmSolution.Load(vmSolutionPath);
                    vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();//Obtain all processes in the solution
                    vmProcedure = VmSolution.Instance[vmProcessInfoList.astProcessInfo[0].strProcessName] as VmProcedure;
                    comboProcedure.Items.Clear();
                    for (int item = 0; item < vmProcessInfoList.nNum; item++)
                    {
                        comboProcedure.Items.Add(vmProcessInfoList.astProcessInfo[item].strProcessName);
                    }
                    if (comboProcedure.Items.Count > 0)
                    {
                        comboProcedure.SelectedIndex = 0;
                        comboProcedure.Text = comboProcedure.SelectedItem.ToString();
                    }
                    renderControl.vmRenderControl1.ModuleSource = vmProcedure;//RenderControl binding procedure
                    mSolutionIsLoad = true;
                    strMsg = "Succeeded to load solution!";
                    LogFunction(strMsg);
                }
                else
                {
                    strMsg = "The Soultion is null!";
                    LogFunction(strMsg);
                }
            }
            catch (VmException ex)
            {
                strMsg = "Failed to load solution, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogFunction(strMsg);
                return;
            }
            catch (Exception ex)
            {
                strMsg = "Failed to load solution: " + ex.ToString();
                LogFunction(strMsg);
                return;
            }
            finally
            {
                buttonLoadSolu.BackColor = Color.DimGray;
                buttonLoadSolu.Enabled = true;

                // Enable button
                buttonSelectSolu.Enabled = true;
                buttonRunOnce.Enabled = true;
                buttonContiRun.Enabled = true;
                buttonSaveSolu.Enabled = true;
                comboProcedure.Enabled = true;
                buttonRender.Enabled = true;
                buttonConfig.Enabled = true;
            }
        }

        /// <summary>
        /// Save Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveSolu_Click(object sender, EventArgs e)
        {
            string strMsg = null;

            if (mSolutionIsLoad == true)
            {
                try
                {
                    VmSolution.Save();
                }
                catch (VmException ex)
                {
                    strMsg = "Failed to save solution, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                    LogFunction(strMsg);
                    return;
                }
                strMsg = "Succeeded to save solution!";
                LogFunction(strMsg);
            }
            else
            {
                strMsg = "Please to load the solution";
                LogFunction(strMsg);
            }
        }

        /// <summary>
        /// Obtain all processes in the solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboProcedure_DropDown(object sender, EventArgs e)
        {
            string strMsg = null;
            try
            {
                if (mSolutionIsLoad)
                {
                    comboProcedure.Items.Clear();
                    vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();//Obtain all processes in the solution
                    for (int item = 0; item < vmProcessInfoList.nNum; item++)
                    {
                        comboProcedure.Items.Add(vmProcessInfoList.astProcessInfo[item].strProcessName);
                    }
                }
                else
                {
                    strMsg ="Pleaase to load solution";
                    LogFunction(strMsg);
                }
            }
            catch (VmException ex)
            {
                strMsg = "Failed to obtain all processes, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogFunction(strMsg);
                return;
            }
            catch (Exception ex)
            {
                strMsg = "Failed to obtain all processes!" + ex.ToString();
                LogFunction(strMsg);
                return;
            }
        }

        /// <summary>
        /// Run once
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRunOnce_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            try
            {
                if (comboProcedure.Text != "")
                {
                    vmProcedure = (VmProcedure)VmSolution.Instance[comboProcedure.Text];
                    if (null == vmProcedure) return;
                    renderControl.vmRenderControl1.ModuleSource = vmProcedure;//RenderControl binding procedure
                    vmProcedure.Run();
                }
                else
                {
                    strMsg = "Please to select procedure! ";
                    LogFunction(strMsg);
                    return;
                }
            }
            catch (VmException ex)
            {
                strMsg = "Failed to run procedure once, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogFunction(strMsg);
                return;
            }
            catch (Exception ex)
            {
                strMsg = "Failed to run procedure once: " + ex.ToString();
                LogFunction(strMsg);
                return;
            }
        }

        /// <summary>
        /// Run Continuous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContiRun_Click(object sender, EventArgs e)
        {
            string strMsg = null;

            try
            {
                if (comboProcedure.Text != "")
                {
                    vmProcedure = (VmProcedure)VmSolution.Instance[comboProcedure.Text];
                    if (null == vmProcedure)
                    {
                        strMsg = comboProcedure.Text + "The procedure does not exist! ";
                        LogFunction(strMsg);
                        return;
                    }
                    vmProcedure.ContinuousRunEnable = vmProcedure.ContinuousRunEnable^true;
                }
                else
                {
                    strMsg = "Please to select procedure! ";
                    LogFunction(strMsg);
                    return;
                }
            }
            catch (VmException ex)
            {
                strMsg = "Failed to run procedure continuous, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogFunction(strMsg);
                return;
            }
            catch (Exception ex)
            {
                strMsg = "Failed to run procedure continuous: " + ex.ToString();
                LogFunction(strMsg);
                return;
            }
        }

        /// <summary>
        /// Print Log
        /// </summary>
        /// <param name="strMsg"></param>
        public void LogFunction(string strMsg)
        {
            this.BeginInvoke(new Action(() =>
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems.Add("");
                listViewItem.SubItems[0].Text = DateTime.Now.ToString();
                listViewItem.SubItems[1].Text = strMsg;
                listViewLog.Items.Insert(0, listViewItem);
            }));
            SaveLog(strMsg);
        }

        /// <summary>
        /// Clear Log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
        }

        /// <summary>
        /// Save Log
        /// </summary>
        /// <param name="str"></param>
        private void SaveLog(string str)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }
                    string filename = logPath + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    StreamWriter mySw = File.AppendText(filename);
                    mySw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss::ffff\t") + str);
                    mySw.Close();
                }
                catch
                {
                    return;
                }
            });
        }

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(null != vmSolutionPath && true == mSolutionIsLoad)
            {
                if (MessageBox.Show("Save solution or not?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (vmProcedure != null)
                        {
                            VmSolution.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        string strMsg = "Failed to save solution!";
                        LogFunction(strMsg);
                    }
                }
            }
        }
        /// <summary>
        ///  Switch between Chinese and English
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChineseOREnglish_Click(object sender, EventArgs e)
        {
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                LoadLanguage(this, typeof(MainForm));
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
                LoadLanguage(this, typeof(MainForm));
            }
            cultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }
        public static void LoadLanguage(Form form, Type formType)
        {
            if (form != null)
            {
                ComponentResourceManager resources = new ComponentResourceManager(formType);
                resources.ApplyResources(form, "$this");
                Loading(form, resources);
            }
        }
        private static void Loading(Control control, ComponentResourceManager resources)
        {
            if (control is ListView)
            {
                resources.ApplyResources(control, control.Name);
                ListView ts = (ListView)control;
                resources.ApplyResources(ts.Columns[0], "timeStampHeader");
                resources.ApplyResources(ts.Columns[1], "infoHeader");
                //foreach (ColumnHeader c in ts.Columns)
                //{
                //    resources.ApplyResources(c, c.Name);
                //}
            }
            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                if (c.Controls != null)
                {
                    Loading(c, resources);
                }
            }

        }
    }
}
