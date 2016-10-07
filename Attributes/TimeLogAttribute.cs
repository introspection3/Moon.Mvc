/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/28
 * 时间: 21:20
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using Moon.Orm;

namespace Moon.Mvc
{
	/// <summary>
	/// Description of TimeLogAttribute.
	/// </summary>
	public class TimeLogAttribute:AspectAttribute
	{
		public TimeLogAttribute(){
			this.Priority=AspectAttributePriority.High;
		}
		protected long _start;
		public override AspectResultType BeforeInvoke(System.Reflection.MethodInfo methodInfo, System.Web.HttpContext context)
		{
			_start=DateTime.Now.Ticks;
			return AspectResultType.Continue;
		}
		public override AspectResultType AfterInvoke (System.Reflection.MethodInfo methodInfo, System.Web.HttpContext context)
		{
			var classFullName=methodInfo.DeclaringType.FullName;
			var methodName=methodInfo.Name;
			var result=DateTime.Now.Ticks-_start;
            TimeSpan ts = new TimeSpan(result); 
			//var time=result/10000000;
            string msg = classFullName + "." + methodName+" spent time(s):"+ts.TotalSeconds;
            Moon.Orm.Util.LogUtil.Write("执行时间", msg);
			return AspectResultType.Continue;
		}
	}
}
