using log4net;
using LYForms.Common;
using LYForms.Models;
using LYForms.Views;
using LYForms.Views.PassBarPages;
using Microsoft.Office.Interop.Word;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LYForms.ViewModel
{
    public class PageAndControl : ElectricityMeter
    {

        public RelayCommand GetLogin => new RelayCommand((arg) =>
        {
      
        });

        public RelayCommand GetWindows => new RelayCommand((arg) =>
        {
            Login login = new Login();
            login.Show();
        });


        public RelayCommand GetPar => new RelayCommand((arg)=>
        {
            pargressBar1 login = new pargressBar1("");
            login.Show();
        });
  

    }
}
