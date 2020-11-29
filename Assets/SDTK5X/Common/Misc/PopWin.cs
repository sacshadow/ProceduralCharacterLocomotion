using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

namespace SDTK.GUI {
	public class PopWin {
	
		public string title;
		public Rect rect;
		
		public Vector3 scroll;
		
		private static Rect GetPos(int x, int y) {
			return new Rect(Screen.width/2 - x/2, Screen.height/2-y/2, x, y);
		}
		
		public PopWin() {
			title = GetType().ToString();
			rect = GetPos(400, 300);
		}
		
		public void SetRect(Rect newRect) {
			var r = newRect;
			r.x = Mathf.Max(0,r.x);
			r.y = Mathf.Max(0,r.y);
			r.x = Mathf.Min(r.x, Screen.width-r.width);
			r.y = Mathf.Min(r.y, Screen.height-r.height);
			rect = r;
		}
		
		public void Set(string title, int x, int y) {
			this.title = title;
			rect = GetPos(x,y);
		}
		
		public void Set(int x, int y) {
			rect = GetPos(x,y);
		}
		
		public void Set(string title) {
			this.title = title;
		}
		
		public virtual void OnEnable() {
			
		}
		
		public virtual void OnGUI(int id) {
			scroll = GUILayout.BeginScrollView(scroll);
				ShowGUI();
			GUILayout.EndScrollView();
			
			UnityEngine.GUI.DragWindow(new Rect(0,0,rect.width, rect.height));
		}
		
		public virtual void ShowGUI() {
		
		}
		
		public virtual void OnClose() {
			
		}
		
	}
}
