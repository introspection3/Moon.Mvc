using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace Moon.Mvc
{
	/// <summary>
	/// 基于用户控件的视图基类，这类用户控件只是用于呈现数据。
	/// </summary>
	/// <typeparam name="TModel">传递给用户控件呈现时所需的数据实体对象类型</typeparam>
	public class GenericUserControl<TModel> : MUserControl
	{
        public TModel Model
        {
            get;
            set;
        }

        public override void SetModel(object model)
        {
            this.Model = (TModel)model;
            if (model != null)
            {
                _ModelType = model.GetType();
                Html = new MHtmlHelper<TModel>(this.Model, this.Context);
            }
        }
        public MHtmlHelper<TModel> Html
        {
            get;
            set;
        }
    }



}
