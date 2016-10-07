/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/24
 * 时间: 20:28
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace Moon.Mvc
{
    /// <summary>
    ///渲染辅助类
    /// </summary>
    public static class RenderUtil
	{
        public static string RenderAspx(HttpContext context, string virtualPath, object model, Dictionary<string, object> viewData)
        {
            var type = typeof(MViewPage);
            var mpage = BuildManager.CreateInstanceFromVirtualPath(virtualPath, type);
            var  handler = mpage as MViewPage;
            handler.SetModel(model);
            handler.ViewData = viewData;
            handler.RootUrl = UrlRouteCenter.ROOT_URL;
            string html = null;
            StringWriter sw = new StringWriter();
            context.Server.Execute(handler, sw, false);
            html = sw.ToString();
            return html;
        }
    }
}
