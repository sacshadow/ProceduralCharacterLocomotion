using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class DrawCube : MonoBehaviour {
#if UNITY_EDITOR
	public Color c = new Color(0,1,1,0.5f);
	public bool show = true;
	private BoxCollider boxCollider;
	
	
	void OnDrawGizmos() {
		if(!show)
			return;
	
		if(boxCollider == null) boxCollider = GetComponent<BoxCollider>();
		
		
		Gizmos.matrix = transform.localToWorldMatrix;
		
		Gizmos.color = c;
		Gizmos.DrawCube(boxCollider.center, boxCollider.size);
		
	}
#endif	
}
