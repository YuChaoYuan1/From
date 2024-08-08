using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace LYForms
{
    /// <summary>
    /// Gold.xaml 的交互逻辑
    /// </summary>
    public partial class Gold : Window
    {
        SerialPort serialPort = new SerialPort();
        SerialPort[] serialPorts = new SerialPort[] { };
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        public Gold()
        {
            InitializeComponent();

            //SetDataReceived();


            //获取需要打开的串口

            GetPort(); //代码运行死锁
            
        }

        private void GetPort()
        {
            string[] port = SerialPort.GetPortNames();
            List<string> list = new List<string>();
            foreach(string item in port)
            {
                if (item.Contains("COM90"))
                {
                    list.Add(item);
                }
            }
            serialPorts = new SerialPort[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                SerialPort serialPorts2 = new SerialPort();
                serialPorts2.PortName = list[i];
                serialPorts2.BaudRate = 9600;
                serialPorts2.DataBits = 8;
                serialPorts2.Parity = Parity.None;
                serialPorts2.StopBits = StopBits.One;
                try
                {
                    serialPorts2.Open();
                }
                catch (Exception ex)
                {
                    serialPorts2.Close();
                    serialPorts2.Open();
                    MessageBox.Show(ex.ToString());
                }
                Jianting(serialPorts2);
            }
        }

        private void Jianting(SerialPort serialPort1)
        {
            serialPort1.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int count = serialPort.BytesToRead;
            Byte[] buffer = new Byte[count];
            serialPort.Read(buffer, 0, count);

            string receivedata = serialPort.PortName +":\t" + DateTime.Now + "\t" + System.Text.Encoding.UTF8.GetString(buffer) + "\r\n";

            txtBox.Text = txtBox.Text + receivedata;
        }

        #region 单个串口监听
        /// <summary>
        /// 开启监听 单个串口
        /// </summary>
        private void SetDataReceived()
        {
            string[] port = SerialPort.GetPortNames();

            serialPort.PortName = "COM9001";
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                serialPort.Close();
                serialPort.Open();
                MessageBox.Show(ex.ToString());
            }
            serialPort.DataReceived += SerialPort1_DataReceived;
        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int count = serialPort.BytesToRead;
            Byte[] buffer = new Byte[count];
            serialPort.Read(buffer, 0, count);

            string receivedata = serialPort.PortName + DateTime.Now + "\t" + System.Text.Encoding.UTF8.GetString(buffer) + "\r\n";
            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtBox.Text = txtBox.Text + receivedata;
            }));//创建异步线程接收
        }
        #endregion
    }

}
