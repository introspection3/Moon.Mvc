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
	/// 通过给客户端设置Expires头来决定过期时间.
	/// </summary>
	public class ClientExpires:AspectAttribute
	{
		/// <summary>
		/// 客户端缓存时间
		/// </summary>
		protected int TimeOut{
			get;
			set;
		}
		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="howlong">过期时间(秒)</param>
		public ClientExpires(int howlong){
			this.TimeOut=howlong;
			this.Priority=AspectAttributePriority.High;
		}
		/// <summary>
		/// action执行前
		/// </summary>
		/// <param name="methodInfo">方法信息</param>
		/// <param name="context">httpcontext</param>
		/// <returns>该方法的反馈信息</returns>
		public override AspectResultType AfterInvoke(System.Reflection.MethodInfo methodInfo, System.Web.HttpContext context)
		{
			return AspectResultType.Continue;
		}
		public override AspectResultType BeforeInvoke(System.Reflection.MethodInfo methodInfo, System.Web.HttpContext context)
		{
			string time=DateTime.Now.AddSeconds(TimeOut).ToUniversalTime().ToString("r");
			context.Response.AddHeader("Expires",time);
			return AspectResultType.Continue;
		}
	}

}
