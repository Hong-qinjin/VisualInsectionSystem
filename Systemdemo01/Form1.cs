using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VM.Core;
using VM.PlatformSDKCS;
using VMControls.BaseInterface;
using VMControls.Winform.Release;

/// <summary>
/// 系统Demos
/// </summary>  

namespace Systemdemo01
{
    /// <summary>
    /// 方案加载与显示界面
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// The control for display picture
        /// </summary>
        //private VmRenderControl vmRenderControl;
        public Form1()
        {
            //VM 资源管理1.5__2:00
            //KillProcess("VisionMasterServerApp");
            //KillProcess("VisionMaster");
            //KillProcess("VmModuleProxy.exe");

            InitializeComponent();

            //vmRenderControl = new VmRenderControl();   
            //vmRenderControl1 = vmRenderControl;  //全局渲染控件
        }
        /// <summary>
        /// 回调事件函数
        /// </summary>
        /// <param name="workStatusInfo"></param>函数
        private void VmSolution_OnWorkStatusEvent(ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            try
            {
                //为0表示执行完毕，为1表示正在执行   10000表示流程1,流程结束后返回回调函数
                if (workStatusInfo.nWorkStatus == 0 && workStatusInfo.nProcessID == 10000)
                {
                    //通过流程获取结果
                    VmProcedure vmProcess1 = (VmProcedure)VmSolution.Instance["OCRDemo"];   //流程对象                    
                    string ocrResult = vmProcess1.ModuResult.GetOutputString("out").astStringVal[0].strValue;  //   获取识别结果  
                    this.BeginInvoke(new Action(() =>  //在回调中对控件操作，需要使用委托
                    {
                        vmRenderControl1.ModuleSource = vmProcess1; //渲染结果

                        listBox1.Items.Add("字符识别结果:" + ocrResult);
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }));

                    //通过模块获取：IMVS获取，实例化模块对象，获取结果
                    IMVSOcrDlModuCCs.IMVSOcrDlModuCTool ocrTool = (IMVSOcrDlModuCCs.IMVSOcrDlModuCTool)VmSolution.Instance["OCRDemo.DL字符识别C1"];
                    string ocrScore1 = ocrTool.ModuResult.CharNum.ToString(); //获取的字符数量
                }
            }
            catch(VmException ex)
            {
                MessageBox.Show(" list error "+ Convert.ToString(ex.errorCode, 16));

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
             // 注册回调函数，推荐回调函数获取结果
            VmSolution.OnWorkStatusEvent += VmSolution_OnWorkStatusEvent;  //所有流程运行状态回调

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void vmProcedureConfigControl1_Load_1(object sender, EventArgs e)
        {

        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }
        private void groupBox2_Enter_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 选择路径按钮功能，打开一个文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string message = null;
            try
            {
                // 调试输出按钮点击
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "E:\\VisualSoftware\\ABVisualSystem\\";
                //openFileDialog.Title = "选择VM Sol文件";
                //openFileDialog.Filter = "All files (*.*)|*.*";  //设置文件类型
                openFileDialog.Filter = "VM Sol File|*.sol";
                DialogResult openFileRes = openFileDialog.ShowDialog(); //显示打开文件对话框

                System.Diagnostics.Debug.WriteLine($"对话框结果: {openFileRes}");     // 调试输出对话框结果

                if (openFileRes == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog.FileName;
                    System.Diagnostics.Debug.WriteLine($"选中的文件路径: {textBox1.Text}");    // 调试输出获取的路径
                }

                listBox1.Items.Add("选择路径成功");
                listBox1.TopIndex = listBox1.Items.Count - 1;   //选择路径成功，显示在listbox中1.2_1:50
            }
            catch (VmException ex)
            {
                message = "Load Sol Failed." + ex.errorCode;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }

        }
        /// <summary>
        /// 方案加载按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                VmSolution.Load(textBox1.Text);   //// 引用VM.Solution.load方法选中路径的sol文件,1.2_1:54
                listBox1.Items.Add("方案加载成功" + textBox1.Text);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                comboBox1.Enabled = true;
            }
            catch (VmException ex)
            {
                string message = "VM_SD异常，Sol Load Failed." + ex.errorCode;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                //MessageBox.Show("VM SDK ERROR." + Convert.ToString(ex.errorCode, 16));
            }
            catch (System.IO.FileNotFoundException ex)
            {
                string message = "File Not Found." + ex.Message;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                //MessageBox.Show("File Not Found." + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 执行一次按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //1.4模块的参数配置
                //IMVSOcrModuCs.IMVSOcrModuTool ocrTool = (IMVSOcrModuCs.IMVSOcrModuTool)VmSolution.Instance[""];
                IMVSOcrDlModuCCs.IMVSOcrDlModuCTool ocrTool = (IMVSOcrDlModuCCs.IMVSOcrDlModuCTool)VmSolution.Instance["OCRDemo.DL字符识别C1"];  //获取方案中的模块对象1.4_1:20  
                vmParamsConfigWithRenderControl1.ModuleSource = ocrTool;  //带渲染的参数控件与模块绑定1.4_1:24

                vmParamsConfigControl1.ModuleSource = ocrTool;  //参数控件与模块绑定1.4_2:39
            }
            catch (VmException ex)
            {
                string message = "参数配置页面失败." + ex.errorCode;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }

