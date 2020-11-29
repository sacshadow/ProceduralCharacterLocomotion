using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

// [ExecuteInEditMode]
public class TwoBoneIK : BoneIK {
	
	public float minModify = 0.05f;
	
	public Transform
		bone0, bone1, bone2;
	
	private float r1 = 0.1f, r2 = 0.1f;
	private float a1, a2;
	private float max = 0.2f, min = 0.05f;
	
	public override void InitData() {
		if(AnyEmpty(bone0, bone1, bone2, target))
			return;
		
		r1 = Distance(bone0,bone1);
		r2 = Distance(bone1,bone2);
		max = r1+r2;
		min = Mathf.Abs(r1 - r2) + minModify;
		
		Update_IK();
	}
	
	public override float GetLength() {
		if(max != 0)
			return max;
		
		return 
			Distance(bone0, bone1)
			+ Distance(bone1, bone2);
	}
	
	public override Vector3 COM() {
		return (bone0.position + bone1.position + bone2.position)/3f;
	}
	
	public override Vector3 GetLocalBound(Transform root, float minRadio = 0.035f) {
		var a = root.InverseTransformPoint(bone0.position);
		var b = root.InverseTransformPoint(bone1.position);
		var c = root.InverseTransformPoint(bone2.position);
		
		var x = MT.Max(a.x, b.x, c.x) - MT.Min(a.x, b.x, c.x) + minRadio;
		var y = MT.Max(a.y, b.y, c.y) - MT.Min(a.y, b.y, c.y) + minRadio;
		var z = MT.Max(a.z, b.z, c.z) - MT.Min(a.z, b.z, c.z) + minRadio;
		
		return new Vector3(x,y,z);
	}
	
	public override Transform GetEndIKTransform() {
		return bone2;
	}
	
	public override bool IsLegal() {
		var rt = !AnyEmpty(bone0, bone1, bone2, target);
		return rt;
	}
	
	public override void Update_IK() {
		if(!ikUpdate) return;
		
		if(voxCalculater!=null)
			voxCalculater.VoxUpdate();
	
		var offset0 = Quaternion.Euler(rotateOffset0);
		var offset1 = Quaternion.Euler(rotateOffset1);
		
		var dir = (voxInverse ? -1 : 1);
		var nor = vox.normalized * (norInverse ? 1 : -1);
		var disp = target.position - transform.position;
		var q0 = Quaternion.LookRotation(disp, nor) * offset0;
		
		var d = Mathf.Clamp(disp.magnitude, min, max);
		var x = (r1*r1-r2*r2+d*d)/(2*d);
		var a0 = Mathf.Acos(Mathf.Clamp(x/r1,-1,1))*Mathf.Rad2Deg * dir;
		var a1 = Mathf.Acos(Mathf.Clamp((d-x)/r2,-1,1))*Mathf.Rad2Deg * -dir;
		
		bone0.rotation = q0 * Quaternion.Euler(a0,0,0) * offset1;
		bone1.rotation = q0 * Quaternion.Euler(a1,0,0) * offset1;
	}
	
	void OnDrawGizmos() {
		if(!IsLegal())
			return;
		
		Gizmos.color = Color.green;
		DrawLine(bone0, bone1, bone2);
		
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, target.position);
		
		if(drawVox) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, transform.position + vox);
		}
	}
	
}
