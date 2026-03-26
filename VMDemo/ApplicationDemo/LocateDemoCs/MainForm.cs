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
using VMControls.Interface;

namespace LocateDemoCs
{
    public partial class MainForm : Form
    {

        /// <summary>
        /// The control for display picture
        /// </summary>
        private RenderControl renderControl;

        /// <summary>
        /// The control for configure parameter
        /// </summary>
        private MainViewControl mainViewControl;

        /// <summary>
        /// Solution Path
        /// </summary>
        private string currentSolutionPath = String.Empty;

        /// <summary>
        /// Process List
        /// </summary>
        private List<VmProcedure> processList;


        /// <summary>
        /// isSolutionLoad = false, indicates that the solution is not loaded
        /// </summary>
        private bool SolutionIsLoaded = false;
         
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
            LoadSolutionIndicateTimer.Interval = 300;
            LoadSolutionIndicateTimer.Tick += LoadSolutionIndicateTimer_Tick;

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
                        var resourceManager = new ResourceManager("LocateDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiStop.Text", new CultureInfo("zh-CN"));
                    }
                    else
                    {
                        var resourceManager = new ResourceManager("LocateDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiStop.Text", new CultureInfo("en-US"));
                    }

                    //Disable button
                    buttonSelectSolu.Enabled = false;
                    buttonRunOnce.Enabled = false;
                    buttonLoadSolu.Enabled = false;
                    buttonSaveSolu.Enabled = false;
                    comboProcedure.Enabled = false;

