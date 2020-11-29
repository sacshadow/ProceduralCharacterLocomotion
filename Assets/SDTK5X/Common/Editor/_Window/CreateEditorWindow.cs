using UnityEngine;
using UnityEditor;
using System.Collections;
using SDTK;

public class CreateEditorWindow : EditorWindow {

	private string windowName="NewEditorWindow";
	private string menuPath="/SDTK/Editor";
	private bool isLegal=false;
	private string scriptInfo="";
	
	[MenuItem("SDTK/Common/CreateEditorWindow")]
	public static void OpenWindow(){
		EditorWindow.GetWindow(typeof(CreateEditorWindow));
	}
	
	void OnEnable(){
		DataRW.CheckDirectory(menuPath);
	}
	
	void OnGUI(){
		GUI.color = isLegal?Color.white : Color.red;
		windowName = EditorGUILayout.TextField("窗口类名",windowName);
		GUI.color = Color.white;
		windowName=windowName.Replace(" ","");
		
		if(GUI.changed)
			UpdateScriptInfo();
			
		
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Assets"+menuPath);
			
			if(GUILayout.Button("Select"))
				SelectPath();
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
		
		GUILayout.BeginHorizontal("box");
		GUILayout.Label(scriptInfo);
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		{
			if(GUILayout.Button("确定") && isLegal)
				CreateEditorScript();
			
			if(GUILayout.Button("取消"))
				Close();
		}
		GUILayout.EndHorizontal();
	}
	
	private void SelectPath(){
		string newPath = Application.dataPath+menuPath;
		newPath = EditorUtility.OpenFolderPanel("Select Editor Folder",newPath,"Editor");
		
		if(newPath.Contains("/Editor") && newPath.Contains(Application.dataPath))
			menuPath = newPath.Substring(Application.dataPath.Length);
	}
	
	private void CreateEditorScript(){
		if(windowName==null || windowName.Length<0)
			throw new System.Exception("Window name not assigned");
		
		UpdateScriptInfo();
		
		DataRW.CreateFile(scriptInfo, Application.dataPath+menuPath+"/"+windowName+".cs");
		AssetDatabase.Refresh();
	}
	
	private void UpdateScriptInfo(){
		isLegal = SDTK.Tools.IsLegalScriptName_CS(windowName);
		
		string s =""+
			"using UnityEngine;"+"\r\n"+
			"using UnityEditor;"+"\r\n"+
			"using System.Collections;"+"\r\n"+
			""+"\r\n"+
			"public class CreateEditorWindow : EditorWindow {"+"\r\n"+
			"	"+"\r\n"+
			"	"+"\r\n"+
			"	[MenuItem(\"My Editor/CreateEditorWindow\")]"+"\r\n"+
			"	public static void OpenWindow(){"+"\r\n"+
			"		EditorWindow.GetWindow(typeof(CreateEditorWindow));"+"\r\n"+
			"	}"+"\r\n"+
			"	"+"\r\n"+
			"	void OnGUI(){"+"\r\n"+
			"		"+"\r\n"+
			"	}"+"\r\n"+
			"	"+"\r\n"+
			"}"+"\r\n"+
			"\r\n";
		
		scriptInfo=s.Replace("CreateEditorWindow",windowName);
	}
	
}
