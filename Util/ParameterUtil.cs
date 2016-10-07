/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/23
 * 时间: 17:17
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Moon.Orm;
using Moon.Orm.Util;
using System.Reflection;
//using System.Web.Helpers;
namespace Moon.Mvc
{
    /// <summary>
    /// Description of ParameterUtil.
    /// </summary>
    public class ParameterUtil
    {
        /// <summary>
        /// 将Request中的QueryString和Form值统一转换为Dictionary
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertRequestToDictionary(HttpRequest Request, bool dontValidateRequest)
        {
            var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var a in Request.QueryString.AllKeys)
            {
                if (a != null)
                {
                    dic[a] = Request.QueryString[a];
                }
            }
            foreach (var a in Request.Form.AllKeys)
            {
                if (a != null)
                {
                    dic[a] = Request.Form[a];
                }
            }
            return dic;
        }
        /// <summary>
        /// 获取名字的前缀
        /// 如User.Name 返回User
        /// A.User.Name 返回A.User
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static string GetPre(string key)
        {
            int index = key.LastIndexOf('.');
            if (index == -1)
            {
                return string.Empty;
            }
            else
            {
                return key.Substring(0, index);
            }
        }
        /// <summary>
        /// 按照前缀的方式对dic分组
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> GroupDictonary(Dictionary<string, string> dic)
        {
            var ret = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in dic)
            {
                var pre = GetPre(kvp.Key);
                if (ret.ContainsKey(pre) == false)
                {
                    ret[pre] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                string name = kvp.Key;
                //A.B这样的格式
                if (string.IsNullOrEmpty(kvp.Key) == false)
                {
                    int index = name.LastIndexOf('.');
                    if (index > -1)
                    {
                        name = name.Substring(index + 1);
                    }
                }
                ret[pre][name] = kvp.Value;
            }
            return ret;
        }
        /// <summary>
        /// 通过变量实体属性和从dicdic中获取的方式，设置某个参数实体的值
        /// </summary>
        /// <param name="dicdic"></param>
        /// <param name="instance"></param>
        /// <param name="instanceType"></param>
        public static void SetEntityValueByDicDic(Dictionary<string, Dictionary<string, string>> dicdic,
                                                  object instance, ParameterInfo pinfo)
        {
            Type instanceType = pinfo.ParameterType;
            string parameterName = pinfo.Name;
            if (instanceType.IsClass)//必须为类
            {
                if (dicdic.Count == 0)
                {
                    return;
                }
                Dictionary<string, string> oneGroup = null;
                if (dicdic.ContainsKey(parameterName))//在对应的参数组里面查找
                {
                    oneGroup = dicdic[parameterName];
                }
                else
                {
                    oneGroup = dicdic[string.Empty];//散数据中查找
                }
                var instanceProperties = instanceType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in instanceProperties)
                {
                    var propertyName = p.Name;
                    if (p.PropertyType == typeof(string) || p.PropertyType.IsValueType)
                    {
                        if (oneGroup.ContainsKey(propertyName))
                        {
                            var vv = TypeUtil.ConvertTo(oneGroup[propertyName], p.PropertyType);
                            MethodInvokeUtil.Invoke(instance, "set_" + p.Name, new object[] { vv });
                        }
                    }
                    else
                    {
                       
                        if (p.GetValue(instance, null) == null)
                        {
                            var pobj =   Activator.CreateInstance(p.PropertyType, null);
                            p.SetValue(instance, pobj, null);
                            SetInstancePropertyValueByDicDic(dicdic, pobj, parameterName, p.Name);

                        }
                       
                    }
                }
            }
        }
        static void SetInstancePropertyValueByDicDic(Dictionary<string, Dictionary<string, string>> dicdic,
                                                  object instance,string belongParameterName,string propertyName)
        {
            //该属性为一个实体。
            var groupName = belongParameterName + "." + propertyName;
            if ("WhereExpression"==propertyName) {
            	return;
            }
            var currentGroup = dicdic[groupName];
            var pps = instance.GetType().GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            foreach(var p in pps)
            {
                if (p.PropertyType == typeof(string) || p.PropertyType.IsValueType)
                {
                    if (currentGroup.ContainsKey(p.Name))
                    {
                        var vv = TypeUtil.ConvertTo(currentGroup[p.Name], p.PropertyType);
                        MethodInvokeUtil.Invoke(instance, "set_" + p.Name, new object[] { vv });
                    }
                }
                else
                {
                    var pobj = Activator.CreateInstance(p.PropertyType, null);
                    p.SetValue(instance, pobj, null);

                    SetInstancePropertyValueByDicDic(dicdic, pobj, groupName, p.Name);
                }
            }
        }
        /// <summary>
        /// 将Request中的QueryString和Form值统一转换为实体对象
        /// </summary>
        /// <param name="Request">HttpRequest对象性</param>
        /// <typeparam name="T">对应的实体类</typeparam>
        /// <returns>实体对象</returns>
        public static T ConvertRequestToEntity<T>(HttpRequest Request, bool dontValidateRequest) where T : class, new()
        {
            var dic = ConvertRequestToDictionary(Request, dontValidateRequest);
            T result = new T();
            foreach (var kvp in dic)
            {
                MoonFastInvoker<T>.SetTValue(result, kvp.Key, kvp.Value);
            }
            return result;
        }

        /// <summary>
        /// 获取一个方法的参数对应的值(或实体)
        /// </summary>
        /// <param name="Request">HttpRequest对象</param>
        /// <param name="methodInfo">方法信息</param>
        /// <returns>参数对应的值</returns>
        public static Object[] GetParametersObject(bool dontValidateRequest, HttpRequest Request, MethodInfo methodInfo,
                                                   ref List<ValidationResult> validateResultList, ref ErrorResultReturnType errorResultReturnType)
        {
            List<object> list = new List<object>();
            var dic = ConvertRequestToDictionary(Request, dontValidateRequest);
            var dicdic = GroupDictonary(dic);
            var parameters = methodInfo.GetParameters();
            foreach (var para in parameters)
            {
                var paraType = para.ParameterType;
                var paraName = para.Name;
                if (paraType == typeof(string))
                {
                    object entity = string.Empty;
                    if (dicdic.ContainsKey(string.Empty))
                    {
                        foreach (var kvp in dicdic[string.Empty])
                        {
                            if (string.Equals(kvp.Key, paraName, StringComparison.OrdinalIgnoreCase))
                            {
                                entity = TypeUtil.ConvertTo(kvp.Value, paraType);
                                break;
                            }
                        }
                    }
                    list.Add(entity);
                }
                else if (paraType.IsClass)
                {
                    var entity = Activator.CreateInstance(paraType, null);
                    SetEntityValueByDicDic(dicdic, entity, para);
                    var paraAttributes = para.GetCustomAttributes(typeof(MoonBaseFluent), false);
                    if (paraAttributes != null && paraAttributes.Length > 0)
                    {
                        var paraAttribute = paraAttributes[0] as MoonBaseFluent;
                        errorResultReturnType = paraAttribute.FluentValidateResultType;
                        var validaterRsult = paraAttribute.Validate(entity);
                        validateResultList.Add(validaterRsult);
                    }
                    list.Add(entity);
                }
                else if (paraType.IsValueType)
                {
                    var entity = Activator.CreateInstance(paraType, null);
                    if (dicdic.ContainsKey(string.Empty))
                    {
                        foreach (var kvp in dicdic[string.Empty])
                        {
                            if (string.Equals(kvp.Key, paraName, StringComparison.OrdinalIgnoreCase))
                            {
                                entity = TypeUtil.ConvertTo(kvp.Value, paraType);
                                break;
                            }
                        }
                    }
                    list.Add(entity);
                }
            }
            return list.ToArray();
        }
    }
}
