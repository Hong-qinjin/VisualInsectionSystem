using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualInsectionSystem.SubForms;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Winform.Release;

namespace VisualInsectionSystem
{
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; private set; }   // 设置form1为主窗口
        public static List<Form> subForms = new List<Form>();   // 子窗口集合
        public HKCamera HKCameraInstance { get; private set; }

        bool mSolutionIsLoaded = false;                //是否加载
        private Timer LoadSolutionIndicateTimer = new Timer();
        bool mFrontedLoad = false;                   // 0330
        private string ProcedureFilePath = string.Empty;       //方案路径
        string strPassword = null;                     //方案密码

        VmProcedure m_VmProc = null;                        //流程                    
        private List<VmProcedure> vmProInfoList;      //流程列表
        string strMsg = null;                          //提示信息
                                                       //
        private PLCCommunicator _plcComm;

        #region  Mainform展示效果  

        // 边框颜色和宽度        
        private Color borderColor = Color.LightSkyBlue;
        private int borderWidth = 2;
        // 用于拖动窗口的变量
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
            InitUI();
        }

        private void InitUI()
        {
            Instance = this;        //设置Mainform为主窗口       
            this.DoubleBuffered = true;     //窗体调整大小时闪烁
            this.BackColor = SystemColors.Window;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
            this.Load += MainForm_Load;
            this.SizeChanged += MainForm_SizeChanged;
            subForms = new List<Form>
            {
                new HKCamera(),
                new DebugForm(),
                new TCPConnect(this)
            };

            //设置logo
            PictureBox pictureBoxLogo = new PictureBox
            {
                Image = VisualInsectionSystem.Properties.Resources.AUTOBOX_LOGO,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(369, 44),
                Location = new Point(1, 1),
                Enabled = false,
                BackColor = SystemColors.Window
            };

            // 标题栏容器
            panelTitleBar = new Panel()
            {
                BackColor = SystemColors.Window,
                Width = 1260,
                Height = 45,
                Dock = DockStyle.Top
            };
            ////添加按钮到标题栏
            AddTitleBarButtons(panelTitleBar);
            panelTitleBar.Controls.Add(pictureBoxLogo);
            panelTitleBar.BringToFront();

            // 内容容器 panel1,顶部留出的标题栏空间           
            panel1.Padding = new Padding(0, 0, 0, 0);
            // 调整panel1的位置和大小
            panel1.Location = new Point(0, panelTitleBar.Height);
            panel1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - panelTitleBar.Height);
            // 事件处理
            SetupEventHandlers();
        }

        private void AddTitleBarButtons(Panel titleBar)
        {
            // 最小化按钮
            btnMinimize = new Button
            {
                Text = "—",
                Size = new Size(40, 30),
                Location = new Point(titleBar.Width - 120, 10),//1260-135=1125
                FlatStyle = FlatStyle.Flat,
                BackColor = SystemColors.ButtonFace

            };
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.Click += btnMinimize_Click;
            // 最大化按钮
            btnMaximize = new Button
            {
                Text = "□",
                Size = new Size(40, 30),
                Location = new Point(titleBar.Width - 80, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent
            };
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.Click += btnMaximize_Click;
            // 关闭按钮
            btnClose = new Button
            {
                Text = "✕",
                Size = new Size(40, 30),
                Location = new Point(titleBar.Width - 40, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.Red
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += btnClose_Click;
            //// 添加按钮
            titleBar.Controls.Add(btnMinimize);
            titleBar.Controls.Add(btnMaximize);
            titleBar.Controls.Add(btnClose);
        }

        private void SetupEventHandlers()
        {
            // 界面切换按钮事件
            buttonLoadFrontendData.Click += buttonLoadFrontendData_Click;

            // 为标题栏添加拖动功能
            panelTitleBar.MouseDown += panelTitleBar_MouseDown;
            panelTitleBar.MouseMove += panelTitleBar_MouseMove;
            panelTitleBar.MouseUp += panelTitleBar_MouseUp;

            // 鼠标悬停效果          
            btnMinimize.MouseEnter += btnMinimize_MouseEnter;
            btnMinimize.MouseLeave += btnMinimize_MouseLeave;

            btnMaximize.MouseEnter += btnMaximize_MouseEnter;
            btnMaximize.MouseLeave += btnMaximize_MouseLeave;

            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;

        }
        #endregion

        #region  // 最大，最小，关闭按钮
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
                // 方案1：关闭当前窗体
                this.Close();
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
            //if (e.Button == MouseButtons.Left && e.Y <= 45.0)     
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

        #region // 主窗口大小改变,组件定位,重绘边框      

        private void MainForm_Load(object sender, EventArgs e)
        {
            _originalFormSize = this.Size;  //1264,668            
            RecordAllControlsOriginalRect(this);

            try
            {
                ListBox.CheckForIllegalCrossThreadCalls = false;
                LogHelper.Info("主窗口加载完成");
            }
            catch (Exception ex)
            {
                string strMsg = " Error Code: " + Convert.ToString(ex.Message);
                MessageBox.Show(strMsg);
                LogHelper.Error("主窗口加载失败", ex);
            }
        }
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);
            using (Pen pen = new Pen(borderColor, borderWidth))
            {
                e.Graphics.DrawRectangle(pen,
                    //new Rectangle(borderWidth / 2,
                    //             borderWidth / 2,
                    //             this.ClientSize.Width - borderWidth,
                    //             this.ClientSize.Height - borderWidth));
                    new Rectangle(0, 0,
                                 this.ClientSize.Width - borderWidth,
                                 this.ClientSize.Height - borderWidth));
            }
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //// 避免窗体最小化时触发错误
            //if (this.WindowState == FormWindowState.Minimized) return;
            //// 计算缩放比例（宽比例、高比例）
            //float scaleX = (float)this.Width / _originalFormSize.Width;
            //float scaleY = (float)this.Height / _originalFormSize.Height;
            //// 递归缩放所有控件
            //ScaleAllControls(this, scaleX, scaleY);

            //base.OnResize(e);
            //// 调整panel1的大小，考虑边框宽度
            //int borderAdjust = borderWidth * 2;

            //// 调整panel1的大小和位置
            //if (panel1 != null)
            //{
            //    panel1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - panelTitleBar.Height);
            //    panel1.Location = new Point(0, panelTitleBar.Height);
            //}
            //// 调整标题栏按钮位置
            //if (panelTitleBar != null && btnMinimize != null)
            //{
            //    btnMinimize.Location = new Point(panelTitleBar.Width - 120, 8);
            //    btnMaximize.Location = new Point(panelTitleBar.Width - 80, 8);
            //    btnClose.Location = new Point(panelTitleBar.Width - 40, 8);
            //}
            ////this.Invalidate(); // 重绘边框
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
                _plcComm = null;                
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

        /// <summary>
        /// 组件加载区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);
                if (null != vmEx)
                {
                    string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);
                    MessageBox.Show(strMsg);
                }
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
                HKCameraInstance = HKCameraForm;
            }
            catch (Exception ex)
            {
                // 记录异常并显示                
                MessageBox.Show($"打开相机窗口失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);
                if (null != vmEx)
                {
                    string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);
                    MessageBox.Show(strMsg);
                }
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
        /// 文件保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            if (mSolutionIsLoaded == true)
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
                    listBox1.Items.Add($"已打开文件：{ProcedureFilePath}");  //添加打开记录
                    listBox1.TopIndex = listBox1.Items.Count - 1;
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
                    listBox1.Items.Add($"已打开文件：{ProcedureFilePath}");  //添加打开记录
                    listBox1.TopIndex = listBox1.Items.Count - 1;
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
                    mSolutionIsLoaded = true;              // 1
                }
            }
            catch (VmException ex)
            {
                strMsg = "LoadSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                MessageBox.Show(strMsg);
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
            try
            {
                if (mSolutionIsLoaded)
                {
                    VmSolution.Save();
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
            catch (VmException ex)
            {
                strMsg = "SaveSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBox1.Items.Add(strMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
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
                listBox1.Items.Add($"手动执行: {result}");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                string errorMsg = $"手动执行失败: 错误码 {Convert.ToString(ex.errorCode, 16)}";
                listBox1.Items.Add(errorMsg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                MessageBox.Show(errorMsg, "执行错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    return $"{procedureName} is not exist!";
                }
                //执行流程
                VmProcedure vmProcess = (VmProcedure)m_VmProc;
                vmProcess.Run();


                ////获取识别结果，string----GetOutputString
                /*
                // public struct StringDataArray
                //{
                //    public int nValueNum;
                //    public StringData[] astStringVal;
                //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
                //    public uint[] nReserved;
                //}
                 */
                string strResult = vmProcess.ModuResult.GetOutputString("out").astStringVal[0].strValue;
                //string strResult1 = vmProcess.ModuResult.GetOutputString("out1").astStringVal[0].strValue;
                //string strResult2 = vmProcess.ModuResult.GetOutputString("out2").astStringVal[0].strValue;
                listBox1.Items.Add($"the result of string:{strResult}");

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

                return $"流程 {procedureName} 执行成功(excute success): resultName1={strResult}";
                /*$"+resultName2={intResult}+resultName3={floatResult}";*/

            }
            catch (VmException ex)
            {
                string errorMsg = $"流程 {procedureName} 执行失败: {Convert.ToString(ex.errorCode, 16)}";
                if (isManual)
                {
                    listBox1.Items.Add(errorMsg);   //执令时添加
                }
                return errorMsg;
            }
        }

        #endregion

        #region

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

        /// <summary>
        /// 递归遍历控件，记录原始矩形
        /// </summary>
        private void RecordAllControlsOriginalRect(Control parentControl)
        {
            foreach (Control ctrl in parentControl.Controls)
            {
                // 记录当前控件的原始位置和大小
                _originalControlRects[ctrl] = new Rectangle(ctrl.Location, ctrl.Size);
                // 递归处理子容器（如GroupBox、Panel、SplitContainer）
                if (ctrl.HasChildren)
                {
                    RecordAllControlsOriginalRect(ctrl);
                }
            }
        }

        /// <summary>
        /// 递归缩放控件
        /// </summary>
        private void ScaleAllControls(Control parentControl, float scaleX, float scaleY)
        {
            foreach (Control ctrl in parentControl.Controls)
            {
                if (_originalControlRects.ContainsKey(ctrl))
                {
                    // 获取原始矩形
                    Rectangle originalRect = _originalControlRects[ctrl];
                    // 计算新的大小和位置
                    int newPx = (int)Math.Round(originalRect.X * scaleX);
                    int newPy = (int)Math.Round(originalRect.Y * scaleY);
                    int newWidth = (int)Math.Round(originalRect.Width * scaleX);
                    int newHeight = (int)Math.Round(originalRect.Height * scaleY);

                    // 设置新的位置和大小
                    ctrl.Location = new Point(newPx, newPy);
                    ctrl.Size = new Size(newWidth, newHeight);

                    // 特殊处理：字体也按比例缩放（可选）
                    if (ctrl.Font != null)
                    {
                        float newFontSize = (float)Math.Round(ctrl.Font.Size * Math.Min(scaleX, scaleY));
                        ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize, ctrl.Font.Style);
                    }
                }
                // 递归处理子容器
                if (ctrl.HasChildren)
                {
                    ScaleAllControls(ctrl, scaleX, scaleY);
                }
            }
        }



        #endregion


    }
}

