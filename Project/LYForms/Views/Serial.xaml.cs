using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
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

namespace LYForms.Views
{
    /// <summary>
    /// Serial.xaml 的交互逻辑
    /// </summary>
    public partial class Serial : UserControl
    {
        SerialPort serialPort = new SerialPort();
        string[] portname = SerialPort.GetPortNames();
        public Serial()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加串口
        /// </summary>
        void readPort()
        {
            foreach(string item in portname)
            {
                cmbPort.Items.Add(item);
            }
        }

    }
}
