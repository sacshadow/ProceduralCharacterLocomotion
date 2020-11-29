using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using URD = UnityEngine.Random;

public static class GizmosTool {
	public static int segment = 32;
	
	public static Vector3[] circleArray;
	
	public static void InitCircleArray() {
		var a = Mathf.PI/segment * 2f;
		circleArray = new Vector3[segment];
		
		for(int i=0; i<segment; i++) {
			circleArray[i] = new Vector3(
				Mathf.Sin(a*i),
				Mathf.Cos(a*i),
				0);
		}
	}
	
	public static void DrawCircle(Vector3 center, Quaternion rotation, float radio) {
		if(circleArray == null || circleArray.Length != segment) InitCircleArray();
		
		var a = Loop.SelectArray(circleArray, x=>rotation * x * radio + center);
		
		for(int i=0; i<segment; i++) {
			Gizmos.DrawLine(a[i], a[(i+1)%segment]);
		}
	}
	
	
}
