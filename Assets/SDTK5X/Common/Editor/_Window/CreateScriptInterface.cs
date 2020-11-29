using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateScriptInterface : EditorWindow {
	
	[MenuItem("My Editor/CreateScriptInterface")]
	public static void OpenWindow(){
		EditorWindow.GetWindow(typeof(CreateScriptInterface));
	}
	
	void OnGUI(){
		
	}
	
}

