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
	[Serializable]
	public class ValidationResult
	{
		
		public virtual bool IsValid
		{
			get
			{
				return _Errors.Count == 0;
			}
		}
        private IList<ValidationFailure> _Errors;
        /// <summary>
        /// 实体名
        /// </summary>
		public string EntityName
        {
			get;
			set;
		}

        public IList<ValidationFailure> Errors
        {
            get
            {
                return _Errors;
            }

            set
            {
                _Errors = value;
            }
        }

        public ValidationResult()
		{
            _Errors = new List<ValidationFailure>();
        }
		
	}

}
