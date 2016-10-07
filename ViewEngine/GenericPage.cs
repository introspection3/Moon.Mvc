using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace Moon.Mvc
{
    /// <summary>
    /// 泛型Model页面
    /// </summary>
    /// <typeparam name="TModel">model的类型</typeparam>
    public class GenericPage<TModel> : MViewPage
    {
        public TModel Model
        {
            get;
            set;
        }

        public override void SetModel(object model)
        {
            if (model != null)
            {
                this.Model = (TModel)model;
                _ModelType = model.GetType();
                Html = new MHtmlHelper<TModel>(this.Model, this.Context);
            }
            else
            {
                this.Model = default(TModel);
                _ModelType = null;
            }
        }
        public MHtmlHelper<TModel> Html
        {
            get;
            set;
        }
    }
}
