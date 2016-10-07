/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/24
 * 时间: 17:07
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace Moon.Web
{
	/// <summary>
	/// 表示一个页面结果（页面将由框架执行）
	/// </summary>
	public sealed class PageResult: IResult
	{
		/// <summary>
		/// 页面所在的虚拟路径
		/// </summary>
		public string VirtualPath
		{
			get;
			private set;
		}
		/// <summary>
		/// 页面所用Model
		/// </summary>
		public object Model
		{
			get;
			private set;
		}
		HttpContext _context;
		public PageResult(string virtualPath, object model,BaseController controller)
		{
			this.VirtualPath = virtualPath;
			this.Model = model;
			_context=controller.CurrentHttpContext;
			WriteToHttpResponse(_context);
		}
		
		public void WriteToHttpResponse(HttpContext context)
		{
			if( string.IsNullOrEmpty(this.VirtualPath))
			{
				this.VirtualPath = context.Request.FilePath;
			}
			context.Response.ContentType = "text/html";
			string html = RenderUtil.RenderAspx(context,VirtualPath,Model);
			context.Response.Write(html);
			context.Response.End();
		}


		
	}
}
