using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

namespace SDTK.Data {
	public static class SACManager {
		
		public static void ExportConfig(string path) {
			var param = new object[]{path};
			ReflectTool.GetMethoidInfo<SAConfig,SACSave>((type,method)=>method.Invoke(null, param));
		}
		
		public static void ImportConfig(string path) {
			var param = new object[]{path};
			ReflectTool.GetMethoidInfo<SAConfig,SACLoad>((type,method)=>method.Invoke(null, param));
		}
		
		
	}
}
