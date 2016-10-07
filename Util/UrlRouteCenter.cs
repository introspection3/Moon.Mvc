/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/9/2
 * 时间: 16:55
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Configuration;

namespace Moon.Mvc
{
    /// <summary>
    /// Moon.Mvc的url路由管理中心
    /// </summary>
    public static class UrlRouteCenter
    {
        static UrlRouteCenter()
        {
            // 设置 FluentValidation 默认的资源文件提供程序 - 中文资源
            ValidatorOptions.ResourceProviderType = typeof(FluentValidationResource);
            /* 比如验证用户名 not null、not empty、length(2,int.MaxValue) 时，链式验证时，如果第一个验证失败，则停止验证 */
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure; // ValidatorOptions.CascadeMode 默认值为：CascadeMode.Continue
                                                                           //
        }

        static readonly Dictionary<string, string> URL_CONTROLLER_NAME_ROUTE_MAP = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        static readonly Dictionary<string, string> URL_CONTROLLER_NAME_ROUTE_MAP_ = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        static string _ROOT_URL;
        /// <summary>
        /// 网站根目录,最后没有/,如果通过配置文件指定了就从配置文件里面读取
        /// </summary>
		public static string ROOT_URL
        {
            get
            {
                var value = ConfigurationManager.AppSettings["ROOT_URL"];
                if (string.IsNullOrEmpty(value) == false)
                {
                    return value;
                }
                if (string.IsNullOrEmpty(_ROOT_URL))
                {
                    var baseURL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
                    string applicationPath = "";
                    if (HttpContext.Current != null)
                        applicationPath = HttpContext.Current.Request.ApplicationPath;
                    if (applicationPath == "/")
                        applicationPath = "";
                    _ROOT_URL = baseURL + applicationPath;
                    return _ROOT_URL;
                }
                else
                {
                    return _ROOT_URL;
                }
            }
        }

        /// <summary>
        /// bin目录,最后有/
        /// </summary>
		public static string BIN_PATH
        {
            get
            {
                var binPath = HttpContext.Current.Server.MapPath("~/Bin/");
                return binPath;
            }
        }

        static readonly object _ROUTE_LOCK = new object();

        [Obsolete]
        /// <summary>
        /// 设置路由规则(不推荐使用)
        /// </summary>
        /// <param name="controllerName">自定义控制器名称</param>
        /// <param name="classFullName">对应的类全名</param>
        public static void MapRoute(string controllerName, string classFullName)
        {
            lock (_ROUTE_LOCK)
            {
                URL_CONTROLLER_NAME_ROUTE_MAP[controllerName] = classFullName;
                URL_CONTROLLER_NAME_ROUTE_MAP_[classFullName] = controllerName;
            }
        }

        [Obsolete]
        /// <summary>
        /// 通过路径注册插件(不推荐使用)
        /// </summary>
        /// <param name="pluginFullPath">插件全路径</param>
        /// <param name="classFullName">类全名</param>
        /// <param name="controllerName">控制器名字</param>
        public static void MapRoutePlugin(string pluginFullPath, string classFullName, string controllerName)
        {
            lock (_ROUTE_LOCK)
            {
                ControllerAssmeblyUtil.RegisterByAssmblyPath(pluginFullPath);
                URL_CONTROLLER_NAME_ROUTE_MAP[controllerName] = classFullName;
                URL_CONTROLLER_NAME_ROUTE_MAP_[classFullName] = controllerName;
            }
        }
        /// <summary>
        /// 注册网站Plugins目录下的所有插件
        /// </summary>
        public static void MapRouteAllPlugin()
        {
            var pluginDirectory = System.Web.HttpContext.Current.Server.MapPath("~/Plugins");
            Moon.Orm.Util.IOUtil.CreateDirectoryWhenNotExist(pluginDirectory);
            var files = Directory.GetFiles(pluginDirectory, "*.dll");
            foreach (var f in files)
            {
                MapRoutePlugin(f);
            }
        }

