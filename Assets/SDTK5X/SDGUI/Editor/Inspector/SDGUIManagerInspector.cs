using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(SDGUIManager))]
public class SDGUIManagerInspector : Editor {
	
	private SDGUIManager guiManager;
	
	void OnEnable(){
		guiManager = target as SDGUIManager;
	}
	
	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
	}
	
	void OnSceneGUI(){
		if( Selection.objects.Length > 1 )
			return;
		
		Event evt = Event.current;
		if(evt.alt)
			return;
		
		int id = GUIUtility.GetControlID( GetType().Name.GetHashCode(), FocusType.Passive );
		EventType eventType = evt.GetTypeForControl(id);
		
		if(eventType != EventType.MouseDown || evt.button != 1)
			return;
		
		Ray ray = HandleUtility.GUIPointToWorldRay(evt.mousePosition);
		RaycastHit hitInfo;
		if(!guiManager.GetComponent<Collider>().Raycast( ray, out hitInfo, 1000 ))
			return;
		
		Debug.Log("right click");
		
		evt.Use();
	}
}
