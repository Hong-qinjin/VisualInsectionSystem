using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VM.Core;
using VM.PlatformSDKCS;

namespace SolutionControlCS_WPF
{
    /// <summary>
    /// 方案信息类
    /// Solution Information
    /// </summary>
    public class VmSolutionInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public VmSolutionInfo()
        {
            _vmSolutionPath = "";
            _vmSolutionPassword = "";
        }

        /// <summary>
        /// 方案路径
        /// Solution Path
        /// </summary>
        private string _vmSolutionPath;
        public string vmSolutionPath
        {

            get { return _vmSolutionPath; }
            set
            {
                _vmSolutionPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vmSolutionPath"));
            }
        }

        /// <summary>
        /// 方案密码
        /// Solution Password
        /// </summary>
        private string _vmSolutionPassword;
        public string vmSolutionPassword
        {

            get { return _vmSolutionPassword; }
            set
            {
                _vmSolutionPassword = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vmSolutionPassword"));
            }
        }

    }

    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public VmSolutionInfo vmSolutionInfo = new VmSolutionInfo();
        bool mSolutionIsLoad = false;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                VmSolutionPath.DataContext = vmSolutionInfo;
                VmSolutionPassword.DataContext = vmSolutionInfo;
            }
            catch (Exception ex)
            {
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);
                if (null != vmEx)
                {
                    string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);
                    System.Windows.MessageBox.Show(strMsg);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 选择方案
        /// Select Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSolution_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VM Sol File|*.sol*";
            System.Windows.Forms.DialogResult openFileRes = openFileDialog.ShowDialog();
            if (System.Windows.Forms.DialogResult.OK == openFileRes)
            {
                vmSolutionInfo.vmSolutionPath = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// 加载方案
        /// Load Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSolution_Click(object sender, RoutedEventArgs e)
        {
            string strMsg = null;

            try
            {
                VmSolution.Load(vmSolutionInfo.vmSolutionPath, vmSolutionInfo.vmSolutionPassword);
                mSolutionIsLoad = true;
            }
            catch (VmException ex)
            {
                strMsg = "LoadSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxResult.Items.Insert(0, strMsg);
                return;
            }
            strMsg = "LoadSolution success";
            listBoxResult.Items.Insert(0, strMsg);
        }

        /// <summary>
        /// 保存方案
        /// Save Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSolution_Click(object sender, RoutedEventArgs e)
        {
            string strMsg = null;

            if (mSolutionIsLoad == true)
            {
                try
                {
                    VmSolution.Save();
                }
                catch (VmException ex)
                {
                    strMsg = "SaveSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                    listBoxResult.Items.Insert(0, strMsg);
                    return;
                }

                strMsg = "SaveSolution success";
                listBoxResult.Items.Insert(0, strMsg);
            }
            else
            {
                strMsg = "No solution file.";
                listBoxResult.Items.Insert(0, strMsg);
            }
        }

        /// <summary>
        /// 获取方案版本
        /// Get Solution Version
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetSolutionVersion_Click(object sender, RoutedEventArgs e)
        {

            if (mSolutionIsLoad == true)
            {
                string strSolutionVersion = "The solution version is " +
                VmSolution.Instance.GetSolutionVersion(vmSolutionInfo.vmSolutionPath, vmSolutionInfo.vmSolutionPassword);
                listBoxResult.Items.Insert(0, strSolutionVersion);
            }
            else
            {
                string strMsg = "No solution file.";
                listBoxResult.Items.Insert(0, strMsg);
            }
        }

        /// <summary>
        /// 关闭方案
        /// Close Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseSolution_Click(object sender, RoutedEventArgs e)
        {
            string strMsg = null;
            try
            {
                if (mSolutionIsLoad == true)
                {
                    VmSolution.Instance.CloseSolution();
                    mSolutionIsLoad = false;
                }
                else
                {
                    strMsg = "No solution file.";
                    listBoxResult.Items.Insert(0, strMsg);
                    return;
                }

            }
            catch (VmException ex)
            {
                strMsg = "CloseSolution failed. Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxResult.Items.Insert(0, strMsg);
                return;
            }
            strMsg = "CloseSolution success";
            listBoxResult.Items.Insert(0, strMsg);
        }

        /// <summary>
        /// 获取方案路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetSolutionPath_Click(object sender, RoutedEventArgs e)
        {
            if (mSolutionIsLoad == true)
            {
                string strSolutionPath = "The solution path is " + VmSolution.Instance.SolutionPath;
                listBoxResult.Items.Insert(0, strSolutionPath);
            }
            else
            {
                string strMsg = "No solution file.";
                listBoxResult.Items.Insert(0, strMsg);
            }
        }

        /// <summary>
        /// 检查方案密码
        /// Check Solution Password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckSolutionPassword_Click(object sender, RoutedEventArgs e)
        {
            string strMsg = null;
            try
            {
                if (mSolutionIsLoad == true)
                {
                    if (VmSolution.Instance.HasPassword(vmSolutionInfo.vmSolutionPath))
                    {
                        strMsg = "The solution has password.";
                    }
                    else
                    {
                        strMsg = "The solution has no password.";
                    }
                    listBoxResult.Items.Insert(0, strMsg);
                }
                else
                {
                    strMsg = "No solution file.";
                    listBoxResult.Items.Insert(0, strMsg);
                }
            }
            catch (VmException ex)
            {
                strMsg = "CheckPassWord failed, Error Code: " + Convert.ToString(ex.errorCode, 16);
                listBoxResult.Items.Insert(0, strMsg);
                return;
            }
        }

        /// <summary>
        /// 清空消息
        /// Clear Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearMessage_Click(object sender, RoutedEventArgs e)
        {
            listBoxResult.Items.Clear();
        }

        /// <summary>
        /// 退出
        /// Quit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            VmSolution.Instance?.Dispose();
        }

        /// <summary>
        /// 切换语言
        /// Switch Language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchLanguage_Click(object sender, RoutedEventArgs e)
        {
            // 仅切换Demo界面语言，控件语言通过配置文件切换
            // Only switch the language of demo interface,and switch the language of control through the configuration file
            var dictionaries = System.Windows.Application.Current.Resources.MergedDictionaries;
            var dictionary = dictionaries[0];
            dictionaries.Remove(dictionary);
            dictionaries.Add(dictionary);
        }
    }
}
