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
using VM.Core;
using VM.PlatformSDKCS;
using MvCameraControl;
using IMVSCircleFindModuCs;
using IMVSGroupCs;


namespace Systemdemo01
{
    /// <summary>
    /// 方案加载与显示界面
    /// </summary>
    public partial class MainForm : Form
    {    
        
        public MainForm()
        {
           
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
                     
        }

        /// <summary>
        /// 选择路径按钮功能，打开一个文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 方案加载按钮
        /// </summary>
        /// <param name="sender"></param>f
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
       
            }
            catch (VmException ex)
            {
                string message = "VM_SD异常，Sol Load Failed." + ex.errorCode;
                //MessageBox.Show("VM SDK ERROR." + Convert.ToString(ex.errorCode, 16));
            }
            catch (System.IO.FileNotFoundException ex)
            {
                string message = "File Not Found." + ex.Message;
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


        }
        /// <summary>
        /// 连续执行按钮,设置连续执行时间间隔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button4_Click(object sender, EventArgs e)
        {
            VmSolution.Instance.SetRunInterval(1000);
            VmSolution.Instance.ContinuousRunEnable = true;
        }

        /// <summary>
        /// 停止,方案停止连续执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button5_Click(object sender, EventArgs e)
        {
            VmSolution.Instance.ContinuousRunEnable = false;
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
                string path = "D:\\AutoBox Camera\\ABVisualSystem\\";
                //VmSolution.Save();   // 保存方案到原始路径并替换原有文件1.2_2:16
                VmSolution.SaveAs(path);   //保存方案到自定义路径
            }
            catch (VmException ex)
            {
                string message = "异常，Sol Save Failed." + ex.errorCode;
            }
        }
        /// <summary>
        /// KillProcess
        /// </summary>       
        void KillProcess(string strKillName)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName.Contains(strKillName))
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                    }
                }
            }
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
                ProcessInfoList vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();  //获取所有流程信息
                VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance["Flow1"];
                if (vmProcessInfoList.nNum == 0)    //判断是否获取到流程列表
                {
                    MessageBox.Show("未获取到流程列表.");
                    return;
                }

            }
            catch (Exception ex)
            {
            }


        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    //InitialDirectory = "E:\\VisualSoftware\\",
                    InitialDirectory = "D:\\AutoBox Camera\\ABVisualSystem\\",
                    Filter = "VM Sol File|*.prc"
                };
                DialogResult openFileRes = openFileDialog.ShowDialog();

                //System.Diagnostics.Debug.WriteLine($"对话框结果: {openFileRes}");

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

            //question：combobox中的流程
            try
            {

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
                //VmProcedure vmProcess = (VmProcedure)VmSolution.Instance[comboBox1.Text];  //绑定下拉框中选中的流程名称
                //vmProcess.Run();
                //vmProcess.ContinuousRunEnable = true;  //连续运行标志   
              
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

        /// <summary>
        /// 执行指令流程并返回结果
        /// </summary>
        /// <param name="procedureNmae"></param>
        /// <returns></returns>
        public string ExecuteProcedure(string procedureNmae)
        {
            try
            {
                var procedure = VmSolution.Instance[procedureNmae];
                if (procedure == null)
                {
                    return $"流程 {procedureNmae} not exist!";
                }

                //zhi xing liu cheng
                VmProcedure vmProcess = (VmProcedure)procedure;
                vmProcess.Run();

                // wait act
                System.Threading.Thread.Sleep(1000);

                // 获取识别结果
                string ocrResult = vmProcess.ModuResult.GetOutputString("out").astStringVal[0].strValue;
                string ocrConfidence = vmProcess.ModuResult.GetOutputString("out0").astStringVal[0].strValue;
                string ocrNum = vmProcess.ModuResult.GetOutputInt("out1").pIntVal[0].ToString();

               

                //返回结果
                return $"执行流程 {procedureNmae} 成功:result1={ocrResult}, result2={ocrConfidence}, result3={ocrNum}";

            }
            catch (VmException ex)
            {
                string errorMsg = $"流程 {procedureNmae} 执行失败: {Convert.ToString(ex.errorCode, 16)}";
     
                return errorMsg;
            }
            catch (Exception ex)
            {
                string errorMsg = $"流程 {procedureNmae} 执行异常: {ex.Message}";
             
                return errorMsg;
            }
        }

    }
}
