/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/9/2
 * 时间: 16:55
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;

namespace Moon.Mvc
{
	public class RouteInfo
	{
		/// <summary>
		/// 构造
		/// </summary>
		public RouteInfo(){
			Area=null;
		}
		
		bool _isParamterOption;
		
		/// <summary>
		/// 参数是否可有可无
		/// </summary>
		public bool IsParamterOption {
			get { return _isParamterOption; }
			set { _isParamterOption = value; }
		}
		/// <summary>
		/// 如果没有就不用写
		/// </summary>
		public string Area
		{
			get;
			set;
		}
		/// <summary>
		/// 控制器类的名称(注意是类的名称)
		/// </summary>
		public string ControllerClassName
		{
			get;
			set;
		}
		/// <summary>
		/// action的名称
		/// </summary>
		public string ActionName
		{
			get;
			set;
		}
	}
}
