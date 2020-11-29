using UnityEngine;
//using UnityEditor;
// using System;
// using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public static class Rand {
	
	public static float value {get {return Random.value; }}
	public static Vector3 insideUnitSphere {get {return Random.insideUnitSphere; }}
	
	public static Vector3 RandXZ() {
		return new Vector3(value-0.5f,0,value-0.5f).normalized;
	}
	
	public static float Range(float a, float b) {
		return Random.Range(a,b);
	}
	
	public static int Range(int a, int b) {
		return Random.Range(a,b);
	}
	
	public static T PickFrom<T>(params T[] pool) {
		return pool[Select(pool.Length)];
	}
	
	public static T PickFrom<T>(List<T> pool) {
		return pool[Select(pool.Count)];
	}
	
	public static int Select(int count) {
		return Random.Range(0,count*100) % count;
	}
	
}
