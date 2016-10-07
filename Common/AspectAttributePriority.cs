using System;
using System.Reflection;

namespace Moon.Mvc
{
    /// <summary>
    /// 注入特性类优先级(如果默认则使用Common)
    /// </summary>
    public static class AspectAttributePriority
    {
        /// <summary>
        /// 最高级别,10000
        /// </summary>
        public const int Highest = 10000;
        /// <summary>
        /// 高级,1000
        /// </summary>
        public const int High = 1000;
        /// <summary>
        /// 通常所用的级别,100
        /// </summary>
        public const int Common = 100;
        /// <summary>
        /// 低级,10
        /// </summary>
        public const int Low = 10;
        /// <summary>
        /// 最低级,0
        /// </summary>
        public const int Lowest = 0;
    }
    
}
