using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
    /// FileOperate.xaml 的交互逻辑
    /// </summary>
    public partial class FileOperate : UserControl
    {
        public FileOperate()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //查找需要上传的文件
            if (openFileDialog.ShowDialog() == true)
            {
                // 获取选中的文件名称
                string filePath = openFileDialog.FileName;
                //  string extension = System.IO.Path.GetExtension(filePath);// 扩展名
                string upload_name = System.IO.Path.GetFileName(filePath);
                //获取绝对路径格式并转化
                string load = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", "/").ToString();
                string dir = load;

                dir = "D://";
                string savePath = dir + upload_name; //保存文件夹带文件名

                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);
                byte[] fileArray = r.ReadBytes((int)fs.Length);

                fs.Dispose();

                DataSet myds = new DataSet();
                Byte[] Files = (Byte[])fileArray;
                string strpath = "D:\\" + upload_name + ".doc";
                //string strpath = "D:\\" + upload_name;
                //if (System.IO.File.Exists(System.IO.Path.GetFullPath(strpath)))
                //{
                //    File.Delete(System.IO.Path.GetFullPath(strpath));
                //}
                BinaryWriter bw = new BinaryWriter(File.Open(strpath, FileMode.OpenOrCreate));
                bw.Write(Files);
                bw.Close();
                MessageBoxX.Show("文件转换完成存放位置:" + strpath, "文件转换成功");
            }
        }
    }
}
