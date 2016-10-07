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
	/// 结果应该是byte[]数组类型
	/// </summary>
	public class BinaryResult:ResultAttribute
	{
		public string ContentType{
			get;
			set;
		}
		public BinaryResult(string contentType){
			this.ContentType=contentType;
		}
		public override void Response(HttpContext context, string controllerFullName,string methodName, object model, Dictionary<string,object> viewData)
		{
			var Response=context.Response;
			Response.ContentType =this.ContentType;
			var bytes=model as byte[];
			Response.BinaryWrite(bytes);
            Response.Flush();
		}
	}
	
}
