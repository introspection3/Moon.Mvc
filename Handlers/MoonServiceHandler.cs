/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/23
 * 时间: 9:25
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Web;
using System.Web.SessionState;
 

namespace Moon.Mvc
{
	/// <summary>
	/// MoonServiceHandler
	/// </summary>
	public class MoonServiceHandler:IHttpHandler,IRequiresSessionState
	{
		public bool IsReusable 
		{
			get {
				return false;
			}
		}
		
		public void ProcessRequest(HttpContext context)
		{
			string classFullName=context.Items[GlobalData.CLASS_FULL_NAME].ToString();
			string methodName=context.Items[GlobalData.METHOD_NAME].ToString();
			MiddleProcessUtil.Process(classFullName,methodName,context);
		}
	}
}
