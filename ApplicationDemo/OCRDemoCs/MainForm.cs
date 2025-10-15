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
using VM.Core;
using VM.PlatformSDKCS;

namespace OCRDemoCs
{
    public partial class MainForm : Form
    {
        private string strSolutionPath = null;//Solution Path
        private VmProcedure procedure = null;
        private bool isSolutionLoad = false;//isSolutionLoad = false, indicates that the solution is not loaded
        private bool isContinuRun = false;//isContinuRun = false,indicates that the procedure stop run
        private string logPath = Application.StartupPath + "/Log/Message";//Log Path
        private MainViewControl mainViewControl;
        private RenderControl renderControl;
        private Timer LoadSolutionIndicateTimer = new Timer();
        private string cultureName = "zh-CN";

        public MainForm()
        {
            InitializeComponent();
            renderControl = new RenderControl();
            mainViewControl = new MainViewControl();
            mainViewControl.Dock = DockStyle.Fill;
            renderControl.Dock = DockStyle.Fill;
            panel1.Controls.Add(mainViewControl);
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
                    isContinuRun = true;

                    if ("zh-CN" == cultureName)
                    {
                        var resourceManager = new ResourceManager("OCRDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiStop.Text", new CultureInfo("zh-CN"));
                    }
                    else
                    {
                        var resourceManager = new ResourceManager("OCRDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiStop.Text", new CultureInfo("en-US"));
                    }

                    //Disable button
                    buttonSelectSolu.Enabled = false;
                    buttonRunOnce.Enabled = false;
                    buttonLoadSolu.Enabled = false;
                    buttonSaveSolu.Enabled = false;
                    comboProcedure.Enabled = false;

                    strMessage = "Start continuous run!";
                    LogUpdate(strMessage);
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
                    isContinuRun = false;

                    if ("zh-CN" == cultureName)
                    {
                        var resourceManager = new ResourceManager("OCRDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiRun.Text", new CultureInfo("zh-CN"));
                    }
                    else
                    {
                        var resourceManager = new ResourceManager("OCRDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiRun.Text", new CultureInfo("en-US"));
                    }

                    //Enable button
                    buttonSelectSolu.Enabled = true;
                    buttonRunOnce.Enabled = true;
                    buttonLoadSolu.Enabled = true;
                    buttonSaveSolu.Enabled = true;
                    comboProcedure.Enabled = true;

                    strMessage = "End Run!";
                    LogUpdate(strMessage);
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
            if (!isSolutionLoad)
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
            if (isSolutionLoad)
            {
                buttonLoadSolu.BackColor = Color.DimGray;
            }
        }

