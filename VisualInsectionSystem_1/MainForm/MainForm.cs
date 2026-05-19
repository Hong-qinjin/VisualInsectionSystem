
using Newtonsoft.Json;
using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualInsectionSystem.SubForms;
using VisualInsectionSystem.Core;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Winform.Release;

namespace VisualInsectionSystem
{
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; private set; }
        public static List<Form> subForms = new List<Form>();   // 子窗口集合
        public HKCamera HKCameraInstance { get; private set; }
        private MESForm _mesForm;

        private bool mSolutionIsLoaded = false;     //是否加载
        bool mFrontedLoad = false;                  // 0330
        private Timer LoadSolutionIndicateTimer = new Timer();
        private string ProcedureFilePath = string.Empty;    //方案路径  
        private string strPassword = null;                  //方案密码
        VmProcedure m_VmProc = null;                //流程                          
        private List<VmProcedure> vmProInfoList;    //流程列表 

        private string strMsg = null;               //提示信息
        private PLCCommunicator _plcComm;

        #region  Mainform展示效果  

        // 边框颜色和宽度        
        private Color borderColor = Color.LightSkyBlue;
        private int borderWidth = 2;
        private bool isDragging = false;
        private Point _dragStartPoint;

        // 页面缩放比例
        private Size _originalFormSize;
        private Dictionary<Control, Rectangle> _originalControlRects = new Dictionary<Control, Rectangle>();

        #endregion

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        #region 相关方法
        public MainForm()
        {
            InitializeComponent();
            InitializePLCCommunicator();
            KillProcess("visionmasterserverapp");
            KillProcess("visionmaster");
            KillProcess("vmmoduleproxy.exe");

            //// 事件处理
            SetupEventHandlers();
            Instance = this;
            subForms = new List<Form>
            {
                new HKCamera(),
                new DebugForm(),
                new TCPConnect(this)
            };
        }
        private void SetupEventHandlers()
        {
            // 界面切换按钮事件
            buttonLoadFrontendData.Click += buttonLoadFrontendData_Click;

            // 为标题栏添加拖动功能
            panelTitleBar.MouseDown += panelTitleBar_MouseDown;
            panelTitleBar.MouseMove += panelTitleBar_MouseMove;
            panelTitleBar.MouseUp += panelTitleBar_MouseUp;
        }
        #endregion

        #region // 主窗口事件,重绘边框      

        private void MainForm_Load(object sender, EventArgs e)
        {
            LogHelper.Info("主窗口加载完成");
        }
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);
            using (Pen pen = new Pen(borderColor, borderWidth))
            {
                e.Graphics.DrawRectangle(pen, 0, 0,
                                 this.ClientSize.Width - borderWidth,
                                 this.ClientSize.Height - borderWidth);
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //UI关闭
                if (MessageBox.Show(@"确定退出？", @"提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                {
                    //e.Cancel = true;
                    return;
                }

                // ===================== 关闭前释放资源 =====================
                // 1. 关闭数据库连接/串口/套接字
                // 2. 停止后台线程
                // 3. 释放图片、文件流

                // 无额外操作 → 正常关闭，不冲突
                VmSolution.Instance?.Dispose();
                // 释放其他子窗口
                foreach (var form in subForms)
                {
                    if (form != null && !form.IsDisposed)
                    {
                        form.Close();
                        form.Dispose();
                    }
                }
                subForms.Clear();

                // 释放PLC资源
                _plcComm?.Dispose();
                LogHelper.Info("应用程序正常退出");
            }
            catch (Exception ex)
            {
                LogHelper.Error("应用程序退出时发生异常", ex);
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 方案2：强制退出程序                      
            Application.Exit();
            Environment.Exit(0);
        }

        #endregion

        #region  // 标题栏 最大，最小，关闭按钮
        // 最小化
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnMinimize_MouseEnter(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.LightGray;
        }
        private void btnMinimize_MouseLeave(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.Transparent;
        }
        // 最大化
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            // 实现最大化/还原功能（切换窗口状态）
            //this.WindowState = this.WindowState == FormWindowState.Maximized
            //    ? FormWindowState.Normal
            //    : FormWindowState.Maximized;
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                btnMaximize.Text = "❐"; // 还原图标
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                btnMaximize.Text = "□"; // 最大化图标
            }
        }
        private void btnMaximize_MouseEnter(object sender, EventArgs e)
        {
            btnMaximize.BackColor = Color.LightGray;
        }
        private void btnMaximize_MouseLeave(object sender, EventArgs e)
        {
            btnMaximize.BackColor = Color.Transparent;
        }
        // 关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {//
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => this.Close()));
                }
                else
                {
                    //this.Close();
                    Application.Exit();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"关闭失败:");
            }


        }
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Red;
        }
        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Transparent;
        }
        #endregion

        #region // 页面鼠标左键拖动
        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
                _dragStartPoint = e.Location;
            }
        }
        private void panelTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int dx = e.X - _dragStartPoint.X;
                int dy = e.Y - _dragStartPoint.Y;
                // 移动窗体
                this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
                _dragStartPoint = e.Location;
            }
        }
        private void panelTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;//释放鼠标后标注为false;            
        }
        #endregion

        #region  组件加载区

        //private void vmMainViewConfigControl1_Load(object sender, EventArgs e)
        //{            
        //    vmMainViewConfigControl1.BindMultiProcedure(); //绑定多流程
        //}

        private void vmFrontendControl1_Load(object sender, EventArgs e)
        {
            if (vmFrontendControl1 == null) return;
            vmFrontendControl1.LoadFrontendSource();
            mFrontedLoad = true;
        }
        private void vmFrontendControl1_SizeChanged(object sender, EventArgs e)
        {
            if ((null != vmFrontendControl1) && mFrontedLoad)
            {
                vmFrontendControl1.AutoChangeSize();
            }
        }

        #endregion

        #region // PLC 通讯事件处理
        private void InitializePLCCommunicator()
        {
            try
            {
                // 初始化S7通信（IP地址根据实际设备修改）
                _plcComm = new PLCCommunicator(
                    CpuType.S71200,
                    "192.168.0.1",  // 实际IP
                    0,              // 机架号
                    1               // 插槽号
                );

                //// 订阅连接状态和错误事件
                //_plcComm.ConnectionStatusChanged += OnPlcConnectionStatusChanged;
                //_plcComm.HardwareErrorOccurred += OnPlcHardwareError;
                LogHelper.Info("PLC通信器初始化完成");
            }
            catch (Exception ex)
            {
                LogHelper.Error("PLC通信器初始化失败", ex);
                MessageBox.Show($"PLC初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
        //private void OnPlcConnectionStatusChanged(bool isConnected)
        //{
        //    Invoke(new Action(() =>
        //    {
        //        string status = isConnected ? "已连接" : "已断开";
        //        toolStripStatusLabel1.Text = $"PLC状态: {status}";

        //        // 如果相机窗口已打开，更新其PLC引用
        //        if (_hkCameraForm != null && !_hkCameraForm.IsDisposed)
        //        {
        //            _hkCameraForm.PlcCommunicator = _plcComm;
        //        }
        //    }));
        //}
        */
        /*
        //private void OnPlcHardwareError(string errorMessage)
        //{
        //    Invoke(new Action(() =>
        //    {
        //        MessageBox.Show($"PLC硬件错误: {errorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }));
        //}
        */
        #endregion

        #region // 菜单栏操作区

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 报警ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void mesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_mesForm == null || _mesForm.IsDisposed)
            {
                _mesForm = new MESForm();
                _mesForm.FormClosed += (s, args) => _mesForm = null;
                //_mesForm.RequestRefreshStatus += OnRequestRefreshMESStatus;
                
            }
            _mesForm.Show();
            _mesForm.BringToFront();
        }
        private void 调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //打开调试页面
                DebugForm debugForm = new DebugForm();
                debugForm.Show();
            }
            catch (Exception ex)
            {
                LogHelper.Error("打开相机窗口失败", ex);
                MessageBox.Show($"打开相机窗口失败：{ex.Message}"); MessageBox.Show(strMsg);
            }
        }
        private void 通讯ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开TCPConnect页面，传递当前MainForm实例
                TCPConnect tcpConnect = new TCPConnect(this);  // 补充this参数
                tcpConnect.Show();
            }
            catch (Exception ex)
            {
                // 建议添加错误日志
                LogHelper.Error("打开通讯窗口失败", ex);
                MessageBox.Show($"打开通讯窗口失败：{ex.Message}");
            }
        }
        private void 相机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 先关闭已存在的相机窗口,（即使相机不存在，也应该可以打开界面）260106记录
                if (HKCameraInstance != null && !HKCameraInstance.IsDisposed)
                {
                    HKCameraInstance.Close();
                    HKCameraInstance.Dispose();
                }
                // 打开新的相机窗口
                HKCamera HKCameraForm = new HKCamera();
                HKCameraForm.Show();
            }
            catch (Exception ex)
            {
                LogHelper.Error("打开相机窗口失败", ex);
                MessageBox.Show($"打开相机窗口失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 文件另存位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        private void 另存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ProcedureFilePath))     //textBox1.text
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
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PRC files (*.prc)|*.prc | Sol File(*.sol)|*.sol | All files (*.*)|*.*";
                saveFileDialog.FileName = Path.GetFileName(ProcedureFilePath);  // textBox1.Text
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(ProcedureFilePath); // 设置初始目录
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(textBox1.Text, saveFileDialog.FileName, true);
                        VmSolution.SaveAs(saveFileDialog.FileName, textBox2.Text);
                        AddLogMessage($"成功保存到：{saveFileDialog.FileName}");                    
                    }
                    catch (Exception ex) { AddLogMessage($"保存失败：{ex.Message}"); }
                }
            }

        }
        /// <summary>
        /// 文件保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            if (!mSolutionIsLoaded)
            {
                AddLogMessage("No solution file.");
                return;
            }
            try
            {
                this.Enabled = false;
                VmSolution.Save();
                AddLogMessage("SaveSolution success");
            }
            catch (VmException ex)
            {
                AddLogMessage($"SaveSolution failed. Error Code: {Convert.ToString(ex.errorCode, 16)}");
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 选中
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = @"VM Sol File(*.sol)|*.sol";
                DialogResult openFileRes = openFileDialog.ShowDialog();
                if (openFileRes == DialogResult.OK)
                {
                    // 同时更新文件路径和文本框
                    ProcedureFilePath = openFileDialog.FileName; // ..\Demo.sol
                    textBox1.Text = ProcedureFilePath;
                    AddLogMessage($"已打开文件：{ProcedureFilePath}");
                }
            }
            catch (VmException ex)
            {
                AddLogMessage($"打开.sol文件失败：{ex.errorCode}");
            }
            catch (Exception ex)    //增加通用异常捕获，防范非VmException的错误
            {
                AddLogMessage($"意外错误：{ex.Message}");
            }           
        }
        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region // 页面操作按钮区

        /// <summary>
        /// 文件打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            try
            {
                // 选中
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "VM Sol File(*.sol)|*.sol";
                //DialogResult openFileRes = openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 更新路径和文本框
                    mSolutionIsLoaded = false;
                    ProcedureFilePath = openFileDialog.FileName; // ..\Demo.sol
                    LoadSolutionIndicateTimer.Enabled = true;    //timer
                    textBox1.Text = ProcedureFilePath;
                    AddLogMessage($"已打开文件：{ProcedureFilePath}");
                }
                else
                {
                    ProcedureFilePath = string.Empty;
                }
            }
            catch (VmException ex)
            {
                listBox1.Items.Add($"打开.sol文件失败：{ex.errorCode}");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
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

        /// <summary>
        /// 方案加载按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string strMsg = null;

            label1.Enabled = false;
            button2.Enabled = false;
            button9.Enabled = false;
            buttonExecuteOnce.Enabled = false;
            buttonContinuExecute.Enabled = false;
            buttonStopExecute.Enabled = false;

            try
            {
                if (ProcedureFilePath != string.Empty)
                {
                    string strFilePath = textBox1.Text;     // file path
                    string strSoluPwd = textBox2.Text;      // password
                    VmSolution.Load(strFilePath, strSoluPwd);
                    //VmSolution.Load(strFilePath);
                    vmProInfoList = GetCurrentSolProcedureList();
                    UpdateProcessComboBox(vmProInfoList);
                    mSolutionIsLoaded = true;
                    AddLogMessage("LoadSolution success");
                }
            }
            catch (VmException ex)
            {
                AddLogMessage($"LoadSolution failed. Error Code: {Convert.ToString(ex.errorCode, 16)}");
                MessageBox.Show($"加载方案失败: {ex.Message}");
                return;
            }
            // enable buttons
            label1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button9.Enabled = true;
            buttonExecuteOnce.Enabled = true;
            buttonContinuExecute.Enabled = true;
            buttonStopExecute.Enabled = true;

            strMsg = "LoadSolution success";
            listBox1.Items.Add(strMsg);
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        /// <summary>
        /// 保存按钮
        /// </summary> 
        private void button2_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            if (!mSolutionIsLoaded)
            {
                AddLogMessage("No solution file.");
                return;
            }
            try
            {
                VmSolution.Save();
                strMsg = "SaveSolution success";
                AddLogMessage(strMsg);
            }               
            catch (VmException ex)
            {
                strMsg = "SaveSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                AddLogMessage(strMsg);
            }
        }

        /// <summary>
        /// 另存按钮
        /// </summary> 
        private void button9_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ProcedureFilePath))     //textBox1.text
            {
                MessageBox.Show("请先通过【文件打开】选择SOL文件", "路径为空",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("确认要另存当前方案吗？", "另存确认",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            strPassword = textBox2.Text;
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
                        string savePath = saveFileDialog.FileName;
                        VmSolution.SaveAs(savePath, strPassword);

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
        }

        /// <summary>
        /// 检查密码并输入
        /// </summary>      
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (mSolutionIsLoaded == true)
                {
                    if (VmSolution.Instance.HasPassword(textBox1.Text))
                    {
                        //VmSolution.Instance.Unlock(textBox1.Text);
                        strMsg = "The solution has password.";
                    }
                    else
                    {
                        strMsg = "No password.";
                    }
                    textBox2.Text = strMsg;
                }
                else
                {
                    strMsg = "No solution file.";
                    listBox1.Items.Add(strMsg);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
            }
            catch (VmException ex)
            {
                strMsg = "SaveSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }

        }

        #region
        /// <summary>
        /// 加载运行界面方案数据按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonLoadFrontendData_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            try
            {
                if (mSolutionIsLoaded == false)
                {
                    strMsg = "Solution Mot loaded yet!";
                    return;
                }
                if (vmFrontendControl1 == null) return;
                vmFrontendControl1.LoadFrontendSource();
                mFrontedLoad = true;
            }
            catch (VmException ex)
            {
                // 后续集成日志系统后替换为日志记录                                
                strMsg = "Load Frontend Data Fail, Error Code: " + Convert.ToString(ex.errorCode, 16);
                MessageBox.Show(strMsg);
                return;
            }
        }
        private bool LoadCurrentConfiguration()
        {
            // 实现加载当前配置的逻辑
            // 这里需要根据实际项目情况实现
            // 例如：从配置文件加载方案
            // 返回是否成功
            return true;
        }

        /// <summary>
        /// 缩小按钮
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 放大按钮
        /// </summary>    
        private void button5_Click(object sender, EventArgs e)
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

        #endregion

        /// <summary>
        /// 选择流程打开
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
            catch (VmException ex)
            {
                strMsg = "流程导入失败(ImportProcess failed.)" + " Error Code: " + Convert.ToString(ex.errorCode, 16);
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
            //绑定下拉框中选中的流程名称
            m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
            if (m_VmProc != null)
            {
                try
                {
                    m_VmProc.Save();
                    //m_VmProc.SaveAs(comboBox1.Text);//导出
                    listBox1.Items.Add("流程导出成功(ExportProcess success)" + comboBox1.Text + ".prc");
                    listBox1.TopIndex = listBox1.Items.Count - 1;

                }
                catch (VmException ex)
                {
                    strMsg = "流程导出失败(ExportProcess failed.)" + " Error Code: " + Convert.ToString(ex.errorCode, 16);
                    listBox1.Items.Add(strMsg);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
            }
            else
            {
                strMsg = "No " + comboBox1.Text + " name procedure";
                MessageBox.Show(strMsg);
            }
        }

        /// <summary>
        /// 删除方案中某流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
                if (m_VmProc != null)
                {
                    listBox1.Items.Add("流程删除成功(DeleteProcess success)" + comboBox1.Text);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
                m_VmProc.IsEnabled = false; // 删除前禁用
                m_VmProc.Dispose();
            }
            catch (VmException ex)
            {

                strMsg = "DeleteProcess failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                MessageBox.Show("流程删除失败." + Convert.ToString(ex.errorCode, 16));
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
                VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance["example"];
                ProcessInfoList vmProInfoList = VmSolution.Instance.GetAllProcedureList();  //获取流程

                if (vmProInfoList.nNum == 0)
                {
                    MessageBox.Show("未获取到流程列表.");
                    return;
                }
                comboBox1.Items.Clear();
                comboBox1.Items.Add("example1");
                for (int item = 0; item < vmProInfoList.nNum; item++)
                {
                    comboBox1.Items.Add(vmProInfoList.astProcessInfo[item].strProcessName);  //添加流程名称到下拉列表
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
            #region
            //string strMsg = null;正常的单次点击执行(不接收指令时)           
            //try
            //{
            //    m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
            //    if (m_VmProc == null)
            //    {
            //        MessageBoxButtons msgType = MessageBoxButtons.OK;
            //        DialogResult diagMsg = MessageBox.Show(comboBox1.Text + " name procedure does't exist", "Prompt", msgType);
            //        if (diagMsg == DialogResult.OK)
            //        {
            //            return;
            //        }
            //    }
            //    m_VmProc.Run();

            //    strMsg = "Process run success.";
            //    listBox1.Items.Add(strMsg);
            //    listBox1.TopIndex = listBox1.Items.Count - 1;
            //}
            //catch (VmException ex)
            //{
            //    strMsg = "Process run failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
            //    listBox1.Items.Add(strMsg);
            //    listBox1.TopIndex = listBox1.Items.Count - 1;
            //}

            ////接收到一次触发的'A','B','C'才会单次执行结果   
            //try
            //{
            //    if (!string.IsNullOrEmpty(comboBox1.Text))
            //    {
            //        ExecuteProcedure(comboBox1.Text);
            //    }
            //    else
            //    {
            //        listBox1.Items.Add("请选择要执行的流程");
            //        listBox1.TopIndex = listBox1.Items.Count - 1;
            //    }
            //}
            //catch (VmException ex)
            //{
            //    MessageBox.Show("单流程执行失败." + Convert.ToString(ex.errorCode, 16));
            //}
            #endregion
            ExecuteManualProcedure();
        }

        /// <summary>
        /// 手动执行
        /// </summary>
        private void ExecuteManualProcedure()
        {
            string selectedProcedure = comboBox1.Text;
            try
            {
                if (string.IsNullOrEmpty(selectedProcedure))
                {
                    string msg = "请先从下拉列表选择要执行的流程";
                    MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // 手动执行时添加标识，区别于指令执行
                string result = ExecuteProcedure(selectedProcedure, true);
                AddLogMessage($"手动执行: {result}");
            }
            catch (VmException ex)
            {
                string errorMsg = $"手动执行失败: 错误码 {Convert.ToString(ex.errorCode, 16)}";
                AddLogMessage(errorMsg);
                MessageBox.Show(errorMsg, "手动执行错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 连续执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void buttonContinuExecute_Click(object sender, EventArgs e)
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
                AddLogMessage(strMsg);
            }
            catch (VmException ex)
            {
                strMsg = "Process Continous Run failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                AddLogMessage(strMsg);
            }
        }

        /// <summary>
        /// stop停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void buttonStopExecute_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
            {
                m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
                if (m_VmProc != null)
                {
                    m_VmProc.ContinuousRunEnable = false;
                }
                strMsg = "StopExecute success";
                AddLogMessage(strMsg);
            }
            catch (VmException ex)
            {
                strMsg = "StopExecute failed. Error Code:" + Convert.ToString(ex.errorCode, 16);
                AddLogMessage(strMsg);
            }

        }

        /// <summary>
        /// 设置运行时间间隔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetTimeInterval_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
            {
                string strTimeInteval = textBoxTimeInterval.Text;  //定位
                if (string.IsNullOrEmpty(strTimeInteval))
                {
                    MessageBoxButtons msgType = MessageBoxButtons.OK;
                    DialogResult diaMsg = MessageBox.Show("Please enter the time interval!", "Prompt", msgType);
                    if (diaMsg == DialogResult.OK)
                    {
                        return;
                    }
                }
                uint nTimeInterval = 0;
                nTimeInterval = uint.Parse(strTimeInteval);
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

                m_VmProc.SetContinousRunInterval(nTimeInterval);  // 设置运行间隔


            }
            catch (VmException ex)
            {
                strMsg = "SetContinousRunInterval failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }

        ///<summary>
        /// 执行指定流程并返回结果
        ///</summary>
        public string ExecuteProcedure(string procedureName, bool isManual = false)
        {
            try
            {
                // check procedure is exist
                m_VmProc = (VmProcedure)VmSolution.Instance[procedureName];
                if (m_VmProc == null)
                {
                    string errorMsg= $"{procedureName} is not exist!";
                    LogHelper.Error(errorMsg);
                    return errorMsg;
                }
                //执行流程                
                VmProcedure vmProcess = (VmProcedure)m_VmProc;
                vmProcess.Run();
                
                
                ////获取识别结果，string----GetOutputString
                string strResult = "";
                try
                {                   
                    strResult = vmProcess.ModuResult.GetOutputString("out").astStringVal[0].strValue;
                    //string strResult1 = vmProcess.ModuResult.GetOutputString("out1").astStringVal[0].strValue;
                    //string strResult2 = vmProcess.ModuResult.GetOutputString("out2").astStringVal[0].strValue;                
                    AddLogMessage($"流程 {procedureName} 执行成功, 结果: {strResult}");
                    return strResult;
                }
                catch {                }

                string strResult0 = "";
                try
                {
                    var output0 = vmProcess.ModuResult.GetOutputString("out0");
                    if (output0.astStringVal != null && output0.astStringVal.Length > 0)
                        strResult0 = output0.astStringVal[0].strValue;
                }
                catch { }

                // //获取识别结果，int----GetOutputInt(string strParam)
                /*              
                //public struct IntDataArray
                //{
                //    public int nValueNum;
                //    public int[] pIntVal;
                //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
                //    public uint[] nReserved;
                //}
                 */
                //int intResult = vmProcess.ModuResult.GetOutputInt("out").pIntVal[0];
                //int intResult1 = vmProcess.ModuResult.GetOutputInt("out1").astIntVal[0].IntValue;
                //int intResult2 = vmProcess.ModuResult.GetOutputInt("out2").astIntVal[0].IntValue;
                //listBox1.Items.Add($"the result of int: {intResult}");

                // //获取识别结果，float----GetOutputFloat(string strParam)
                /*
                // public struct FloatDataArray
                //{
                //    public int nValueNum;
                //    public float[] pFloatVal;
                //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
                //    public uint[] nReserved;
                //}
                 */
                //float floatResult = vmProcess.ModuResult.GetOutputFloat("out").pFloatVal[0];
                //float floatResult1 = vmProcess.ModuResult.GetOutputFloat("out1").astFloatVal[0];
                //float floatResult2 = vmProcess.ModuResult.GetOutputFloat("out2").astFloatVal[0];
                //listBox1.Items.Add($"the result of float: {floatResult}");                

                //更新渲染控件,显示图像
                //vmRenderControl1.ModuleSource = vmProcess;

                // 记录到日志                
                string logMessage = $"流程 {procedureName} 执行结果:\n  out: {strResult}\n  out0: {strResult0}";
                listBox1.Items.Add(logMessage);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                LogHelper.Info(logMessage);

                // 保存执行结果到JSON
                string jsonPath = SaveExecutionResultToJson(procedureName, strResult);

                // 更新UI显示执行结果1
                var displayResult = new
                {
                    Procedure = procedureName,
                    Time = DateTime.Now.ToString("HH:mm:ss.fff"),
                    Out = strResult,
                    Out0 = strResult0
                };
                string resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(displayResult,
                    Newtonsoft.Json.Formatting.Indented);
                //UpdateExecutionResultDisplay(resultJson);

                // 保存相机图像
                string imageFileName = null;
                if(HKCameraInstance != null && HKCameraInstance.IsConnect)
                {

                }
                return $"流程 {procedureName} 执行成功(excute success): resultName1={strResult}";
            }
            catch (VmException ex)
            {
                string errorMsg = $"流程 {procedureName} 执行失败: {Convert.ToString(ex.errorCode, 16)}";
                // 保存错误信息到JSON
                SaveErrorResultToJson(procedureName, ex);
                if (isManual)
                {
                    AddLogMessage(errorMsg);   //执令时添加
                }
                LogHelper.Error(errorMsg);
                return errorMsg;
            }
            catch (Exception ex)
            {
                string errorMsg = $"执行流程 {procedureName} 时发生异常: {ex.Message}";
                SaveErrorResultToJson(procedureName, ex);
                LogHelper.Error(errorMsg, ex);
                return errorMsg;
            }
        }

        #endregion

        #region

        /// <summary>
        /// 在MainForm类中添加执行结果保存方法
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="resultData"></param>
        /// <param name="imageFileName"></param>
        private string SaveExecutionResultToJson(string procedureName, string resultData, string imageFileName = null)
        {
            try
            {
                // 1.创建保存目录
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string jsonDir = Path.Combine(baseDir, "ExecutionResults");
                if (!Directory.Exists(jsonDir))
                {
                    Directory.CreateDirectory(jsonDir);
                }

                // 2.文件名
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string filename = $"Result_{procedureName}_{timestamp}.json";
                string filePath = Path.Combine(jsonDir, filename);

                // 3.构建JSON数据结构
                var executionResult = new
                {
                    ProcedureName = procedureName,
                    ExecutionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    Results = ParseDetectionResult(resultData), // 解析检测结果
                    ImageFileName = imageFileName,
                    ImageFilePath = imageFileName != null ? Path.Combine("CapturedImages", imageFileName) : null,
                    Status = "Success"
                };

                // 4.序列化为JSON
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(executionResult, Newtonsoft.Json.Formatting.Indented);

                // 5.保存文件
                File.WriteAllText(filePath, json, Encoding.UTF8);

                LogHelper.Info($"执行结果已保存：{filename}");
                return filePath;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"保存执行结果JSON失败：{ex.Message}");
            }
            return null;
        }

        // 检测的string结果解析
        private object ParseDetectionResult(string resultData)
        {
            try
            {
                // 尝试解析2d检测结果字符串：{x,y, r}
                if (resultData.StartsWith("{") && resultData.EndsWith("}"))
                {
                    string content = resultData.Trim('{', '}');
                    string[] parts = content.Split(',');

                    if (parts.Length >= 3)
                    {
                        return new
                        {
                            X = double.Parse(parts[0].Trim()),
                            Y = double.Parse(parts[1].Trim()),
                            R = double.Parse(parts[2].Trim()),
                            RawResult = resultData
                        };
                    }
                }
                // 其他则返回原始字符串
                return new { RawResult = resultData };
            }
            catch (Exception ex)
            {
                LogHelper.Warn($"解析检测结果失败: {ex.Message}");
                return new { RawResult = resultData, ParseError = ex.Message };
            }
        }

        // 保存错误结果到JSON
        private void SaveErrorResultToJson(string procedureName, Exception ex)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string jsonDir = Path.Combine(baseDir, "ExecutionResults");
                if (!Directory.Exists(jsonDir))
                {
                    Directory.CreateDirectory(jsonDir);
                }

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName = $"Error_{procedureName}_{timestamp}.json";
                string filePath = Path.Combine(jsonDir, fileName);

                var errorResult = new
                {
                    ProcedureName = procedureName,
                    ExecutionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    Error = ex.Message,
                    StackTrace = ex.StackTrace,
                    ExceptionType = ex.GetType().Name,
                    Status = "Failed"
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(errorResult,
                    Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);
                LogHelper.Error($"错误结果已保存: {fileName}");
            }
            catch (Exception saveEx)
            {
                LogHelper.Error($"保存错误结果JSON失败: {saveEx.Message}");
            }
        }

        /// <summary>
        /// KillProcess
        /// </summary>       
        private void KillProcess(string strKillName)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            //foreach(var p in Process.GetProcesses())
            {
                if (p.ProcessName.Contains(strKillName))
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        LogHelper.Error($"Kill process {strKillName} failed", ex);
                    }
                }
            }
        }

        private void AddLogMessage(string msg)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            listBox1.Items.Add($"[{time}] {msg}");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            LogHelper.Info(msg);
        }

        /// <summary>
        /// CH:获取当前方案的所有流程  || Obtain all processes in the solution
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
        /// 更新combobox  || update combobox
        /// </summary> 
        /// <param name=lst"></param>
        private void UpdateProcessComboBox(List<VmProcedure> processInfoList)
        {
            comboBox1.Items.Clear();
            foreach (var vmProcedure in processInfoList)
            {
                comboBox1.Items.Add(vmProcedure.Name);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        // 辅助方法：从检测结果中提取产品代码
        private string ExtractProductCode(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return "UNKNOWN";

            // 简单提取：取前10个字符作为产品代码
            // 实际应用中可以根据具体格式调整
            return result.Length > 10 ? result.Substring(0, 10).Trim() : result.Trim();
        }

        // 辅助方法：从检测结果中提取检测结果（OK/NG）
        private string ExtractDetectionResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return "NG";

            // 简单判断：如果包含OK则为OK，否则为NG
            return result.ToUpper().Contains("OK") ? "OK" : "NG";
        }





        #endregion
    }
}