        /// <summary>
        /// 设置系统的默认页面,只能在Application_BeginRequest中调用此方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="application"></param>
        /// <param name="actionName"></param>
        public static void SetDefaultRoute<T>(this HttpApplication application, String actionName)
            where T : BaseController
        {
            var request = application.Request;
            var url = UrlUtil.Action<T>(actionName);
            if (request.Url.AbsolutePath.EndsWith("/"))
            {
                HttpWebRequestHelper helper = new HttpWebRequestHelper();
                foreach (var key in HttpContext.Current.Request.Cookies.AllKeys)
                {
                    HttpCookie ck = HttpContext.Current.Request.Cookies[key];
                    var cookie = new System.Net.Cookie();
                    cookie.Name = ck.Name;
                    cookie.Domain = HttpContext.Current.Request.Url.Host;
                    cookie.Expires = ck.Expires;
                    cookie.HttpOnly = ck.HttpOnly;
                    cookie.Path = ck.Path;
                    cookie.Value = ck.Value;
                    helper.CookieContainer.Add(cookie);
                }
                string content = null;
                try
                {
                    content = helper.GetContent(url);
                }
                catch (Exception)
                {
                    application.Response.Redirect(url);
                    return;
                }
                application.Response.Write(content);
                application.Response.StatusCode = 200;
                application.Response.ContentType = "text/html";
                application.Response.Flush();
                application.Response.End();
            }
        }
        /// <summary>
        /// 注册bin目录下的dll
        /// </summary>
        /// <param name="dllName">dll名称,注意没有.dll后缀,一般就写项目名称</param>
        public static void MapRouteBinPlugin(string dllName)
        {
            string fullPath = BIN_PATH + dllName + ".dll";
            MapRoutePlugin(fullPath);
        }
        /// <summary>
        /// 注册指定路径的插件
        /// </summary>
        /// <param name="pluginFullPath">插件全路径</param>
        public static void MapRoutePlugin(string pluginFullPath)
        {
            if (File.Exists(pluginFullPath) == false)
            {
                throw new FileNotFoundException("指定路径的插件文件不存在,请查看此路径:" + pluginFullPath);
            }
            Moon.Orm.Util.LogUtil.Debug("发现插件,PluginFullPath:" + pluginFullPath);
            var assmebly = ControllerAssmeblyUtil.RegisterByAssmblyPath(pluginFullPath);
            var allTypes = assmebly.GetTypes();
            foreach (var type in allTypes)
            {
                if (type.IsClass && type.IsSubclassOf(typeof(BaseController)))
                {
                    string classFullName = type.FullName;
                    Moon.Orm.Util.LogUtil.Debug("载入插件Controller:" + classFullName);
                    int index = classFullName.LastIndexOf('.');
                    string userDefineName = null;
                    userDefineName = classFullName.Substring(index + 1);
                    userDefineName = userDefineName.Replace("Controller", "");//去掉Controller得到对应的名字
                    bool existArea;
                    var areaName = AreaUtil.GetControllerAreaName(type, out existArea);
                    if (existArea)
                    {
                        userDefineName = areaName + "/" + userDefineName;
                    }
                    lock (_ROUTE_LOCK)
                    {
                        URL_CONTROLLER_NAME_ROUTE_MAP[userDefineName] = classFullName;
                        URL_CONTROLLER_NAME_ROUTE_MAP_[classFullName] = userDefineName;
                    }
                }
            }
            lock (_ROUTE_LOCK)
            {
                ControllerAssmeblyUtil.RegisterByAssmblyPath(pluginFullPath);
            }
        }
        /// <summary>
        /// 手动注册一个控制器为三级路由规则
        /// </summary>
        /// <typeparam name="T">控制器类</typeparam>
        /// <param name="areaName">三级路由的名字</param>
        /// <param name="controllerName">自定义控制器名</param>
        public static void SetArea<T>(string areaName, string controllerName) where T : BaseController
        {

            string controllerName2 = areaName + "/" + controllerName;
            string classFullName = typeof(T).FullName;
            lock (_ROUTE_LOCK)
            {
                URL_CONTROLLER_NAME_ROUTE_MAP[controllerName2] = classFullName;
                URL_CONTROLLER_NAME_ROUTE_MAP_[classFullName] = controllerName2;
            }
            ControllerAssmeblyUtil.Register<T>();
        }
        public static void SetArea<T>(string areaName) where T : BaseController
        {
            string classFullName = typeof(T).FullName;
            string userDefineName = null;
            int index = classFullName.LastIndexOf('.');
            userDefineName = classFullName.Substring(index + 1);
            userDefineName = userDefineName.Replace("Controller", "");
            SetArea<T>(areaName, userDefineName);
        }

