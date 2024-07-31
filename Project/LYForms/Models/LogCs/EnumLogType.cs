using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYForms.Models.LogCs
{
     public enum EnumLogType
    { /// <summary>
      /// 异常的数据--必定会保存和显示出来
      /// </summary>
        错误信息 = 1,
        /// <summary>
        /// 仅显示在下发提示栏-没开始保存的情况不会保存该日志-不会添加到日志表格中
        /// </summary>
        提示信息 = 2,
        /// <summary>
        ///过程中所有的数据--会保存数据库-开启状态下才会显示
        /// </summary>
        详细信息 = 3,
        /// <summary>
        /// 正常的检定流程日志--默认保存
        /// </summary>
        流程信息 = 4,
        /// <summary>
        /// 即显示流程日志也显示提示信息
        /// </summary>
        提示与流程信息 = 5,
        ///// <summary>
        ///// 黄色提示,指有可能出现的异常-
        ///// </summary>
        //告警信息=6,
    }

    /// 日志源
    /// <summary>
    /// 日志源
    /// </summary>
    public enum EnumLogSource
    {
        用户操作日志 = 4,
        数据库存取日志 = 5,
        检定业务日志 = 6,
        设备操作日志 = 7,
        服务器日志 = 8,
    }


    /// 异常信息的枚举
    /// <summary>
    /// 异常信息的枚举
    /// </summary>
    public enum EnumLevel
    {
        /// <summary>
        /// 显示信息
        /// </summary>
        Information = 0,
        /// <summary>
        /// 告警
        /// </summary>
        Warning = 1,
        /// <summary>
        /// 故障
        /// </summary>
        Error = 2,
        /// <summary>
        /// 提示信息
        /// </summary>
        Tip = 3,
        /// <summary>
        /// 故障-不弹出错误界面
        /// </summary>
        TipsError = 99,



        ///// <summary>
        ///// 提示信息 不弹出错误界面
        ///// </summary>
        //TipsTip = 98,
        ///// <summary>
        ///// 信息带语音
        ///// </summary>
        //InformationSpeech = 90,
        ///// <summary>
        ///// 告警带语音
        ///// </summary>
        //WarningSpeech = 91,
        ///// <summary>
        ///// 故障带语音
        ///// </summary>
        //ErrorSpeech = 92,

    }
}
