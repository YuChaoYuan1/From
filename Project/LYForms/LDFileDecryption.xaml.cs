using LYForms.Models;
using LYForms.Models.LogCs;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml;

namespace LYForms
{
    /// <summary>
    /// LDFileDecryption.xaml 的交互逻辑
    /// </summary>
    public partial class LDFileDecryption : Window
    {
        ElectricityMeter electricityMeter = new ElectricityMeter();
        public string pathFile = "D:\\";

        List<string> list = new List<string>();
        public LDFileDecryption()
        {
            InitializeComponent();
            txt.Text= pathFile;
        }

        
        /// <summary>
        /// 单个文件解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                string strpath = pathFile + upload_name + ".ini";
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

        /// <summary>
        /// 选择文件(一个或多个文件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = true;
            //查找需要上传的文件
            if (openFileDialog.ShowDialog() == true)
            {
                //判断选择文件个数
                // 获取选中的文件名称
                for(int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    string filePath = openFileDialog.FileNames[i];
                    if (!list.Contains(filePath))
                    {
                        list.Add(filePath);
                    }
                    else
                    {
                        MessageBoxX.Show("添加文件重复:" + filePath, "文件未添加");
                    }
                }
            }

            foreach(string item in list)
            {
                tvw.Items.Add(item);
            }
            
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// 选择解密文件存放位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();

            txt.Text = folderBrowserDialog.SelectedPath;
            pathFile = txt.Text;
        }

        /// <summary>
        /// 文件解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                electricityMeter.MessageAdd("解密文件"+i+":"+list[i] , EnumLogType.流程信息, true);
                Task task = new Task(() => { ReadIDData(i); });
                task.Start();
                Thread.Sleep(500); //不加这个会出现i超过 list.Count 的情况（不知道是什莫原因）
            }
        }

        /// <summary>
        /// 多线程解密文件，并提示用户
        /// </summary>
        /// <param name="j"></param>
        private void ReadIDData(int j)
        {
            // 获取选中的文件名称
            string filePath = list[j];
            //  string extension = System.IO.Path.GetExtension(filePath);// 扩展名
            string upload_name = System.IO.Path.GetFileName(filePath);
            //获取绝对路径格式并转化
            string load = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", "/").ToString();
            string dir = load;

            string savePath = dir + upload_name; //保存文件夹带文件名

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            byte[] fileArray = r.ReadBytes((int)fs.Length);

            fs.Dispose();

            DataSet myds = new DataSet();
            Byte[] Files = (Byte[])fileArray;
            string strpath = pathFile + upload_name + ".ini";
            BinaryWriter bw = new BinaryWriter(File.Open(strpath, FileMode.OpenOrCreate));
            bw.Write(Files);
            bw.Close();

            //加下面的代码调用UI线程
            Dispatcher.BeginInvoke(new Action(delegate
            {
                MessageBoxX.Show("文件转换完成存放位置:" + strpath, "文件转换成功");
            })); 
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.Description = "选择文件所在文件夹目录";  //提示的文字
            folderBrowserDialog.ShowDialog();
            tvww.Items.Add(folderBrowserDialog.SelectedPath);
            File_Copy(folderBrowserDialog.SelectedPath);
        }

        private void File_Copy(string Sdir)
        {
            DirectoryInfo dir = new DirectoryInfo(Sdir);
            try
            {
                //判断所指的文件夹/文件是否存在  
                if (!dir.Exists)
                    return;
                DirectoryInfo dirD = dir as DirectoryInfo;
                FileSystemInfo[] files = dirD.GetFileSystemInfos();//获取文件夹下所有文件和文件夹  
                //对单个FileSystemInfo进行判断，如果是文件夹则进行递归操作  
                foreach (FileSystemInfo FSys in files)
                {
                    FileInfo fileInfo = FSys as FileInfo;

                    if (fileInfo != null)
                    {
                        //如果是文件，进行文件操作  
                        FileInfo SFInfo = new FileInfo(fileInfo.DirectoryName + "\\" + fileInfo.Name);
                    }
                    else
                    {
                        //如果是文件夹，则进行递归调用 
                        string pp = FSys.Name;
                        File_Copy(Sdir + "\\" + FSys.ToString());

                        NodesAdd(Sdir, pp);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HusbandNode"> 父节点</param>
        /// <param name="Nodes">当前添加的节点</param>
        private void NodesAdd(string HusbandNode ,string Nodes)
        {
            //(tvww.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem).Items.Add(Nodes);

            string extension = System.IO.Path.GetExtension(tvww.ItemContainerGenerator.Items[0].ToString());// 扩展名
            if ( tvww.ItemContainerGenerator.Items[0].ToString() == HusbandNode)
            {
                (tvww.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem).Items.Add(Nodes);
            }
        }
    }
}
