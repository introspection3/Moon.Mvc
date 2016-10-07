/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/9/3
 * 时间: 15:33
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using Moon.Orm;

namespace Moon.Mvc
{
    /// <summary>
    /// Controller的xml描述
    /// </summary>
    public class ControllerXml
    {

        public ControllerXml()
        {
            Actions = new List<ActionXml>();
        }
        /// <summary>
        /// ControllerFullName
        /// </summary>
        public string ControllerFullName
        {
            get;
            set;
        }
        public List<ActionXml> Actions
        {
            get;
            set;
        }


    }
    public class ActionXml
    {
        public ActionXml()
        {
            Roles = new List<string>();
        }
        public string Name
        {
            get;
            set;
        }
        public List<string> Roles
        {
            get;
            set;
        }
    }
}
