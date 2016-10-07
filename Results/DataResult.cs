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
using System.Web;

namespace Moon.Mvc
{
	/// <summary>
	/// 仅仅返回数据
	/// </summary>
	public class DataResult:ResultAttribute
	{
		public override void Response(HttpContext context,string controllerFullName, string methodName,object model, Dictionary<string,object> viewData)
		{
            SetResponseEncoding(context);
			context.Response.Write(model.ToString());
			context.Response.Flush();
			//context.Response.End(); //2015年9月13日15:03:54
		}
	}
}
