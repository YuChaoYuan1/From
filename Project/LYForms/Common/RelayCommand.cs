/*----------------------------------------------------------------------
// 
// 文件名： RelayCommand
// 功能： 帮助类实现左侧菜单栏右侧界面
// 作者： YuanYuChao
// 时间： 2023-10-31
// 版本： 1.0
// 
// 修改人：
// 时间：
// 说明：
----------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LYForms.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> m_execute;
        private readonly Predicate<object> m_canExecute;

        public RelayCommand(Action<object> execute)
        {
            this.m_execute = execute;
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.m_execute = execute;
            this.m_canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (m_canExecute == null)
                return true;

            return m_canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            this.m_execute(parameter);
        }
    }
}
