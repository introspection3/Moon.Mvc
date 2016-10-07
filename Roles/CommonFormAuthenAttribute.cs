/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/9/3
 * 时间: 15:16
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace Moon.Mvc
{
	/// <summary>
	/// Moon.Mvc中通用的Form验证机制
	/// </summary>
	public class CommonFormAuthenAttribute:AspectAttribute
	{
		public CommonFormAuthenAttribute()
		{
			this.Priority=AspectAttributePriority.Highest;
		}
		public CommonFormAuthenAttribute(string failedUrl)
		{
			FailedUrl=failedUrl;
			this.Priority=AspectAttributePriority.Highest;
		}
		public string FailedUrl
		{
			get; set;
		}
		public override AspectResultType AfterInvoke(MethodInfo methodInfo, HttpContext context)
		{
			return AspectResultType.Continue;
		}
		public override AspectResultType BeforeInvoke(MethodInfo methodInfo,HttpContext context)
		{
			var controllerName=methodInfo.DeclaringType.FullName;
			var actionName=methodInfo.Name;
			var actionRoles=ActionRoleManager.GetActionRoles(actionName,controllerName);
			if (IsCurrentUsersRolesLegal(actionRoles,context)==false) {
				if (string.IsNullOrEmpty(FailedUrl)) {
					context.Response.Redirect(FailedUrl);
				}else{
					context.Response.Redirect(FailedUrl);
				}
				return AspectResultType.Stop;
			}
			return AspectResultType.Continue;
		}
		bool IsCurrentUsersRolesLegal(List<string> actionRoles,HttpContext context)
		{
			if (actionRoles.Count==0) {
				return true;
			}else{
				if (!context.User.Identity.IsAuthenticated) {
					return false;
				}
				var currentUsersRoles=GetCurrentUsersRoles(context);
				foreach (var userRole in currentUsersRoles) {
					if(actionRoles.Contains(userRole)){
						return true;
					}
				}
				return false;
			}
		}
		/// <summary>
		/// 获取当前用户的角色列表
		/// </summary>
		/// <returns></returns>
		public static List<string> GetCurrentUsersRoles(HttpContext context)
		{
			List<string> list=new List<string>();
			if (context.User.Identity.IsAuthenticated) {
				var roles= context.User.Identity.Name.Split(',');
				list.AddRange(roles);
			}
			return list;
		}
	}
	
	
}
