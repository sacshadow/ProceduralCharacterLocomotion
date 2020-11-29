using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

using SDTK;

namespace SDTK.Data {
	public static class StaticAsConfig  {
		public class GameTextInfo {
			public string className;
			public string paramType;
			public string paramName;
		}
		
		public static List<GameTextInfo> textField;
		
		[MenuItem("SDTK/Generate Config Script")]
		public static void GenerateConfig() {
			textField = new List<GameTextInfo>();
			SACTool.ForEachClass(PraseClassInFolder());
			ExportGameText();
			textField.Clear();
			textField = null;
			
			AssetDatabase.Refresh();
			Debug.Log("finish");
		}
		
		public static Action<Type> PraseClassInFolder() {
			var scriptFolderPath = EditorCfg.GetRelativePath("Script Root Folder");
			var exportFolderPath = EditorCfg.GetRelativePath("SAC Config Export");
			var templatePath = EditorCfg.GetRelativePath("SAC Template") + "/SACTemplate.txt";
			var template = AssetDatabase.LoadAssetAtPath(templatePath, typeof(TextAsset)) as TextAsset;
			
			var dir = new DirectoryInfo(scriptFolderPath);
			var classList = new List<string>();
			
			var supportType = new List<Type> {
				typeof(int),
				typeof(int[]),
				typeof(List<int>),
				typeof(float),
				typeof(float[]),
				typeof(List<float>),
				typeof(bool),
				typeof(bool[]),
				typeof(List<bool>),
				typeof(Color),
				typeof(Color[]),
				typeof(List<Color>),
			};
			
			var textType = new List<Type> {
				typeof(string),
				typeof(string[]),
				typeof(List<string>),
			};
			
			GetClassName(dir, classList);
			
			return type=> {
				if(classList.Contains(type.Name)) {
					GenerateScript(type, exportFolderPath, supportType, template.text);
					PushGameText(type, textType);
				}
			};
			
		}
		
		public static void GetClassName(DirectoryInfo dir, List<string> list) {
			list.AddRange(dir.GetFiles().Where(x=>x.Extension == ".cs").Select(x=>x.Name.Substring(0, x.Name.IndexOf("."))));
			foreach(var d in dir.GetDirectories()) GetClassName(d, list);
		}
		
		public static void ExportGameText() {
			// var className = "GameText";
		}
		
		
		public static void PushGameText(Type type, List<Type> textType) {
			var className = type.FullName;
			
			var field = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField).Where(x=> textType.Contains(x.FieldType) && !x.IsLiteral && !x.IsInitOnly);
			
			foreach(var f in field) {
				textField.Add(new GameTextInfo {
					className = className,
					paramType = f.FieldType.ToString(),
					paramName = f.Name});
			}
		}
		
		public static void GenerateScript(Type type, string path, List<Type> supportType, string template) {
			var field = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField).Where(x=>supportType.Contains(x.FieldType) && !x.IsLiteral && !x.IsInitOnly).ToList();
			if(field.Count == 0)
				return;
			
			var classNameTxt = type.FullName.Replace(".","_") + "_config";
			var classNameOrg = type.FullName;
			var param = new List<string>();
			var saveParam = new List<string>();
			var loadParam = new List<string>();
			
			foreach(var f in field) {
				var fieldName = f.Name;
				param.Add("public " + f.FieldType + " " + f.Name + ";");
				saveParam.Add(fieldName + " = " + classNameOrg+"."+fieldName);
				loadParam.Add(classNameOrg + "." + fieldName + " = " + "config." + fieldName + ";");
			}
			
			DataRW.CreateFile(
				template
					.Replace("#ConfigName", classNameTxt)
					.Replace("#Param", String.Join("\n\t", param.ToArray()))
					.Replace("#SaveParam", String.Join(",", saveParam.ToArray()))
					.Replace("#LoadParam", String.Join("\n\t\t", loadParam.ToArray())),
				path + "/" + classNameTxt + ".cs");
		}
		
	}
}