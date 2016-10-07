/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/24
 * 时间: 17:00
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Web;

namespace Moon.Web
{
	/// <summary>
	/// http请求的返回规范
	/// </summary>
	public interface IResult
	{
		void  WriteToHttpResponse(HttpContext context);
	}
}
