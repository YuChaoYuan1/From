/*----------------------------------------------------------------------
// 
// 文件名： IndexView
// 功能： Xml文件的处理
// 作者： YuanYuChao
// 时间： 2023-10-31
// 版本： 1.0
// 
// 修改人：
// 时间：
// 说明：
----------------------------------------------------------------------*/
using LYForms.Common;
using LYForms.Models;
using Microsoft.Win32;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
using System.Xml;

namespace LYForms.Views
{
    /// <summary>
    /// IndexView.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class IndexView : UserControl
    {
        #region

        private int PageShuliang = 0;  //页数
        private int pageData = 1;      //当前显示第几页
        private int dataQuantity = 0;  //每页显示多少数据
        private int Datashuliang = 0; //显示数据的数量

        List<int> list = new List<int>() { 50, 100, 150 };

        #endregion
        DataTable dt = new DataTable();
        public IndexView()
        {
            InitializeComponent();
            Cmb.ItemsSource = list;
            Cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 读取Xml文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            //符合条件的
            string FilePath = OpenFileVoid();
            if (string.IsNullOrWhiteSpace(FilePath))
            {
                MessageBoxX.Show("请选择文件", "操作错误");
                return;
            }

            List<ElectricityMeter> list2 = new List<ElectricityMeter>();   //修改的
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);

            //XmlNode xn = doc.SelectSingleNode("DataFlagInfo");
            XmlNode xn = doc.LastChild;

            XmlNodeList xnl = xn.ChildNodes;

            int row = 0;
            //数量
         

            //第几页；

            //要修改的数据

            if (xnl.Count > dataQuantity)
            {
                //取整
                PageShuliang = xnl.Count / dataQuantity+1;


                XmlNode xmlNodeList = xnl.Item(0);
            }
            else
            {
                foreach (XmlNode item in xnl)
                {
                    for (int i = 0; i < item.Attributes.Count; i++)
                    {
                        if (!dt.Columns.Contains(item.Attributes[i].Name))
                        {
                            dt.Columns.Add(item.Attributes[i].Name);
                        }
                    }
                    int k = 0;
                    dt.Rows.Add();
                    dt.Rows[row][0] = row;
                    for (int i = 0; i < item.Attributes.Count; i++)
                    {
                        dt.Rows[row][i] = item.Attributes[i].Value;
                        if (item.Attributes[i].Name == "Key")
                        {
                            string strr = k.ToString().PadLeft(4, '0');
                            dt.Rows[row][i] = strr;
                        }
                    }
                    k++;
                    row++;
                }
            }
         

            DataView dv = new DataView(dt); 
            dgvshowxml.ItemsSource = dv;  

            //foreach (XmlNode item in xnl)
            //{
            //    ElectricityMeter _electricityMeter = new ElectricityMeter();

            //    foreach (XmlAttribute keyValue in item.Attributes)
            //    {
            //        string str = keyValue.Name;
            //        string value = keyValue.Value;
            //        //反射
            //        var property = _electricityMeter.GetType().GetProperty(str);
            //        if (property != null)
            //        {
            //            property.SetValue(_electricityMeter, value);
            //        }
            //        else
            //        {
            //            MessageBoxX.Show($"{str}+该字段无匹配项", "xml字段");
            //        }
            //    }
            //    list2.Add(_electricityMeter);
            //}

            if (list2.Count > 0)
            {
                dgvshowxml.ItemsSource = list2;
                MessageBoxX.Show($"{list2.Count}", "xml数据量");
            }
        }

        private string OpenFileVoid()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "  (.xml)|*.xml|Text documents (.txt)|*.txt|All files (*.*)|*.*"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 写入Xml文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            string path = ShowPath();

            WriteXml(dgvshowxml, path);
        }

        /// <summary>
        /// 获取文件保存的目录
        /// </summary>
        /// <returns></returns>
        private string ShowPath()
        {
            string path = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml files (*.xml)|*.xml|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = "C:"; //设置初始目录
            if ((bool)saveFileDialog.ShowDialog())
            {
                path=saveFileDialog.FileName; //获取选择的文件，或者自定义的文件名的全路径。
            }
            return path;
        }

        /// <summary>
        /// 写入Xml文件
        /// </summary>
        /// <param name="dgvshowxml"></param>
        /// <param name="path"></param>
        private void WriteXml(DataGrid dgvshowxml, string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmlDeclar;
            xmlDeclar = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclar);

            XmlElement xmlElement = xmlDoc.CreateElement("DataFlagInfo");
            xmlDoc.AppendChild(xmlElement);

            //添加节点
            XmlNode root = xmlDoc.SelectSingleNode("DataFlagInfo");

            //
            //xe1.SetAttribute("ISB", "2-3631-4");  
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                XmlElement xe1 = xmlDoc.CreateElement("R");
                for (int columns = 0; columns < dgvshowxml.Columns.Count; columns++)
                {
                    if (dgvshowxml.Columns[columns].Header.ToString() == "Default" | dgvshowxml.Columns[columns].Header.ToString() == "ReadData")
                    {
                        continue;
                    }
                    string str = (dgvshowxml.Columns[columns].GetCellContent(dgvshowxml.Items[row]) as TextBlock).Text;
                    xe1.SetAttribute(dgvshowxml.Columns[columns].Header.ToString(), str);
                    root.AppendChild(xe1);
                }
            }
            if (!File.Exists(path))
            {
                //创建文件
                try
                {
                    File.Create(path);
                }
                catch (Exception e)
                {
                }
            }
            xmlDoc.Save(path);//保存的路径
            MessageBoxX.Show("写入成功", "成功");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelHelper excelHelper = new ExcelHelper();

            List<ElectricityMeter> list = new  List<ElectricityMeter>();

            for (int i = 0; i < 9; i++)
            {
                ElectricityMeter electricityMeter1 = new ElectricityMeter() { 
                    Chip =i.ToString(),
                    SortNo = i+1.ToString(),
                    ClassName = i+2.ToString(),
                    DataFlagDi = i+3.ToString(),
                };
                list.Add(electricityMeter1);

            }
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("1", "2");
            keyValuePairs.Add("2", "3");
            List<string> str = new List<string>();
            excelHelper.List2ExcelAsync(list, "导出文件", keyValuePairs, str);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "dat文件(*.dat)|*.dat|All files(*.*)|*.*";
            string filePath = ofd.FileName;
            string fileName = ofd.SafeFileName;

            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "dat文件(*.dat)|*.dat|All files(*.*)|*.*"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                filePath= openFileDialog.FileName;
                fileName= openFileDialog.SafeFileName;
            }
            else
            {
            
            }
            if (string.IsNullOrWhiteSpace(ofd.Filter))
            {
                MessageBoxX.Show("请选择文件", "操作错误");
                return;
            }
            //使用“打开”对话框中选择的文件名实例化FileStream对象
            FileStream myStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //使用FileStream对象实例化BinaryReader二进制写入流对象
            BinaryReader myReader = new BinaryReader(myStream);
        }
    }
}
