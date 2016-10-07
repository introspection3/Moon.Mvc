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
	/// <summary>
	/// 错误验证时返回的结果类型
	/// </summary>
	public enum ErrorResultReturnType:byte
	{
		/// <summary>
		/// 纯数据,则需要自己进一步对错误数据自行处理.
		/// </summary>
		PureData,
		/// <summary>
		/// 以json的格式返回 ValidationResult
		/// </summary>
		Json
	}
	
}
