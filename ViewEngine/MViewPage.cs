using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace Moon.Mvc
{
    [FileLevelControlBuilder(typeof(ViewPageControlBuilder))]
    public class MViewPage : Page
    {
        protected Type _ModelType;
        protected object _Model
        {
            get;
            set;
        }
        public virtual void SetModel(object model)
        {
            if (model != null)
            {
                _ModelType = model.GetType();
                _Model = model;
            }
        }
        public virtual T Model<T>()
        {
        	
            if (_ModelType!=null&&_ModelType != typeof(T))
            {
                throw new Exception("Model的实际类型和你指定的类型不一致,Model实际类型为:" + _ModelType.FullName);
            }
            if (_Model == null)
            {
                return default(T);
            }
            return (T)_Model;
        }
        /// <summary>
        /// Model's Type
        /// </summary>
        public Type ModelType
        {
            get
            {
                return _ModelType;
            }
        }
        /// <summary>
        /// html编码
        /// </summary>
        /// <param name="html">需要编码的html</param>
        /// <returns></returns>
        public string HtmlEncode(string html){
        	return System.Web.HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="html">需要解码的html</param>
        /// <returns></returns>
         public string HtmlDecode(string html){
        	return System.Web.HttpUtility.HtmlDecode(html);
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
        private string _RootUrl;
        /// <summary>
        /// 当前网站的根地址
        /// </summary>
        public string RootUrl
        {
            internal set
            {
                _RootUrl = value;
            }
            get
            {
                return _RootUrl;
            }
        }
        
		 
    }



}
