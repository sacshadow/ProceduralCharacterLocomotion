using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

[ExecuteInEditMode]
public class IKVox : MonoBehaviour {
	
	public Transform body, link;
	public BoneIK ik;
	public float side = 1;
	[Range(-15f,15f)]
	public float voxModify = 1;
	
	private void UpdateVox() {
		var dir = body.InverseTransformDirection(ik.target.position - link.position);
		var nor = Vector3.Cross(dir, Vector3.right).normalized;
		
		if(dir.x * side > 0 )
			nor.x += dir.x * voxModify;
		
		if(dir.y > 0)
			nor.z = -nor.z;
		
		// vox.position = link.position + body.TransformDirection(nor);
		ik.vox = body.TransformDirection(nor);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(body == null || ik == null || ik.target == null)
			return;
			
		UpdateVox();
	}
	
}
