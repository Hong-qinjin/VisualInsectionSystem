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
using VMControls.Winform.Release;
using VMControls.WPF.Release;

namespace VisualInsectionSystem
{
    public partial class DebugForm : Form    // 继承自MainForm
    {
        /// <summary>
        /// 图片显示面板
        /// </summary>
        private readonly RenderControl renderControl;

        /// <summary>
        /// 参数面板config parameter
        /// </summary>
        private readonly MainViewControl mainViewControl;

        /// <summary>
        /// Solution Path
        /// </summary>
        private string currentSolutionPath = null;

        /// <summary>
        /// Process List
        /// </summary>    
        private List<VmProcedure> processList;
        // private ProcessInfoList processInfoList = new ProcessInfoList();

        /// <summary>
        /// isSolutionLoad = false, indicates that the solution is not loaded
        /// 默认方案不加载
        /// </summary>
        private bool SolutionIsLoaded = false;        

        private readonly string logPath = Application.StartupPath + "/Log/Message";

        private readonly Timer LoadSolutionIndicateTimer = new Timer();
        private string cultureName = "zh-CN";

        public DebugForm()
        {
            InitializeComponent();
            renderControl = new RenderControl();        //实例化渲染面板
            mainViewControl = new MainViewControl();    //参数设置
            renderControl.Dock = DockStyle.Fill;        //填充
            mainViewControl.Dock = DockStyle.Fill;

            LoadSolutionIndicateTimer.Interval = 300;                           //设置定时器
            LoadSolutionIndicateTimer.Tick += LoadSolutionIndicateTimer_Tick;   //指示方案是否加载
            
            //VmSolution.OnProcessStatusStartEvent += VmSolution_OnProcessStatusStartEvent;   // Registration callback for the start of the procedure continuous run
            //VmSolution.OnProcessStatusStopEvent += VmSolution_OnProcessStatusStopEvent;     // Registration callback for the stop of the procedure continuous run

            cultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }
    

