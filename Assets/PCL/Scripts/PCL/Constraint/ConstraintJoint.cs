using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class ConstraintJoint : ConstraintBase {

	public ConfigurableJoint joint;
	public int[] offset = new int[]{0,1,2,3};
	public int[] reverse = new int[]{1,1,1,1};
	
	private Quaternion startLocalRotation;
	private float disToParent;
	private Quaternion keepRotation;
	
	public override void Setup() {
		base.Setup();
		startLocalRotation = Quaternion.Inverse(target.localRotation);
		disToParent = transform.localPosition.magnitude;
	}
	
	public override void FrameInit() {
		base.FrameInit();
		transform.localPosition = transform.localPosition.normalized * disToParent;
	}
	
	public override void FrameUpdate(float deltaTime) {
		// transform.localPosition = transform.localPosition.normalized * disToParent;
		base.FrameUpdate(deltaTime);
		CalcuJoint(deltaTime);
	}
	
	protected virtual void CalcuJoint(float deltaTime) {
		if(modify > 0.85f)
			keepRotation = target.localRotation;
		
		// var q = target.localRotation * startLocalRotation;
		var q = keepRotation * startLocalRotation;
		var p = q;
		
		for(int i=0; i<4; i++) {
			p[i] = q[offset[i]] * reverse[offset[i]];
		}
		
		joint.targetRotation = p;
		var slerpDrive = joint.slerpDrive;
		slerpDrive.positionSpring = attr.jointKeepForce * attr.keepDriveWeight * (0.9f + attr.stiffness);
		joint.slerpDrive = slerpDrive;
	}
	
	protected override void OnWillRenderObject() {
		transform.localPosition = transform.localPosition.normalized * disToParent;
	}
}
