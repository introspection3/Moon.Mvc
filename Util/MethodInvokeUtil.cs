using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Web;

namespace Moon.Mvc
{
	public class MethodInvokeUtil
	{
		
		
		static readonly Dictionary<string,Type> _classFullNameTypeDic=new Dictionary<string, Type>();
		static object _classFullNameTypeDicLock=new object();
		
		
		static object _keyFastInvokeHandlerLock=new object();
		static readonly Dictionary<string,FastInvokeHandler> _keyFastInvokeHandler=new Dictionary<string, FastInvokeHandler>();
		
		public static object Invoke(object instance,string methodName,object[] paramters){
			var type=instance.GetType();
			string classFullName=instance.GetType().ToString();
			string key=classFullName+methodName;
			FastInvokeHandler handler=null;
			bool existKey=true;
			lock(_keyFastInvokeHandlerLock){
				if (_keyFastInvokeHandler.ContainsKey(key)) {
					handler=_keyFastInvokeHandler[key];
				}else{
					existKey=false;
				}
			}
			if (existKey==false) {
				var methodInfo=type.GetMethod(methodName,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
				if (methodInfo==null) {
					return null;
				}
				handler= GetMethodInvoker(methodInfo);
				lock(_keyFastInvokeHandlerLock){
					_keyFastInvokeHandler[key]=handler;
				}
			}
			try {
				return handler.Invoke(instance,paramters);
			} catch (Exception ex) {
				StringBuilder msg=new StringBuilder();
				msg.AppendLine("调用方法:"+classFullName+"-->"+methodName);
				msg.AppendLine("异常信息:"+ex.Message);
				msg.AppendLine("StackTrace:"+ex.StackTrace);
				string info=msg.ToString();
				Moon.Orm.Util.LogUtil.Error(info);
				throw new Exception(info);
			}
			
		}
		
		public static object Invoke(string classFullName,string methodName,object[] paramters){
			 
			string key=classFullName+methodName;
			FastInvokeHandler handler=null;
			bool existKey=true;
			lock(_keyFastInvokeHandlerLock){
				if (_keyFastInvokeHandler.ContainsKey(key)) {
					handler=_keyFastInvokeHandler[key];
				}else{
					existKey=false;
				}
			}
			if (existKey==false) {
				Type type=null;
				bool existTheType=false;
				lock(_classFullNameTypeDicLock){
					existTheType=_classFullNameTypeDic.ContainsKey(classFullName);
					if (existTheType) {
						type=_classFullNameTypeDic[classFullName];
					}
				}
				if (existTheType==false) {
					type=ControllerAssmeblyUtil.CreateType(classFullName);
					lock(_classFullNameTypeDicLock){
						_classFullNameTypeDic[classFullName]=type;
					}
				}
				var methodInfo=type.GetMethod(methodName);
				if (methodInfo==null) {
					return null;
				}
				handler= GetMethodInvoker(methodInfo);
				lock(_keyFastInvokeHandlerLock){
					_keyFastInvokeHandler[key]=handler;
				}
			}
			var obj=ControllerAssmeblyUtil.CreateInstance(classFullName);
			return handler.Invoke(obj,paramters);
		}
		public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
		{
			DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);
			ILGenerator il = dynamicMethod.GetILGenerator();
			ParameterInfo[] ps = methodInfo.GetParameters();
			Type[] paramTypes = new Type[ps.Length];
			for (int i = 0; i < paramTypes.Length; i++)
			{
				if (ps[i].ParameterType.IsByRef)
					paramTypes[i] = ps[i].ParameterType.GetElementType();
				else
					paramTypes[i] = ps[i].ParameterType;
			}
			LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];
			
			for (int i = 0; i < paramTypes.Length; i++)
			{
				locals[i] = il.DeclareLocal(paramTypes[i], true);
			}
			for (int i = 0; i < paramTypes.Length; i++)
			{
				il.Emit(OpCodes.Ldarg_1);
				EmitFastInt(il, i);
				il.Emit(OpCodes.Ldelem_Ref);
				EmitCastToReference(il, paramTypes[i]);
				il.Emit(OpCodes.Stloc, locals[i]);
			}
			if (!methodInfo.IsStatic)
			{
				il.Emit(OpCodes.Ldarg_0);
			}
			for (int i = 0; i < paramTypes.Length; i++)
			{
				if (ps[i].ParameterType.IsByRef)
					il.Emit(OpCodes.Ldloca_S, locals[i]);
				else
					il.Emit(OpCodes.Ldloc, locals[i]);
			}
			if (methodInfo.IsStatic)
				il.EmitCall(OpCodes.Call, methodInfo, null);
			else
				il.EmitCall(OpCodes.Callvirt, methodInfo, null);
			if (methodInfo.ReturnType == typeof(void))
				il.Emit(OpCodes.Ldnull);
			else
				EmitBoxIfNeeded(il, methodInfo.ReturnType);
			
			for (int i = 0; i < paramTypes.Length; i++)
			{
				if (ps[i].ParameterType.IsByRef)
				{
					il.Emit(OpCodes.Ldarg_1);
					EmitFastInt(il, i);
					il.Emit(OpCodes.Ldloc, locals[i]);
					if (locals[i].LocalType.IsValueType)
						il.Emit(OpCodes.Box, locals[i].LocalType);
					il.Emit(OpCodes.Stelem_Ref);
				}
			}
			
			il.Emit(OpCodes.Ret);
			FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
			return invoder;
		}
		
		private static void EmitCastToReference(ILGenerator il, System.Type type)
		{
			if (type.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, type);
			}
			else
			{
				il.Emit(OpCodes.Castclass, type);
			}
		}
		
		private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
		{
			if (type.IsValueType)
			{
				il.Emit(OpCodes.Box, type);
			}
		}
		
		private static void EmitFastInt(ILGenerator il, int value)
		{
			switch (value)
			{
				case -1:
					il.Emit(OpCodes.Ldc_I4_M1);
					return;
				case 0:
					il.Emit(OpCodes.Ldc_I4_0);
					return;
				case 1:
					il.Emit(OpCodes.Ldc_I4_1);
					return;
				case 2:
					il.Emit(OpCodes.Ldc_I4_2);
					return;
				case 3:
					il.Emit(OpCodes.Ldc_I4_3);
					return;
				case 4:
					il.Emit(OpCodes.Ldc_I4_4);
					return;
				case 5:
					il.Emit(OpCodes.Ldc_I4_5);
					return;
				case 6:
					il.Emit(OpCodes.Ldc_I4_6);
					return;
				case 7:
					il.Emit(OpCodes.Ldc_I4_7);
					return;
				case 8:
					il.Emit(OpCodes.Ldc_I4_8);
					return;
			}
			
			if (value > -129 && value < 128)
			{
				il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4, value);
			}
		}
	}
}
