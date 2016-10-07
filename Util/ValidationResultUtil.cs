/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/9/4
 * 时间: 15:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace Moon.Mvc
{
	public class ValidationResultUtil{
		public static  ValidationResult OneErrorResult(string entityName,string propertyName,string error){
			var re=new ValidationResult();
			re.EntityName=entityName;
			ValidationFailure f=new ValidationFailure();
			f.PropertyName=propertyName;
			f.ErrorMessage=error;
			re.Errors.Add(f);
			return re;
		}
		public static  ValidationResult SuccessResult(){
			var re=new ValidationResult();
			re.EntityName="";
			return re;
		}
		public static  ValidationResult SuccessResult(object value){
			var re=new ValidationResult();
			re.EntityName=value.ToString();
			return re;
		}
	}
}
