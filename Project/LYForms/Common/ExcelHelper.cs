/*----------------------------------------------------------------------
// 
// 文件名： ExcelHelper
// 功能： Excel操作
// 作者： YuanYuChao
// 时间： 2024-3-12
// 版本： 1.0
// 
// 修改人：
// 时间：
// 说明：
----------------------------------------------------------------------*/
using Aspose.Cells;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Panuon.UI.Silver;
using Panuon.UI.Silver.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using form = System.Windows.Forms;

namespace LYForms.Common
{
    public class ExcelHelper
    {
        #region 将List导出至Excel  导出一个类型的数据。都在一张表上。
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list">集合</param>
        /// <param name="_sheetName"></param>
        /// <param name="_colums">列和标题对应</param>
        /// <param name="_hiddleColumns">隐藏列名</param>
        public async void List2ExcelAsync<T>(List<T> _list, string _sheetName = "导出文件_Zyue", Dictionary<string, string> _colums = null, List<string> _hiddleColumns = null)
        {
            if (_colums == null || _colums.Count == 0)
            {
                MessageBoxX.Show("没有显示列数据", "空值提醒");
                return;
            }


            string _savePath = Directory.GetCurrentDirectory() + "\\log" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".xml";
            var folderBrowser = new form.FolderBrowserDialog();//Winform dll中调用选择文件夹
                                                               //var folderBrowser = new form.FolderBrowserDialog();//Winform dll中调用选择文件夹
            if (folderBrowser.ShowDialog() == form.DialogResult.OK)
            {
                _savePath = folderBrowser.SelectedPath;//选中的文件夹路径
            }
            else
            {
                MessageBox.Show("已取消");
            }

            string excelFullPath = $"{_savePath}\\[{_sheetName}]{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";//导出文件路径

            //Panuon.UI.Silver消息框
            var _handler = PendingBox.Show("正在导出Excel...", "请等待", false, null, new PendingBoxConfigurations()
            {
                LoadingForeground = "#5DBBEC".ToColor().ToBrush(),
                ButtonBrush = "#5DBBEC".ToColor().ToBrush(),
            });


            await Task.Run(() =>
            {
                IWorkbook wk = null;
                wk = new XSSFWorkbook();//创建一个工作簿

                var sheet = wk.CreateSheet(_sheetName);//创建Sheet

                if (_list != null && _list.Count > 0)
                {
                    #region 添加第一行标题数据

                    int propertiesCount = 0;//属性

                    foreach (var key in _colums.Keys)
                    {
                        //属性名称
                        string name = key;
                        string title = _colums[key];

                        if (_hiddleColumns != null)
                        {
                            if (_hiddleColumns.Any(c => c == name))
                            {
                                continue;
                            }
                        }

                        var _row = propertiesCount == 0 ? sheet.CreateRow(0) : sheet.GetRow(0);//创建或获取第一行

                        var _headerCell = _row.CreateCell(propertiesCount);//创建列
                        _headerCell.SetCellValue(name);//设置列值
                        propertiesCount++;
                    }

                    #endregion

                    var _headerRow = sheet.GetRow(0);//获取第一行

                    #region 添加数据

                    for (int i = 0; i < _list.Count; i++)
                    {
                        var _item = _list[i];

                        var _currRow = sheet.CreateRow(1 + i);//创建当前数据的行
                        for (int j = 0; j < propertiesCount; j++)
                        {
                            string _propertityName = _headerRow.GetCell(j).StringCellValue;//获取属性名称
                            if (_hiddleColumns != null)
                            {
                                if (_hiddleColumns.Any(c => c == _propertityName))
                                {
                                    continue;
                                }
                            }
                            string _propertityValue = _item.GetType().GetProperty(_propertityName).GetValue(_item, null).ToString();//获取属性值

                            _currRow.CreateCell(j).SetCellValue(_propertityValue);//设置单元格数据
                        }
                    }

                    #endregion

                    #region 更新列标题

                    for (int j = 0; j < propertiesCount; j++)
                    {
                        string _propertityName = _headerRow.GetCell(j).StringCellValue;//获取属性名称
                        if (_colums != null && _colums.ContainsKey(_propertityName)) _headerRow.CreateCell(j).SetCellValue(_colums[_propertityName]);//更新标题
                    }

                    #endregion 
                }
                else
                {
                    UIGlobal.RunUIAction(() =>  //其它县城内调用UI主线程方法
                    {
                        MessageBox.Show("没有导出数据");
                        _handler.Close();
                    });
                    return;
                }

                try
                {
                    using (FileStream fs = new FileStream(excelFullPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        wk.Write(fs);//保存文件
                    }
                }
                catch (Exception ex)
                {
                    UIGlobal.RunUIAction(() =>
                    {
                        MessageBoxX.Show($"导出失败, [{ex.Message}]", "文件占用提示");
                        _handler.Close();
                    });
                }
            });

            Notice.Show(excelFullPath, "Excel导出成功", 3, MessageBoxIcon.Success);
            _handler.Close();
        }
        #endregion

        #region  导出多个文件，一个文件内表的列也有所不同

        public void ClockIn(List<object> list)
        {
            //导出数据
            if (list.Count > 0)
            {
                Aspose.Cells.Workbook wbook = new Aspose.Cells.Workbook();
                Aspose.Cells.Worksheet sheet = wbook.Worksheets[0];
                GetDataTabelS(list);
            }
        }

        private void GetDataTabelS(List<object> list)
        {
            //for (int count = 0; count < list.Count; count++)
            //{
            //    DataTable[] dataTables = new DataTable[6];
            //    DataTable dataTable = new DataTable();
            //    bool 误差flag = false;
            //    bool 走字flag = false;
            //    //MT_METER_DATA model = list[count] as MT_METER_DATA;
            //    //var md = new LYTest.ViewModel.HistoricalData.MT_METER_DATA(model.MD_ADDRESS, model.TIME);

            //    #region 表信息
            //    string[] str = { "表条码", "表地址", "时间", "波特率", "协议" };
            //    for (int i = 0; i < str.Length; i++)
            //    {
            //        dataTable.Columns.Add(str[i].ToString().Trim());//构建表头 
            //    }

            //    DataRow row2 = dataTable.NewRow();
            //    row2[0] = md.表信息[0].MD_BAR;
            //    row2[1] = md.表信息[0].MD_ADDRESS;
            //    row2[2] = md.表信息[0].TIME;
            //    row2[3] = md.表信息[0].BTL;
            //    row2[4] = md.表信息[0].MD_PROTOCOL;
            //    dataTable.Rows.Add(row2);
            //    dataTables[0] = dataTable;
            //    #endregion

            //    #region 电压电流
            //    string[] str1 = { " ", "A相", "B相", "C相" };
            //    dataTable = new DataTable();
            //    for (int i = 0; i < str1.Length; i++)
            //    {
            //        dataTable.Columns.Add(str1[i].ToString());//构建表头 
            //    }
            //    for (int count1 = 0; count1 < md.电压电流.Count; count1++)
            //    {
            //        row2 = dataTable.NewRow();
            //        row2[0] = md.电压电流[count1].Data1;
            //        row2[1] = md.电压电流[count1].Data2;
            //        row2[2] = md.电压电流[count1].Data3;
            //        row2[3] = md.电压电流[count1].Data4;
            //        dataTable.Rows.Add(row2);
            //    }
            //    dataTables[1] = dataTable;
            //    #endregion

            //    #region 功率
            //    string[] str2 = { " ", "有功功率", "无功功率", "视在功率" };
            //    dataTable = new DataTable();
            //    for (int i = 0; i < str2.Length; i++)
            //    {
            //        dataTable.Columns.Add(str2[i].ToString());//构建表头 
            //    }
            //    for (int count1 = 0; count1 < md.功率.Count; count1++)
            //    {
            //        row2 = dataTable.NewRow();
            //        row2[0] = md.功率[count1].Data1;
            //        row2[1] = md.功率[count1].Data2;
            //        row2[2] = md.功率[count1].Data3;
            //        row2[3] = md.功率[count1].Data4;
            //        dataTable.Rows.Add(row2);
            //    }
            //    dataTables[2] = dataTable;
            //    #endregion

            //    #region 尖峰平谷信息
            //    string[] str3 = { " ", "总(kWh)", "尖(kWh)", "峰(kWh)", "平(kWh)", "谷(kWh)" };
            //    dataTable = new DataTable();
            //    for (int i = 0; i < str3.Length; i++)
            //    {
            //        dataTable.Columns.Add(str3[i].ToString());//构建表头 
            //    }
            //    for (int count1 = 0; count1 < md.尖峰.Count; count1++)
            //    {
            //        row2 = dataTable.NewRow();
            //        row2[0] = md.尖峰[count1].Data1;
            //        row2[1] = md.尖峰[count1].Data2;
            //        row2[2] = md.尖峰[count1].Data3;
            //        row2[3] = md.尖峰[count1].Data4;
            //        row2[4] = md.尖峰[count1].Data5;
            //        row2[5] = md.尖峰[count1].Data6;
            //        dataTable.Rows.Add(row2);
            //    }
            //    dataTables[3] = dataTable;
            //    #endregion

            //    #region 误差
            //    if (md.误差.Count > 0)
            //    {
            //        误差flag = true;
            //    }
            //    string[] str4 = { " ", "常数", "圈数", "误差1", "误差2", "误差3", "误差4" };
            //    if (误差flag)
            //    {
            //        dataTable = new DataTable();
            //        for (int i = 0; i < str4.Length; i++)
            //        {
            //            dataTable.Columns.Add(str4[i].ToString());//构建表头 
            //        }
            //        for (int count1 = 0; count1 < md.误差.Count; count1++)
            //        {
            //            row2 = dataTable.NewRow();
            //            row2[0] = md.误差[count1].Data1;
            //            row2[1] = md.误差[count1].Data2;
            //            row2[2] = md.误差[count1].Data3;
            //            row2[3] = md.误差[count1].Data4;
            //            row2[4] = md.误差[count1].Data5;
            //            row2[5] = md.误差[count1].Data6;
            //            row2[6] = md.误差[count1].Data7;
            //            dataTable.Rows.Add(row2);
            //        }
            //        dataTables[4] = dataTable;
            //    }
            //    #endregion

            //    #region 走字
            //    if (md.走字.Count > 0)
            //    {
            //        走字flag = true;
            //    }
            //    string[] str5 = { " ", "标准表_起始电量", "标准表_当前电量", "标准表_电量差值", "被检表_起始电量", "被检表_当前电量", "被检表_电量差值", "走电量", "误差" };
            //    if (走字flag)
            //    {
            //        dataTable = new DataTable();
            //        for (int i = 0; i < str5.Length; i++)
            //        {
            //            dataTable.Columns.Add(str5[i].ToString());//构建表头 
            //        }
            //        for (int count1 = 0; count1 < md.走字.Count; count1++)
            //        {
            //            row2 = dataTable.NewRow();
            //            row2[0] = md.走字[count1].Data1;
            //            row2[1] = md.走字[count1].Data2;
            //            row2[2] = md.走字[count1].Data3;
            //            row2[3] = md.走字[count1].Data4;
            //            row2[4] = md.走字[count1].Data5;
            //            row2[5] = md.走字[count1].Data6;
            //            row2[6] = md.走字[count1].Data7;
            //            row2[7] = md.走字[count1].Data8;
            //            row2[8] = md.走字[count1].Data9;
            //            dataTable.Rows.Add(row2);
            //        }
            //        dataTables[4] = dataTable;
            //    }
            //    #endregion

            //    Save_Excel(dataTables);
            //}
            //MessageBox.Show($"文件路径:【{System.IO.Directory.GetCurrentDirectory() + @"\Res\Excel\"}】保存成功");
        }

        private void Save_Excel(DataTable[] dataTables)
        {
            /* DataTable[] dataTables = new DataTable();  */ //获取到这个表的所有项目的datatable

            Aspose.Cells.Workbook wbook = new Aspose.Cells.Workbook();
            Aspose.Cells.Worksheet sheet = wbook.Worksheets[0];
            sheet.Name = "数据";
            int row = 0;
            int col = 0;
            Aspose.Cells.Style style = wbook.Styles[wbook.Styles.Add()];
            Aspose.Cells.Style style2 = wbook.Styles[wbook.Styles.Add()];
            style.ForegroundColor = System.Drawing.Color.FromArgb(31, 78, 120);
            style.Pattern = Aspose.Cells.BackgroundType.Solid;
            style.Font.IsBold = true;
            style.Font.Color = System.Drawing.Color.FromArgb(255, 255, 255);
            style.Font.Name = "宋体";//文字字体 
            style.Font.Size = 15;//文字大小
            style.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //左边框 
            style.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = CellBorderType.Thin; //右边框  
            style.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = CellBorderType.Thin; //上边框  
            style.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //下边框
            //style = sheet.Cells["A1"].GetStyle();


            style2.Pattern = Aspose.Cells.BackgroundType.Solid;
            style2.Font.IsBold = false;
            style2.Font.Name = "宋体";//文字字体 
            style2.Font.Size = 15;//文字大小
            style2.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //左边框 
            style2.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = CellBorderType.Thin; //右边框  
            style2.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = CellBorderType.Thin; //上边框  
            style2.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //下边框
            //style2 = sheet.Cells["A1"].GetStyle();

            int Start_A = 0;
            int Start_B = 0;
            int End_A = 0;
            int End_B = 0;
            //cells.GetColumnWidthPixel
            for (int t = 0; t < dataTables.Length; t++)
            {
                if (dataTables[t] == null)
                {
                    break;
                }
                DataTable table = dataTables[t];
                Start_A = row;
                Start_B = col;

                for (int i = 0; i < table.Columns.Count; i++) //列名
                {
                    sheet.Cells[row, col].Value = table.Columns[i].ColumnName.ToString().Trim();
                    sheet.Cells[row, col].PutValue(table.Columns[i].ColumnName.ToString().Trim());
                    sheet.Cells[row, col].SetStyle(style);
                    //sheet.Cells[row, col].
                    col++;
                }
                End_B = col;
                col = 0;
                row++;
                int index = 1;
                End_A = 1;
                foreach (DataRow Row in table.Rows)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        sheet.Cells[row, col].Value = Row[i].ToString().Trim();
                        sheet.Cells[row, col].PutValue(Row[i].ToString().Trim());
                        if (index % 2 == 0)
                        {
                            style2.ForegroundColor = System.Drawing.Color.FromArgb(254, 255, 255);
                        }
                        else
                        {
                            style2.ForegroundColor = System.Drawing.Color.FromArgb(216, 230, 243);
                        }
                        sheet.Cells[row, col].SetStyle(style2);
                        //System.Threading.Thread.Sleep(2);
                        col++;
                    }
                    col = 0;
                    row++;
                    index++;
                    End_A++; ;
                }
                var range = sheet.Cells.CreateRange(Start_A, Start_B, End_A, End_B);
                range.SetOutlineBorder(Aspose.Cells.BorderType.TopBorder, Aspose.Cells.CellBorderType.Thick, System.Drawing.Color.Blue);
                range.SetOutlineBorder(Aspose.Cells.BorderType.BottomBorder, Aspose.Cells.CellBorderType.Thick, System.Drawing.Color.Blue);
                range.SetOutlineBorder(Aspose.Cells.BorderType.LeftBorder, Aspose.Cells.CellBorderType.Thick, System.Drawing.Color.Blue);
                range.SetOutlineBorder(Aspose.Cells.BorderType.RightBorder, Aspose.Cells.CellBorderType.Thick, System.Drawing.Color.Blue);
                row++;
            }
            sheet.AutoFitColumns();

