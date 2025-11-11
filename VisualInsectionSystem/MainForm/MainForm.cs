using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualInsectionSystem.SubForms;
using VM.Core;
using VM.PlatformSDKCS;  namespace VisualInsectionSystem {     public partial class MainForm : Form     {                  public static MainForm form1 = null;   // 设置form1为主窗口                public static List<Form> subForms = new List<Form>();   // 子窗口集合

        bool mSolutionIsLoad = false;  //是否加载方案

        VmProcedure m_VmProc = null;   //流程
       
        private string ProcedureFilePath = null;    /// Solution Path

        private List<VmProcedure> processList;  //流程列表

        string strMsg = null;  //提示信息

        private PLCCommunicator _plcComm;

        public static object Instance { get; internal set; }

        #region

        public MainForm()         {             InitializeComponent();             InitUI();             InitializePLCCommunicator();         }          private void InitUI()
        {
             //设置form1为主窗口             form1 = this;             //初始化子窗口集合             subForms.Add(new HKCamera());             //subForms.Add(new CommunicationForm());             subForms.Add(new DebugForm());             //subForms.Add(new AlarmForm());             //subForms.Add(new LogForm());             //subForms.Add(new UserForm());             //subForms.Add(new HelpForm());

            vmMainViewConfigControl1.Dock = DockStyle.Fill;
        }          private void InitializePLCCommunicator()
        {
            // 初始化S7-1200通信（IP地址根据实际设备修改）
            _plcComm = new PLCCommunicator(
                CpuType.S71200,
                "192.168.0.1",  // PLC实际IP
                0,              // 机架号
                1               // 插槽号
            );

            //// 订阅连接状态和错误事件
            //_plcComm.ConnectionStatusChanged += OnPlcConnectionStatusChanged;
            //_plcComm.HardwareErrorOccurred += OnPlcHardwareError;
        }          private void Form1_Load_1(object sender, EventArgs e)         {
            //UI_Control加载
            try
            {
                ListBox.CheckForIllegalCrossThreadCalls = false;
            }
            catch (Exception ex)
            {
                string strMsg = " Error Code: " + Convert.ToString(ex.Message);                
                MessageBox.Show(strMsg);
            }         }
        private void Form1_Closing(object sender, FormClosingEventArgs e)         {
            //UI关闭
            if (MessageBox.Show(@"确定退出？", @"提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)             {                 e.Cancel = true;                  return;             }

            //释放资源
            VmSolution.Instance?.Dispose();             Application.Exit();  //确保应用程序完全退出

        }

        #endregion 
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)         {          }         private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)         {          }         private void 相机ToolStripMenuItem_Click(object sender, EventArgs e)         {             try             {
                //打开相机页面
                HKCamera HKCameraForm = new HKCamera();                 HKCameraForm.Show();             }             catch (Exception ex)             {                 VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);                 if (null != vmEx)                 {                     string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);                     MessageBox.Show(strMsg);                 }
            }         }         private void 通讯ToolStripMenuItem_Click(object sender, EventArgs e)         {             try             {
                //打开通讯页面
                DebugForm debugForm = new DebugForm();                 debugForm.Show();             }             catch (Exception ex)             {                 VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);                 if (null != vmEx)                 {                     string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);                     MessageBox.Show(strMsg);                 }
            }         }         private void 调试ToolStripMenuItem_Click(object sender, EventArgs e)         {             try             {
                //打开调试页面
                DebugForm debugForm = new DebugForm();                 debugForm.Show();             }             catch (Exception ex)             {                 VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);                 if (null != vmEx)                 {                     string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);                     MessageBox.Show(strMsg);                 }
            }         }         private void 报警ToolStripMenuItem_Click(object sender, EventArgs e)         {          }         private void 日志ToolStripMenuItem_Click(object sender, EventArgs e)         {          }         private void 用户ToolStripMenuItem_Click(object sender, EventArgs e)         {          }         private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)         {          }         private void groupBox1_Enter(object sender, EventArgs e)         {          }         private void groupBox2_Enter(object sender, EventArgs e)         {          }         private void groupBox3_Enter(object sender, EventArgs e)         {          }         private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)         {          }         private void 打开方案ToolStripMenuItem_Click(object sender, EventArgs e)         {
            // disabled button
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;

            string message;             try             {                 using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = @"VM Sol File(*.sol)|*.sol";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 同时更新文件路径和文本框
                        ProcedureFilePath = openFileDialog.FileName; // ..\Demo.sol
                        textBox1.Text = ProcedureFilePath;
                        
                        listBox1.Items.Add($"已打开文件：{ProcedureFilePath}");  //添加打开记录
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }
                }                
            }             catch (VmException ex)
            {                 message = $"打开SOL文件失败：{ex.errorCode}";                 listBox1.Items.Add(message);                 listBox1.TopIndex = listBox1.Items.Count - 1;             }
            catch (Exception ex)    //增加通用异常捕获，防范非VmException的错误
            {
                listBox1.Items.Add($"意外错误：{ex.Message}");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            finally
            {
                button1.Enabled = !string.IsNullOrEmpty(ProcedureFilePath);
            }
        }

        /// <summary>         /// 加载方案按钮         /// </summary>         /// <param name="sender"></param>         /// <param name="e"></param>         private void button1_Click(object sender, EventArgs e)         {             //string strMsg = null;             try
            {
                this.Enabled = false;

                string strFilePath = textBox1.Text;     // 复制方案路径
                string strSoluPwd = textBox2.Text;      // password

                VmSolution.Load(strFilePath, strSoluPwd);

                processList = GetCurrentSolProcedureList();

                UpdateProcessComboBox(processList);
                
                mSolutionIsLoad = true;              // 1
            }
            catch (VmException ex)
            {
                strMsg = "LoadSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);                               
                MessageBox.Show(strMsg);
                return;
            }
            finally
            {
                this.Enabled = true;
            }                         // enable buttons
            button1.Enabled = true;
            button2.Enabled = true;             button3.Enabled = true;             button4.Enabled = true;

            strMsg = "LoadSolution success";
            listBox1.Items.Add(strMsg);
            listBox1.TopIndex = listBox1.Items.Count - 1;

        }

        /// <summary>
        /// CH:获取当前方案的所有流程  || Obtain all processes in the solution
        /// </summary>
        private List<VmProcedure> GetCurrentSolProcedureList()  
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
            comboBox1.Items.Clear();
            foreach (var vmProcedure in processList)
            {
                comboBox1.Items.Add(vmProcedure.Name);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }
        /// <summary>         /// 方案保存按钮         /// </summary>          private void button2_Click(object sender, EventArgs e)         {
            //string strMsg = null;

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
                    listBox1.Items.Add(strMsg);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                    return;
                }
                finally
                {
                    this.Enabled = true;
                }
                strMsg = "SaveSolution success";
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;

            }
            else
            {
                strMsg = "No solution file.";
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }
        /// <summary>
        /// 检查密码并输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)         {
            //string strMsg = null;             try
            {
                if (mSolutionIsLoad == true)
                {
                    if(VmSolution.Instance.HasPassword(textBox1.Text))
                    {
                        //VmSolution.Instance.Unlock(textBox1.Text);
                        strMsg = "The solution has password.";
                    }
                    else
                    {
                        strMsg = "The solution No password set";
                    }
                    textBox2.Text = strMsg;                    
                }
                else
                {
                    strMsg = "No solution file.";
                    listBox1.Items.Add(strMsg);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
            }             catch (VmException ex)
            {
                strMsg = "SaveSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }                       }         /// <summary>         /// 锁定按钮,方便后续         /// </summary>         private void button4_Click(object sender, EventArgs e)         {             vmMainViewConfigControl1.LockWorkArea();         }
        /// <summary>
        /// 解锁按钮
        /// </summary>    
        private void button5_Click(object sender, EventArgs e)
        {
            vmMainViewConfigControl1.UnlockWorkArea();

        }
        //文件另存为xxx
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(ProcedureFilePath))     //textBox1.text
            {
                MessageBox.Show("请先通过【文件打开】选择SOL文件", "路径为空",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("确认要另存当前解决方案吗？", "另存确认",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                this.Enabled = false;

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PRC files (*.prc)|*.prc | Sol File(*.sol)|*.sol | All files (*.*)|*.*"; // 设置文件过滤器
                    saveFileDialog.FileName = Path.GetFileName(ProcedureFilePath);  // textBox1.Text
                    saveFileDialog.InitialDirectory = Path.GetDirectoryName(ProcedureFilePath); // 设置初始目录

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 执行文件保存操作（这里假设是复制文件）
                        File.Copy(textBox1.Text, saveFileDialog.FileName, true);
                        //string savePath = saveFileDialog.FileName;
                        //VmSolution.SaveAs(savePath);


                        strMsg = /*[{DateTime.Now:HH:mm:ss}]*/ $"成功保存到：{saveFileDialog.FileName}";
                        listBox1.Items.Add(strMsg);
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }
                }

            }
            catch (VmException ex)
            {
                listBox1.Items.Add($"保存失败：{ex.Message}");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (IOException ioEx)
            {
                listBox1.Items.Add($"文件操作失败：{ioEx.Message}");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            finally
            {
                this.Enabled = true;
            }


        }           private void 打开调试ToolStripMenuItem_Click(object sender, EventArgs e)         {                       } 
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 选择一个流程打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPathLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openProcDialog = new OpenFileDialog
            {
                Filter = "VM Process Files|*.prc*"
            };
            DialogResult openFileRes = openProcDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                comboBox1.Text = openProcDialog.FileName;
            }
        }
        /// <summary>
        /// 导入操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
            {
                m_VmProc = VmProcedure.Load(comboBox1.Text);
                strMsg = "流程导入成功(ImportProcess success)";
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch(VmException ex)
            {
                strMsg = "流程导入失败(ImportProcess failed.)"+" Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                MessageBox.Show(strMsg);
            }
            
        }

        /// <summary>
        /// 导出操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
            {
                VmProcedure m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];   //绑定下拉框中选中的流程名称
                if (m_VmProc != null)
                {
                    try
                    {                        
                        m_VmProc.Save();
                        listBox1.Items.Add("流程导出成功" + comboBox1.Text + ".prc");
                        listBox1.TopIndex = listBox1.Items.Count - 1;

                    }
                    catch (VmException ex)
                    {
                        strMsg = "流程导出失败(ExportProcess failed.)"+" Error Code: " + Convert.ToString(ex.errorCode, 16);
                        listBox1.Items.Add(strMsg);
                        listBox1.TopIndex = listBox1.Items.Count - 1;                        
                    }
                }
                
            }
            catch (VmException ex)
            {
                MessageBox.Show("流程绑定出现失败." + Convert.ToString(ex.errorCode, 16));
            }
        }
        /// <summary>
        /// 删除一个流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            //string strMsg = null;            
            try
            {
                m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
                if(m_VmProc != null)
                {
                    listBox1.Items.Add("流程删除成功" + comboBox1.Text);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
                //m_VmProc.IsEnabled = false; // 删除前禁用
                m_VmProc.Dispose();
            }
            catch (VmException ex)
            {
                
                strMsg = "DeleteProcess failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                //MessageBox.Show("流程删除失败." + Convert.ToString(ex.errorCode, 16));
            }
        }

        /// <summary>
        /// 流程列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            try
            {
                ProcessInfoList vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();  //获取流程
                VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance["ExampleDemo1"];
                if (vmProcessInfoList.nNum == 0)
                {
                    MessageBox.Show("未获取到流程列表.");
                    return;

                }
                comboBox1.Items.Clear();
                comboBox1.Items.Add("ExampleDemo1");
                for (int item = 0; item < vmProcessInfoList.nNum; item++)
                {
                    comboBox1.Items.Add(vmProcessInfoList.astProcessInfo[item].strProcessName);  //添加流程名称到下拉列表
                }
            }
            catch (VmException ex)
            {
                string message = "获取流程列表失败." + ex.errorCode;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }

        }           

        /// <summary>
        /// 单次执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExecuteOnce_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
            {
                m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
                if (m_VmProc == null)
                {
                    MessageBoxButtons msgType = MessageBoxButtons.OK;
                    DialogResult diagMsg = MessageBox.Show(comboBox1.Text + " name procedure does't exist", "Prompt", msgType);
                    if (diagMsg == DialogResult.OK)
                    {
                        return;
                    }
                }
                m_VmProc.Run();

                strMsg = "Process run success.";
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                strMsg = "Process run failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }

        /// <summary>
        /// 连续执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContinuExecute_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
            {
                m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
                if (m_VmProc == null)
                {
                    MessageBoxButtons msgType = MessageBoxButtons.OK;
                    DialogResult diagMsg = MessageBox.Show(comboBox1.Text + " name procedure does't exist", "Prompt", msgType);
                    if (diagMsg == DialogResult.OK)
                    {
                        return;
                    }
                }
                m_VmProc.ContinuousRunEnable = true;

                strMsg = "ContinuExecute success.";
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                strMsg = "Process Continous Run failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;

            }
        }
        /// <summary>
        /// stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStopExecute_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
            {
                m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
                if (m_VmProc == null)
                {
                    MessageBoxButtons msgType = MessageBoxButtons.OK;
                    DialogResult diagMsg = MessageBox.Show(comboBox1.Text + " name procedure does't exist", "Prompt", msgType);
                    if (diagMsg == DialogResult.OK)
                    {
                        return;
                    }
                }
                m_VmProc.ContinuousRunEnable = false;

                strMsg = "StopExecute success";
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                strMsg = "StopExecute failed. Error Code:" + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;

            }
            
        }

        private void buttonSetTimeInterval_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
            {
                string strTimeInteval = textBoxTimeInterval.Text;  //定位
                if(string.IsNullOrEmpty(strTimeInteval))
                {
                    MessageBoxButtons msgType = MessageBoxButtons.OK;
                    DialogResult diaMsg = MessageBox.Show("Please enter the time interval!", "Prompt", msgType);
                    if(diaMsg == DialogResult.OK)
                    {
                        return;
                    }
                }
                uint nTimeInterval = 0;
                nTimeInterval = uint.Parse(strTimeInteval);
                m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
                
                if(m_VmProc == null)
                {
                    MessageBoxButtons msgType = MessageBoxButtons.OK;
                    DialogResult diagMsg = MessageBox.Show(comboBox1.Text + " name procedure does't exist", "Prompt", msgType);
                    if (diagMsg == DialogResult.OK)
                    {
                        return;
                    }
                }

                m_VmProc.SetContinousRunInterval(nTimeInterval);  // 设置运行间隔

               
            }
            catch(VmException ex)
            {
                strMsg = "SetContinousRunInterval failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }

        private void vmMainViewConfigControl1_Load(object sender, EventArgs e)
        {
            vmMainViewConfigControl1.BindMultiProcedure(); //绑定多流程
        } 

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
     } 