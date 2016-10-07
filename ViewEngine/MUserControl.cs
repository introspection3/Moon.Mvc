using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.CodeDom;
using System.Web.UI;


namespace Moon.Mvc
{
	/// <summary>
	/// 一个“用户控件”基类
	/// </summary>
	[FileLevelControlBuilder(typeof(ViewUserControlControlBuilder))]
	public class MUserControl : System.Web.UI.UserControl
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
    }


	internal sealed class ViewUserControlControlBuilder : FileLevelUserControlBuilder
	{
		internal string UserControlBaseType
		{
			get;
			set;
		}

		public override void ProcessGeneratedCode(
			CodeCompileUnit codeCompileUnit,
			CodeTypeDeclaration baseType,
			CodeTypeDeclaration derivedType,
			CodeMemberMethod buildMethod,
			CodeMemberMethod dataBindingMethod)
		{

			// 如果分析器找到一个有效的类型，就使用它。
			if( UserControlBaseType != null ) {
				derivedType.BaseTypes[0] = new CodeTypeReference(UserControlBaseType);
			}
		}
	}
	


}
