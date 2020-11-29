using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public class IKVox_Leg : IKVoxBase {
	
	public Transform body, link;
	
	public float side = 1;
	[Range(-15f,15f)]
	public float voxModify = 1;
	
	public override void VoxUpdate() {
		if(!GT.AllExist(body, link, ik, ik.target)) return;
		
		var dir = body.InverseTransformDirection(ik.target.position - link.position);
		var nor = Vector3.Cross(dir, Vector3.right).normalized;
		
		if(dir.x * side > 0 )
			nor.x += dir.x * voxModify;
		
		if(dir.y > 0)
			nor.z = -nor.z;
		
		ik.vox = body.TransformDirection(nor);
	}
	
}
