/*----------------------------------------------------------------------
// 
// 文件名： ElectricityMeter
// 功能： wpfuser的类库，存放电表的属性
// 作者： YuanYuChao
// 时间： 2023-10-31
// 版本： 1.0
// 
// 修改人：
// 时间：
// 说明：下拉框的---items--加两个参数的方法,并返回。
----------------------------------------------------------------------*/

using log4net;
using LYForms.Models.LogCs;
using Panuon.UI.Silver;
using Panuon.UI.Silver.Core;
using System;
using System.IO;

namespace LYForms.Models
{
    public class ElectricityMeter
    {
        #region
        /// <summary>
        /// 读写权限
        /// </summary>
        public string Rights
        {
            get;
            set;
        }

        /// <summary>
        /// 安全模式
        /// </summary>
        public string EmSecurityMode
        {
            get;
            set;
        }


        /// <summary>
        /// 序号
        /// </summary>
        public string SortNo
        {
            get;
            set;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public string Chip
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string ClassName
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public string DataFormat
        {
            get;
            set;
        }

        /// <summary>
        /// 字节长度
        /// </summary>
        public string DataSmallNumber
        {
            get;
            set;
        }

        /// <summary>
        ///数据长度
        /// </summary>
        public string DataLength
        {
            get;
            set;
        }
        /// <summary>
        ///698
        /// </summary>
        public string DataFlagOi
        {
            get;
            set;
        }
        /// <summary>
        ///645
        /// </summary>
        public string DataFlagDi
        {
            get;
            set;
        }
        /// <summary>
        ///说明
        /// </summary>
        public string DataFlagName
        {
            get;
            set;
        }

        #endregion


        string dirName = "";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex">错误信息详情</param>
        /// <param name="type">信息分类</param>
        /// <param name="flag">是否写入文件</param>

        public void MessageAdd(string ex, EnumLogType type, bool flag)
        {
            switch (type)
            {
                case EnumLogType.错误信息:
                    string file = Directory.GetCurrentDirectory() + "\\log\\错误信息\\" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    WriteErrLog(file, ex);
                    break;
                case EnumLogType.提示信息:
                    file = Directory.GetCurrentDirectory() + "\\log\\提示信息\\" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    WriteErrLog(file, ex);
                    break;
                case EnumLogType.流程信息:
                    file = Directory.GetCurrentDirectory() + "\\log\\流程信息\\" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    WriteErrLog(file, ex);
                    break;
                case EnumLogType.详细信息:
                    file = Directory.GetCurrentDirectory() + "\\log\\详细信息\\" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    WriteErrLog(file, ex);
                    break;
            }
        }


        public static void WriteErrLog(string file, string ex)
        {
            string strMessage = "";
            FileStream fs = Create(file);
            if (fs == null)
            {
                return;
            }
            fs.Close();
            fs.Dispose();
            strMessage = DateTime.Now + "   :" + ex.ToString();
            System.IO.File.AppendAllText(file, strMessage + "\r\n\r\n");

        }

        /// <summary>
        /// 创建文件、如果目录不存在则自动创建、路径既可以是绝对路径也可以是相对路径
        /// 返回文件数据流，如果创建失败在返回null、如果文件存在则打开它
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static FileStream Create(string FileName)
        {

            string folder = FileName.Substring(0, FileName.LastIndexOf('\\') + 1);

            string tmpFolder = folder.Substring(0, FileName.IndexOf('\\')); //磁盘跟目录
            //逐层创建文件夹
            try
            {
                while (tmpFolder != folder)
                {
                    tmpFolder = folder.Substring(0, FileName.IndexOf('\\', tmpFolder.Length) + 1);
                    if (!System.IO.Directory.Exists(tmpFolder))
                        System.IO.Directory.CreateDirectory(tmpFolder);
                }
            }
            catch { return null; }

            if (System.IO.File.Exists(FileName))
            {
                return System.IO.File.Open(FileName, FileMode.Open, FileAccess.ReadWrite);
                //return null;
            }
            else
            {
                try
                {
                    return System.IO.File.Create(FileName);
                }
                catch { return null; }
            }
        }



        /// <summary>
        /// 优化后的提示框
        /// </summary>
        /// <param name="str">提示的信息</param>
        /// <param name="tooltip">标题</param>
        /// <param name="error">图标</param>
        public void MessageBoxTooltip(string str, string tooltip, MessageBoxIcon error)
        {
            MessageBoxX.Show(str, tooltip, null,System.Windows.MessageBoxButton.OK, new MessageBoxXConfigurations { MessageBoxIcon = error});
        }



    }
}