            try
            {
                //VmSolution.Instance.SyncRun();  //单次运行
                VmSolution.Instance.SyncRun();   //同步执行一次方案中所有流程1.2_2:07                
            }
            catch (VmException ex)
            {
                string message = "运行异常，SyncRun Failed." + ex.errorCode;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                //MessageBox.Show("VM SDK ERROR." + Convert.ToString(ex.errorCode, 16));
            }
            listBox1.Items.Add("单次执行成功");
            listBox1.TopIndex = listBox1.Items.Count - 1;


            #region 

            // 1.3获取结果,分为渲染结果以及数据结果
            VMControls.Winform.Release.VmRenderControl vmRenderControl = new VMControls.Winform.Release.VmRenderControl();          
            VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance["OCRDemo"];   //流程对象            
            vmRenderControl1.ModuleSource = vmProcedure;  //设置全局渲染控件的显示源与流程绑定

            vmRenderControl1.Show();  //显示渲染控件                                       
            string ocrResult = vmProcedure.ModuResult.GetOutputString("out").astStringVal[0].strValue;  //   获取识别结果
            string ocrConfidence = vmProcedure.ModuResult.GetOutputString("out0").astStringVal[0].strValue;  //  获取置信度
            string ocrNum = vmProcedure.ModuResult.GetOutputInt("out1").pIntVal[0].ToString();  //  数量

            listBox1.Items.Add("字符识别结果:" + ocrResult);
            listBox1.TopIndex = listBox1.Items.Count - 1;
            listBox1.Items.Add("字符置信度:" + ocrConfidence);
            listBox1.TopIndex = listBox1.Items.Count - 1;
            listBox1.Items.Add("字符数量:" + ocrNum);
            listBox1.TopIndex = listBox1.Items.Count - 1;


