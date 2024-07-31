
/*----------------------------------------------------------------------
// 
// 文件名： DataShow
// 功能： 数据处理
// 作者： YuanYuChao
// 时间： 2024-7-17
// 版本： 1.0
// 
// 修改人：
// 时间：对数据的一系列操作
// 说明：
----------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace LYForms.Views
{

   
    /// <summary>
    /// DataShow.xaml 的交互逻辑
    /// </summary>
    public partial class DataShow : UserControl
    {
        private int dataQuantity = 0;  //每页显示多少数据
        List<int> list = new List<int>() { 20, 30, 40, 50, 60 };
        public DataShow()
        {
            InitializeComponent();
            //加载配置文件
            //Cmb.ItemsSource = list;
            //Cmb.SelectedIndex = 0;
        }

    }
}
