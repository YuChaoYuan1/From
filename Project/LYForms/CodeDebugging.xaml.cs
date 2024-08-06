using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace LYForms
{
    //68 01 fe 08 10 30 02 d5

    /// <summary>
    /// CodeDebugging.xaml 的交互逻辑
    /// </summary>
    public partial class CodeDebugging : Window
    {
        public CodeDebugging()
        {
            InitializeComponent();
           ReadCK();
            settextevent += set;
        }

        private SerialPort sp1 = new SerialPort();
        StringBuilder builder = new StringBuilder();
        private void button1_Click(object sender, EventArgs e)
        {
            if (sp1.IsOpen)
            {
                if (!string.IsNullOrEmpty(this.textBox1.Text))
                {
                    sp1.WriteLine(this.textBox1.Text + "\r\n");

                }
                else
                {
                    MessageBox.Show("发送数据为空");
                }
            }
            else
            {
                MessageBox.Show("COM1未打开！");
            }


        }

        public delegate void settext(string text);
        public event settext settextevent;
        public void set(string text)
        {
            this.textBox2.Text = text;
        }



        StringBuilder builder1 = new StringBuilder();
        //在接收到了ReceivedBytesThreshold设置的字符个数或接收到了文件结束字符并将其放入了输入缓冲区时被触发
        public void sp1_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("接收中...");
            int n = sp1.BytesToRead;      //先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
            byte[] buf = new byte[n];   //声明一个临时数组存储当前来的串口数据
            sp1.Read(buf, 0, n);      //读取缓冲数据
            builder1.Remove(0, builder1.Length); //清除字符串构造器的内容
            builder1.Append(Encoding.ASCII.GetString(buf));
            string comdata = builder1.ToString();
            Console.WriteLine("data: + " + comdata);

          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!sp1.IsOpen)
            {
                try
                {
                    //串口号
                    sp1.PortName = "COM1";
                    //波特率
                    sp1.BaudRate = 384000;
                    //数据位
                    sp1.DataBits = 8;
                    //停止位
                    sp1.StopBits = StopBits.One;
                    //奇偶校验位
                    sp1.Parity = Parity.None;
                    //DataReceived事件发送前，内部缓冲区里的字符数
                    sp1.ReceivedBytesThreshold = 1;
                    sp1.RtsEnable = true; sp1.DtrEnable = true; sp1.ReadTimeout = 3000;
                    // Control.CheckForIllegalCrossThreadCalls = false;
                    //表示将处理 System.IO.Ports.SerialPort 对象的数据接收事件的方法。
                    sp1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(sp1_DataReceived_1);
                    //打开串口
                    sp1.Open();
                    MessageBox.Show("COM1打开成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("COM1打开失败！");
                }
            }
            else
            {
                MessageBox.Show("COM1打开成功！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sp1.IsOpen)
            {
                sp1.Close();
                MessageBox.Show("COM1关闭成功！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!sp1.IsOpen)
            {
                try
                {
                    //串口号
                    sp1.PortName = "COM2";
                    //波特率
                    sp1.BaudRate = 384000;
                    //数据位
                    sp1.DataBits = 8;
                    //停止位
                    sp1.StopBits = StopBits.One;
                    //奇偶校验位
                    sp1.Parity = Parity.None;
                    sp1.ReceivedBytesThreshold = 1;
                    sp1.RtsEnable = true; sp1.DtrEnable = true; sp1.ReadTimeout = 3000;
                    //Control.CheckForIllegalCrossThreadCalls = false;
                    //表示将处理 System.IO.Ports.SerialPort 对象的数据接收事件的方法。
                    sp1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(sp1_DataReceived_1);
                    //打开串口
                    sp1.Open();
                    MessageBox.Show("COM2打开成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("COM2打开失败！");
                }
            }
            else
            {
                MessageBox.Show("COM2打开成功！");
            }
        }


        /// <summary>
        /// 获取所有串口
        /// </summary>
        private void ReadCK()
        {
            List<string> list = new List<string>();
            //先清空ComboBox，再进行获取
            cmbCk.Items.Clear();
            //获取本地配置文件(注册表文件)        
            Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
            //读取HARDWARE节点的键值
            Microsoft.Win32.RegistryKey software11 = hklm.OpenSubKey("HARDWARE");
            //打开"HARDWARE"子键
            Microsoft.Win32.RegistryKey software = software11.OpenSubKey("DEVICEMAP");
            Microsoft.Win32.RegistryKey sitekey = software.OpenSubKey("SERIALCOMM");
            //获取当前子键
            string[] Str2 = sitekey.GetValueNames();
            //获得当前子键存在的键值
            int i;
            for (i = 0; i < Str2.Count(); i++)
            {
                if (!Str2[i].Contains("BthModem"))
                {//排除蓝牙的COM口（BthModem）

                    string str = (string)sitekey.GetValue(Str2[i]);
                    list.Add(str.Replace("COM", ""));
                }
            }

            cmbCk.ItemsSource = list;
            this.cmbCk.Text = (string)cmbCk.Items[0];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sp1.IsOpen)
            {
                sp1.Close();
                MessageBox.Show("COM2关闭成功！");
            }
        }

    }
   
}
