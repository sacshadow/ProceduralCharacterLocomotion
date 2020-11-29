using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using URD = UnityEngine.Random;

public static class IPT {
	
	
	
	public static void KeyDown(KeyCode key, Action Process) {
		if(Input.GetKeyDown(key)) Process();
	}
	
	public static void KeyPress(KeyCode key, Action Process) {
		if(Input.GetKey(key)) Process();
	}
	
	public static void KeyUp(KeyCode key, Action Process) {
		if(Input.GetKeyUp(key)) Process();
	}
	
	public static Vector3 AxisXYZ(string x, string y, string z) {
		return new Vector3(
			Input.GetAxis(x),
			Input.GetAxis(y),
			Input.GetAxis(z));
	}
	
	public static Vector3 AxisXY(string x, string y) {
		return new Vector3(
			Input.GetAxis(x),
			Input.GetAxis(y),
			0);
	}
	
	public static Vector3 AxisXZ(string x, string z) {
		return new Vector3(
			Input.GetAxis(x),
			0,
			Input.GetAxis(z));
	}
	
	public static void LMB_Down(Action Process) {if(Input.GetMouseButtonDown(0)) Process();}
	public static void LMB(Action Process) {if(Input.GetMouseButton(0)) Process();}
	public static void LMB_Up(Action Process) {if(Input.GetMouseButtonUp(0)) Process();}
	
	public static void RMB_Down(Action Process) {if(Input.GetMouseButtonDown(1)) Process();}
	public static void RMB(Action Process) {if(Input.GetMouseButton(1)) Process();}
	public static void RMB_Up(Action Process) {if(Input.GetMouseButtonUp(1)) Process();}
	
	public static Vector3 GetMouseDirectionXZ(Vector3 center, Camera cam) {
		return GetMouseDirection(center, Vector3.up, cam);
	}
	public static Vector3 GetMouseDirection(Vector3 center, Vector3 normal, Camera cam) {
		var p = new Plane(normal, center);
		var ray = cam.ScreenPointToRay(Input.mousePosition);
		float dis = 0f;
		
		if(p.Raycast(ray, out dis))
			return (ray.GetPoint(dis) - center).normalized;
		else
			return Vector3.zero;
	}
}
