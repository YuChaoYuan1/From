using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LYForms.Views
{
    /// <summary>
    /// ReadComputer.xaml 的交互逻辑
    /// </summary>
    public partial class ReadComputer : UserControl
    {
        [DllImport("Kernel32.dll")]
        public static extern bool GetSystemPowerStatus(ref SystemPowerStatus systemPowerStatus);
        public ReadComputer()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += DataJIAZAI;
            timer.IsEnabled = true;
        }
        public void DataJIAZAI(object sender, EventArgs e)
        {
            //Task.Run(() =>
            //{
            //    this.Dispatcher.Invoke(() =>
            //    {
                    SystemPowerStatus status = new SystemPowerStatus();
                    if (GetSystemPowerStatus(ref status))
                    {
                        lbl1.Content = status.BatteryLifePercent + "%";
                        lbl2.Content = Wwitch(status.BatteryFlag);
                        lbl3.Content = (status.ACLineStatus == 1) ? "正在充电" : "未在充电";
                        lbl4.Content = (status.SystemStatusFlag == 1) ? "开启" : "关闭";

                    }
            //    });
            //});
        }


        private object Wwitch(int i)
        {
            string getStatus = string.Empty;
            switch (i)
            {
                case 0:
                    getStatus = "未充电，且电池电量33-66";
                    break;
                case 1:
                    getStatus = "电量大于66%";
                    break;
                case 2:
                    getStatus = "低，小于33%";
                    break;
                case 8:
                    getStatus = "充电中";
                    break;
                case 9:
                    getStatus = "充电中+电量大于66%";
                    break;
                case 128:
                    getStatus = "没有电池";
                    break;
                case 255:
                    getStatus = "无法读取电池标志信息";
                    break;

            }

            return getStatus;
        }

        #region
        public struct SystemPowerStatus
        {  
            public byte ACLineStatus;  // 交流电源状态
            public byte BatteryFlag;  // 电池充电状态
            public byte BatteryLifePercent;  // 剩余电量的百分比。该成员的值可以在0到100的范围内，如果状态未知，则可以是255
            public byte SystemStatusFlag;  //  省电模式
            public int BatteryLifeTime;  //  剩余电池寿命的秒数。如果未知剩余秒数或设备连接到交流电源，则为–1
            public int BatteryFullLifeTime;  // 充满电时的电池寿命秒数。如果未知电池的完整寿命或设备连接到交流电源，则为–1。
        }
        #endregion
    }
}
