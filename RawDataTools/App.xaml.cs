using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RawDataTools
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();

            if (e.Args.Length == 1)
            {
                FileInfo fileInfo = new FileInfo(e.Args[0]);
                if (fileInfo.Exists)
                {
                    MessageBox.Show("当前打开文件:" + fileInfo.FullName);
                    
                    mainWindow.StartOnDoubleClickFile(fileInfo.FullName);
                }
            }
        }
    }
}
