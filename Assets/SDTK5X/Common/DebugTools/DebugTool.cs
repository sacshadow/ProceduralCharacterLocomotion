using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public static class DebugTool {
	
	public static void DrawCube(Vector3 center, Vector3 size, Quaternion rotation) {
		DrawCube(center, size, rotation, Color.white);
	}
	public static void DrawCube(Vector3 center, Vector3 size, Quaternion rotation, Color color) {
		var x = rotation * Vector3.right * size.x/2f;
		var y = rotation * Vector3.up * size.y/2f;
		var z = rotation * Vector3.forward * size.z/2f;
		
		var p = new Vector3[]{
			center + z - x - y,
			center + z + x - y,
			center - z - x - y,
			center - z + x - y,
			center + z - x + y,
			center + z + x + y,
			center - z - x + y,
			center - z + x + y,
		};
		
		DrawLine(p, color,
			0,1,1,3,3,2,2,0,
			4,5,5,7,7,6,6,4,
			0,4,1,5,3,7,2,6);
	}
	
	private static void DrawLine(Vector3[] array, Color color, params int[] link) {
		for(int i=0; i<link.Length; i+=2)
			Debug.DrawLine(array[link[i]], array[link[i+1]], color);
	}
	
}
