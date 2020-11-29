using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
//using System.Linq;

public static class ReflectTool {
	//************************************************************************************//
	//Type t = Type.GetType("TypeName");
	//TypeName typeInstance = (TypeName)Activator.CreateInstance(t);
	
	//Object[]  constructParms  =  new  object[] {param1,param2...};
	//TypeName typeInstance = (TypeName)Activator.CreateInstance(t, constructParms);
	
	//static func
	//Delegate.CreateDelegate(type, methodInfo) as Action/Func; 
	//func
	//Delegate.CreateDelegate(type,object, methodInfo) as Action/Func;
	
	//call static func
	//Object[]  constructParms  =  new  object[] {param1,param2...};
	//Object return = methodInfo.Invoke(null,constructParms);
	//call func
	//Class c = new Class();
	//Object return = methodInfo.Invoke(c,constructParms);
	//************************************************************************************//

	public static void GetMethoidInfo<T,K>(Action<K,MethodInfo> Process) where T : Attribute where K : Attribute {
		GetClassType<T>((attr, type) => GetMethoidInfo<K>(type, Process));
	}
	
	public static void GetClassType<T>(Action<T,Type> Process) where T : Attribute {
		Assembly asm = Assembly.GetExecutingAssembly();
		foreach(Type type in asm.GetTypes()) {
			if(!type.IsClass)
				continue;
			
			foreach(Attribute attr in type.GetCustomAttributes(false)) {
				if(attr is T) {
					Process(attr as T, type);
					break;
				}
			}
		}
	}
	
	public static void GetMethoidInfo<T>(Type type, Action<T,MethodInfo> Process) where T : Attribute {
		foreach(MethodInfo mInfo in type.GetMethods()) {
			foreach(Attribute attr in Attribute.GetCustomAttributes(mInfo)) {
				if(attr is T) {
					Process(attr as T, mInfo);
					break;
				}
			}
		}
	}
	
}
