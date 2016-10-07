using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Collections;
namespace Moon.Mvc
{
    public class MHtmlHelper<TModel>
    {
        public TModel Model
        {
            get;
            set;
        }
        protected Dictionary<string, object> _ViewData = new Dictionary<string, object>();
        /// <summary>
        /// 视图数据字典
        /// </summary>
        public Dictionary<string, object> ViewData
        {
            get
            {
                return _ViewData;
            }
            set
            {
                _ViewData = value;
            }
        }
        public static object TypeDescriptor { get; private set; }

        protected System.Web.HttpContext _context;
        protected Type _modelType;
        public MHtmlHelper(TModel model, System.Web.HttpContext context)
        {
            this.Model = model;
            _context = context;
            _modelType = model.GetType();
        }
       static string ConvertIDictionaryToHtml(IDictionary<string, object> htmlAttributes)
        {
            StringBuilder sb = new StringBuilder();
            if (htmlAttributes != null)
            {
                foreach (var kvp in htmlAttributes)
                {
                    sb.Append(kvp.Key + "='" + System.Web.HttpUtility.HtmlEncode(kvp.Value) + "' ");
                }
            }
            return sb.ToString();
        }
        public string RadioButtonFor<TProperty>(Expression<Func<TModel, TProperty>> expression, bool setChecked, string paramterName, object htmlAttributes = null)
        {
            var dic = AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (dic.ContainsKey("checked") == false && setChecked)
            {
                dic["checked"] = "checked";
            }
            else if (dic.ContainsKey("checked"))
            {
                var value = dic["checked"].ToString();
                if (value == "checked" || value == true.ToString())
                {
                    dic["checked"] = "checked";
                }
                else
                {
                    dic.Remove("checked");
                }
            }
            return InputFor(expression, "radio", paramterName, dic);
        }
        public string DropDownListFor<TProperty>(Expression<Func<TModel, TProperty>> expression, List<SelectListItem> allData, string paramterName, object htmlAttributes = null)
        {
            string value = string.Empty;
            if (this.Model != null)
            {
                var comp = expression.Compile();
                value = comp.Invoke(this.Model).ToString();
            }
            string rvalue = System.Web.HttpUtility.HtmlEncode(value);
            string expressionName = expression.Parameters[0].Name;
            string name = ExpressionHelper.GetExpressionText(expression);
            if (!string.IsNullOrEmpty(paramterName))
            {
                name = paramterName + "." + name;
            }
            else
            {
                name = expressionName + "." + name;
            }

            string id = name.Replace('.', '_');
            var dic = AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (dic.ContainsKey("id") == false)
            {
                dic["id"] = id;
            }
            string otherInfo = ConvertIDictionaryToHtml(dic);
            if (allData == null)
            {
                allData = new List<SelectListItem>();
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<select name='{0}' {1}>");
            foreach (var item in allData)
            {
                if (item.Value == rvalue)
                {
                    sb.Append("<option selected='selected'>");
                }
                else
                {
                    sb.Append("<option>");
                }
                sb.Append(rvalue);
                sb.Append("</option>");
            }
            sb.Append("</select>");
            var ret = string.Format(sb.ToString(), name, otherInfo);
            return ret;
        }

        public string LabelFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string info, object htmlAttributes = null)
        {
            return LabelFor(expression, info, null, htmlAttributes);
        }

