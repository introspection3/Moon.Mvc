/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/27
 * 时间: 14:54
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Moon.Mvc
{
    /// <summary>
    /// jsonp格式的数据返回
    /// </summary>
    public class JsonpResult : DataResult
    {
        /// <summary>
        /// 回调方法的名字
        /// </summary>
        public string MethodParameterName
        {
            get;
            set;
        }
        /// <summary>
        /// 默认构造,回调方法默认通过callback参数指定,和jquery一致
        /// </summary>
        public JsonpResult() : this("callback")
        {

        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="jsonp">回调方法名,通过这个Request获取</param>
        public JsonpResult(string jsonp)
        {
            this.MethodParameterName = jsonp;
        }
        public override void Response(HttpContext context, string controllerFullName, string methodName, object model, Dictionary<string, object> viewData)
        {
            SetResponseEncoding(context);
            string content = Moon.Orm.Util.JsonUtil.ConvertObjectToJson(model);
            string call = context.Request[MethodParameterName] + "(" + content + ")";
            context.Response.Write(call);
            context.Response.Flush();
        }
    }
}