using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.CodeDom;

namespace Moon.Mvc
{

	internal sealed class ViewPageControlBuilder : FileLevelPageControlBuilder
	{
		public string PageBaseType
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
			if( PageBaseType != null ) {
				derivedType.BaseTypes[0] = new CodeTypeReference(PageBaseType);
			}
		}
	}



	



}
