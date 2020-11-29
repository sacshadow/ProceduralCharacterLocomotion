using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using URD = UnityEngine.Random;

public class Box : MonoBehaviour {
	
	public Color color = new Color(0,1,1,1);
	
	public Vector3 center = Vector3.zero;
	public Vector3 size = Vector3.one;
	
	public bool show = true;
	
	
	void OnDrawGizmos() {
		if(!show) return;
		
		Gizmos.matrix = transform.localToWorldMatrix;
		
		var c = color;
		c.a = 0.5f;
		Gizmos.color = c;
		Gizmos.DrawCube(center, size);
		
		Gizmos.color = color;
		Gizmos.DrawWireCube(center, size);
		
		DrawDir(Color.red, Vector3.right);
		DrawDir(Color.green, Vector3.up);
		DrawDir(Color.blue, Vector3.forward);
	}
	
	private void DrawDir(Color c, Vector3 dir) {
		Gizmos.color = c;
		Gizmos.DrawLine(Vector3.zero, dir * 0.2f);
	}
}
