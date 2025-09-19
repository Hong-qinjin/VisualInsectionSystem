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
//using System.Windows.Controls;
using System.Windows.Forms;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Interface;
using VMControls.Winform.Release;

namespace VisualInsectionSystem
{
    public partial class DebugForm : Form    /// Debug form
    {
        /// <summary>
        /// 图片显示面板
        /// </summary>
        private RenderControl renderControl;

        /// <summary>
        /// 参数面板
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
        /// 默认方案不加载
        /// </summary>
        private bool SolutionIsLoaded = false;

        private Timer LoadSolutionIndicateTimer = new Timer();
        private string cultureName = "zh-CN";

        public DebugForm()
        {           
            InitializeComponent();
            renderControl = new RenderControl();        //实例化渲染面板
            mainViewControl = new MainViewControl();    //参数设置
            renderControl.Dock = DockStyle.Fill;        //填充
            mainViewControl.Dock = DockStyle.Fill;

            buttonRender.BackColor = Color.Blue;
            buttonConfig.BackColor = Color.Blue;

            LoadSolutionIndicateTimer.Interval = 300;                           //设置定时器
            LoadSolutionIndicateTimer.Tick += LoadSolutionIndicateTimer_Tick;   //设置定时器事件

            VmSolution.OnProcessStatusStartEvent += VmSolution_OnProcessStatusStartEvent;   // Registration callback for the start of the procedure continuous run
            VmSolution.OnProcessStatusStopEvent += VmSolution_OnProcessStatusStopEvent;     // Registration callback for the stop of the procedure continuous run

            cultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }

