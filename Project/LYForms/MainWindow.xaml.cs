using LYForms.Common;
using LYForms.Models;
using LYForms.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LYForms
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            //
            
            ViewShow();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.IsEnabled = true;

            this.txtLogin.Text = "LongYuan";
            this.logo.Source = new BitmapImage(new Uri("../../Images/Logo2.ico", UriKind.Relative));

            MainWindowViewModel model = new MainWindowViewModel();
            model.StackPanelMain = StackPanelMain;
            this.DataContext = model;

        }

        /// <summary>
        /// 全屏显示
        /// </summary>
        private void ViewShow()
        {
            //this.WindowState = System.Windows.WindowState.Maximized;//默认最大化
            //this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;//将宽度设置为屏幕的宽度
            //this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;//将高度设置为屏幕的高度

            this.WindowStyle = System.Windows.WindowStyle.None;
            this.WindowState = System.Windows.WindowState.Maximized;
        }


        //按Esc键退出全屏
        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(this.WindowState == System.Windows.WindowState.Maximized)
            {
                if (e.Key == Key.Escape)//Esc键
                {
                    this.WindowState = System.Windows.WindowState.Normal;
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中
                }
            }
      
        }


        /// <summary>
        /// 显示实时时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            this.lbltime.Content = "当前时间:"+ DateTime.Now.ToString();
        }

        //窗口拖动
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
