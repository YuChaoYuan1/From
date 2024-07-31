using Microsoft.Office.Interop.Word;
using System;
using System.Collections.ObjectModel;
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
        /// 报文解析  现只解析APDU的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

          
            //if (!string.IsNullOrEmpty(txtBW.Text))
            //{
            //    string str = txtBW.Text.Replace(" ", "");
            //    string str1 = str.Replace("-", "");
            //    string result = Regex.Replace(str1, @".{2}", "$0 ").Trim();
            //    txtBW.Text = result;
            //    //APDU 格式
            //    //get    占用两个数字   05  01  
            //    //PIID   占用一个数字       01  服务优先级
            //    //OAD

            //  //  07 01 0c 60 14 7f 00 01 01 02 06 11 01 12 01 00 02 02 11 00 00 01 0a 5b 00 00 10 02 00 5b 00 00 10 02 00 5b 00 00 10 02 00 5b 00 00 10 02 00 5b 00 00 10 02 00 5b 00 00 10 02 00 5b 00 00 10 02 00 5b 00 20 00 02 00 5b 00 20 00 02 00 5b 00 20 01 02 00 5c 03 01 07 05 09 00 00 00 00 01 16 01 00


            //}
        }
    }
}
