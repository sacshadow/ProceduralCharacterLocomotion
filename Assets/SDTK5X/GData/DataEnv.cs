using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using SDTK;

public static class DataEnv {
	
	public static Dictionary<string,Func<GData>> Initialize;
	
	public static void InitEnv() {
		Debug.Log("DataEnv InitEnv");
		Initialize = new Dictionary<string,Func<GData>>();
		
		ReflectTool.GetMethoidInfo<SDTKData,DataEnvInit>((type,method)=>method.Invoke(null,null));
	}
	
}