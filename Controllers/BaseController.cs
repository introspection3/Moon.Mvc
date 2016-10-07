

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.SessionState;
using System.Configuration;


namespace Moon.Mvc
{
    /// <summary>
    /// 控制器的基类
    /// </summary>
    public class BaseController
    {
        /// <summary>
        /// 当前HttpContext
        /// </summary>
        public HttpContext CurrentHttpContext
        {
            get;
            set;
        }
        public bool IsModelValidate
        {
            get;
            internal set;
        }
        internal List<ValidationResult> ValidationResults
        {
            get;
            set;
        }
        /// <summary>
        ///               获取当前http请求的 <see cref="T:System.Web.HttpRequest" /> 对象
        /// </summary>
        /// <returns>
        ///               当前http请求的 <see cref="T:System.Web.HttpRequest" /> 对象
        ///</returns>
        public HttpRequest Request
        {
            get
            {
                return CurrentHttpContext.Request;
            }
        }
        /// <summary>
        ///               获取当前http响应的 <see cref="T:System.Web.HttpResponse" /> 对象
        /// </summary>
        /// <returns>
        ///               当前http响应的 <see cref="T:System.Web.HttpResponse" /> 对象
        ///</returns>
        public HttpResponse Response
        {
            get
            {
                return CurrentHttpContext.Response;
            }
        }
        /// <summary>
        ///               获取 <see cref="T:System.Web.HttpServerUtility" /> 对象(它提供了许多用于处理web请求的方法)
        /// </summary>
        /// <returns>
        ///               当前http请求的 <see cref="T:System.Web.HttpServerUtility" /> .
        /// </returns>
        public HttpServerUtility Server
        {
            get
            {
                return CurrentHttpContext.Server;
            }
        }
        /// <summary>
        /// 当前Session对象
        /// </summary>
        public HttpSessionState Session
        {
            get
            {
                return CurrentHttpContext.Session;
            }
        }
        /// <summary>
        /// 返回当前系统的根路径,
        /// 可以在配置文件通过ROOT_URL
        /// </summary>
        public string ROOT_URL
        {
            get
            {
                return UrlRouteCenter.ROOT_URL;
            }
        }
        /// <summary>
        /// 写入http响应流
        /// </summary>
        /// <param name="content">要返回的内容</param>
        protected void ReturnText(string content)
        {
            Response.Write(content);
            Response.Flush();
            //Response.End(); //2015年9月13日14:07:11
        }
        /// <summary>
        /// 序列化对象后,然后写入http响应流
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        protected void ReturnSerializeObject(object obj)
        {
            string json = Moon.Orm.Util.JsonUtil.ConvertObjectToJson(obj);
            ReturnText(json);
        }
        /// <summary>
        /// 反馈model到指定aspx文件中
        /// </summary>
        /// <param name="aspxFileVrtualPath"></param>
        /// <param name="model"></param>
        protected void ReturnModel(string aspxFileVrtualPath, object model, Dictionary<string, object> viewData)
        {
            string content = RenderUtil.RenderAspx(CurrentHttpContext, aspxFileVrtualPath, model, viewData);
            ReturnText(content);
        }
        protected Dictionary<string, object> _ViewData = new Dictionary<string, object>();
        /// <summary>
        /// 只能在模板的情况下,比如在BinaryResult下面是无效的
        /// </summary>
        public Dictionary<string, object> ViewData
        {
            get
            {
                return _ViewData;
            }
        }
        public void Redirect(string url)
        {
            Response.Redirect(url);
            Response.End();
        }
        /// <summary>
        /// 跳转到refferurl上,如果没有则到failedUrl
        /// </summary>
        /// <param name="failedUrl">没有reffer,则到这个地址</param>
        /// <returns>返回为空</returns>
        public void RedirectToReffer(string failedUrl)
        {
            string url = failedUrl;
            if (Request.UrlReferrer != null)
            {
                url = Request.UrlReferrer.ToString();
            }
            Response.Redirect(url);
            Response.End();
        }
        /// <summary>
        /// 是否是ajax请求
        /// </summary>
        public bool IsAjaxRequest
        {
            get
            {
                HttpContext context = CurrentHttpContext;
                if (context != null && context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return true;
                }
                return false;
            }
        }
    }
}
