using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;


[ExecuteInEditMode]
public class ThreeBoneIK : BoneIK {
	public const float borderMin = 0.05f;
	
	public Transform
		bone0, bone1, bone2, bone3;
	
	public AnimationCurve muscleModify = AnimationCurve.Linear(0,0,1,1);

	private float min, max, minl,
		r0, r1, r2;
	
	public override void InitData() {
		if(AnyEmpty(bone0, bone1, bone2, bone3, target))
			return;
			
		r0 = Distance(bone0, bone1);	
		r1 = Distance(bone1, bone2);
		r2 = Distance(bone2, bone3);
		
		max = r0 + r1 + r2 - 0.02f;
		minl = Mathf.Abs(r0 - r1) + borderMin;
		min = Mathf.Abs(minl - r2) + borderMin;
		
		Update_IK();
	}
	
	public override float GetLength() {
		if(max != 0)
			return max;
		return 
			Distance(bone0, bone1)
			+ Distance(bone1, bone2)
			+ Distance(bone2, bone3);
	}
	
	public override Vector3 COM() {
		return (bone0.position + bone1.position + bone2.position + bone3.position)/4;
	}
	
	public override Vector3 GetLocalBound(Transform root, float minRadio = 0.035f) {
		var a = root.InverseTransformPoint(bone0.position);
		var b = root.InverseTransformPoint(bone1.position);
		var c = root.InverseTransformPoint(bone2.position);
		var d = root.InverseTransformPoint(bone3.position);
		
		var x = MT.Max(a.x, b.x, c.x, d.x) - MT.Min(a.x, b.x, c.x, d.x) + minRadio;
		var y = MT.Max(a.y, b.y, c.y, d.y) - MT.Min(a.y, b.y, c.y, d.y) + minRadio;
		var z = MT.Max(a.z, b.z, c.z, d.z) - MT.Min(a.z, b.z, c.z, d.z) + minRadio;
		
		return new Vector3(x,y,z);
	}
	
	public override Transform GetEndIKTransform() {
		return bone3;
	}
	
	public override bool IsLegal() {
		return !AnyEmpty(bone0, bone1, bone2, bone3, target);
	}
	
	public override void Update_IK() {
		if(voxCalculater!=null)
			voxCalculater.VoxUpdate();
	
		var offset0 = Quaternion.Euler(rotateOffset0);
		var offset1 = Quaternion.Euler(rotateOffset1);
		
		var dir = (voxInverse ? -1 : 1);
		var nor = vox.normalized * (norInverse ? 1 : -1);
		var disp = target.position - transform.position;
		
		var q0 = Quaternion.LookRotation(disp, nor) * offset0;
		
		var d = Mathf.Clamp(disp.magnitude, min, max);
		var l = Mathf.Lerp(minl, r0 + r1, muscleModify.Evaluate((d - min)/(max - min)));
		
		var x = (l*l-r2*r2+d*d)/(2*d);
		var a0 = Mathf.Acos(Mathf.Clamp(x/l,-1,1))* Mathf.Rad2Deg * dir;
		var a1 = Mathf.Acos(Mathf.Clamp((d-x)/r2,-1,1)) * Mathf.Rad2Deg * dir;
		
		var q1 = q0 * Quaternion.Euler(-a0,0,0) * offset1;
		var q2 = q0 * Quaternion.Euler(a1,0,0) * offset1;
		var p = q1 * Vector3.forward * l + transform.position;
		
		x = (r0*r0-r1*r1+l*l)/(2*l);
		
		a0 = Mathf.Acos(Mathf.Clamp(x/r0,-1,1)) * Mathf.Rad2Deg * dir;
		a1 = Mathf.Acos(Mathf.Clamp((l-x)/r1,-1,1)) * Mathf.Rad2Deg * dir;
		
		var q3 = Quaternion.LookRotation(p-transform.position, -nor);
		
		var q4 = q3 * Quaternion.Euler(-a0,0,0);
		// var q4 = q3 * Quaternion.Euler(axis * -a0);
		var q5 = q3 * Quaternion.Euler(a1,0,0);
		// var q5 = q3 * Quaternion.Euler(axis * a1);
		
		bone0.rotation = q4;
		bone1.rotation = q5;
		bone2.rotation = q2;
	}
	
	
	
	void OnDrawGizmos() {
		if(AnyEmpty(bone0, bone1, bone2, bone3, target))
			return;
	
		Gizmos.color = Color.green;
		DrawLine(bone0, bone1, bone2, bone3);
		
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, target.position);
		
		if(drawVox) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, transform.position + vox);
		}	
	}
}
