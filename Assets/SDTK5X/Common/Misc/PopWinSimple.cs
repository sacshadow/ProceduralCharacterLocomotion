using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

namespace SDTK.GUI {
	public class PopWinSimple : PopWin {
	
		public Action GUIDisplay;
		public Action CloseCallback;
		
		public static void Display(Action GUIDisplay, Action CloseCallback = null) {
			PopWinManager.Show(new PopWinSimple{GUIDisplay = GUIDisplay, CloseCallback = CloseCallback});
		}
		
		public static void Display(int width, int height, Action GUIDisplay, Action CloseCallback = null) {
			var pop = new PopWinSimple{GUIDisplay = GUIDisplay, CloseCallback = CloseCallback};
			pop.Set(width, height);
			PopWinManager.Show(pop);
		}
		
		public static void Display(string title, int width, int height, Action GUIDisplay, Action CloseCallback = null) {
			var pop = new PopWinSimple{GUIDisplay = GUIDisplay, CloseCallback = CloseCallback};
			pop.Set(title, width, height);
			PopWinManager.Show(pop);
		}
		
		public static void Display(string title, Action GUIDisplay, Action CloseCallback = null) {
			var pop = new PopWinSimple{GUIDisplay = GUIDisplay, CloseCallback = CloseCallback};
			pop.Set(title);
			PopWinManager.Show(pop);
		}
		
		public override void OnGUI(int id) {
			scroll = GUILayout.BeginScrollView(scroll);
				GUIDisplay();
			GUILayout.EndScrollView();
			
			GUILayout.FlexibleSpace();
			
			GUILayout.Space(10);
			
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			if(GUILayout.Button("Close"))
				PopWinManager.Close();
			
			GUILayout.EndHorizontal();
			
			UnityEngine.GUI.DragWindow(new Rect(0,0,rect.width, rect.height));
		}
		
		public override void OnClose() {
			if(CloseCallback != null)
				CloseCallback();
		}
		
	}
}
