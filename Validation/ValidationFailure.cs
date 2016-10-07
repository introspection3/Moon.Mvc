/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/9/4
 * 时间: 15:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace Moon.Mvc
{
	[Serializable]
	public class ValidationFailure
	{
		/// <summary>
		/// The name of the property.
		/// </summary>
		public string PropertyName
		{
			get;
			  set;
		}
		/// <summary>
		/// The error message
		/// </summary>
		public string ErrorMessage
		{
			get;
			  set;
		}
		/// <summary>
		/// The property value that caused the failure.
		/// </summary>
		public object AttemptedValue
		{
			get;
			  set;
		}
		/// <summary>
		/// Custom state associated with the failure.
		/// </summary>
		public object CustomState
		{
			get;
			set;
		}
		public ValidationFailure()
		{
		}
		
		/// <summary>
		/// Creates a textual representation of the failure.
		/// </summary>
		public override string ToString()
		{
			return this.ErrorMessage;
		}
	}
}
