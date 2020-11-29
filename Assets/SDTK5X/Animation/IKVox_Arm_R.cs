using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class IKVox_Arm_R : IKVoxBase {
	
	public Transform body, target;
	// public int segment = 0;
	
	private Action<Vector3>[] CalcuVox;
	
	public override void VoxUpdate() {
		if(!GT.AllExist(body, target, ik)) return;
		if(CalcuVox == null || CalcuVox.Length == 0) Awake();
	
		var dir = body.InverseTransformDirection(target.position - body.position).normalized;
		
		// segment = GetIndex(dir);
		
		CalcuVox[GetIndex(dir)](dir);
	}
	
	private int GetIndex(Vector3 dir) {
		var rt = dir.x > 0 ? 0 : 1;
		rt += dir.y > 0 ? 0 : 2;
		rt += dir.z > 0 ? 0 : 4;
		return rt;
	}
	
	private void Out(Vector3 d0, Vector3 d1, Vector3 d2) {
		var vox = d0 + d1 + d2;
		
		if(ik != null)
			ik.vox = body.rotation * vox;
		
		// Draw(d0, Color.red);
		// Draw(d1, Color.green);
		// Draw(d2, Color.blue);
		// Draw(vox, new Color(1,0,1,1));
	}
	
	
	private void Zone0(Vector3 dir) {
		var d0 = Vector3.Cross(Vector3.right, dir);
		var d1 = Vector3.Cross(Vector3.up * dir.x, dir);
		var d2 = Vector3.Cross(Vector3.forward * -1, dir);
		Out(d0, d1, d2);
	}
	
	private void Zone1(Vector3 dir) {
		var d0 = Vector3.Cross(Vector3.right, dir);
		var d1 = Vector3.Cross(Vector3.up * Mathf.Lerp(0,1,-dir.x), dir);
		var d2 = Vector3.Cross(Vector3.forward * Mathf.Lerp(-1,0,-dir.x), dir);
		Out(d0, d1, d2);
	}
	
	private void Zone2(Vector3 dir) {
		var d0 = Vector3.Cross(Vector3.right, dir);
		var d1 = Vector3.Cross(Vector3.up * dir.x, dir);
		var d2 = Vector3.Cross(Vector3.forward * Mathf.Lerp(-1,1,-dir.y), dir);
		Out(d0, d1, d2);
	}
	
	private void Zone3(Vector3 dir) {
		var d0 = Vector3.Cross(Vector3.right * Mathf.Lerp(1, Mathf.Lerp(1, -1,-dir.y),-dir.x), dir);
		var d1 = Vector3.Cross(Vector3.up * -dir.x, dir);
		var d2 = Vector3.Cross(Vector3.forward * Mathf.Lerp(Mathf.Lerp(-1,1,-dir.y),0,-dir.x), dir);
		Out(d0, d1, d2);
	}
	
	private void Zone4(Vector3 dir) {
		var d0 = Vector3.Cross(Vector3.right, dir);
		var d1 = Vector3.Cross(Vector3.up * dir.x, dir);
		var d2 = Vector3.Cross(Vector3.forward * -1, dir);
		Out(d0, d1, d2);
	}
	
	private void Zone5(Vector3 dir) {
		var d0 = Vector3.Cross(Vector3.right, dir);
		var d1 = Vector3.Cross(Vector3.up * Mathf.Lerp(0,1,-dir.x), dir);
		var d2 = Vector3.Cross(Vector3.forward * Mathf.Lerp(-1,0,-dir.x), dir);
		Out(d0, d1, d2);
	}
	
	private void Zone6(Vector3 dir) {
		var d0 = Vector3.Cross(Vector3.right, dir);
		var d1 = Vector3.Cross(Vector3.up, dir);
		var d2 = Vector3.Cross(Vector3.forward * Mathf.Lerp(-1,1,-dir.y), dir);
		Out(d0, d1, d2);
	}
	
	private void Zone7(Vector3 dir) {
		var d0 = Vector3.Cross(Vector3.right * Mathf.Lerp(1, Mathf.Lerp(1, -1,-dir.y),-dir.x), dir);
		var d1 = Vector3.Cross(Vector3.up*Mathf.Lerp(1,-1,-dir.x), dir);
		var d2 = Vector3.Cross(Vector3.forward * Mathf.Lerp(Mathf.Lerp(-1,1,-dir.y),0.5f,-dir.x), dir);
		Out(d0, d1, d2);
	}
	
	private void Draw(Vector3 dir, Color c) {
		Debug.DrawLine(body.position, body.position + body.rotation * dir, c);
	}
	
	void Awake() {
		CalcuVox = new Action<Vector3>[] {
			Zone0,
			Zone1,
			Zone2,
			Zone3,
			Zone4,
			Zone5,
			Zone6,
			Zone7,
		};
	}
	
}
