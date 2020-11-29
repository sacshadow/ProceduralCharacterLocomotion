using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
//using System.Linq;

namespace SDTK.Data {
	public static class SACTool {
	
		public static void ForEachClass(Action<Type> Callback) {
			
			Assembly asm = Assembly.GetExecutingAssembly();
			foreach(Type type in asm.GetTypes()) {
				if(!type.IsClass)
					continue;
				Callback(type);
			}
		}
	}
}
