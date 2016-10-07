using System;
using System.Reflection;

namespace Moon.Mvc
{
	public enum AspectResultType
	{
		/// <summary>
		/// 继续后续操作
		/// </summary>
		Continue,
		/// <summary>
		/// 终止后续操作
		/// </summary>
		Stop,
	}
}
