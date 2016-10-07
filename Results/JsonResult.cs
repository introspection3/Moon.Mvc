
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Moon.Mvc
{
    /// <summary>
    /// 返回json格式的数据,和jsonstr的区别在于返回的ContentType不一样
    /// </summary>
    public class JsonResult : DataResult
    {
        public override void Response(HttpContext context, string controllerFullName, string methodName, object model, Dictionary<string, object> viewData)
        {
        	string content =Moon.Orm.Util.JsonUtil.ConvertObjectToJson(model);
            context.Response.ContentType = "application/json";
            SetResponseEncoding(context);
            context.Response.Write(content);
            context.Response.Flush();
           // context.Response.End();
        }
    }

}