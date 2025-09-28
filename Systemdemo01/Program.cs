using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Systemdemo01
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 指示应用程序如何响应未经处理的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //// 处理UI线程异常
            //Application.ThreadException += Application_ThreadException;
            //// 处理非UI线程异常
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;  //程序集解析事件处理

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

           //VmSolution.Instance.Dispose();  //释放VM资源
        }

        /*
       private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
       {
           try
           {
               //string assemblyName = new AssemblyName(args.Name).Name + ".dll";    //引用
               string assemblyName = new AssemblyName(args.Name).Name;
               //string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libs", assemblyName);
               string vmPath = @"C:\Program Files\VisionMaster4.4.0\Development\V4.x\ComControls\Assembly\";

               // 尝试从VisionMaster目录加载
               string assemblyPath = Path.Combine(vmPath, assemblyName + ".dll");

               if (System.IO.File.Exists(assemblyPath))
               {
                   // 使用LoadFrom会引发同样的警告，所以我们使用Load(byte[])来避免LoadFrom上下文
                   byte[] assemblyBytes = System.IO.File.ReadAllBytes(assemblyPath);
                   return Assembly.Load(assemblyBytes);
               }

               // 如果未找到，尝试从应用程序目录加载
               string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
               assemblyPath = Path.Combine(appDirectory, assemblyName + ".dll");
               if (File.Exists(assemblyPath))
               {
                   return Assembly.LoadFrom(assemblyPath);
               }

           }
           catch (Exception ex)
           {
               MessageBox.Show($"程序集加载错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }

           return null;

       }
       */
    }
}
