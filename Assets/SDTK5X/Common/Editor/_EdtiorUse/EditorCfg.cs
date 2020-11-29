/*
	编辑器配置
*/
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SDTK{

	public static class EditorCfg {
		public static readonly string  dataPath=Application.dataPath+"/SDTK/";
		
		private static void CheckDirectory(){
			DataRW.CheckDirectory(dataPath);
		}
		
		public static bool IsDataExists(string cfgName){
			CheckDirectory();
			return DataRW.IsDataExists(dataPath+cfgName);
		}
		
		public static void SaveData<T>(T data, string cfgName){
			SaveData<T>(data, cfgName, cfgName);
		}
		public static void SaveData<T>(T data, string cfgPath, string cfgName){
			CheckDirectory();
			DataRW.SetClassToXML<T>(data,dataPath+cfgPath+"/Data/"+cfgName+".xml");
		}
		public static T LoadData<T>(string cfgName){
			return LoadData<T>(cfgName, cfgName);
		}
		public static T LoadData<T>(string cfgPath, string cfgName){
			CheckDirectory();
			return DataRW.GetClassFromXML<T>(dataPath+cfgPath+"/Data/"+cfgName+".xml");
		}
		
		public static string AP2RP(string absolutePath) {
			var temp = Application.dataPath;
			return absolutePath.Replace(temp.Substring(0,temp.LastIndexOf("/")+1),"");
		}
		
		public static string RP2AP(string relativePath) {
			var temp = Application.dataPath;
			return temp.Substring(0,temp.LastIndexOf("/")+1) + relativePath;
		}
		
		public static string GetRelativePath(string key) {
			string pathRoot = Application.dataPath.Substring(0,Application.dataPath.LastIndexOf("/"));
			var rt = EditorPrefs.GetString(key, "");
			if(rt == "" || !Directory.Exists(RP2AP(rt)))
				rt = SelectPath(key, pathRoot);
			
			Debug.Log("GetPath - " + key + " : "+ rt);
			return rt;
		}
		
		public static string GetAbsolutePath(string key) {
			return RP2AP(GetRelativePath(key));
		}
		
		public static void CleanPath(string key) {
			EditorPrefs.SetString(key,"");
		}
		
		public static string GetNewPath(string key, string pathRoot) {
			return EditorUtility.OpenFolderPanel(key,pathRoot,"");
		}
		
		public static string SaveFile(string key, string defName, string ext) {
			var pr = EditorPrefs.GetString(key);
			var rt = EditorUtility.SaveFilePanel(key, pr, defName, ext);
			EditorPrefs.SetString(key, rt);
			Debug.Log("Save File to path : " + rt);
			return rt;
		}
		
		public static string SaveFileInProject(string key, string defName, string ext) {
			string pathRoot = Application.dataPath.Substring(0,Application.dataPath.LastIndexOf("/"));
			var pr = EditorPrefs.GetString(key);
			if(pr == "" || !Directory.Exists(RP2AP(pr)))
				pr = "";
			
			var rt = EditorUtility.SaveFilePanel(key, pr, defName, ext);
			
			if(rt == null || rt == "")
				return "";
			
			if(!rt.Contains(pathRoot))
				throw new System.Exception("Must select path in project folder");
			
			rt = rt.Substring(pathRoot.Length+1);
			pr = rt.Substring(0, rt.LastIndexOf("/"));
			
			EditorPrefs.SetString(key, pr);
			Debug.Log("Save File to path : " + rt);
			
			return rt;
		}
		
		public static void CreateAsset<T>(string key, T asset, string ext) where T : ScriptableObject {
			var path = SaveFileInProject(key, asset.name, ext);
			if(path == null || path == "")
				return;
			
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
		}
		
		private static string SelectPath(string key, string pathRoot) {
			var rt = "";
			while(rt == "") {
				var select = GetNewPath(key, pathRoot);
				if(select == "")
					throw new Exception("Process canceled");
				if(!select.Contains(pathRoot)  
					&& !EditorUtility.DisplayDialog("Select path error", "Please select a folder inside project.","Conform","Cancel"))
				{
					throw new Exception("Process canceled");
				}
				else
					rt = AP2RP(select);
			}
			EditorPrefs.SetString(key, rt);
			return rt;
		}
		
	}
}