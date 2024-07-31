/*----------------------------------------------------------------------
// 
// 文件名： PassBarPage
// 功能： 进度条
// 作者： YuanYuChao
// 时间： 2023-10-31
// 版本： 1.0
// 
// 修改人：
// 时间：
// 说明：
----------------------------------------------------------------------*/
using LYForms.Common;
using LYForms.ViewModel;
using Panuon.UI.Silver;
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
    /// PassBarPage.xaml 的交互逻辑
    /// </summary>
    public partial class PassBarPage : UserControl
    {
        public PassBarPage()
        {
            InitializeComponent();

            PageAndControl pageAndControl = new PageAndControl();
            this.DataContext = pageAndControl;
        }
    }
}
