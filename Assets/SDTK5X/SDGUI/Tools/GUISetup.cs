using UnityEngine;
using System.Collections;


namespace SDTK.GUI.Tools{
	public static class GUISetup {
		
		public static void CreateGUI(){
			GameObject go = new GameObject("SDGUIManager");
			go.AddComponent<SDGUIManager>();
			
		}
		
		public static SDGUILayer GetLayer(string name, SDGUIManager manager){
			GameObject go = new GameObject(name);
			SDGUILayer rt = go.AddComponent<SDGUILayer>();
			return rt;
		}
		
	}
}
