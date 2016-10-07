/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/9/5
 * 时间: 14:41
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Moon.Mvc
{
	/// <summary>
	///  向Moon.Mvc注册程序集
	/// </summary>
	public class ControllerAssmeblyUtil
	{
		/// <summary>
		/// 通过该程序集中的一个类型(只要是当前这个程序集中的类就可以了)来辅助注册
		/// </summary>
		public static Assembly Register<T>(){
			var asssmbly=typeof(T).Assembly;
			string path=asssmbly.Location.ToLower();
			lock(PATH_ASSEMBLY_MAP_LOCK){
				if (PATH_ASSEMBLY_MAP.ContainsKey(path)==false) {
					PATH_ASSEMBLY_MAP[path]=asssmbly;
				}
			}
			return asssmbly;
		}
		static readonly Dictionary<string,Type> PATH_TYPE_MAP=new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
		static readonly object PATH_TYPE_MAP_LOCK=new object();
		/// <summary>
		/// 通过type名称获取所有注册了的类型
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Type CreateType(string name){
			lock(PATH_TYPE_MAP_LOCK){
				if (PATH_TYPE_MAP.ContainsKey(name)) {
					return PATH_TYPE_MAP[name];
				}
			}
			var assemblies=PATH_ASSEMBLY_MAP.Values;
			Type ret=null;
			foreach (var ass in assemblies) {
				ret=ass.GetType(name);
				if (ret!=null) {
					break;
				}
			}
			if (ret!=null) {
				lock(PATH_TYPE_MAP_LOCK){
					return PATH_TYPE_MAP[name]=ret;
				}
			}
			return ret;
		}
		static readonly Dictionary<string,Assembly> CLASSNAME_ASSEMBLY_MAP=new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);
		static readonly object CLASSNAME_ASSEMBLY_MAP_LOCK=new object();
		public static object CreateInstance(string classFullName){
			lock(CLASSNAME_ASSEMBLY_MAP_LOCK){
				if (CLASSNAME_ASSEMBLY_MAP.ContainsKey(classFullName)) {
					return CLASSNAME_ASSEMBLY_MAP[classFullName].CreateInstance(classFullName);
				}
			}
			var assemblies=PATH_ASSEMBLY_MAP.Values;
			object ret=null;
			foreach (var ass in assemblies) {
				ret=ass.CreateInstance(classFullName);
				if (ret!=null) {
					lock(CLASSNAME_ASSEMBLY_MAP_LOCK){
						CLASSNAME_ASSEMBLY_MAP[classFullName]=ass;
					}
					break;
				}
			}
			return ret;
		}
		static readonly Dictionary<string,Assembly> PATH_ASSEMBLY_MAP=new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);
		static readonly object PATH_ASSEMBLY_MAP_LOCK=new object();
        /// <summary>
        /// 通过该程序集的路径进行注册.
        /// </summary>
        /// <param name="assemblyFullPath">程序集的全路径</param>
        public static Assembly RegisterByAssmblyPath(string assemblyFullPath){
			lock(PATH_ASSEMBLY_MAP_LOCK){
				if (PATH_ASSEMBLY_MAP.ContainsKey(assemblyFullPath)) {
					return PATH_ASSEMBLY_MAP[assemblyFullPath];
				}
			}
			var asssmbly=Assembly.LoadFrom(assemblyFullPath);
			lock(PATH_ASSEMBLY_MAP_LOCK){
				PATH_ASSEMBLY_MAP[assemblyFullPath]=asssmbly;
			}
			return asssmbly;
		}
	}
}
