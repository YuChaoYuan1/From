/*----------------------------------------------------------------------
// 
// 文件名：
// 功能： 服务器
// 
// 修改人：
// 时间：
// 说明：
----------------------------------------------------------------------*/
using LYForms.Models;
using LYForms.Models.LogCs;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace LYForms.Views
{
    /// <summary>
    /// Server.xaml 的交互逻辑
    /// </summary>
    public partial class Server : UserControl
    {
        ElectricityMeter electricityMeter = new ElectricityMeter();
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        public Server()
        {
            InitializeComponent();
            if (textBoxIP.Text == "" || textBoxport.Text == "")
                MessageBox.Show("请输入IP和端口！");
            else
            {
                try
                {
                    listener = new TcpListener(IPAddress.Parse(textBoxIP.Text), Int32.Parse(textBoxport.Text));
                    listener.Start();//开始监听
                    MessageBox.Show("服务器开始监听！");
                    Thread t3 = new Thread(() => listen_connct());
                    t3.IsBackground = true;//设置t3为后台进程
                    t3.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void listen_connct()
        {
            while (true)
            {
                if (client == null)
                {
                    client = listener.AcceptTcpClient();
                }
                stream = client.GetStream();//获取客户端网络流
                byte[] data1 = new byte[1024];
                if (stream.DataAvailable)
                    Msg_Recv(stream, data1);
                Thread.Sleep(10);
            }
        }

        private void Msg_Recv(NetworkStream st, byte[] data)
        {
            int len = st.Read(data, 0, 1024);
            string messge = Encoding.UTF8.GetString(data, 0, len);
            string str =DateTime.Now + "\t" + messge;
            Dispatcher.Invoke(() => { textBoxRecv.Text = DateTime.Now + "\t" + messge; });
            electricityMeter.MessageAdd(str, EnumLogType.详细信息, true);
        }

        private void button_send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = textBoxSend.Text;
                byte[] data = new byte[1024];
                data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxIP.Text == "" || textBoxport.Text == "")
                MessageBox.Show("请输入IP和端口！");
            else
            {
                try
                {
                    listener = new TcpListener(IPAddress.Parse(textBoxIP.Text), Int32.Parse(textBoxport.Text));
                    listener.Start();//开始监听
                    MessageBox.Show("服务器开始监听！");
                    Thread t3 = new Thread(() => listen_connct());
                    t3.IsBackground = true;//设置t3为后台进程
                    t3.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
