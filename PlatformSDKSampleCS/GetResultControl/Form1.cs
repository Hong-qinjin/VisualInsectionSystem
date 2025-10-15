using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VM.Core;
using VM.PlatformSDKCS;

namespace GetResultControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        /****************************************************************************
        * @fn           初始化
        * @fn           Init
        ****************************************************************************/
        private void Init()
        {
            textBoxSolutionPath.Text = System.IO.Path.GetFullPath("./SampleSolution/GetResultSample.sol");
            buttonExecuteOnce.Enabled = false;
        }

        /****************************************************************************
         * @fn           清空消息
         * @fn           Clear the contents of the List Box
         ****************************************************************************/
        private void buttonDeleteMsg_Click(object sender, EventArgs e)
        {
            listBoxMsg.Items.Clear();
        }

        /****************************************************************************
         * @fn           选择方案路径
         * @fn           Select solution's path
         ****************************************************************************/
        private void buttonChooseSoluPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VM Sol File|*.sol*";
            DialogResult openFileRes = openFileDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                textBoxSolutionPath.Text = openFileDialog.FileName;
            }
        }

        /****************************************************************************
        * @fn           加载方案
        * @fn           Load solution
        ****************************************************************************/
        private void buttonLoadSolution_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            int nProgress = 0;
            progressBarSaveAndLoad.Value = nProgress;
            labelProgress.Text = nProgress.ToString();
            labelProgress.Refresh();

            try
            {
                this.Enabled = false;
                VmSolution.Load(textBoxSolutionPath.Text, textBoxPassword.Text);
            }
            catch (VmException ex)
            {
                strMsg = "LoadSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }
            finally
            {
                this.Enabled = true;
            }

            strMsg = "LoadSolution success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;

            nProgress = 100;
            labelProgress.Text = nProgress.ToString();
            labelProgress.Refresh();
            progressBarSaveAndLoad.Value = Convert.ToInt32(nProgress);

            //方案加载完成后，关闭所有模块的结果回调，提高运行效率
            //After the solution is loaded, disable the callback of all modules to improve efficiency.
            VmSolution.Instance.DisableModulesCallback();
            buttonExecuteOnce.Enabled = false;
        }

        /****************************************************************************
        * @fn           获取图像源结果
        * @fn           Get ImageSource Module Result
        ****************************************************************************/
        private void buttonGetImageSourceResult_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                var pro = VmSolution.Instance["Flow1"] as VmProcedure;
                if (pro == null)
                {
                    strMsg = "can not find procedure Flow1";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                var module = pro["Image Source1"] as ImageSourceModuleCs.ImageSourceModuleTool;
                if (module == null)
                {
                    strMsg = "can not find module Image Source1";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                module.EnableResultCallback();

                pro.Run();
                var imageSourceData = module.ModuResult.ImageData;
                ShowImageDialog(imageSourceData);

                VmSolution.Instance.DisableModulesCallback();
                buttonExecuteOnce.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ex is VmException)
                {
                    VmException vmex = (VmException)ex;
                    strMsg = "Get Image result is failed. Error Code: " + Convert.ToString(vmex.errorCode, 16);
                }
                else
                {
                    strMsg = "Get Image result is failed: " + ex.Message;
                }
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            strMsg = "Get Image result success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }
        /****************************************************************************
         * @fn           通过回调获取图像源结果
         * @fn           Get ImageSource Module Result From CallBack
         ****************************************************************************/
        private void buttonGetImageSourceResultFromCallBack_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                var pro = VmSolution.Instance["Flow1"] as VmProcedure;
                if (pro == null)
                {
                    strMsg = "can not find procedure Flow1";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                var module = pro["Image Source1"] as ImageSourceModuleCs.ImageSourceModuleTool;
                if (module == null)
                {
                    strMsg = "can not find module Image Source1";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                module.EnableResultCallback();

                module.ModuleResultCallBackArrived += Module_ModuleResultCallBackArrived;

                pro.Run();

                module.ModuleResultCallBackArrived -= Module_ModuleResultCallBackArrived;

                VmSolution.Instance.DisableModulesCallback();
                buttonExecuteOnce.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ex is VmException)
                {
                    VmException vmex = (VmException)ex;
                    strMsg = "Get Image result from callback is failed. Error Code: " + Convert.ToString(vmex.errorCode, 16);
                }
                else
                {
                    strMsg = "Get Image result from callback is failed." + ex.Message;
                }
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            strMsg = "Get Image result from callback success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        /****************************************************************************
         * @fn           模块结果回调
         * @fn           Module Result CallBack Function
         ****************************************************************************/
        private void Module_ModuleResultCallBackArrived(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                var pro = VmSolution.Instance["Flow1"] as VmProcedure;
                var module = pro["Image Source1"] as ImageSourceModuleCs.ImageSourceModuleTool;

                var imageSourceData = module.ModuResult.ImageData;

                ShowImageDialog(imageSourceData);
            }
            catch (VmException ex)
            {
                strMsg = "ModuleResultCallBack is exception Error Code: " + Convert.ToString(ex.errorCode, 16);
                System.Diagnostics.Debugger.Log(0, null, strMsg);
            }
        }

        private void ShowImageDialog(ImageBaseData imageSourceData)
        {
            if (imageSourceData == null) { return; }
            try
            {
                Bitmap outImageData = imageSourceData.ToBitmap();

                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = outImageData;
                pictureBox.Size = new Size(600, 600);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                Form form = new Form() { Width = 600, Height = 600, StartPosition = FormStartPosition.CenterParent };
                form.Controls.Add(pictureBox);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debugger.Log(0, null, ex.Message);
            }
        }

        /****************************************************************************
         * @fn           获取流程结果
         * @fn           Get Procedure Result
         ****************************************************************************/
        private void buttonGetProcedureResult_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                var pro = VmSolution.Instance["Flow1"] as VmProcedure;
                if (pro == null)
                {
                    strMsg = "can not find procedure Flow1";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                var module = pro["Image Source1"] as ImageSourceModuleCs.ImageSourceModuleTool;
                if (module == null)
                {
                    strMsg = "can not find module Image Source1";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                module.EnableResultCallback();
                pro.EnableResultCallback();
                pro.Run();
                var imageSourceData = pro.ModuResult.GetOutputImageV2("ImageData");
                ShowImageDialog(imageSourceData);
                VmSolution.Instance.DisableModulesCallback();
                buttonExecuteOnce.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ex is VmException)
                {
                    VmException vmex = (VmException)ex;
                    strMsg = "Get Procedure result is failed. Error Code: " + Convert.ToString(vmex.errorCode, 16);
                }
                else
                {
                    strMsg = "Get Procedure result is failed. Error:" + ex.Message;
                }
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            strMsg = "Get Procedure result success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        /****************************************************************************
        * @fn           获取模块结果并在渲染控件中添加图形
        * @fn           Get Module Result And Add Shape To RenderControl
        ****************************************************************************/
        private void buttonAddShape_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                var moduleCircleFind = VmSolution.Instance["Flow1.Circle Search1"] as IMVSCircleFindModuCs.IMVSCircleFindModuTool;
                var moduleImageSource = VmSolution.Instance["Flow1.Image Source1"] as ImageSourceModuleCs.ImageSourceModuleTool;
                if (moduleImageSource == null || moduleCircleFind == null)
                {

                    strMsg = moduleImageSource == null ? "can not find module Flow1.Image Source1" : "can not find module Flow1.Circle Search1";
                    listBoxMsg.Items.Add(strMsg);
                    listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                    return;
                }
                moduleCircleFind.EnableResultCallback();
                moduleImageSource.EnableResultCallback();

                vmRenderControl1.ModuleSource = moduleImageSource;

                moduleCircleFind.ModuleResultCallBackArrived -= ModuleCircleFind_ModuleResultCallBackArrived;
                moduleCircleFind.ModuleResultCallBackArrived += ModuleCircleFind_ModuleResultCallBackArrived;
                buttonExecuteOnce.Enabled = true;
            }
            catch (Exception ex)
            {
                if (ex is VmException)
                {
                    VmException vmex = (VmException)ex;
                    strMsg = "Add shape is failed. Error Code: " + Convert.ToString(vmex.errorCode, 16);
                }
                else
                {
                    strMsg = "Add shape is failed. Error: " + ex.Message;
                }
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            strMsg = "Add shape success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        private void ModuleCircleFind_ModuleResultCallBackArrived(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                var module = VmSolution.Instance["Flow1.Circle Search1"] as IMVSCircleFindModuCs.IMVSCircleFindModuTool;
                if (module == null) return;
                var circle = module.ModuResult.OutputCircle;
                var shape = new VMControls.WPF.CircleEx();
                shape.CenterPoint = new System.Windows.Point(circle.CenterPoint.X, circle.CenterPoint.Y);
                shape.Radius = circle.Radius;
                shape.StrokeThickness = 2;
                shape.Stroke = "#FF0000";
                vmRenderControl1.AddShape(shape);
            }
            catch (Exception ex)
            {
                if (ex is VmException)
                {
                    VmException vmex = (VmException)ex;
                    strMsg = "ModuleResultCallBack is exception Error Code: " + Convert.ToString(vmex.errorCode, 16);
                }
                else
                {
                    strMsg = "ModuleResultCallBack is exception Error: " + ex.Message;
                }
                System.Diagnostics.Debugger.Log(0, null, strMsg);
            }
        }

        private void buttonExecuteOnce_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                var pro = VmSolution.Instance["Flow1"] as VmProcedure;
                if (pro == null) return;
                pro.Run();
            }
            catch (VmException ex)
            {
                strMsg = "Execute Procedure failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }
            strMsg = "Execute Procedure Succeed";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
            return;
        }

        private void buttonRemoveShape_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                vmRenderControl1.ModuleSource = null;
                var moduleCircleFind = VmSolution.Instance["Flow1.Circle Search1"] as IMVSCircleFindModuCs.IMVSCircleFindModuTool;
                if (moduleCircleFind != null)
                {
                    moduleCircleFind.ModuleResultCallBackArrived -= ModuleCircleFind_ModuleResultCallBackArrived;
                }
                VmSolution.Instance.DisableModulesCallback();
                buttonExecuteOnce.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ex is VmException)
                {
                    VmException vmex = (VmException)ex;
                    strMsg = "Remove shape is failed. Error Code: " + Convert.ToString(vmex.errorCode, 16);
                }
                else
                {
                    strMsg = "Remove shape is failed. Error: " + ex.Message;
                }

                listBoxMsg.Items.Add(strMsg);
                listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
                return;
            }

            strMsg = "Remove shape success";
            listBoxMsg.Items.Add(strMsg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        /****************************************************************************
         * @fn           退出
         * @fn           Quit
         ****************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            VmSolution.Instance?.Dispose();
        }

        /****************************************************************************
         * @fn           切换语言
         * @fn           Switch language
        ****************************************************************************/
        private void buttonLang_Click(object sender, EventArgs e)
        {
            // 仅切换Demo界面语言，控件语言通过配置文件切换
            // Only switch the language of demo interface,and switch the language of control through the configuration file
            int currentLCID = Thread.CurrentThread.CurrentUICulture.LCID;
            currentLCID = (2052 == currentLCID) ? 1033 : 2052;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLCID);

            ApplyLangResource(this);
        }

        private void ApplyLangResource(Control control)
        {
            foreach (Control ct in control.Controls)
            {
                resourceManager.ApplyResources(ct, ct.Name, Thread.CurrentThread.CurrentUICulture);
                if (ct.HasChildren)
                {
                    ApplyLangResource(ct);
                }
            }
        }

        private ComponentResourceManager resourceManager = new ComponentResourceManager(typeof(Form1));
    }
}
