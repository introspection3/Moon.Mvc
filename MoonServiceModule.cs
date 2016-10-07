/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/22
 * 时间: 16:57
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

using Moon.Orm;
using Moon.Orm.Util;
using System.IO;

namespace Moon.Mvc
{
	
	public class MoonServiceModule:IHttpModule
	{
		public const string URL_PATTERN = "/MS.MOON";
		public void Init(HttpApplication app)
		{
			if (string.IsNullOrEmpty(GlobalData.BaseUrl)) {
				
				GlobalData.BaseUrl="";
			}
			app.BeginRequest += new EventHandler(app_BeginRequest);
		}
		// http://localhost/MOON_SERVICE/ClassFullName/Method?A=3&B=4
		// http://localhost/{areas}/ClassFullName/Method/{id}
		void app_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			var absolutePath=app.Context.Request.Url.AbsolutePath;
			if (absolutePath.StartsWith(GlobalData.MOON_SERVICE,StringComparison.OrdinalIgnoreCase))
			{
				string[] array=absolutePath.Split('/');
				if (array.Length==4) {
					string userDefineName=array[2];
					string classFullName=UrlRouteCenter.GetClassFullNameByUserDefineName(userDefineName);
					string methodName=array[3];
					string query=app.Context.Request.Url.Query;
					app.Context.Items[GlobalData.CLASS_FULL_NAME]=classFullName;
					app.Context.Items[GlobalData.METHOD_NAME]=methodName;
					app.Context.RewritePath(URL_PATTERN,false);
				}
			}
			
		}
		public void Dispose()
		{
			
		}
		
		
	}
}