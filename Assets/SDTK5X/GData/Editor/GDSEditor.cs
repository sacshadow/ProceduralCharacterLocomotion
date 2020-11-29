using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using SDTK;

// namespace SDTK.GenerateDataScript {
	public static class GDSEditor {
		
		
		public static TextAsset classTemplate, envTemplate;
		public static string savePath = "";
		
		[MenuItem("SDTK/Cfg/Clean Generate Data Scripts path")]
		public static void Clean() {
			EditorCfg.CleanPath("GDS Template Path");
			EditorCfg.CleanPath("GDS Generate Path");
		}
		
		
		[MenuItem("SDTK/Generate Data Scripts")]
		public static void GDS() {
			GenerateDataScript.CreateScript();
			
			AssetDatabase.Refresh();
		}
		
		
		public static void Init() {
			var tempPath = EditorCfg.GetRelativePath("GDS Template Path");
			savePath = EditorCfg.GetRelativePath("GDS Generate Path");
			
			classTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>(tempPath + "/DataClass.txt");
			envTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>(tempPath + "/DataEnvPush.txt");
		}
		
		
		
	}
// }
