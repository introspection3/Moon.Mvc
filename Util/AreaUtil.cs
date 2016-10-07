using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Moon.Mvc
{
    public static class AreaUtil
    {
        /// <summary>
        /// 获取指定控制器类型的area
        /// </summary>
        /// <param name="classType">控制器的类型</param>
        /// <param name="existArea">是否存在area</param>
        /// <returns></returns>
        public static string GetControllerAreaName(Type classType,out bool existArea)
        {
            var atts = classType.GetCustomAttributes(typeof(AreaAttribute), true);
            AreaAttribute resultAttr = null;
            if (atts.Length > 0)
            {
                resultAttr = atts[0] as AreaAttribute;
                existArea = true;
                return resultAttr.AreaName;
            }
            else
            {
                existArea = false;
                return null;
            }
        }
    }
}
