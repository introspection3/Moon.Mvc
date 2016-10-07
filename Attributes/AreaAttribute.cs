using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moon.Mvc
{
    /// <summary>
    /// 用于修饰控制器域名
    /// </summary>
    public class AreaAttribute : Attribute
    {
        public string AreaName
        {
            get; set;
        }
        public AreaAttribute(string areaName)
        {
            this.AreaName = areaName;
        }
    }
}
