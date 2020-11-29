using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

[ExecuteInEditMode]
public class IKVoxBone : IKVoxBase {
	
	// public BoneIK ik;
	public Transform target;
	
	public Vector3 dir = Vector3.forward;
	
	public override void VoxUpdate() {
		if(ik == null || target == null)
			return;
		
		ik.vox = target.TransformDirection(dir);
	}
}
