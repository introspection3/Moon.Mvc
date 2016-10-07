using System;
using System.Reflection;

namespace Moon.Mvc
{
    /// <summary>
    /// 告诉action不要验证请求是否安全(往往包括html标签)
    /// </summary>
    public class DontValidateRequestAttribute : Attribute
    {

    }
    /// <summary>
    /// Post请求
    /// </summary>
    public class PostAttribute : Attribute
    {

    }
    /// <summary>
    /// Get 请求
    /// </summary>
    public class GetAttribute : Attribute
    {

    }
    /// <summary>
    /// 方面注入的基类
    /// </summary>
	public abstract class AspectAttribute : Attribute
	{
        /// <summary>
        /// 优先级,数值越大优先级越大,AspectAttributePriority枚举
        /// </summary>
        public int Priority{
			get;
			set;
		}
		public AspectAttribute(){
			Priority=AspectAttributePriority.Common;
		}
		public virtual AspectResultType BeforeInvoke(MethodInfo methodInfo,System.Web.HttpContext context )
		{
			return AspectResultType.Continue;
		}
		public virtual AspectResultType AfterInvoke(MethodInfo methodInfo,System.Web.HttpContext context )
		{
			return AspectResultType.Continue;
		}
	}
}
