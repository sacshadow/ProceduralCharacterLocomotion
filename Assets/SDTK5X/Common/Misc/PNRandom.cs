using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//Perlin Noise Random
public static class PNRandom {
	
	public static void DrawFrom<T>(IEnumerable<T> array, int count, Action<T> Callback, Vector3 pos) {
		var pickList = new List<T>(array);
		var ct = Mathf.Min(count, pickList.Count);
		
		for(int i=0; i<ct; i++) {
			var temp = PickFrom(pickList, pos * i);
			pickList.Remove(temp);
			Callback(temp);
		}
	}
	
	public static T PickFrom<T>(T[] array, Vector3 pos) {
		return array[Range(pos,0,array.Length)];
	}
	public static T PickFrom<T>(List<T> array, Vector3 pos) {
		return array[Range(pos,0,array.Count)];
	}
	
	public static float Value(float x, float y, float scale = 31.415927f) {
		return Mathf.PerlinNoise(x*scale, y*scale);
	}
	
	public static float Value(Vector3 pos, float scale = 0.99173f) {
		var p = pos * scale;
		return Mathf.PerlinNoise(p.x * p.y + p.z, p.x - p.y * p.z);
	}
	
	public static float Value3D(Vector3 pos, float scale = 0.2718273f) {
		var p = pos * scale;
		var rt = Mathf.PerlinNoise(p.x, p.y)
			+ Mathf.PerlinNoise(p.y, p.z)
			+ Mathf.PerlinNoise(p.x, p.z)
			+ Mathf.PerlinNoise(p.y, p.x)
			+ Mathf.PerlinNoise(p.z, p.y)
			+ Mathf.PerlinNoise(p.z, p.x) / 6f;
		return rt;
	}
	
	public static int Range(Vector3 pos, int min, int max, float scale = 0.975137f) {
		int count = max - min;
		var v = Mathf.Lerp(0f, count, Value(pos, scale));
		return (Mathf.RoundToInt(v) % count) + min;
	}
	
	public static float FloatRange(Vector3 pos, float min, float max, float scale = 0.975137f) {
		return  Mathf.Lerp(min, max, Value(pos, scale));
	}
	
	public static Vector3 InsideArea(Vector3 p, float offset, float size) {
		var rt = p;
		
		var o0 = p.x + p.y - p.z + offset;
		var o1 = p.x - p.y - p.z + offset;
		var o2 = -p.x + p.y + p.z + offset;
		var o3 = -p.x + p.y - p.z + offset;
		
		rt.x += Mathf.PerlinNoise(0.478232f * o0, 0.72312f * o1) * size - size/2f;
		rt.z += Mathf.PerlinNoise(0.725762f * o2, 0.26578f * o3) * size - size/2f;
		
		return rt;
	}
	
	public static Vector3 RandV3(Vector3 r) {
		return new Vector3(
			Mathf.PerlinNoise(r.x, r.y),
			Mathf.PerlinNoise(r.y, r.z),
			Mathf.PerlinNoise(r.z, r.x));
	}
	
	public static int SelectV3(int count, Vector3 seed) {
		return Mathf.RoundToInt(
			Mathf.Clamp01(Mathf.PerlinNoise(seed.x, seed.z)) * count) % count;
	}
	
	public static Quaternion RandYRotate(Vector3 pos, float scale = 0.0917328f) {
		return Quaternion.Euler(0,Value(pos, scale) * 360f,0);
	}
	
}
