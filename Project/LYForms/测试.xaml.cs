using Microsoft.Office.Interop.Word;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LYForms
{
    /// <summary>
    /// 测试.xaml 的交互逻辑
    /// </summary>
    public partial class 测试 : System.Windows.Window
    {
        public 测试()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 粘贴 并解析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = "68 17 ff 43 05 01 00 00 00 00 00 00 a7 e6 05 01 01 00 10 02 00 00 fe 19 16";


           str =   str.Replace(" ", "");

            //str 转Byte数组
            _698Parse(str);
        }

        private void _698Parse(string str)
        {
            string[] message = StringToString(str);
            FrameStructure frameStructure = new FrameStructure();


            frameStructure.APDU = new APDUData();
           

            #region 帧头
            frameStructure.Feameheader = new FeameheaderData();
            frameStructure.Feameheader.H68 = message[0];                    //起始符
            frameStructure.Feameheader.L = message[1] + message[2];         //长度域

            //获取报文长度 
            
            string L = LenghtData(message[2] , message[1]);

            frameStructure.Feameheader.C = message[3] ;                     //控制域

            frameStructure.Feameheader.A  = new AddRess();

            frameStructure.Feameheader.A.SA = "";                           //服务器地址
            frameStructure.Feameheader.A.CA = "";                           //客户机地址
            frameStructure.Feameheader.HCS  ="";                            //帧头校验
            #endregion

            #region 帧尾
            frameStructure.FeameEnd = new FeameEndData();

            int len = message.Length;
            //frameStructure.FeameEnd.FCS = message[len - 3] + message[len - 2];          //帧校验
            //frameStructure.FeameEnd.H16 = message[message.Length-1];                    //结束符
            #endregion
        }

        private string LenghtData(string v,string v2)
        {
            string L = BinaryConversion(v, 16, 2);
            if (L.Length >6)
            {
                L = L.Substring(L.Length - 6);
            }
            string L2 = BinaryConversion(v2, 16, 2);
            string ll= BinaryConversion(L, 2, 10);
            return L;
        }

        #region 所用方法
        /// <summary>
        /// HEX字符串转字节数组,并反转
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        private byte[] StringToByte(string hexStr ,bool flag)
        {
            if (flag)
            {
                int len = hexStr.Length / 2;
                byte[] frame = new byte[len];
                for (int i = 0; i <len;i++)
                {
                    string bb = hexStr.Substring(i * 2, 2);
                    frame[i] = Convert.ToByte(bb, 16); ;
                }
                return frame;
            }
            else
            {
                int len = hexStr.Length / 2;
                byte[] frame = new byte[len];
                for (int i = len - 1; i >= 0; i--)
                {
                    string bb = hexStr.Substring(i * 2, 2);
                    frame[len - 1 - i] = Convert.ToByte(bb, 16); ;
                }
                return frame;
            }
           

        }

        /// <summary>
        /// HEX字符串转字节数组,并反转
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        private string[] StringToString(string hexStr)
        {

            int len = hexStr.Length / 2;
            string[] frame = new string[len];
            for (int i = 0; i < len; i++)
            {
                string bb = hexStr.Substring(i * 2, 2);
                frame[i] = bb;
            }
            return frame;
        }

        /// <summary>
        /// 进制转化
        /// </summary>
        /// <param name="str">要转的数据</param>
        /// <param name="i">被转数据的进制</param>
        /// <param name="toi">转化后的进制数</param>
        /// BinaryConversion(message[2] + message[1],16,10)  //把16进制的数转换成10进制
        private string BinaryConversion(string str ,int i,int toi)
        {
            int data= Convert.ToInt32(str, i); //先统一转成十进制
            return Convert.ToString(data, toi);
        }
        #endregion
    }



    /// <summary>
    /// 帧结构
    /// </summary>
    public class FrameStructure
    {
        public FeameheaderData Feameheader { get;set;}
        public APDUData APDU { get; set; }
        public FeameEndData FeameEnd { get; set; }
    }

    #region  帧头
    /// <summary>
    /// 帧头
    /// </summary>
    public class FeameheaderData 
    {
        //BIN 字节

        /// <summary>
        /// 起始符  68H   1BIN
        /// </summary>
        public string H68 { get; set; }

        /// <summary>
        /// 长度域 L      2BIN
        /// </summary>
        public string L { get; set; }

        /// <summary>
        /// 控制域 C      1BIN
        /// </summary>
        public string C { get; set; }

        /// <summary>
        /// 地址域 A      1BIN
        /// </summary>
        public AddRess A { get; set; }

        /// <summary>
        /// 帧头校验HCS   2BIN
        /// </summary>
        public string HCS { get; set; }
        
    }

    public class AddRess
    {
        /// <summary>
        /// 服务器地址 (字节长度可变)
        /// </summary>
        public string SA { get; set; }

        /// <summary>
        /// 客户机地址  1BIN
        /// </summary>
        public string CA { get; set; }
    }

    #endregion

    #region  APDU
    /// <summary>
    /// APDU
    /// </summary>
    public class APDUData
    {
        

    }
    #endregion

    #region  帧尾
    /// <summary>
    /// APDU
    /// </summary>
    public class FeameEndData 
    {
        
        /// <summary>
        /// 帧校验 2BIN
        /// </summary>
     
        public string FCS { get; set; }


        /// <summary>
        /// 结束符  1BIN
        /// </summary>
        public string H16 { get; set; }

    }
    #endregion
}
