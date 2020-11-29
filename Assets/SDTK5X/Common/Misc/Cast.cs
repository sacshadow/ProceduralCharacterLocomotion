using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class CastResult {
	public bool isHit = false;
	public RaycastHit hit;
	
	public bool HitLessThan(float distance) {
		return isHit && hit.distance < distance;
	}
	
	public void OnHitSuccess(RaycastHit hit) {
		isHit = true;
		this.hit = hit;
	}
	public void OnHitFail() {
		isHit = false;
	}
}

public static class Cast {
#if UNITY_EDITOR
	private const int sphereSegment = 24;
	
	// private static bool debug = true;
	private static bool debug = false;
#endif
	
	public static void LineRay(Vector3 origin, Vector3 direction, float distance, LayerMask mask, Action<RaycastHit> Consequence, Action Alternative = null) {
		RaycastHit hit;
		
		if(Physics.Raycast(origin, direction, out hit, distance, mask)) {
		#if UNITY_EDITOR
			if(debug)
				Debug.DrawLine(origin, hit.point, Color.green);
		#endif
			Consequence(hit);
		} else {
		#if UNITY_EDITOR
			if(debug)
				Debug.DrawLine(origin, origin + direction * Mathf.Min(distance, 5000), Color.red);
		#endif
			Call(Alternative);
		}
	}

	public static void LineRay(Ray ray, float distance, LayerMask mask, Action<RaycastHit> Consequence, Action Alternative = null) {
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, distance, mask)) {
		#if UNITY_EDITOR
			if(debug)
				Debug.DrawLine(ray.origin, hit.point, Color.green);
		#endif
			Consequence(hit);
		} else {
		#if UNITY_EDITOR
			if(debug)
				Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Min(distance, 5000), Color.red);
		#endif
			Call(Alternative);
		}
	}
	
	public static void SphereCast(Vector3 origin, Vector3 direction, float radio, float distance, LayerMask mask, Action<RaycastHit> Consequence, Action Alternative = null) {
		RaycastHit hit;
		
		if(Physics.SphereCast(origin, radio, direction, out hit, distance, mask)) {
		#if UNITY_EDITOR	
			if(debug)
				DrawSphereLine(origin, direction, hit.distance, radio, Color.green);
		#endif
			Consequence(hit);
		} else {
		#if UNITY_EDITOR
			if(debug)
				DrawSphereLine(origin, direction, Mathf.Min(distance, 5000), radio, Color.red);
		#endif
			Call(Alternative);
		}
	}
	
	public static void SphereCast(Ray ray, float radio, float distance, LayerMask mask, Action< RaycastHit> Consequence, Action Alternative = null) {
		RaycastHit hit;
		
		if(Physics.SphereCast(ray, radio, out hit, distance, mask)) {
		#if UNITY_EDITOR
			if(debug)
				DrawSphereLine(ray.origin, ray.direction, hit.distance, radio, Color.green);
		#endif
			Consequence(hit);
		} else {
		#if UNITY_EDITOR
			if(debug)
				DrawSphereLine(ray.origin, ray.direction, Mathf.Min(distance, 5000), radio, Color.red);
		#endif
			Call(Alternative);
		}
	}

#if UNITY_EDITOR	
	private static void DrawSphereLine(Vector3 from, Vector3 dir, float distance, float radio, Color c) {
		var direction = dir.normalized;
		var d = direction * distance;
		
		var q = Quaternion.LookRotation(direction);
		var y = Vector3.up * radio;
		var x = Vector3.right * radio;
		
		var segment0 = new Vector3[sphereSegment];
		var segment1 = new Vector3[sphereSegment];
		var segment2 = new Vector3[sphereSegment];
		var a = 360f/sphereSegment;
		
		for(int i=0; i<sphereSegment; i++) {
			segment0[i] = q * Quaternion.Euler(0,0, a * i) * y + from;
			segment1[i] = q * Quaternion.Euler(-a * i,0,0) * y + from;
			segment2[i] = q * Quaternion.Euler(0,a * i,0) * x + from;
		}
		
		for(int i=0; i<sphereSegment; i++) {
			Debug.DrawLine(segment0[i], segment0[(i+1)%sphereSegment], c);
			Debug.DrawLine(segment0[i] + d, segment0[(i+1)%sphereSegment] + d, c);
		}
		var j = sphereSegment/4;
		for(int i=0; i<sphereSegment; i+=j) {
			Debug.DrawLine(segment0[i], segment0[i]+d,c);
		}
		
		var half = sphereSegment/2;
		for(int i=0; i<half; i++) {
			Debug.DrawLine(segment1[i], segment1[i+1], c);
			Debug.DrawLine(segment1[i+half]+d, segment1[(i+half+1)%sphereSegment]+d, c);
			Debug.DrawLine(segment2[i], segment2[i+1], c);
			Debug.DrawLine(segment2[i+half]+d, segment2[(i+half+1)%sphereSegment]+d, c);
		}
	}
#endif
	
	private static void Call(Action Process) {
		if(Process != null) Process();
	}
	
}
