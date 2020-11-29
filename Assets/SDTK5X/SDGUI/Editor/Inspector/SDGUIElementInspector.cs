using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SDGUIElement))]
public class SDGUIElementInspector : Editor {

	private SDGUIElement guiElement;
	private Vector3 lastPosition;
	
	void OnEnable(){
		guiElement = target as SDGUIElement;
		lastPosition = guiElement.transform.localPosition;
	}
	
	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		
		if(guiElement.transform.localPosition != lastPosition)
			guiElement.RecalculatePosition();
		
		lastPosition = guiElement.transform.localPosition;
	}
	
}
