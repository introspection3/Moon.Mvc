/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/22
 * 时间: 17:28
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Reflection;
using System.Web;
using System.Configuration;

namespace Moon.Mvc
{


    public static class GlobalData
    {
        /// <summary>
        /// 初始化所有常量
        /// </summary>
        static GlobalData()
        {
            string requst_suffix = ConfigurationManager.AppSettings["REQUST_SUFFIX"];
            if (string.IsNullOrEmpty(requst_suffix))
            {
                REQUST_SUFFIX = ".htm";
            }
            else
            {
                REQUST_SUFFIX = requst_suffix;
            }
        }
        /// <summary>
        /// 请求的后缀格式,如:  .html .ajax
        /// </summary>
        public static readonly string REQUST_SUFFIX;
        public static string BaseUrl;
        /// <summary>
        /// MOON_SERVICE地址
        /// </summary>
        public readonly static string MOON_SERVICE;
        /// <summary>
        /// 类的全名
        /// </summary>
        public const string CLASS_FULL_NAME = "classFullName";
        /// <summary>
        /// 方法名
        /// </summary>
        public const string METHOD_NAME = "methodName";
        /// <summary>
        /// 验证错误所用的content.item
        /// </summary>
        public const string FLUENT_VALIDATE_ERROR = "FLUENT_VALIDATE_ERROR";

    }
}
