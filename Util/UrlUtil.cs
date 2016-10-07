/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/22
 * 时间: 17:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Web;

namespace Moon.Mvc
{
	/// <summary>
	/// Url辅助类
	/// </summary>
	public class UrlUtil
	{
		public static string Action<T>(string method) 
            where T : BaseController
        {
			var Request=HttpContext.Current.Request;
			var baseURL=Request.Url.Scheme+"://"+Request.Url.Authority;
			var userDefineName=UrlRouteCenter.GetUserDefineName<T>();
			var url=baseURL+"/"+userDefineName+"/"+method+GlobalData.REQUST_SUFFIX;
			return url;
		}
        public static string Action2<T>(string method)
        where T : BaseController
        {
            var Request = HttpContext.Current.Request;
            var baseURL = Request.Url.Scheme + "://" + Request.Url.Authority;
            var userDefineName = UrlRouteCenter.GetUserDefineName<T>();
            var url = baseURL + "/" + userDefineName + "/" + method + GlobalData.REQUST_SUFFIX;
            return url;
        }
    }
}