        public string LabelFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string info, string paramterName, object htmlAttributes = null)
        {
            string value = string.Empty;
            if (this.Model != null)
            {
                var comp = expression.Compile();
                value = comp.Invoke(this.Model).ToString();
            }
            string rvalue = System.Web.HttpUtility.HtmlEncode(value);
            string expressionName = expression.Parameters[0].Name;
            string name = ExpressionHelper.GetExpressionText(expression);
            if (!string.IsNullOrEmpty(paramterName))
            {
                name = paramterName + "." + name;
            }
            else
            {
                name = expressionName + "." + name;
            }
            string forID = name.Replace('.', '_');
            var dic = AnonymousObjectToHtmlAttributes(htmlAttributes);
            string otherInfo = ConvertIDictionaryToHtml(dic);
            string inputBox = "<label for='{0}' {1} >{2}</label>";
            string ret = string.Format(inputBox, forID, otherInfo, info);
            return ret;
        }
        public string InputFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string inputType, string paramterName, object htmlAttributes = null)
        {
            string value = string.Empty;
            if (this.Model != null)
            {
                var comp = expression.Compile();
                value = comp.Invoke(this.Model).ToString();
            }
            string rvalue = System.Web.HttpUtility.HtmlEncode(value);
            string expressionName = expression.Parameters[0].Name;
            string name = ExpressionHelper.GetExpressionText(expression);
            if (!string.IsNullOrEmpty(paramterName))
            {
                name = paramterName + "." + name;
            }
            else
            {
                name = expressionName + "." + name;
            }
            string id = name.Replace('.', '_');
            var dic = AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (dic.ContainsKey("id") == false)
            {
                dic["id"] = id;
            }
            string otherInfo = ConvertIDictionaryToHtml(dic);
            string inputBox = "<input type='{0}'  name='{1}'   value='{2}'  {3} />";
            string ret = string.Format(inputBox, inputType, name, rvalue, otherInfo);
            return ret;
        }
        public string TextAreaFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return TextAreaFor(expression, null, htmlAttributes);
        }
        public string TextAreaFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string paramterName, object htmlAttributes = null)
        {
            string value = string.Empty;
            if (this.Model != null)
            {
                var comp = expression.Compile();
                value = comp.Invoke(this.Model).ToString();
            }
            string rvalue = value;
            string name = ExpressionHelper.GetExpressionText(expression);
            string expressionName = expression.Parameters[0].Name;
            if (!string.IsNullOrEmpty(paramterName))
            {
                name = paramterName + "." + name;
            }
            else
            {
                name = expressionName + "." + name;
            }
            string id = name.Replace('.', '_');
            var dic = AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (dic.ContainsKey("id") == false)
            {
                dic["id"] = id;
            }
            string otherInfo = ConvertIDictionaryToHtml(dic);
            string textarea = "<textarea   name='{0}'   {1} />{2}</textarea>";
            string ret = string.Format(textarea, name, otherInfo, rvalue);
            return ret;
        }
        public string TextBoxFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return InputFor(expression, "text", null, htmlAttributes);
        }
        public string TextBoxFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string paramterName, object htmlAttributes = null)
        {
            return InputFor(expression, "text", paramterName, htmlAttributes);
        }
        public string HiddenFor(Expression<Func<TModel, object>> expression, object htmlAttributes = null)
        {
            return InputFor(expression, "hidden", null, htmlAttributes);
        }
        public string HiddenFor(Expression<Func<TModel, object>> expression, string paramterName, object htmlAttributes = null)
        {
            return InputFor(expression, "hidden", paramterName, htmlAttributes);
        }
        public string PasswordFor(Expression<Func<TModel, object>> expression, object htmlAttributes = null)
        {
            return InputFor(expression, "password", null, htmlAttributes);
        }
        public string PasswordFor(Expression<Func<TModel, object>> expression, string paramterName, object htmlAttributes = null)
        {
            return InputFor(expression, "password", paramterName, htmlAttributes);
        }
        public string CheckBoxFor(Expression<Func<TModel, object>> expression, object htmlAttributes = null)
        {
            return InputFor(expression, "checkbox", null, htmlAttributes);
        }
        public string CheckBoxFor(Expression<Func<TModel, object>> expression, string paramterName, object htmlAttributes = null)
        {
            return InputFor(expression, "checkbox", paramterName, htmlAttributes);
        }

        public static Dictionary<string, Object> AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            if (htmlAttributes is Dictionary<string, object>)
            {
                return htmlAttributes as Dictionary<string, Object>;
            }
            var result = new System.Collections.Generic.Dictionary<string, Object>(StringComparer.OrdinalIgnoreCase);
            if (htmlAttributes != null)
            {
                var ps = htmlAttributes.GetType().GetProperties();
                foreach (var property in ps)
                {
                    result.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes, null));
                }
            }
            return result;
        }
        //----------------
        public string Partial(string virtualPath, TModel model, Dictionary<string, object> viewData)
        {
            string content = RenderUtil.RenderAspx(_context, virtualPath, model, viewData);
            return content;
        }
        public string Partial(string virtualPath)
        {
            string content = RenderUtil.RenderAspx(_context, virtualPath, this.Model, this.ViewData);
            return content;
        }
        public string Action<TController>(string action) where TController : BaseController
        {
            var url = UrlUtil.Action<TController>(action);
            HttpWebRequestHelper helper = new HttpWebRequestHelper();
            return helper.GetContent(url);
        }
        public string Action<TController>(string action, object datas) where TController : BaseController
        {
            var dic = AnonymousObjectToHtmlAttributes(datas);
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
            HttpWebRequestHelper helper = new HttpWebRequestHelper();
            return helper.GetContent(url);
        }
        public MvcForm BeginForm<TController>(string action, RequestType formType) where TController : BaseController
        {
            return BeginForm<TController>(action, formType, null);
        }
        public MvcForm BeginForm<TController>(string action, RequestType formType, object htmlAttributes) where TController : BaseController
        {
            var url = UrlUtil.Action<TController>(action);
            var dic = MHtmlHelper<object>.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var name = typeof(TController).Name + "_form";
            if (dic.ContainsKey("id") == false)
            {
                dic["id"] = name;
                if (dic.ContainsKey("name") == false)
                {
                    dic["name"] = name;
                }
            }
            else
            {
                if (dic.ContainsKey("name") == false)
                {
                    dic["name"] = dic["id"];
                }
                else
                {
                    dic["name"] = name;
                }
            }
            string otherInfo = ConvertIDictionaryToHtml(dic);
            string formInfo = "<form   method='{0}' action='{1}'  {2}>"+Environment.NewLine;
            formInfo = string.Format(formInfo, formType.ToString(),url, otherInfo);
            var ret = new MvcForm(formInfo,_context);
            return ret;
        }
    }
    public  class MvcForm : IDisposable
    {
        public MvcForm(string formInfo, System.Web.HttpContext context)
        {
            _context = context;
            _context.Response.Write(formInfo);
        }
        System.Web.HttpContext _context;
        static string ConvertIDictionaryToHtml(IDictionary<string, object> htmlAttributes)
        {
            StringBuilder sb = new StringBuilder();
            if (htmlAttributes != null)
            {
                foreach (var kvp in htmlAttributes)
                {
                    sb.Append(kvp.Key + "='" + System.Web.HttpUtility.HtmlEncode(kvp.Value) + "' ");
                }
            }
            return sb.ToString();
        }
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    string info = "</form>" + Environment.NewLine;
                    _context.Response.Write(info);
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~MvcForm() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

}
