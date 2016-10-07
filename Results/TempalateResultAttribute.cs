
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Moon.Mvc
{
    /// <summary>
    /// 返回模板类型的数据
    /// </summary>
    public class TemplateResultAttribute : ResultAttribute
    {
        /// <summary>
        /// aspx引擎解析和方法同名的模板文件
        /// </summary>
        public TemplateResultAttribute()
        {
            TempateEngineType = TempateEngine.aspx;
        }
        /// <summary>
        /// 指定引擎解析和方法同名的模板文件
        /// </summary>
        /// <param name="tempateEngine"></param>
        public TemplateResultAttribute(TempateEngine tempateEngine)
        {
            TempateEngineType = tempateEngine;
        }
        /// <summary>
        /// 引擎类型
        /// </summary>
        public TempateEngine TempateEngineType;
        /// <summary>
        /// 模板的序列路径
        /// </summary>
        public string TemplateVirtualPath
        {
            get;
            protected set;
        }
        /// <summary>
        /// 虚拟路径设置，注意以~开始
        /// </summary>
        /// <param name="templateVirtualPath"></param>
        public TemplateResultAttribute(string templateVirtualPath)
        {
            TemplateVirtualPath = templateVirtualPath;
            TempateEngineType = TempateEngine.aspx;
        }
        public TemplateResultAttribute(TempateEngine tempateEngine, string templateVirtualPath)
        {
            TemplateVirtualPath = templateVirtualPath;
            TempateEngineType = tempateEngine;
        }
        public override void Response(HttpContext context, string controllerFullName, string methodName, object model, Dictionary<string, object> viewData)
        {
            string tType = TempateEngineType.ToString();
            string virtualPath = null;
            string controllerName = UrlRouteCenter.GetUserDefineName(controllerFullName);
            var basString = "~/Views/" + controllerName + "/" + methodName;
            if (string.IsNullOrEmpty(TemplateVirtualPath))
            {
                if (IsMoblieRquest(context))
                {
                    virtualPath = basString + ".Mobile." + tType;
                    var str = context.Server.MapPath(virtualPath);
                    if (File.Exists(str) == false)
                    {
                        virtualPath = basString + "." + tType;
                    }
                }
                else
                {
                    virtualPath = basString + ".PC." + tType;
                    var str = context.Server.MapPath(virtualPath);
                    if (File.Exists(str) == false)
                    {
                        virtualPath = basString + "." + tType;
                    }
                }
            }
            else
            {
                virtualPath = this.TemplateVirtualPath;
            }
            string result = null;
            result = RenderUtil.RenderAspx(context, virtualPath, model, viewData);
            SetResponseEncoding(context);
            context.Response.Write(result);
            context.Response.Flush();
            // context.Response.End();//2015年9月13日15:02:52
        }
    }
}