            #endregion

        }
        /// <summary>
        /// 连续执行按钮,设置连续执行时间间隔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            
            VmSolution.Instance.SetRunInterval(1000);
            VmSolution.Instance.ContinuousRunEnable = true;

            listBox1.Items.Add("连续执行中");
            listBox1.TopIndex = listBox1.Items.Count - 1;

        }

        /// <summary>
        /// 停止,方案停止连续执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            
            VmSolution.Instance.ContinuousRunEnable = false;

            listBox1.Items.Add("停止.");
            listBox1.TopIndex = listBox1.Items.Count - 1;

        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                VmSolution.Save();   // 保存方案到原始路径并替换原有文件1.2_2:16
                //VmSolution.SaveAs("path");   //保存方案到自定义路径
            }
            catch (VmException ex)
            {
                string message = "异常，Sol Save Failed." + ex.errorCode;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                //MessageBox.Show("VM SDK ERROR." + Convert.ToString(ex.errorCode, 16));
            }
            ////检测硬盘大小是否足够保存文件
            ///VmSolution.Instance.SaveFailed = false;   //假设硬盘空间足够保存文件 
            //if (VmSolution.Instance.SaveFailed)
            //{
            //    MessageBox.Show("硬盘空间保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            //    //文件保存成功提示
            //    MessageBox.Show("文件保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            listBox1.Items.Add("保存成功.");
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }
        /// <summary>
        /// KillProcess
        /// </summary>       
        /// //void KillProcess(string strKillName)
        //{
        //    foreach(System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
        //    {
        //        if (p.ProcessName.Contains(strKillName))
        //        {
        //            try
        //            {
        //                p.Kill();
        //                p.WaitForExit();

        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message.ToString());
        //            }
        //        }
        //    }
        //}




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void vmRenderControl1_Load(object sender, EventArgs e)
        {
            //VMControls.Winform.Release.VmRenderControl vmRenderControl = new VMControls.Winform.Release.VmRenderControl();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void vmParamsConfigWithRenderControl1_Load(object sender, EventArgs e)
        {

        }

        private void vmParamsConfigControl1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                vmProcedureConfigControl1.BindSingleProcedure(comboBox1.Text); //绑定下拉框中选中的流程名称
                //vmProcedureConfigControl1.BindSingleProcedure(textBox1.Text); //绑定下拉框中选中的流程名称
            }
            catch (VmException ex)
            {
                MessageBox.Show("单流程失败." + Convert.ToString(ex.errorCode, 16));
            }
        }
        /// <summary>
        /// 多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                vmProcedureConfigControl1.BindMultiProcedure(); //绑定多流程

            }
            catch (VmException ex)
            {
                MessageBox.Show("多流程失败." + Convert.ToString(ex.errorCode, 16));
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            try
            {
                ProcessInfoList vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();      //获取所有流程信息
                VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance["OCRDemo"];
                if (vmProcessInfoList.nNum == 0)  //判断是否获取到流程列表
                {
                    MessageBox.Show("未获取到流程列表.");
                    return;
                }
                comboBox1.Items.Clear();  //清除原有列表
                comboBox1.Items.Add("OCRDemo");
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

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "E:\\VisualSoftware\\";
                openFileDialog.Filter = "VM Sol File|*.prc";
                DialogResult openFileRes = openFileDialog.ShowDialog();

                //System.Diagnostics.Debug.WriteLine($"对话框结果: {openFileRes}");

                if (openFileRes == DialogResult.OK)
                {
                    textBox2.Text = openFileDialog.FileName;
                    //System.Diagnostics.Debug.WriteLine($"选中的文件路径: {textBox1.Text}");
                }
                listBox1.Items.Add("选择路径成功");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                MessageBox.Show("选择路径失败." + Convert.ToString(ex.errorCode, 16));
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                VmProcedure.Load(textBox2.Text, "");   //选中路径的prc文件
                listBox1.Items.Add("流程导入成功" + textBox2.Text);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                MessageBox.Show("流程导入失败." + Convert.ToString(ex.errorCode, 16));
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                VmProcedure vmProcess = (VmProcedure)VmSolution.Instance[comboBox1.Text];   //绑定下拉框中选中的流程名称
                vmProcess.SaveAs("E:\\" + comboBox1.Text + ".prc");   //保存方案到自定义路径
                listBox1.Items.Add("流程导出成功" + "E:\\" + comboBox1.Text + ".prc");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                MessageBox.Show("流程导出失败." + Convert.ToString(ex.errorCode, 16));
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                VmSolution.Instance.DeleteOneProcedure(comboBox1.Text);
                listBox1.Items.Add("流程删除成功" + comboBox1.Text);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                MessageBox.Show("流程删除失败." + Convert.ToString(ex.errorCode, 16));
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                VmProcedure vmProcess = (VmProcedure)VmSolution.Instance[comboBox1.Text];  //绑定下拉框中选中的流程名称
                vmProcess.Run();
                //vmProcess.ContinuousRunEnable = true;  //连续运行标志
                listBox1.Items.Add("执行一次" + comboBox1.Text + "成功");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (VmException ex)
            {
                MessageBox.Show("单流程执行失败." + Convert.ToString(ex.errorCode, 16));
            }

            #region
            ////回调获取结果，渲染结果和数据结果
            //VmProcedure vmProcess1 = (VmProcedure)VmSolution.Instance["OCRDemo"];   //流程对象
            //vmRenderControl1.ModuleSource = vmProcess1;
            //string ocrResult = vmProcess1.ModuResult.GetOutputString("out").astStringVal[0].strValue;  //获取识别结果  
            //listBox1.Items.Add("字符识别结果:" + ocrResult);
            //listBox1.TopIndex = listBox1.Items.Count - 1;            


            //使用通讯或硬件通讯进行外部触发时：调用回调函数进行结果获取

            #endregion

        }
    }
}
