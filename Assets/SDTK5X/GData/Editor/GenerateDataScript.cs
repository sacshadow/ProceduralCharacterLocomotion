using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using SDTK;

public class GenerateDataScript {
	public static Dictionary<string, ParamInfo> paramInfo;
	public static List<string> env;
	public static ParamInfo classParam;
	
	// public static GDSTemplate template;
	
	public static void Init() {
		// InitTemplate();
		
		GDSEditor.Init();
		
		env = new List<string>();
		paramInfo = new Dictionary<string, ParamInfo>();
		
		paramInfo.Add("int", new ParamInfo(
			"int #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadInt();"));
		paramInfo.Add("int[]", new ParamInfo(
			"int[] #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadIntArray();"));
		paramInfo.Add("List<int>", new ParamInfo(
			"List<int> #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadIntList();"));
		
		paramInfo.Add("bool", new ParamInfo(
			"bool #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadBool();"));
		
		paramInfo.Add("float", new ParamInfo(
			"float #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadFloat();"));
		paramInfo.Add("float[]", new ParamInfo(
			"float[] #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadFloatArray();"));
		paramInfo.Add("List<float>", new ParamInfo(
			"List<float> #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadFloatList();"));
		
		paramInfo.Add("string", new ParamInfo(
			"string #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadString();"));
		paramInfo.Add("string[]", new ParamInfo(
			"string[] #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadStringArray();"));
		paramInfo.Add("List<string>", new ParamInfo(
			"List<string> #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadStringList();"));
		
		classParam = new ParamInfo(
			"#paramType #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadClass<#paramType>();");
		paramInfo.Add("array", new ParamInfo(
			"#paramType[] #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadClassArray<#paramType>();",1));
		paramInfo.Add("list", new ParamInfo(
			"List<#paramType> #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadClassList<#paramType>();",1));
		paramInfo.Add("dict", new DictInfo(
			"Dictionary<int,#paramType> #paramName;",
			"writer.Write(#paramName);",
			"#paramName = reader.ReadClassDict<#paramType>(x=>x.#id);"));
	}
	
	
	// public static void InitTemplate() {
		// var path = EditorCfg.GetRelativePath("GDSTemplate");
		// template = AssetDatabase.LoadAssetAtPath(path + "/GDSTemplate.asset", typeof(GDSTemplate)) as GDSTemplate;
	// }
	
	public static void CreateScript() {
		Init();
		
		DataScript.Scripts();
		
		GenerateEnv();
		
		Debug.Log("Done");
		AssetDatabase.Refresh();
	}
	
	public static void GenerateEnv() {
		GenerateEnv("DataEnvPush");
	}
	
	public static void GenerateEnv(string envName) {
		string envInit = "";
		env.ForEach(x=>envInit += "\t\tDataEnv.Initialize.Add(\"" + x + "\", ()=>new " + x + "());\n");
		DataRW.CreateFile(GDSEditor.envTemplate.text.Replace("#ClassName",envName).Replace("#Init", envInit), GDSEditor.savePath + "/" + envName + ".cs");
	}
	
	public static void GClass(string classInfo, params string[] paramInfo) {
		var dataClass = new DataClass(classInfo);
		Loop.ForEach(paramInfo, PushParamInfo(dataClass));
		CreateScript(dataClass);
	}
	
	public static void CreateScript(DataClass classInfo) {
		string script = GDSEditor.classTemplate.text;
		string param = "", writer = "", reader = "";
		
		classInfo.dataParam.ForEach(x=> {
			param += "\tpublic " + x.DeclareParam() + "\n";
			writer += "\t\t" + x.WriteData() + "\n";
			reader += "\t\t" + x.ReadData() + "\n";
		});
		
		script = script.Replace("#DataType", classInfo.className).Replace("#InheritType", classInfo.classInert);
		script = script.Replace("#Params", param).Replace("#DataReader", reader).Replace("#DataWriter", writer);
		
		env.Add(classInfo.className);
		DataRW.CreateFile(script, GDSEditor.savePath + "/" + classInfo.className + ".cs");
	}
	
	public static Action<string> PushParamInfo(DataClass dataClass) {
		return pInfo => {
			var info = pInfo.Split(' ');
			if(paramInfo.ContainsKey(info[0]))
				dataClass.dataParam.Add(paramInfo[info[0]].Set(info));
			else
				dataClass.dataParam.Add(classParam.Set(info));
		};
	}	
	
}
