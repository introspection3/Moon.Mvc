/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/27
 * 时间: 16:59
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using Moon.Orm.Util;
namespace Moon.Mvc
{
	/// <summary>
	/// 通过告诉客户端304状态进行缓存
	/// </summary>
	public class Client304Expires:AspectAttribute
	{
		/// <summary>
		/// 缓存有效期
		/// </summary>
		protected int TimeOut{
			get;
			set;
		}
		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="howlong">客户端缓存有效期</param>
		public Client304Expires(int howlong){
			this.TimeOut=howlong;
			this.Priority=AspectAttributePriority.High;
		}
		/// <summary>
		/// action执行之前
		/// </summary>
		/// <param name="methodInfo"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override AspectResultType AfterInvoke(System.Reflection.MethodInfo methodInfo, System.Web.HttpContext context)
		{
			return AspectResultType.Continue;
		}
		/// <summary>
		/// action执行之后
		/// </summary>
		/// <param name="methodInfo"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override AspectResultType BeforeInvoke(System.Reflection.MethodInfo methodInfo, System.Web.HttpContext context)
		{
			 
			DateTime dt;
			if (DateTime.TryParse(context.Request.Headers["If-Modified-Since"], out dt))
			{
				// 注意：如果是20秒内，我就以304的方式响应。
				if ((DateTime.Now - dt).TotalSeconds < TimeOut)
				{
					context.Response.StatusCode = 304;
					context.Response.End();
					return AspectResultType.Stop;
				}
			}

            string time = DateTime.Now.AddSeconds(TimeOut).ToUniversalTime().ToString("r");
            context.Response.AddHeader("Last-Modified", time);
			return AspectResultType.Continue;
		}
	}
}
