using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
/// 系统Demo01-02方案加载与显示，执行，保存界面
/// </summary>  

namespace Systemdemo01
{
    /// <summary>
    /// 方案加载与显示界面
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                                                                        // 调试输出对话框结果
                System.Diagnostics.Debug.WriteLine($"对话框结果: {openFileRes}");

                if (openFileRes == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog.FileName; 

                    // 调试输出获取的路径
                    System.Diagnostics.Debug.WriteLine($"选中的文件路径: {textBox1.Text}");
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
            }
            catch (VmException ex)
            {
                string message = "Clear Sol Failed." + ex.errorCode;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                string message = "File Not Found." + ex.Message;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
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
            catch(VmException ex)
            {
                string message = "参数配置页面失败." + ex.errorCode;
                listBox1.Items.Add(message);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }


            VmSolution.Instance.SyncRun();   //同步执行一次方案中所有流程1.2_2:07
            listBox1.Items.Add("单次执行成功");
            listBox1.TopIndex = listBox1.Items.Count - 1;

            #region 

            // 1.3获取结果,分为渲染结果以及数据结果
            VMControls.Winform.Release.VmRenderControl vmRenderControl = new VMControls.Winform.Release.VmRenderControl();
            //VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance[""];   //流程对象 
            VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance["OCRDemo"];   //流程对象            
            
            vmRenderControl1.ModuleSource = vmProcedure;  //设置渲染控件的显示源与流程绑定
            vmRenderControl.Show();  //显示渲染控件                                       
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
        /// 连续执行按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {

            listBox1.Items.Add("连续执行中");
            listBox1.TopIndex = listBox1.Items.Count - 1;

        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {

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
            
            VmSolution.Save();   // VM.Solution.save方法,保存方案到原始路径并替换原有文件1.2_2:16
            //VmSolution.SaveAs("path");   //保存方案到自定义路径

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 渲染控件加载,图像显示控件
        /// 1.3渲染以及结果获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
    }
}