        /// <summary>
        /// 开始the start of the procedure continuous run
        /// </summary>
        /// <param name="statusInfo"></param>
        private void VmSolution_OnProcessStatusStartEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_START_CONTINUOUSLY_INFO statusInfo)
        {
            if (!this.IsHandleCreated || this.IsDisposed)
            {
                // 输出调试日志，跟踪句柄状态
                System.Diagnostics.Debug.WriteLine($"StartEvent: Handle未创建（{this.IsHandleCreated}）或已释放（{this.IsDisposed}）");
                return;
            }
            //error，在创建窗口句柄之前，不能在控件上调用 Invoke 或 BeginInvoke。
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateStartStatusUI(statusInfo)));
            }
            else
            {
                UpdateStartStatusUI(statusInfo);
            }
        }

        /// <summary>
        ///  停止 the stop of the procedure continuous run
        /// </summary>
        /// <param name="statusInfo"></param>
        private void VmSolution_OnProcessStatusStopEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_STOP_INFO statusInfo)
        {

            if (!this.IsHandleCreated || this.IsDisposed)
            {
                System.Diagnostics.Debug.WriteLine($"StopEvent: Handle未创建（{this.IsHandleCreated}）或已释放（{this.IsDisposed}）");
                return;
            }
            //句柄error：在创建窗口句柄之前，不能在控件上调用 Invoke 或 BeginInvoke。
            // 确保在UI线程执行
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateStopStatusUI(statusInfo)));
            }
            else
            {
                UpdateStopStatusUI(statusInfo);
            }          
        }

        // 拆分UI更新逻辑为独立方法（提高可读性）
        private void UpdateStartStatusUI(ImvsSdkDefine.IMVS_STATUS_PROCESS_START_CONTINUOUSLY_INFO statusInfo)
        {
            if (statusInfo.nStatus == 0)
            {
                string strMessage;
                var resourceManager = new ResourceManager("VisualInsectionSystem.DebugForm", typeof(DebugForm).Assembly);
                buttonContiRun.Text = "zh-CN" == cultureName
                    ? resourceManager.GetString("buttonContiStop.Text", new CultureInfo("zh-CN"))
                    : resourceManager.GetString("buttonContiStop.Text", new CultureInfo("en-US"));

                // 禁用按钮
                buttonSelectSolu.Enabled = false;
                buttonRunOnce.Enabled = false;
                buttonLoadSolu.Enabled = false;
                buttonSaveSolu.Enabled = false;
                comboProcedure.Enabled = false;

                strMessage = "Start continuous run!";
                AppendLog(strMessage);
            }
        }

        private void UpdateStopStatusUI(ImvsSdkDefine.IMVS_STATUS_PROCESS_STOP_INFO statusInfo)
        {
            if (statusInfo.nStopAction == 1)
            {
                string strMessage;
                var resourceManager = new ResourceManager("VisualInsectionSystem.DebugForm", typeof(DebugForm).Assembly);
                buttonContiRun.Text = "zh-CN" == cultureName
                    ? resourceManager.GetString("buttonContiRun.Text", new CultureInfo("zh-CN"))
                    : resourceManager.GetString("buttonContiRun.Text", new CultureInfo("en-US"));

                // 启用按钮
                buttonSelectSolu.Enabled = true;
                buttonRunOnce.Enabled = true;
                buttonLoadSolu.Enabled = true;
                buttonSaveSolu.Enabled = true;
                comboProcedure.Enabled = true;

                strMessage = "End Run!";
                AppendLog(strMessage);
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
        ///  Load Form 加载组件
        /// </summary>     
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebugForm_Load(object sender, EventArgs e)
        {
            try
            {
                renderPanel.Controls.Clear();
                renderPanel.Controls.Add(mainViewControl);
                renderPanel.Controls.Add(renderControl);
                renderPanel.Controls.Remove(mainViewControl);               

                // 句柄创建后注册事件（关键修复）
                VmSolution.OnProcessStatusStartEvent += VmSolution_OnProcessStatusStartEvent;
                VmSolution.OnProcessStatusStopEvent += VmSolution_OnProcessStatusStopEvent;

                // 输出句柄状态日志（调试用）
                System.Diagnostics.Debug.WriteLine($"DebugForm_Load: 句柄创建状态 - {this.IsHandleCreated}");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                if (listViewLog.Items.Count > 1000)
                    listViewLog.Items.Clear();
                if (listViewLog.InvokeRequired && listViewLog.IsHandleCreated && !listViewLog.IsDisposed)
                {
                    listViewLog.BeginInvoke(new Action(() =>
                    {
                        listViewLog.Items.Insert(0, new ListViewItem(new string[] { timeStamp, message }));
                    }));
                }
                else if (!listViewLog.InvokeRequired)
                {
                    listViewLog.Items.Insert(0, new ListViewItem(new string[] { timeStamp, message }));
                }
                else
                {
                    // 句柄未就绪时记录到文件，避免丢失日志
                    System.Diagnostics.Debug.WriteLine($"AppendLog: listViewLog句柄未就绪，消息：{message}");
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
        /// 更新listbox,显示结果
        /// </summary>
        /// <param name="result"></param>
        private void UpdateResultListBox(string result)
        {
            if (listBoxResult.Items.Count > 1000)
                listBoxResult.Items.Clear();
            string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");

            //if (listBoxResult.InvokeRequired)
            if (listBoxResult.InvokeRequired && listBoxResult.IsHandleCreated && !listBoxResult.IsDisposed)
            {
                listBoxResult.BeginInvoke(new Action(() =>
                {
                    listBoxResult.Items.Insert(0, timeStamp + result);
                }));
            }
            else if (!listBoxResult.InvokeRequired)
            {
                listBoxResult.Items.Insert(0, timeStamp + result);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"UpdateResultListBox: listBoxResult句柄未就绪，结果：{result}");
            }            
        }
        /// <summary>
        /// 更新结果标签ok
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
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    //openFileDialog.InitialDirectory =
                    Filter = "VM Sol File(*.sol)|*.sol"
                };  // 文件夹路径
                DialogResult oFresult = openFileDialog.ShowDialog();

                //System.Diagnostics.Debug.WriteLine($"对话框结果: {oFresult}");   // 调试输出对话框结果
                if (oFresult == DialogResult.OK)
                {                    
                    currentSolutionPath = openFileDialog.FileName;
                    //System.Diagnostics.Debug.WriteLine($"选中的文件路径: {currentSolutionPath}");  // 调试输出获取的路径
                    LoadSolutionIndicateTimer.Enabled = true;
                    AppendLog("The solution path is: " + currentSolutionPath);
                  
                    listBoxResult.Items.Add("The solution loaded");
                    listBoxResult.TopIndex = listBoxResult.Items.Count - 1;
                    SolutionIsLoaded = true;
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
            buttonSaveSolu.Enabled = false;
            buttonRunOnce.Enabled = false;
            buttonContiRun.Enabled = false;

            comboProcedure.Enabled = false;
            buttonRender.Enabled = false;
            buttonConfig.Enabled = false;
            try
            {
                if (currentSolutionPath != String.Empty)
                {
                    VmSolution.Load(currentSolutionPath);           //加载方案
                    SolutionIsLoaded = true;

                    processList = GetCurrentSolProcedureList();     //列举
                    UpdateProcessComboBox(processList);             //更新combobox                    
                    RegisterProcedureWorkEndCallback(processList);  //Registration callback for the end of the procedure run                    
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
            string processName;
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
                    //***Get the selected process
                    if (VmSolution.Instance[processName] is VmProcedure procedure)
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
        /// 持续运行
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
                    if (VmSolution.Instance[processName] is VmProcedure procedure)
                    {
                        //procedure.ContinuousRunEnable = true;
                        procedure.ContinuousRunEnable ^= true;  //Toggle the continuous run status                        
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }


        #region 
        /// <summary>
        /// Registration callback for the end of the procedure run
        /// </summary>
        /// <param name="lst"></param>
        public void RegisterProcedureWorkEndCallback(List<VmProcedure> lst)
        {
            if (lst is null)
            {
                throw new ArgumentNullException(nameof(lst));
            }

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
                if (sender is VmProcedure procedure)
                {
                    var outputList = procedure.ModuResult.GetAllOutputNameInfo();  //输出信息
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
        /// RenderControl bingding procedure绑定流程
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
            // 关闭窗口后，移除事件订阅
            VmSolution.OnProcessStatusStartEvent -= VmSolution_OnProcessStatusStartEvent;
            VmSolution.OnProcessStatusStopEvent -= VmSolution_OnProcessStatusStopEvent;

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

            // Dispose VmSolution instance to release resources
            VmSolution.Instance.Dispose();
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
            if (control is ListView view)
            {
                resources.ApplyResources(control, control.Name);
                ListView ts = view;
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
