using Panuon.UI.Silver;
using Panuon.UI.Silver.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LYForms.Views
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        Storyboard stdStart;

        public Login()
        {
            InitializeComponent();


            #region 动画

            stdStart = (Storyboard)this.Resources["start"];
            stdStart.Completed += (a, b) =>
            {
                this.root.Clip = null;
            };
            this.Loaded += Window_Loaded;
            this.Unloaded += Window_Unloaded;
            this.Closed += Login_Closed;

            #endregion 
        }

        private void Login_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stdStart.Begin();//启动动画

            login.Visibility = Visibility.Visible;

            //selectPlugins.Visibility = Visibility.Collapsed;

            #region 事件监听

            //login.OnLoginSucceed += OnLoginSucceed;
            //login.OnLoginClosed += OnLoginClosed;

            //selectPlugins.OnBackLoginClick += SelectPlugins2Login;
            //selectPlugins.OnGoMainWindowClick += OnGoMainWindowClick;

            #endregion

            InitLoginData();//所有数据库操作在此操作后执行
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            #region 解除事件监听

            Loaded -= Window_Loaded;
            Unloaded -= Window_Unloaded;
            Closed -= Login_Closed;

            //login.OnLoginSucceed -= OnLoginSucceed;
            //login.OnLoginClosed -= OnLoginClosed;

            //selectPlugins.OnBackLoginClick -= SelectPlugins2Login;
            //selectPlugins.OnGoMainWindowClick -= OnGoMainWindowClick;

            #endregion
        }

        private void OnLoginClosed()
        {
            Close();
        }

        private async void InitLoginData()
        {
            IsEnabled = false;
            var handler = PendingBox.Show("连接数据库...", "请等待", false, Application.Current.MainWindow, new PendingBoxConfigurations()
            {
                LoadingForeground = "#5DBBEC".ToColor().ToBrush(),
                ButtonBrush = "#5DBBEC".ToColor().ToBrush(),
            });
            bool connectionSucceed = false;
            //检查数据
            //await Task.Run(() =>
            //{
            //    connectionSucceed = InitData.NullDataCheck();
            //});
            //await Task.Delay(200);
            //if (connectionSucceed)
            //{
            //    handler.UpdateMessage("连接成功,正在加载插件...");
            //}
            await Task.Delay(200);
            handler.Close();

            IsEnabled = true;
            if (!connectionSucceed)
            {
                MessageBoxX.Show("数据库连接失败", "连接错误");
            }
          
        }

    }
}
