/*----------------------------------------------------------------------
// 
// 文件名： MainWindowViewModel
// 功能： 主界面实现方法
// 作者： YuanYuChao
// 时间： 2023-10-31
// 版本： 1.0
// 
// 修改人：
// 时间：
// 说明：
----------------------------------------------------------------------*/
using log4net;
using LYForms.Common;
using LYForms.Models;
using LYForms.Models.LogCs;
using LYForms.Views;
using Panuon.UI.Silver;
using Panuon.UI.Silver.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LYForms.ViewModel
{
    public class MainWindowViewModel : ElectricityMeter
    {

    
        public StackPanel StackPanelMain
        {
            get;
            set;
        }
        public MainWindowViewModel()
        {
            _ = TbPageCommand;
        }



        public RelayCommand TbPageCommand => new RelayCommand((arg) =>
        {
            try
            {
                string context = arg as string;
                Type t = Type.GetType(context);
                t = System.Type.GetType(context);
                MessageAdd("打开界面"+ context, EnumLogType.流程信息, true);
                if (t != null)
                {
                    this.StackPanelMain.Children.Clear();
                    StackPanelMain.Children.Add((System.Windows.UIElement)Activator.CreateInstance(t));
                }
                MessageAdd("打开界面" + context, EnumLogType.流程信息, true);
            }
            catch (Exception ex)
            {
                MessageAdd(ex.ToString(), EnumLogType.错误信息, true);
                MessageBoxTooltip("打开界面" + arg + "错误","失败" ,MessageBoxIcon.Error);
            }
        });
        public RelayCommand Close => new RelayCommand((arg) =>
        {
            try
            {
                MessageAdd(DateTime.Now.ToString()+"退出程序", EnumLogType.错误信息, true);
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageAdd(ex.ToString(), EnumLogType.错误信息, true);
                MessageBoxTooltip("打开界面" + arg + "错误", "失败", MessageBoxIcon.Error);
            }
        });
        public RelayCommand DataShow => new RelayCommand((arg) =>
        {
            try
            {
                MessageAdd("打开数据操作界面", EnumLogType.流程信息, true);
                FileOperate fileOperate = new FileOperate();
                fileOperate.Show();
            }
            catch (Exception ex)
            {
               
            }
        });
    }
}
