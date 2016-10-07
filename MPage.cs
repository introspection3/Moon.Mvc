/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/29
 * 时间: 15:42
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Web;
using System.Collections.Generic;

namespace Moon.Mvc
{
	/// <summary>
	/// 处理页面部分布局的情况
	/// </summary>
	public class MPage
	{
		public static string Partial(string virtualPath,object model,Dictionary<string,object> viewData)
		{
			string content=RenderUtil.RenderAspx(HttpContext.Current,virtualPath,model,viewData);
			return content;
		}
		
		public static string Partial(string virtualPath)
		{
			return Partial(virtualPath,null,null);
		}
		
		public static string Partial(string virtualPath,object model)
		{
			return Partial(virtualPath,model,null);
		}
		
		public static string Partial(string virtualPath,Dictionary<string,object> viewData)
		{
			return Partial(virtualPath,null,viewData);
		}
	}
}
