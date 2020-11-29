using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;
using UnityLogic;
using UnityLogic.UI;
using UnityLispEditor;

namespace UnityLogicEditor {
	public class BindParser : EditorWindow {
		
		public Vector2 scrollPosition;
		
		[MenuItem("UnityLogic/Bind Parser")]
		public static void OpenWindow() {
			GetWindow(typeof(BindParser));
		}
		
		// [MenuItem("UnityLogic/Bind Dialog")]
		// public static void BDDialog() {
			// ULispREPL.SetEnv("Dialog", DialogPopup.hookEnv);
		// }
		
		void OnGUI() {
			
			scrollPosition = GUILayout.BeginScrollView(scrollPosition); {
				
				foreach(var kvp in ULogic.parser) {
					if(GUILayout.Button(kvp.Key))
						BindEnv(kvp.Key, kvp.Value);
				}
				
			} GUILayout.EndScrollView();
		}
		
		private void BindEnv(string envName, List<Parser> parser) {
			ULispREPL.SetEnv(envName, parser[0].rootEnv);
			Close();
		}
		
	}
}
