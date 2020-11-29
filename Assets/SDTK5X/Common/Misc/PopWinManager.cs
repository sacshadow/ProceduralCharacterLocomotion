using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

namespace SDTK.GUI {

	public class PopWinManager : InstanceBehaviour<PopWinManager> {
	
		
		public PopWin popWin;
		
		public static void Show(PopWin win) {
			win.OnEnable();
			Instance.popWin = win;
		}
		
		public static void Close() {
			if(Instance.popWin != null)
				Instance.popWin.OnClose();
			Instance.popWin = null;
		}
		
		void OnGUI() {
			if(popWin == null)
				return;
		
			popWin.SetRect(UnityEngine.GUI.Window(0,popWin.rect, popWin.OnGUI, popWin.title));
		}
		
	}
}
