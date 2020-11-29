using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;


[ExecuteInEditMode]
public class ArmIKSet : MonoBehaviour {
	
	public Vector2 limit = new Vector2(0.1f, 0.1f);
	public TwoBoneIK arm;
	
	public Transform body, target, vox;
	public float side = 1;
	
	public Vector3 dirValue;
	public float kvalue;
	
	public void UpdateArm() {
		var disp = body.InverseTransformDirection(arm.bone1.position - transform.position - 
			Vector3.right * arm.transform.localPosition.x).normalized;
		
		var yRt = - disp.z * limit.y * side;
		var zRt = 0f;
		
		if(disp.x * side < 0)
			yRt += disp.x * limit.y * Mathf.Sign(disp.z);
		
		if(disp.y > 0)
			zRt = disp.y * limit.x * side;
		
		transform.localRotation = Quaternion.Euler(0, yRt, zRt);
	
	/********************************************************************************/
		var dir = body.InverseTransformDirection(arm.bone2.position - arm.bone0.position).normalized;
		
		var norX = Vector3.Cross(dir, Vector3.right);
		var norY = Vector3.Cross(dir, Vector3.up * side);
		
		// norX.x *= side;
		// norY.x *= side;
		
		// Debug.DrawLine(arm.transform.position, arm.transform.position + body.TransformDirection(norX));
		// var k = Mathf.Abs(dir.y);
		var k = 1f;
		
		if(dir.z < 0)
			k = Mathf.Abs(dir.y);
		
		dirValue = dir;
		
		if(dir.x * side <0)
			k = 1 + dir.x * side;
		
		kvalue = Mathf.Lerp(kvalue, k, 4*Time.deltaTime);
		
		var p = norX * kvalue + norY * (1-kvalue);
		// var p = norY;
		
		vox.position = arm.bone0.position + body.TransformDirection(p);
	/********************************************************************************/		
	}
	
	
	// Update is called once per frame
	void LateUpdate () {
		if(arm == null || target == null || vox == null)
			return;
			
		UpdateArm();
	}
	
	
	void OnDrawGizmos() {
		if(arm == null || target == null || vox == null)
			return;
			
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(arm.transform.position, vox.position);
	}
	
}
