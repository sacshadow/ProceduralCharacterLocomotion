using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class SDGUI {
	public static SDGUIManager Create(int layer){
		GameObject go = new GameObject("SDGUIManager");
		go.layer = layer;
		return go.AddComponent<SDGUIManager>();
	}
	
	public static SDGUILayer AddLayer(string name){
		GameObject go = new GameObject(name);
		SDGUILayer rt = go.AddComponent<SDGUILayer>();
		SDGUIManager.Instance.AddLayer(rt);
		return rt;
	}
	
	
	
}
