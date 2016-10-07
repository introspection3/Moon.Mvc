using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moon.Mvc
{
    public static class Url
    {
        public static string Content(string virtrualPath)
        {
            string ret = virtrualPath.Replace("~", UrlRouteCenter.ROOT_URL);
            return ret;
        }
        public static string Action<TController>(string action) where TController : BaseController
        {
            var url = UrlUtil.Action<TController>(action);
            return url;
        }
        public static string Action<TController>(string action, object datas) where TController : BaseController
        {
            var dic = MHtmlHelper<object>.AnonymousObjectToHtmlAttributes(datas);
            var url = UrlUtil.Action<TController>(action);
            if (dic.Count > 0)
            {
                url = url + "?";
                foreach (var kvp in dic)
                {
                    url += kvp.Key + "=" + System.Web.HttpUtility.UrlEncode(kvp.Value.ToString()) + "&";
                }
                url = url.TrimEnd('&');
            }
            return url;
        }
    }
}
