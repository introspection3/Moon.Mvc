/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/9/4
 * 时间: 11:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using FluentValidation;
using Moon.Mvc;
using FluentValidation.Attributes;
using FluentValidation.Validators;
namespace Moon.Mvc
{  
    /// <summary>
    /// 验证器,直接用在action的参数上
    /// ,记得引用fluent
    /// </summary>
    public class ValidatorAttribute:MoonBaseFluent
	{
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="type">Fluent的验证类,如,UserValidator : AbstractValidator&ltuser&gt</param>
        /// <param name="fluentValidateResultType">错误默认返回格式,如果是json格式出现错误则终止
        /// 如果是数据格式,则交用户自己解决</param>
        public ValidatorAttribute(Type type,ErrorResultReturnType fluentValidateResultType)
			:base(type,fluentValidateResultType)
		{
			
		}
        /// <summary>  
        /// 构造
        /// </summary>
        /// <param name="type">
        /// Fluent的验证类,如,UserValidator : AbstractValidator&ltuser&gt
        /// 错误信息默认会以json格式返回,发生错误后直接停止后续执行
        /// </param>
        public ValidatorAttribute(Type type)
			:base(type,ErrorResultReturnType.Json)
		{
			
		}
        /// <summary>
        /// 验证指定的实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>该实体的验证结果</returns>
		public override ValidationResult Validate(object entity)
		{
			var name=_customerValidatorType.FullName;
			var customerInstance=_customerValidatorType.Assembly.CreateInstance
				(_customerValidatorType.FullName);
			dynamic ci=customerInstance;
			dynamic ent=entity;
			var f=ci.Validate(ent);
			ValidationResult ret=new ValidationResult();
			ret.EntityName=entity.GetType().Name;
			ret.Errors=new List<ValidationFailure>();
			if (f.Errors!=null&&f.Errors.Count>0) {
				foreach (var e in f.Errors) {
					ValidationFailure mee=new ValidationFailure();
					mee.AttemptedValue=e.AttemptedValue;
					mee.CustomState=e.CustomState;
					mee.ErrorMessage=e.ErrorMessage;
					mee.PropertyName=e.PropertyName;
					ret.Errors.Add(mee);
				}
			}
			return ret;
		}
 
	}
}
