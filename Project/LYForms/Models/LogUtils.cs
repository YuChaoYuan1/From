using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYForms.Models
{
	public class LogUtils
	{
		private static readonly ILog loginfo = LogManager.GetLogger("loginfo");

		/// <summary>
		/// 从缺省配置文件获取日志配置
		/// </summary>
		public static void SetConfig()
		{
			XmlConfigurator.Configure();
		}

		/// <summary>
		/// 从指定配置文件获取日志配置
		/// </summary>
		/// <param name="configFile">指定的配置文件</param>
		public static void SetConfig(FileInfo configFile)
		{
			XmlConfigurator.Configure(configFile);
		}

		/// <summary>
		/// 生成分类日志
		/// </summary>
		/// <param name="info">日志信息</param>
		/// <param name="dirName">保存目录名,形如d:\log\aaa</param>
		private static void WriteSortLog(string info, string dirName)
		{
			try
			{
				if (false == System.IO.Directory.Exists(dirName))
				{
					System.IO.Directory.CreateDirectory(dirName);
				}
				string path = dirName + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
				StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
				sw.WriteLine(DateTime.Now.ToString("HH:mm:ss: ") + info);
				sw.Close();
			}
			catch (Exception ex)
			{
				string expMsg = "WriteSortLog异常：" + ex.Message + Environment.NewLine + ex.StackTrace;

				if (ex.InnerException != null)
					expMsg += Environment.NewLine + "InnerException:" + ex.InnerException.Message;

				Error(expMsg, ex);
			}
		}

		/// <summary>
		/// Info级 常规日志
		/// </summary>
		/// <param name="info">日志信息</param>
		public static void Info(string info)
		{
			if (loginfo.IsInfoEnabled)
			{
				loginfo.Info(info);
			}
		}

		/// <summary>
		/// Info 先生成常规日志，然后在指定目录另外创建一份日志
		/// 主要用来需要对日志进行分类时使用
		/// </summary>
		/// <param name="info"></param>
		/// <param name="dirName"></param>
		public static void Info(string info, string dirName)
		{
			if (loginfo.IsInfoEnabled)
			{
				//生成常规日志
				loginfo.Info(info);

				//生成分类日志
				WriteSortLog(info, dirName);
			}
		}

		/// <summary>
		/// Debug级 常规日志
		/// </summary>
		/// <param name="info">日志信息</param>
		public static void Debug(string info)
		{
			if (loginfo.IsDebugEnabled)
			{
				loginfo.Debug(info);
			}
		}

		/// <summary>
		/// Debug级 异常日志
		/// </summary>
		/// <param name="info">日志信息</param>
		/// <param name="exp">异常信息</param>
		public static void Debug(string info, Exception exp)
		{
			if (loginfo.IsDebugEnabled)
			{
				loginfo.Debug(info, exp);
			}
		}

		/// <summary>
		/// Error级 常规的日志
		/// </summary>
		/// <param name="info">日志信息</param>
		public static void Error(string info)
		{
			if (loginfo.IsErrorEnabled)
			{
				loginfo.Error(info);
			}
		}

		/// <summary>
		/// Error 异常日志
		/// </summary>
		/// <param name="info">日志信息</param>
		/// <param name="exp">异常信息</param>
		public static void Error(string info, Exception exp)
		{
			if (loginfo.IsErrorEnabled)
			{
				loginfo.Error(info, exp);
			}
		}

		/// <summary>
		/// Fatal级 常规日志
		/// </summary>
		/// <param name="info">日志信息</param>
		public static void Fatal(string info)
		{
			if (loginfo.IsFatalEnabled)
			{
				loginfo.Fatal(info);
			}
		}

		/// <summary>
		/// Fatal级 异常日志
		/// </summary>
		/// <param name="info">日志信息</param>
		/// <param name="exp">异常信息</param>
		public static void Fatal(string info, Exception exp)
		{
			if (loginfo.IsFatalEnabled)
			{
				loginfo.Fatal(info, exp);
			}
		}

		/// <summary>
		/// Warn级 常规日志
		/// </summary>
		/// <param name="info">日志信息</param>
		public static void Warn(string info)
		{
			if (loginfo.IsWarnEnabled)
			{
				loginfo.Warn(info);
			}
		}

		/// <summary>
		/// Warn级 异常日志
		/// </summary>
		/// <param name="info">日志</param>
		/// <param name="exp">异常信息</param>
		public static void Warn(string info, Exception exp)
		{
			if (loginfo.IsWarnEnabled)
			{
				loginfo.Warn(info, exp);
			}
		}
	}
}