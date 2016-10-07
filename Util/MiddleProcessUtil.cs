/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/23
 * 时间: 12:57
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;

using Moon.Orm;

namespace Moon.Mvc
{
    public class AspectAttributeComparer : IComparer<AspectAttribute>
    {
        public int Compare(AspectAttribute x, AspectAttribute y)
        {
            var result = 0;
            if (x.Priority > y.Priority)
            {
                result = -1;
            }
            else if (x.Priority < y.Priority)
            {
                result = 1;
            }
            return result;
        }
    }
    public class MiddleProcessUtil
    {
        static bool ValidateMode(List<ValidationResult> resultList)
        {
            foreach (var r in resultList)
            {
                if (r.IsValid == false)
                {
                    return false;
                }
            }
            return true;
        }
        static void OrderByAspectAttributePriority(List<AspectAttribute> list)
        {
            var ret = list.ToArray();
            Array.Sort<AspectAttribute>(ret, new AspectAttributeComparer());
            list.Clear();
            list.AddRange(ret);
        }
        static bool ExistAttribute<T>(MethodInfo methodInfo)
        {
            var atts = methodInfo.GetCustomAttributes(typeof(T), true);
            return atts.Length > 0;
        }
        static int GetMethodSupportRequestType(MethodInfo methodInfo)
        {
            var get = ExistAttribute<GetAttribute>(methodInfo);
            var post = ExistAttribute<PostAttribute>(methodInfo);
            if (get && post)
            {
                return 3;
            }
            else if (get && !post)
            {
                return 1;
            }
            else if (!get && post)
            {
                return 2;
            }
            else
            {
                return 2;//某人提交方式POST
            }
        }
        /// <summary>
        /// moon所有的请求
        /// </summary>
        /// <param name="classFullName">类的完全限定名</param>
        /// <param name="methodName">方法名(只能是public)</param>
        /// <param name="context">当前的HttpContext</param>
        public static void Process(string classFullName, string methodName, HttpContext context)
        {
            var classType = ControllerAssmeblyUtil.CreateType(classFullName);
            var classInstance = ControllerAssmeblyUtil.CreateInstance(classFullName);
            if (classInstance == null)
            {
                Moon.Orm.Util.LogUtil.Error("classInstance null,classFullName:" + classFullName+"  "+context.Request.RawUrl);
                var err = "current request doesn't exist";
                throw new Exception(err);
            }
            if (classType == null)
            {
                var err = "current request doesn't exist";
                throw new Exception(err);
            }

            var methodInfo = classType.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (methodInfo == null)
            {
                return;
            }
            //=================
            BaseController baseControllerInstance = null;
            Dictionary<string, object> viewData = new Dictionary<string, object>();
            if (classInstance is BaseController)
            {
                BaseController obj = classInstance as BaseController;
                obj.CurrentHttpContext = context;
                viewData = obj.ViewData;
                baseControllerInstance = obj;
            }
            else
            {
                throw new Exception("请求的类没有继承BaseController");
            }
            var atts = methodInfo.GetCustomAttributes(typeof(ResultAttribute), true);
            ResultAttribute resultAttr = null;
            if (atts.Length > 0)
            {
                resultAttr = atts[0] as ResultAttribute;
            }
            else
            {
                resultAttr = new TemplateResultAttribute();//默认为模板类型
            }
            //------------
            object resultObj = null;
            //------------
            var customAspectAttributes = methodInfo.GetCustomAttributes(typeof(AspectAttribute), true);

            List<ValidationResult> resultList = new List<ValidationResult>();
            ErrorResultReturnType fluentValidateResultType = ErrorResultReturnType.Json;
            bool dontValidateRequest = ExistAttribute<DontValidateRequestAttribute>(methodInfo);
            int methodRequestType = GetMethodSupportRequestType(methodInfo);

            if (context.Request.RequestType == "GET")
            {
                if (methodRequestType == (int)RequestType.POST)
                {
                    var err = "Current request is GET,but you didn't set a GET attribute to your method :"+classFullName+"->"+methodName;
                    Moon.Orm.Util.LogUtil.Error(err);
                    throw new Exception(err);

                }
            }
            else
            {
                if (methodRequestType == (int)RequestType.GET)
                {
                    var err = "Current request is POST,but you didn't set a POST attribute to your method :"+classFullName+"->"+methodName;
                    Moon.Orm.Util.LogUtil.Error(err);
                    throw new Exception(err);
                }
            }

            object[] parameters = ParameterUtil.GetParametersObject(dontValidateRequest, context.Request, methodInfo, ref resultList, ref fluentValidateResultType);
            //---------验证参数----------
            if (ValidateMode(resultList) == false)
            {
                baseControllerInstance.IsModelValidate = false;
                if (fluentValidateResultType == ErrorResultReturnType.Json)
                {                    
                    var json = Moon.Orm.Util.JsonUtil.ConvertObjectToJson(resultList);
                    context.Response.Write(json);
                    context.Response.Flush();
                    //context.Response.End();//2015年9月13日14:03:43
                    return;
                }
                else if(fluentValidateResultType==ErrorResultReturnType.PureData)
                {                   
                    baseControllerInstance.ValidationResults = resultList;
                }
            }
            else
            {
                baseControllerInstance.IsModelValidate = true;
            }
            //
            if (customAspectAttributes.Length > 0)
            {
                List<AspectAttribute> aspectAttributeList = new List<AspectAttribute>();
                foreach (var oneAspect in customAspectAttributes)
                {
                    aspectAttributeList.Add(oneAspect as AspectAttribute);
                }
                OrderByAspectAttributePriority(aspectAttributeList);
                foreach (var oneAspect in aspectAttributeList)
                {
                    var aspectResult = oneAspect.BeforeInvoke(methodInfo, context);
                    if (aspectResult == AspectResultType.Stop)
                    {
                        return;
                    }
                }
                resultObj = MethodInvokeUtil.Invoke(classInstance, methodName, parameters.Length == 0 ? null : parameters);
                foreach (var oneAspect in aspectAttributeList)
                {
                    var rr = oneAspect.AfterInvoke(methodInfo, context);
                    if (rr == AspectResultType.Stop)
                    {
                        return;
                    }
                }
            }
            else
            {
                resultObj = MethodInvokeUtil.Invoke(classInstance, methodName, parameters.Length == 0 ? null : parameters);
            }
            resultAttr.Response(context, classFullName, methodName, resultObj, viewData);
        }
    }
}