                    strMessage = "Start continuous run!";
                    AppendLog(strMessage);
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
                        var resourceManager = new ResourceManager("LocateDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiRun.Text", new CultureInfo("zh-CN"));
                    }
                    else
                    {
                        var resourceManager = new ResourceManager("LocateDemoCs.MainForm", typeof(MainForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiRun.Text", new CultureInfo("en-US"));
                    }

                    //Enable button
                    buttonSelectSolu.Enabled = true;
                    buttonRunOnce.Enabled = true;
                    buttonLoadSolu.Enabled = true;
                    buttonSaveSolu.Enabled = true;
                    comboProcedure.Enabled = true;

                    strMessage = "End Run!";
                    AppendLog(strMessage);
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
            if (!SolutionIsLoaded)
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
            if (SolutionIsLoaded)
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
            renderPanel.Controls.Add(mainViewControl);
            renderPanel.Controls.Add(renderControl);
            renderPanel.Controls.Remove(mainViewControl);
        }

        /// <summary>
        /// Append Log
        /// </summary>
        /// <param name="message"></param>
        private void AppendLog(string message)
        {
            try
            {
                string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");
                if (listViewLog.Items.Count > 10000)
                    listViewLog.Items.Clear();
                if (listViewLog.InvokeRequired)
                {
                    listViewLog.BeginInvoke(new Action(() =>
                    {
                        listViewLog.Items.Insert(0,new ListViewItem(new string[] {timeStamp, message}));
                    }));
                }
                else
                {
                    listViewLog.Items.Insert(0,new ListViewItem(new string[] {timeStamp, message}));
                }

                if (!Directory.Exists("./log"))
                {
                    Directory.CreateDirectory("./log");
                }
                using (FileStream fs = new FileStream("./log/LocateDemoCs.log", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(timeStamp + ":" + message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        ///  Update ListBox
        /// </summary>
        /// <param name="result"></param>
        private void UpdateResultListBox(string result)
        {
            if (listBoxResult.Items.Count > 10000)
                listBoxResult.Items.Clear();          
            string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");
            if (listBoxResult.InvokeRequired)
            {
                listBoxResult.BeginInvoke(new Action(() =>
                {
                    listBoxResult.Items.Insert(0,timeStamp + result);
                }));
            }
            else
            {
                listBoxResult.Items.Insert(0, timeStamp + result);
            }
        }

        /// <summary>
        /// Update Results
        /// </summary>
        /// <param name="result"></param>
        private void UpdateLableState(string result)
        {
            try
            {
                string[] strArray = result.Split(',');
                if (labelResultState.InvokeRequired)
                {
                    labelResultState.BeginInvoke(new Action(() =>
                    {
                        if (strArray[0] == "1")
                        {
                            labelResultState.BackColor = Color.FromArgb(0, 192, 0);
                            labelResultState.Text = "OK";
                        }
                        else
                        {
                            labelResultState.BackColor = Color.Red;
                            labelResultState.Text = "NG";
                        }
                    }));
                }
                else
                {
                    if (strArray[0] == "1")
                    {
                        labelResultState.BackColor = Color.DarkGreen;
                        labelResultState.Text = "OK";
                    }
                    else
                    {
                        labelResultState.BackColor = Color.Red;
                        labelResultState.Text = "NG";
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
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
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "VM Sol File(*.sol)|*.sol";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SolutionIsLoaded = false;
                    currentSolutionPath = ofd.FileName;
                    LoadSolutionIndicateTimer.Enabled = true;
                    AppendLog("The solution path is: " + currentSolutionPath);
                    MessageBox.Show("The solution path is: " + currentSolutionPath + "\nNext click the Load solution button!",
                        "Information", MessageBoxButtons.OK,MessageBoxIcon.Information);                                        
                }
                else
                {
                    currentSolutionPath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// Load Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadSolu_Click(object sender, EventArgs e)
        {
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
                if (currentSolutionPath != String.Empty)
                {
                    VmSolution.Load(currentSolutionPath);
                    processList = GetCurrentSolProcedureList();
                    UpdateProcessComboBox(processList);
                    RegisterProcedureWorkEndCallback(processList);//Registration callback for the end of the procedure run
                    SolutionIsLoaded = true;
                    renderControl.ModuleSource = processList[0];
                    AppendLog("Loading Solution succeeded!");
                    MessageBox.Show("Loading Solution succeeded!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
            catch (Exception ex)
            {
                listViewLog.Items.Add(new ListViewItem(new string[]
                    {DateTime.Now.ToString("YY-MM-DD HH:mm:ss-fff"), ex.Message}));
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
        /// Obtain all processes in the solution
        /// </summary>
        private List<VmProcedure> GetCurrentSolProcedureList()
        {
            List<VmProcedure> procedureList = new List<VmProcedure>();
            string processName = "";
            var processInfoList = VmSolution.Instance.GetAllProcedureList();
            for (int i = 0; i < processInfoList.nNum; i++)
            {
                processName = processInfoList.astProcessInfo[i].strProcessName;
                procedureList.Add((VmProcedure)VmSolution.Instance[processName]);          
            }
            return procedureList;
        }

        /// <summary>
        /// UpdateProcessComboBox
        /// </summary>
        /// <param name="lst"></param>
        private void UpdateProcessComboBox(List<VmProcedure> lst)
        {
            comboProcedure.Items.Clear();
            foreach (var vmProcedure in lst)
            {
                comboProcedure.Items.Add(vmProcedure.Name);
            }
            if (comboProcedure.Items.Count > 0)
            {
                comboProcedure.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// Save solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveSolu_Click(object sender, EventArgs e)
        {
            try
            {
                if (SolutionIsLoaded)
                {
                    VmSolution.Save();
                    AppendLog("Succeeded to save solution!");
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
                    
        }

        /// <summary>
        /// Run once
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRunOnce_Click(object sender, EventArgs e)
        {
            try
            {
                if (SolutionIsLoaded && comboProcedure.SelectedIndex != -1)
                {
                    string processName = comboProcedure.SelectedItem.ToString();
                    VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;
                    if (procedure != null)
                    {                       
                        procedure.Run();
                        AppendLog("Succeeded to run procedure once!");
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// Run Continuous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContiRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (SolutionIsLoaded && comboProcedure.SelectedIndex != -1)
                {
                    string processName = comboProcedure.SelectedItem.ToString();
                    VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;
                    if (procedure != null)
                    {
                        procedure.ContinuousRunEnable = procedure.ContinuousRunEnable ^ true;
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// Registration callback for the end of the procedure run
        /// </summary>
        /// <param name="lst"></param>
        public void RegisterProcedureWorkEndCallback(List<VmProcedure> lst)
        {
            try
            {
                foreach (var vmProcedure in processList)
                {
                    vmProcedure.OnWorkEndStatusCallBack += VmProcedure_OnWorkEndStatusCallBack;
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// Registration callback for the end of the procedure run
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VmProcedure_OnWorkEndStatusCallBack(object sender, EventArgs e)
        {
            try
            {
                VmProcedure procedure = sender as VmProcedure;
                if (procedure != null)
                {
                    var outputList = procedure.ModuResult.GetAllOutputNameInfo();
                    bool outputConfigIsWrong = true;
                    foreach (var ioNameInfo in outputList)
                    {
                        if (ioNameInfo.Name == "out" &&
                            ioNameInfo.TypeName == IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
                        {
                            var result = procedure.ModuResult.GetOutputString(ioNameInfo.Name);
                            string resultStrValue = result.astStringVal[0].strValue;
                            AppendLog("Result: "+resultStrValue);
                            UpdateResultListBox("Result: " + resultStrValue);
                            UpdateLableState(resultStrValue);
                            outputConfigIsWrong = false;
                        }
                    }
                    if (outputConfigIsWrong)
                    {
                        AppendLog("The result argument (out) is not exit or is not string format！");
                    }
                    AppendLog("Time" + procedure.ProcessTime);
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }                       
        }

        /// <summary>
        /// RenderControl binding procedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                renderControl.ModuleSource = (VmProcedure)VmSolution.Instance[comboProcedure.SelectedItem.ToString()];
            }   
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// Clear Log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemClearMsg_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
        }

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(null != currentSolutionPath && true == SolutionIsLoaded)
            {
                if (MessageBox.Show("Save solution or not? ", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (currentSolutionPath != String.Empty)
                        {
                            VmSolution.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLog(ex.Message);
                    }
                }
            }
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

        private void listViewLog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelResultState_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxSolution_Enter(object sender, EventArgs e)
        {

        }

        private void renderPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
