using LYForms.Models;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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

namespace LYForms
{
    /// <summary>
    /// FileOperate.xaml 的交互逻辑
    /// </summary>
    public partial class FileOperate : Window
    {

        private int dataQuantity = 0;  //当前第一个是多少条数据
        List<int> list = new List<int>() { 20, 30, 40, 50, 60 };

        DataTable ds = new DataTable();
        public FileOperate()
        {
            InitializeComponent();
            this.Topmost = false;
            this.Topmost = true;

            for (int i = 0; i < 10; i++)
            {
                ds.Columns.Add("第" + i + "列");

            }
            for (int i = 0; i < 100000; i++)
            {
                ds.Rows.Add("第" + i + "行");
            }
            //加载配置文件

            Cmb.ItemsSource = list;
            Cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnreaddata_Click(object sender, RoutedEventArgs e)
        {
            //DataTable ds = Import(OpenExcelDialog());
            //获取每页显示数据
        }
        private void Cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int j = int.Parse(Cmb.SelectedValue.ToString());
            DataTable dtLast = ds.AsEnumerable().Take(j).CopyToDataTable();
            dtLast = dtLast.DefaultView.ToTable();
            DataView dv = new DataView(dtLast);
            dgvDataShow.ItemsSource = dv;
        }


        #region 读取数据 因为文件加密所以出现报错

        private DataTable Import(string filepath)
        {
            DataTable ds = new DataTable();
            if (filepath.Contains(".xlsx") || filepath.Contains(".xls"))
            {
                ds = ExcelToDatatable(filepath, "", true);

            }
            return ds;
        }
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <returns></returns>
        private string OpenExcelDialog()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files|*.xlsx;*.xls"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
                MessageBox.Show("请选择数据文件");
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名，true是</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDatatable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            NPOI.SS.UserModel.ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            FileStream fs;
            NPOI.SS.UserModel.IWorkbook workbook = null;
            int cellCount = 0;//列数
            int rowCount = 0;//行数
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                {
                    workbook = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
                }
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                {
                    workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
                }
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);//根据给定的sheet名称获取数据
                }
                else
                {
                    //也可以根据sheet编号来获取数据
                    sheet = workbook.GetSheetAt(0);//获取第几个sheet表（此处表示如果没有给定sheet名称，默认是第一个sheet表）  
                }
                if (sheet != null)
                {
                    NPOI.SS.UserModel.IRow firstRow = sheet.GetRow(0);
                    cellCount = firstRow.LastCellNum; //第一行最后一个cell的编号 即总的列数
                    if (isFirstRowColumn)//如果第一行是标题行
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)//第一行列数循环
                        {
                            DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);//获取标题
                            data.Columns.Add(column);//添加列
                        }
                        startRow = sheet.FirstRowNum + 1;//1（即第二行，第一行0从开始）
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;//0
                    }
                    //最后一行的标号
                    rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)//循环遍历所有行
                    {
                        NPOI.SS.UserModel.IRow row = sheet.GetRow(i);//第几行
                        if (row == null)
                        {
                            continue; //没有数据的行默认是null;
                        }
                        //将excel表每一行的数据添加到datatable的行中
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
                return null;
            }
        }


        #endregion

        /// <summary>
        /// 下一页数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int j= int.Parse(Cmb.SelectedValue.ToString());
            dataQuantity = dataQuantity + j;
            DataTable dtLast = ds.AsEnumerable().Take(j).CopyToDataTable();
            dtLast = dtLast.DefaultView.ToTable();
            DataView dv = new DataView(dtLast);
            dgvDataShow.ItemsSource = dv;
        }

        /// <summary>
        /// 下一页 判断是否有下一页。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int j = int.Parse(Cmb.SelectedValue.ToString());
            int i= int.Parse(lbl.Content.ToString());
            if (ds.Rows.Count > (i-1) * j)
            {
                DataTable NewTable = ds.Clone();
                DataRow[] rows = ds.Select("1=1");
        
                for (int row = j * (i); row < ds.Rows.Count; row++)
                {
                    if (row > j * (i + 1))
                    {
                        break;
                    }
                    NewTable.ImportRow((DataRow)rows[row]);
                }
                DataView dv = new DataView(NewTable);
                dgvDataShow.ItemsSource = dv;
            }
            else
            {
                return;
            }
        }
    }
}
