
using System;
using System.Web;
using System.Web.SessionState;
using Moon.Orm.Util;
namespace Moon.Mvc
{
    public class RequestHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            //http://localhost:8081/area/Admin/Login.htm?ReturnUrl=http://localhost:8081/trtr/a.htm

            string url = context.Request.Path.Substring(1);//        area/Admin/Login.htm
            string host = context.Request.Url.Host;
            string actionName = null;
            string userDefineName = null;
            int suffixIndex = url.LastIndexOf(GlobalData.REQUST_SUFFIX, StringComparison.OrdinalIgnoreCase);

            string tempurl = url.Substring(0, suffixIndex);// area/Admin/Login
            var actionNameSplitIndex = tempurl.LastIndexOf('/');// Login前的那个分隔符的位置
            actionName = tempurl.Substring(actionNameSplitIndex + 1);// Login

            int index = actionNameSplitIndex;
            string urlLeft = tempurl.Substring(0, index);// area/Admin

            if (urlLeft.Contains("/") == false)
            {
                userDefineName = urlLeft;//没有域的形式,如 http://localhost:8081/Home/Index.htm
            }
            else
            {
                int index2 = urlLeft.LastIndexOf('/');//  Admin前的分隔符位置
                var cname = urlLeft.Substring(index2 + 1);// Admin
                var areaName = urlLeft.Substring(0, index2);// area
                if (areaName.Contains("/"))
                {
                    string error = "The request path in RequestHandler system doesn't suppurt,context.Request.Path=" + url;
                    LogUtil.Error(error);
                    throw new Exception(error);
                }
                else
                {
                    userDefineName = urlLeft;
                }
            }
            if (string.IsNullOrEmpty(actionName) == false)
            {
                string classFullName = UrlRouteCenter.GetClassFullNameByUserDefineName(userDefineName);
                MiddleProcessUtil.Process(classFullName, actionName, context);
            }
            else
            {
                string error = "The request path in RequestHandler doesn't exist,context.Request.Path=" + url;
                LogUtil.Error(error);
                throw new Exception(error);
            }

        }
    }
}
