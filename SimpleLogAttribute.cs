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
namespace Moon.Web
{
	/// <summary>
	/// Description of SimpleLogAttribute.
	/// </summary>
	public class SimpleLogAttribute:AspectAttribute
	{
		public override AspectResultType BeforeInvoke(System.Reflection.MethodInfo methodInfo,System.Web.HttpContext context )
		{
			var classFullName=methodInfo.DeclaringType.FullName;
			var methodName=methodInfo.Name;
			LogUtil.Debug(classFullName+"->"+methodName+"被执行前");
			Console.WriteLine(classFullName+"->"+methodName+"被执行前");
			return AspectResultType.Continue;
		
		}
		public override AspectResultType AfterInvoke(System.Reflection.MethodInfo methodInfo,System.Web.HttpContext context )
		{
			var classFullName=methodInfo.DeclaringType.FullName;
			var methodName=methodInfo.Name;
			LogUtil.Debug(classFullName+"->"+methodName+"被执行后");
			Console.WriteLine(classFullName+"->"+methodName+"被执行后");
			return AspectResultType.Continue;
		}
	}

}
