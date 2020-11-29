using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WireLine))]
[CanEditMultipleObjects]
public class WireLineInterface : Editor {
	
	SerializedProperty renderedCamera, material,layer,useWorldSpace;
	int layerValue;
	
	void OnEnable(){
		renderedCamera=serializedObject.FindProperty("renderedCamera");
		material=serializedObject.FindProperty("material");
		layer=serializedObject.FindProperty("layer");
		useWorldSpace=serializedObject.FindProperty("useWorldSpace");
	}
	
	public override void OnInspectorGUI(){
		// EditorGUIUtility.LookLikeControls();
		EditorGUILayout.PropertyField(renderedCamera,new GUIContent ("Rendered Camera"));
		EditorGUILayout.PropertyField(material,new GUIContent ("Material"));
		
		layerValue=EditorGUILayout.LayerField("Layer",layer.intValue);
		
		if(!layer.hasMultipleDifferentValues)
			layer.intValue=layerValue;
		
		EditorGUILayout.PropertyField(useWorldSpace,new GUIContent ("useWorldSpace"));
		//~ EditorGUILayout.LayerField("Layer",layer.intValue);
		//~ EditorGUILayout.Toggle("Use World Space",useWorldSpace.boolValue);
		
		serializedObject.ApplyModifiedProperties();
	}
}