        /// <summary>
        /// Callback function for the procedure working status
        /// </summary>
        /// <param name="workStatusInfo"></param>
        private void VmSolution_OnWorkStatusEvent(VM.PlatformSDKCS.ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            string strMessage = null;
            try
            {
                if (workStatusInfo.nWorkStatus == 0 && workStatusInfo.nProcessID == 10000)//When the process is running, the nWorkStatus is 1
                {
                    List<VmDynamicIODefine.IoNameInfo> ioNameInfos = procedure.ModuResult.GetAllOutputNameInfo();
                    if (ioNameInfos.Count != 0)//Determine the number of process results
                    {
                        if (ioNameInfos[0].TypeName == IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
                        {
                            //Obtain the results of the process
                            string strResult = procedure.ModuResult.GetOutputString(ioNameInfos[0].Name).astStringVal[0].strValue;
                            if (strResult != null)
                            {
                                UpdateResult(strResult);
                                strMessage = "Process running time：" + procedure.ProcessTime.ToString() + "ms";
                                LogUpdate(strMessage);
                            }
                            else
                            {
                                strMessage = "Failed to get results: The result is empty!";
                                LogUpdate(strMessage);
                            }
                        }
                    }
                    else
                    {
                        strMessage = "Failed to get results: the number of process results is 0";
                        LogUpdate(strMessage);
                    }
                }
            }
            catch (VmException ex)
            {
                strMessage = "Failed to get results, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "Failed to get results: " + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// Update Results
        /// </summary>
        /// <param name="strResult"></param>
        private void UpdateResult(string strResult)
        {
            this.BeginInvoke(new Action(() =>
            {
                string[] vs = strResult.Split(';');
                if (vs[0] == "1")
                {
                    labelResult.Text = "OK";
                    labelResult.BackColor = Color.FromArgb(0, 192, 0);
                }
                else
                {
                    labelResult.Text = "NG";
                    labelResult.BackColor = Color.Red;
                }
                string result = "Results：CodeInfo: " + vs[2] + ";  Number of characters: " + vs[1] + "; Confidence: " + vs[3];
                listBoxResult.Items.Add(result);
                listBoxResult.TopIndex = listBoxResult.Items.Count - 1;
            }));
        }

        /// <summary>
        /// Load Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadSolu_Click(object sender, EventArgs e)
        {
            string strMessage = null;
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
                if (isSolutionLoad == true)
                {
                    isSolutionLoad = false;
                }
                VmSolution.Load(strSolutionPath);
                isSolutionLoad = true;
                strMessage = "Loading Solution succeeded!";
                LogUpdate(strMessage);
                MessageBox.Show("Loading Solution succeeded!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (comboProcedure.Items.Count != 0)
                {
                    comboProcedure.Items.Clear();
                }

                ProcessInfoList processInfoList = VmSolution.Instance.GetAllProcedureList();//Obtain all processes in the solution
                for (int i = 0; i < processInfoList.nNum; i++)
                {
                    comboProcedure.Items.Add(processInfoList.astProcessInfo[i].strProcessName);
                }
                if (comboProcedure.Items.Count > 0)
                {
                    comboProcedure.SelectedIndex = 0;//Defaults to the first process
                    procedure = VmSolution.Instance[processInfoList.astProcessInfo[0].strProcessName] as VmProcedure;
                    if (procedure == null)
                    {
                        strMessage = "The procedure is null. Please check the solution!";
                        LogUpdate(strMessage);
                        return;
                    }
                    //RenderControl binding procedure
                    renderControl.vmRenderControl1.ModuleSource = procedure;
                }
                else
                {
                    strMessage = "If the number of flows is 0, check the solution!";
                    LogUpdate(strMessage);
                }
            }
            catch (VmException ex)
            {
                strMessage = "Failed to load solution, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "Failed to load solution: " + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
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
        /// Select procedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxAllProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                procedure = VmSolution.Instance[comboProcedure.SelectedItem.ToString()] as VmProcedure;
                renderControl.vmRenderControl1.ModuleSource = procedure;
                strMessage = "Select[" + comboProcedure.SelectedItem.ToString() + "]succeeded!";
                LogUpdate(strMessage);
            }
            catch (VmException ex)
            {
                strMessage = "Failed to select procedure, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "Failed to select procedure: " + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// Save Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveSolu_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                VmSolution.Save();
                strMessage = "Succeeded to save solution!";
                LogUpdate(strMessage);
            }
            catch (VmException ex)
            {
                strMessage = "Failed to save solution, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "Failed to save solution: " + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(null != strSolutionPath && true == isSolutionLoad)
            {
                if (MessageBox.Show("Save solution or not?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (procedure != null || isContinuRun == true)
                        {
                            procedure.ContinuousRunEnable = false;
                            VmSolution.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        string strMsg = "Failed to save solution!";
                        LogUpdate(strMsg);
                    }
                }
            }
        }

        /// <summary>
        /// Load Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(renderControl);
            buttonRender.BackColor = Color.Orange;
        }

        /// <summary>
        /// Select solution path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectSolu_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VM Sol File|*.sol*";
            DialogResult openFileRes = openFileDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                strSolutionPath = openFileDialog.FileName;
                isSolutionLoad = false;
                LoadSolutionIndicateTimer.Enabled = true;
                strMessage = "Succeeded to select solution path!";
                LogUpdate(strMessage);
                MessageBox.Show("The solution path is： " + strSolutionPath + "\nNext click the Load solution button!",
    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Run once
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRunOnce_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                if (isSolutionLoad == true && null != procedure)
                {
                    procedure.Run();
                }
                else
                {
                    strMessage = "The procedure does not exist!";
                    LogUpdate(strMessage);
                }
            }
            catch (VmException ex)
            {
                strMessage = "Failed to run procedure once, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "Failed to run procedure once: " + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// Run Continuous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContiRun_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                if (isSolutionLoad == true && null != procedure)
                {
                    procedure.ContinuousRunEnable = procedure.ContinuousRunEnable ^ true;
                    isContinuRun = isContinuRun ^ true;
                }
                else
                {
                    strMessage = "The procedure does not exist!";
                    LogUpdate(strMessage);
                }
            }
            catch (VmException ex)
            {
                strMessage = "Failed to run procedure continuous, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "Failed to run procedure continuous: " + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// Display picture page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRender_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                panel1.Controls.Clear();
                panel1.Controls.Add(renderControl);
                buttonRender.BackColor = Color.Orange;
                buttonConfig.BackColor = Color.Gray;
                strMessage = "Successed to switch Display picture page!";
                LogUpdate(strMessage);
            }
            catch (VmException ex)
            {
                strMessage = "Failed to switch Display picture page, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "Failed to switch Display picture page: " + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// Configure parameter page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfig_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                panel1.Controls.Clear();
                panel1.Controls.Add(mainViewControl);
                buttonConfig.BackColor = Color.Orange;
                buttonRender.BackColor = Color.Gray;
                strMessage = "Successed to switch Configure parameter page!";
                LogUpdate(strMessage);
            }
            catch (VmException ex)
            {
                strMessage = "Failed to switch Configure parameter page, Error code: 0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "Failed to switch Configure parameter page: " + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }

        }

        /// <summary>
        /// Update Log
        /// </summary>
        /// <param name="str"></param>
        private void LogUpdate(string str)
        {
            string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");         
            if (listViewLog.Items.Count > 10000)
                listViewLog.Items.Clear();

            listViewLog.BeginInvoke(new Action(() =>
            {
                listViewLog.Items.Insert(0, new ListViewItem(new string[] { timeStamp, str }));
            }));

            SaveLog(str);
        }

        /// <summary>
        /// Save log
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
        /// Clear Meaage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logClear_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
        }

        /// <summary>
        /// Switch between Chinese and English
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
                resources.ApplyResources(ts.Columns[0], "logTimeHeader");
                resources.ApplyResources(ts.Columns[1], "logMessageHeader");
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
