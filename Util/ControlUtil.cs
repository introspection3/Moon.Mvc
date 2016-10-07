/*
 * 由SharpDevelop创建。
 * 用户： qsmy_
 * 日期: 2015/10/31
 * 时间: 20:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using Moon.Orm;
using Moon.Orm.Util;

namespace Moon.Mvc
{
	/// <summary>
	/// 控件辅助类
	/// </summary>
	public class ControlUtil
	{
		public static string SelectByEnum<T>(string id,string name,bool setDefaultValue,object defaultValue)
		{
			StringBuilder sb=new StringBuilder();
			sb.AppendLine("<select id='"+id+"' name='"+name+"'>");
			string defaultValueStr=((int)defaultValue).ToString();
			foreach (var v in Enum.GetValues(typeof(T)))
			{
				var des=EnumUtil.GetEnumDescription((Enum) v);
				string value=((int)v).ToString();				
				if (des.AttachValue!=null) {
					value=des.AttachValue.ToString();
				}
				string text=des.Description;
				if (setDefaultValue==false) {
					sb.AppendLine("<option value='"+value+"'>"+text+"</option>");
				}else{
					if(defaultValueStr==value)
						sb.AppendLine("<option value='"+defaultValueStr+"' selected='selected'>"+text+"</option>");
					else{
						sb.AppendLine("<option value='"+value+"'>"+text+"</option>");
					}
				}
			}
			sb.AppendLine("</select>");
			return sb.ToString();
		}
		
		public static string SelectByEnum<T>(string id,string name)
		{
			return SelectByEnum<T>(id,name,false,null);
		}
		
	}
}