        [Obsolete]
        /// <summary>
        /// 注册二级路由规则(建议不用再此方法)
        /// </summary>
        /// <typeparam name="T">控制器类</typeparam>
        /// <param name="userDefineName">控制器名</param>
        public static void MapRoute<T>(string userDefineName) where T : BaseController
        {
            string classFullName = typeof(T).FullName;
            lock (_ROUTE_LOCK)
            {
                URL_CONTROLLER_NAME_ROUTE_MAP[userDefineName] = classFullName;
                URL_CONTROLLER_NAME_ROUTE_MAP_[classFullName] = userDefineName;
            }
            ControllerAssmeblyUtil.Register<T>();
        }

        [Obsolete]
        /// <summary>
        /// 注册二级路由规则(建议不用再此方法)
        /// </summary>
        /// <typeparam name="T">控制器类</typeparam>
        public static void MapRoute<T>() where T : BaseController
        {
            string classFullName = typeof(T).FullName;
            string userDefineName = null;
            int index = classFullName.LastIndexOf('.');
            userDefineName = classFullName.Substring(index + 1);
            userDefineName = userDefineName.Replace("Controller", "");
            lock (_ROUTE_LOCK)
            {
                URL_CONTROLLER_NAME_ROUTE_MAP[userDefineName] = classFullName;
                URL_CONTROLLER_NAME_ROUTE_MAP_[classFullName] = userDefineName;
            }
            ControllerAssmeblyUtil.Register<T>();
        }
        public static string GetUserDefineName<T>() where T : BaseController
        {
            var classFullName = typeof(T).FullName;
            lock (_ROUTE_LOCK)
            {
                if (URL_CONTROLLER_NAME_ROUTE_MAP_.ContainsKey(classFullName))
                {
                    return URL_CONTROLLER_NAME_ROUTE_MAP_[classFullName];
                }
            }
            return classFullName;
        }
        public static string GetUserDefineName(string classFullName)
        {
            lock (_ROUTE_LOCK)
            {
                if (URL_CONTROLLER_NAME_ROUTE_MAP_.ContainsKey(classFullName))
                {
                    return URL_CONTROLLER_NAME_ROUTE_MAP_[classFullName];
                }
                else
                {
                    var ex = "classFullName={0}不存在对应UserDefineName";
                    ex = string.Format(ex, classFullName);
                    Moon.Orm.Util.LogUtil.Error(ex);
                    throw new Exception(ex);
                }
            }

        }
        /// <summary>
        /// 通过用户自定义控制器名找到对应类全名
        /// </summary>
        /// <param name="userDefineName"></param>
        /// <returns></returns>
        public static string GetClassFullNameByUserDefineName(string userDefineName)
        {
            lock (_ROUTE_LOCK)
            {
                if (URL_CONTROLLER_NAME_ROUTE_MAP.ContainsKey(userDefineName))
                {
                    return URL_CONTROLLER_NAME_ROUTE_MAP[userDefineName];
                }
            }
            return userDefineName;
        }


    }


}
