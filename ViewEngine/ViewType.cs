using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.CodeDom;
using System.Collections;

namespace Moon.Mvc
{
	/// <summary>
	/// ViewType
	/// </summary>
	[ControlBuilder(typeof(ViewTypeControlBuilder))]
	[NonVisualControl]
	public class ViewType : Control
	{
		private string _typeName;

		/// <summary>
		/// TypeName
		/// </summary>
		[DefaultValue("")]
		public string TypeName
		{
			get
			{
				return _typeName ?? String.Empty;
			}
			set
			{
				_typeName = value;
			}
		}
	}


	internal sealed class ViewTypeControlBuilder : ControlBuilder
	{
		private string _typeName;

		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs)
		{
			base.Init(parser, parentBuilder, type, tagName, id, attribs);

			_typeName = (string)attribs["typename"];
		}

		public override void ProcessGeneratedCode(
			CodeCompileUnit codeCompileUnit,
			CodeTypeDeclaration baseType,
			CodeTypeDeclaration derivedType,
			CodeMemberMethod buildMethod,
			CodeMemberMethod dataBindingMethod)
		{

			// Override the view's base type with the explicit base type
			derivedType.BaseTypes[0] = new CodeTypeReference(_typeName);
		}
	}

}
