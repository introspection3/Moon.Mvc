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

    /// <summary>
    /// Description of IFluentAPI.
    /// </summary>
    public abstract class MoonBaseFluent : Attribute
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="customerValidatorType">自定义的fluent验证类</param>
        /// <param name="errorResultReturnType">发生错误的返回数据格式类型</param>
        public MoonBaseFluent(Type customerValidatorType, ErrorResultReturnType errorResultReturnType)
        {
            _customerValidatorType = customerValidatorType;
            FluentValidateResultType = errorResultReturnType;
        }
        protected Type _customerValidatorType;
        public ErrorResultReturnType FluentValidateResultType
        {
            get;
            set;
        }
        public virtual ValidationResult Validate(object entity)
        {
            return null;
        }
    }
}