            //限制最大列宽
            for (int i = 0; i < 10; i++)
            {
                //当前列宽
                int pixel = sheet.Cells.GetColumnWidthPixel(i);
                //设置最大列宽
                if (pixel > 200)
                {
                    sheet.Cells.SetColumnWidthPixel(i, 200);
                }
                else if (pixel < 80)
                    sheet.Cells.SetColumnWidthPixel(i, 100);
                else
                {
                    sheet.Cells.SetColumnWidthPixel(i, pixel);
                }
            }
            if (!System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Res\Excel"))
            {
                System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Res\Excel");
                System.Threading.Thread.Sleep(500);
            }
            dataTables[0].Rows[0].ItemArray[1].ToString();
            string file = System.IO.Directory.GetCurrentDirectory() + @"\Res\Excel\" + dataTables[0].Rows[0].ItemArray[1].ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            wbook.Save(file, Aspose.Cells.SaveFormat.Excel97To2003);
            System.Threading.Thread.Sleep(100);
            //MessageBox.Show($"文件路径:【{file}】保存成功");
            //System.Diagnostics.Process.Start(file); //打开excel文件
        }

        #endregion
    }

    public partial class UIGlobal
    {
        /// <summary>
        /// 运行UI线程
        /// </summary>
        /// <param name="action"></param>
        public static void RunUIAction(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
