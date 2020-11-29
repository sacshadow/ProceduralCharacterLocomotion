using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public static class PCLSetups {
	
	/*********************************************************************************/
	[MenuItem("PCL/Set Constraint Link")]
	public static void SetConstraintLink() {
		var t = Selection.activeTransform;
		
		var cbArray = t.GetComponentsInChildren<ConstraintBase>();
		
		Undo.RecordObjects(cbArray, "Set Constraint Link");
		
		Loop.ForEach(cbArray, SetLink);
	}
	private static void SetLink(ConstraintBase cb){
		var link = new List<ConstraintBase>();
		var t = cb.transform;
		AddParent(t.parent, link);
		
		foreach(Transform child in t) {
			AddChild(child,link);
		}
		cb.linked = link.ToArray();
	}
	private static void AddParent(Transform t, List<ConstraintBase> link) {
		if(t == null) return;
		
		var cb = t.GetComponent<ConstraintBase>();
		if(cb != null)
			link.Add(cb);
		else
			AddParent(t.parent, link);
	}
	private static void AddChild(Transform t, List<ConstraintBase> link) {
		var cb = t.GetComponent<ConstraintBase>();
		if(cb != null) 
			link.Add(cb);
		else {
			foreach(Transform child in t) {
				AddChild(child,link);
			}
		}
	}
	/*********************************************************************************/
	
}
