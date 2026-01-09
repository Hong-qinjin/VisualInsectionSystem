using FrontendUI.Design;
using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualInsectionSystem.SubForms;
using VM.Core;
using VM.PlatformSDKCS;

namespace VisualInsectionSystem
{
    public partial class MainForm : Form
    {

        public static MainForm Instance { get; private set; }   // 设置form1为主窗口
        public static List<Form> subForms = new List<Form>();   // 子窗口集合
        public HKCamera HKCameraInstance { get; private set; }

        bool mSolutionIsLoad = false;  //方案
        VmProcedure m_VmProc = null;   //流程       
        private string ProcedureFilePath = null;    // Solution Path
        private List<VmProcedure> processList;      //流程列表
        string strMsg = null;  //提示信息
        string strPassword = null; //方案密码

        private PLCCommunicator _plcComm;


        #region  form展示效果

        // 用于拖动窗口的变量
        private bool isDragging = false;
        private Point dragStartPoint;
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键     
        // 边框颜色和宽度        
        private Color borderColor = Color.LightSkyBlue;
        private int borderWidth = 3;

        #region 相关方法
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        public MainForm()
        {
            InitializeComponent();
            InitializePLCCommunicator();
            InitUI();
        }

        private void InitUI()
        {
            //form1
            Instance = this;    //设置form1为主窗口       
            this.DoubleBuffered = true;     //窗体调整大小时闪烁
            this.BackColor = SystemColors.Window;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;

            //初始化标题栏
            InitializeTitleBar();
           
            subForms = new List<Form>
            {
                new HKCamera(),
                new DebugForm(),
                new TCPConnect(this)
            };
            // 考虑边框和标题栏           

            // 事件处理
            SetupEventHandlers();

        }
        private void InitializeTitleBar()
        {
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
            
            //标题栏容器
            panelTitleBar = new Panel()
            {
                BackColor = SystemColors.Window,
                Width =1260,
                Height = 45,
                Dock = DockStyle.Top
            };
            //添加按钮到标题栏
            AddTitleBarButtons(panelTitleBar);
            panelTitleBar.Controls.Add(pictureBoxLogo);

            // 添加标题栏到窗体
            //this.Controls.Add(panelTitleBar);
            panelTitleBar.BringToFront();

            //内容容器 panel1,顶部留出的标题栏空间           
            panel1.Padding = new Padding(0, 0, 0, 0);
            // 调整panel1的位置和大小
            panel1.Location = new Point(0, panelTitleBar.Height);
            panel1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - panelTitleBar.Height);

        }
        // 绘制边框
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(borderColor, borderWidth))
            {                
                e.Graphics.DrawRectangle(pen,
                    new Rectangle(borderWidth / 2,
                                 borderWidth / 2,
                                 this.ClientSize.Width - borderWidth,
                                 this.ClientSize.Height - borderWidth));
            }
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

            // 添加按钮到标题栏
            titleBar.Controls.Add(btnMinimize);
            titleBar.Controls.Add(btnMaximize);
            titleBar.Controls.Add(btnClose);
        }   
        private void SetupEventHandlers()
        {
            // 标题栏拖动事件
            this.MouseDown += panelTitleBar_MouseDown;
            this.MouseMove += panelTitleBar_MouseMove;
            this.MouseUp += panelTitleBar_MouseUp;

            // 按钮鼠标悬停效果          
            btnMinimize.MouseEnter += btnMinimize_MouseEnter;
            btnMinimize.MouseLeave += btnMinimize_MouseLeave;
            
            btnMaximize.MouseEnter += btnMaximize_MouseEnter;
            btnMaximize.MouseLeave += btnMaximize_MouseLeave;

            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;           
        }
        #endregion

        #region 最大，最小，关闭，
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

        //最大化
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
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            // 实现关闭功能
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

        #region 拖动

        //鼠标移动
        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {            
            //if (e.Button == MouseButtons.Left && e.Y <= 45.0)     
            if(e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true; //点击左键按下时标注为true;
            }
        }
        private void panelTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y); //设置移动后的位置
                Location = mouseSet;
            }
        }
        private void panelTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
        // 窗口大小改变,组件定位,重绘边框
        private void MainForm_Resize(object sender, EventArgs e)
        {
            base.OnResize(e);
            // 调整panel1的大小，考虑边框宽度
            int borderAdjust = borderWidth * 2;

            // 调整panel1的大小和位置
            if (panel1 != null)
            {
                panel1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - panelTitleBar.Height);
                panel1.Location = new Point(0, panelTitleBar.Height);
            }
            // 调整标题栏按钮位置
            if (panelTitleBar != null && btnMinimize != null)
            {
                btnMinimize.Location = new Point(panelTitleBar.Width - 120, 8);
                btnMaximize.Location = new Point(panelTitleBar.Width - 80, 8);
                btnClose.Location = new Point(panelTitleBar.Width - 40, 8);
            }
            this.Invalidate(); // 重绘边框
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
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
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //UI关闭
            if (MessageBox.Show(@"确定退出？", @"提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            {
                //e.Cancel = true;
                return;
            }

            //释放资源            
            if (vmMainViewConfigControl1 != null)
            {
                vmMainViewConfigControl1.Dispose();
                vmMainViewConfigControl1 = null;
            }
            try
            {
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
                VmSolution.Instance?.Dispose();
                // 释放PLC资源
                _plcComm?.Dispose();
                _plcComm = null;
                LogHelper.Info("应用程序正常退出");
            }
            catch (Exception ex)
            {
                LogHelper.Error("应用程序退出时发生异常", ex);
            }
            Application.Exit();  //确保应用程序完全退出
        }
        #endregion
        #endregion


        #region  PLC 通讯事件处理
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
        /// 文件另存为xxx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // disabled button
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;

            string message;
            try
            {
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
                message = $"打开SOL文件失败：{ex.errorCode}";
                listBox1.Items.Add(message);
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

        /// <summary>
        /// 加载方案按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //string strMsg = null;
            try
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
            }

            // enable buttons
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;

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

        /// <summary>
        /// 方案保存按钮
        /// </summary> 
        private void button2_Click(object sender, EventArgs e)
        {
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
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (mSolutionIsLoad == true)
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

        /// <summary>
        /// 锁定按钮
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            vmMainViewConfigControl1.LockWorkArea();
            //锁定参数配置页，不允许编辑流程/Group/模块的参数配置页
            vmMainViewConfigControl1.SetParamTabEditable(false);

        }

        /// <summary>
        /// 解锁按钮
        /// </summary>    
        private void button5_Click(object sender, EventArgs e)
        {
            vmMainViewConfigControl1.UnlockWorkArea();
            //解锁参数配置页，允许编辑流程/Group/模块的参数配置页
            vmMainViewConfigControl1.SetParamTabEditable(true);


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
        {//绑定下拉框中选中的流程名称
            VmProcedure m_VmProc = (VmProcedure)VmSolution.Instance[comboBox1.Text];
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
        /// 删除一个流程
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
                ProcessInfoList vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();  //获取流程
                VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance["example1"];
                if (vmProcessInfoList.nNum == 0)
                {
                    MessageBox.Show("未获取到流程列表.");
                    return;

                }
                comboBox1.Items.Clear();
                comboBox1.Items.Add("example1");
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

        private void vmMainViewConfigControl1_Load(object sender, EventArgs e)
        {
            vmMainViewConfigControl1.BindMultiProcedure(); //绑定多流程
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

                //等待流程执行完全
                System.Threading.Thread.Sleep(1000);


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
                vmRenderControl1.ModuleSource = vmProcess;

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


    }

}