        /// <summary>
        /// callback function for the 开始 of the procedure continuous run
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
                        var resourceManager = new ResourceManager("Demo.DebugForm", typeof(DebugForm).Assembly);
                        buttonContiRun.Text = resourceManager.GetString("buttonContiStop.Text", new CultureInfo("zh-CN"));
                    }
                    else
                    {
                        var resourceManager = new ResourceManager("Demo.DebugForm", typeof(DebugForm).Assembly);
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
        /// Callback function for the 停止 of the procedure continuous run
        /// </summary>
        /// <param name="statusInfo"></param>
        private void VmSolution_OnProcessStatusStopEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_STOP_INFO statusInfo)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    if (statusInfo.nStopAction == 1)
                    {
                        string strMessage = null;

                        if ("zh-CN" == cultureName)
                        {
                            var resourceManager = new ResourceManager("Form1.DebugForm", typeof(DebugForm).Assembly);  //resource`
                            buttonContiRun.Text = resourceManager.GetString("buttonContiRun.Text", new CultureInfo("zh-CN"));
                        }
                        else
                        {
                            var resourceManager = new ResourceManager("Form1.DebugForm", typeof(DebugForm).Assembly);
                            buttonContiRun.Text = resourceManager.GetString("buttonContiRun.Text", new CultureInfo("en-US"));
                        }

                        //Enable button
                        buttonSelectSolu.Enabled = true;
                        buttonRunOnce.Enabled = true;
                        buttonLoadSolu.Enabled = true;
                        buttonSaveSolu.Enabled = true;
                        comboProcedure.Enabled = true;

                        strMessage = "End continuous run!";
                        AppendLog(strMessage);
                    }
                }));
            }
            else
            {
                AppendLog("Window handle not created yet. Unable to update UI.");
            }
               
        }

        /// <summary>
        /// The button for loading the solution flashes, prompting you to load the solution after selecting the path
        /// </summary>         
        private void LoadSolutionIndicateTimer_Tick(object sender, EventArgs e)
        {
            if (!SolutionIsLoaded)
            {
                if (buttonLoadSolu.BackColor == Color.Blue)
                {
                    buttonLoadSolu.BackColor = Color.Orange;
                }
                else
                {
                    buttonLoadSolu.BackColor = Color.Blue;
                }
            }
            if (SolutionIsLoaded)
            {
                buttonLoadSolu.BackColor = Color.White;
            }
        }

        /// <summary>
        ///  Lform
        /// </summary>     
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebugForm_Load(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(mainViewControl);
            renderPanel.Controls.Add(renderControl);
            renderPanel.Controls.Remove(mainViewControl);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppendLog(string message)
        {
            try
            {
                string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");
                if (listViewLog.Items.Count > 10)
                    listViewLog.Items.Clear();
                if (listViewLog.InvokeRequired)
                {
                    listViewLog.BeginInvoke(new Action(() =>
                    {
                        listViewLog.Items.Insert(0, new ListViewItem(new string[] { timeStamp, message }));
                    }));
                }
                else
                {
                    listViewLog.Items.Insert(0, new ListViewItem(new string[] { timeStamp, message }));
                }

                if (!Directory.Exists("./log"))
                {
                    Directory.CreateDirectory("./log");
                }
                using (FileStream fs = new FileStream("./log/Demo.log", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(timeStamp + ":" + message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 更新列表框
        /// </summary>
        /// <param name="result"></param>
        private void UpdateResultListBox(string result)
        {
            if (listBoxResult.Items.Count > 100)
                listBoxResult.Items.Clear();
            string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");
            if (listBoxResult.InvokeRequired)
            {
                listBoxResult.BeginInvoke(new Action(() =>
                {
                    listBoxResult.Items.Insert(0, timeStamp + result);
                }));
            }
            else
            {
                listBoxResult.Items.Insert(0, timeStamp + result);
            }
        }
        /// <summary>
        /// 更新结果标签
        /// </summary>
        /// <param name="result"></param>        
        private void UpdateLableState(string result)
        {
            try
            {
                string[] strArray = result.Split(',');
                if(labelResultState.InvokeRequired)
                {
                    labelResultState.BeginInvoke(new Action(() =>
                    {
                        if (strArray[0] == "1")  //OK
                        {
                            labelResultState.BackColor = Color.FromArgb(0, 192, 0);  //
                            labelResultState.Text = "OK";
                        }
                        else  //NG
                        {
                            labelResultState.BackColor = Color.Red;
                            labelResultState.Text = "NG";
                        }
                    }));
                }
                else  //
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
                }

            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
                //MessageBox.Show(ex.Message);
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
            
        }

        /// <summary>
        /// 参数显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfig_Click(object sender, EventArgs e)
        {
            try
            {
                renderPanel.Controls.Clear();
                renderPanel.Controls.Add(mainViewControl);  //  加载资源面板
                //验证
                if (!renderPanel.Controls.Contains(mainViewControl))
                {
                    AppendLog("The main view control is not loaded!");
                    //MessageBox.Show("The main view control is not loaded!");
                }

            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
                //MessageBox.Show(ex.Message);
            }  
                                                        
        }

        /// <summary>
        /// 选择方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectSolu_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();  //文件夹路径                
                openFileDialog.Filter = "VM Sol File(*.sol)|*.sol";                
                DialogResult oFresult = openFileDialog.ShowDialog();
                // 调试输出对话框结果
                System.Diagnostics.Debug.WriteLine($"对话框结果: {oFresult}");
                if ( DialogResult.OK == oFresult)
                {
                    SolutionIsLoaded = false;
                    currentSolutionPath = openFileDialog.FileName;

                    // 调试输出获取的路径
                    System.Diagnostics.Debug.WriteLine($"选中的文件路径: {currentSolutionPath}");

                    LoadSolutionIndicateTimer.Enabled = true;                    
                    AppendLog("The solution path is: " + currentSolutionPath);
                    MessageBox.Show("The solution path is: " + currentSolutionPath + "\nNext click the Load solution button!",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    listBoxResult.Items.Add("The solution loaded");
                    listBoxResult.TopIndex = listBoxResult.Items.Count - 1;
                }                
                else
                {
                    currentSolutionPath = String.Empty;
                }
                
            }
            catch (Exception ex)
            {                
                AppendLog(ex.Message);  //MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 加载方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadSolu_Click(object sender, EventArgs e)
        {
            LoadSolutionIndicateTimer.Enabled = false;
            buttonLoadSolu.BackColor = Color.Yellow;
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
                    VmSolution.Load(currentSolutionPath);           //加载方案
                    processList = GetCurrentSolProcedureList();     //列举
                    UpdateProcessComboBox(processList);             //更新combobox                    
                    RegisterProcedureWorkEndCallback(processList);  //Registration callback for the end of the procedure run
                    SolutionIsLoaded = true;    
                    renderControl.ModuleSource = processList[0];    //绑定渲染面板

                    AppendLog("Loading Solution succeeded!");       
                    MessageBox.Show("Loading Solution succeeded!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                listViewLog.Items.Add(new ListViewItem(new string[]  //ListViewItem异常
                    {DateTime.Now.ToString("YY-MM-DD HH:mm:ss-fff"), ex.Message}));
            }
            finally
            {
                buttonLoadSolu.BackColor = Color.DeepSkyBlue;
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
        private List<VmProcedure> GetCurrentSolProcedureList()  //获取当前方案的所有流程
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
        /// 更新combobox
        /// </summary> 
        /// <param name=lst"></param>
        private void UpdateProcessComboBox(List<VmProcedure> processList)
        {
            comboProcedure.Items.Clear();
            foreach (var vmProcedure in processList)
            {
                comboProcedure.Items.Add(vmProcedure.Name);
            }
            if (comboProcedure.Items.Count > 0)
            {
                comboProcedure.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 保存方案
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
                    AppendLog("Save Solution succeeded!");                    
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 运行一次
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
                    //VmProcedure procedure = (VmProcedure)VmSolution.Instance[processName];  //
                    VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;  //***Get the selected process
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
                //MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 持续yun
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
                    //VmProcedure procedure = (VmProcedure)VmSolution.Instance[processName];  //
                    VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;
                    if (procedure != null)
                    {
                        procedure.ContinuousRunEnable = procedure.ContinuousRunEnable ^ true;  //Toggle the continuous run status                        
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }


        #region 回调函数   
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
                            AppendLog("Result: " + resultStrValue);
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

        #endregion


        /// <summary>
        /// RenderControl bingding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                renderControl.ModuleSource = (VmProcedure)VmSolution.Instance[comboProcedure.SelectedItem.ToString()];  //***Get the selected process
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// 日志清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemClearMsg_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != currentSolutionPath && true == SolutionIsLoaded)
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
        /// Switch between Chinese and English 需要确保资源文件存在并且正确配置。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChineseOREnglish_Click(object sender, EventArgs e)
        {
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                LoadLanguage(this, typeof(DebugForm));
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
                LoadLanguage(this, typeof(DebugForm));
            }
            cultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }

        /// <summary>
        /// Load language
        /// </summary>
        public static void LoadLanguage(Form form, Type formType)
        {
            if (form != null)
            {
                ComponentResourceManager resources = new ComponentResourceManager(formType);
                resources.ApplyResources(form, "$this");
                //加载在DebugForm上的资源文件
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

        private void listBoxResult_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void renderPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listViewLog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBoxResult_Enter(object sender, EventArgs e)
        {

        }

        private void groupBoxSolution_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        
        private void labelResultState_Click(object sender, EventArgs e)
        {

        }
    }
}
