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
using System.IO;
using System.Text;

namespace Moon.Mvc
{

    /// <summary>
    ///action的权限管理
    /// </summary>
    public class ActionRoleManager
    {
        static ActionRoleManager()
        {
            var dirPath = Moon.Orm.GlobalData.MOON_WORK_DIRECTORY_PATH +
                Moon.Orm.GlobalData.OS_SPLIT_STRING +
                "authen" + Moon.Orm.GlobalData.OS_SPLIT_STRING;

            string authenXmlPath = dirPath + "authen.config";
            if (File.Exists(authenXmlPath) == false)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\"?>");
                sb.AppendLine("<CommonFormAuthen>");
                sb.AppendLine("	<Controller controllerFullName=\"类全名\">");
                sb.AppendLine("		<Action name=\"Index\">Admin</Action>");
                sb.AppendLine("	</Controller>");
                sb.AppendLine("</CommonFormAuthen>");
                StreamWriter sw = new StreamWriter(authenXmlPath, false, System.Text.Encoding.UTF8);
                sw.Write(sb.ToString());
                sw.Close();
            }
            AUTHEN_CONFIG = Load(authenXmlPath);
        }
        public static List<ControllerXml> AUTHEN_CONFIG;
        public static Dictionary<string, List<string>> action_controller_roles = new Dictionary<string, List<string>>();
        static readonly object action_controller_roles_lock = new object();
        public static List<string> GetActionRoles(string action, string controller)
        {

            if (AUTHEN_CONFIG == null)
            {
                var dirPath = Moon.Orm.GlobalData.MOON_WORK_DIRECTORY_PATH +
                  Moon.Orm.GlobalData.OS_SPLIT_STRING +
                  "authen" + Moon.Orm.GlobalData.OS_SPLIT_STRING;

                string authenXmlPath = dirPath + "authen.config";
                if (File.Exists(authenXmlPath) == false)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<?xml version=\"1.0\"?>");
                    sb.AppendLine("<CommonFormAuthen>");
                    sb.AppendLine("	<Controller controllerFullName=\"类全名\">");
                    sb.AppendLine("		<Action name=\"Index\">Admin</Action>");
                    sb.AppendLine("	</Controller>");
                    sb.AppendLine("</CommonFormAuthen>");
                    StreamWriter sw = new StreamWriter(authenXmlPath, false, System.Text.Encoding.UTF8);
                    sw.Write(sb.ToString());
                    sw.Close();
                }
                AUTHEN_CONFIG = Load(authenXmlPath);
            }
            else {
                lock (action_controller_roles_lock)
                {
                    if (action_controller_roles.ContainsKey(action + controller))
                    {
                        return action_controller_roles[action + controller];
                    }
                }
                foreach (var contrl in AUTHEN_CONFIG)
                {
                    if (contrl.ControllerFullName == controller)
                    {
                        foreach (var act in contrl.Actions)
                        {
                            if (act.Name == action)
                            {
                                lock (action_controller_roles_lock)
                                {
                                    action_controller_roles[action + controller] = act.Roles;
                                }
                                return act.Roles;
                            }
                        }
                        break;
                    }
                }
            }
            List<string> list = new List<string>();
            return list;
        }
        /// <summary>
        /// 加载指定文件的路径的xml到 List ControllerXml 中
        /// </summary>
        /// <param name="fileFullPath">文件全路径</param></param>
        static List<ControllerXml> Load(string fileFullPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileFullPath);
            var list = doc.DocumentElement.ChildNodes;
            List<ControllerXml> ret = new List<ControllerXml>();
            foreach (XmlNode element in list)
            {
                ControllerXml data = new ControllerXml();
                string controllerFullName = element.Attributes["controllerFullName"].Value;
                data.ControllerFullName = controllerFullName;
                var allChildren = element.ChildNodes;
                foreach (XmlNode action in allChildren)
                {
                    var actionName = action.Attributes["name"].Value;
                    var rolesString = action.InnerText.Trim();
                    ActionXml act = new ActionXml();
                    act.Name = actionName;
                    if (Moon.Orm.Util.StringUtil.IsNullOrWhiteSpace(rolesString))
                    {
                        act.Roles = new List<string>();
                    }
                    else {
                        var roles = rolesString.Split(',');
                        act.Roles.AddRange(roles);
                    }
                    data.Actions.Add(act);
                }
                ret.Add(data);
            }
            return ret;
        }

    }
}
