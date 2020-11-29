using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;
using UnityLogic;
using UnityLogic.UI;

public class LogicEditorGUI : MonoBehaviour {
	// public static string newDescription = "(logic (tag \"New Tag\" (node 0 0 (\"New-Topic\"))))";
	
	// public LogicCanvas logicCanvas;
	
	public LogicGUI logicGUI;
	
	// Use this for initialization
	void Start () {
		logicGUI = new LogicGUI();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		// GUI.color = Color.black;
	
		logicGUI.CheckEventBefore();
		logicGUI.OnGUI(Screen.width, Screen.height);
		logicGUI.CheckEventAfter();
		
		// GUI.color = Color.white;
		
		// if(logicCanvas == null) {
			// logicCanvas = new LogicCanvas(Reader.Read_Input(newDescription));
			// logicCanvas.canvasName = "New Logic";
		// }
	
		// logicCanvas.Draw(new Rect(0,0,Screen.width, Screen.height));
	}
}
