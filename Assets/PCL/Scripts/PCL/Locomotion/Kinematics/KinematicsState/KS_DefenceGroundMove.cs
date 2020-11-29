using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_DefenceGroundMove : KinematicsState {
	
	public KinematicsState groundMovement;
	
	public S_Grab grab;
	// public float percentage = 0;
	
	public KS_DefenceGroundMove () {
		groundMovement = new KS_LimbStanceMove{standeredMoveSpeed = 1.4f, limbCheckRadio = 0.2f, moveSpeedOffest = 0.0f};
		grab = new S_Grab();
	}
	
	public override void Reset() {
		groundMovement.kinematics = this.kinematics;
		groundMovement.Reset();
		// percentage = 0f;
		grab.Setup(locomotion);
		
		grab.arm_L.SetMode(LimbMode.PHYSICS);
		grab.arm_R.SetMode(LimbMode.PHYSICS);
	}
	
	public override (float speed, float percent) GetSpeedAndPercentage(float deltaTime) {
		return groundMovement.GetSpeedAndPercentage(deltaTime);
	}
	
	public override void Simulate(float deltaTime) {
		// StepOffset();
		ProcessHandMovement(deltaTime);
		
		groundMovement.Simulate(deltaTime);
		// return percentage;
	}
	
	public void ProcessHandMovement(float deltaTime) {
		var fdata = kinematics.animate.current.GetCurrentFrame();
		var q = locomotion.transform.rotation;
		
		var p_l = q * fdata.leftHand + grab.arm_L.link.linkPoint.position;
		var p_r = q * fdata.rightHand + grab.arm_R.link.linkPoint.position;
		
		grab.arm_L.link.targetPoint = p_l;
		grab.arm_R.link.targetPoint = p_r;
		
		var p = locomotion.skeleton.spine[2].position + Vector3.up * 0.25f;
		var dl = (grab.arm_L.link.linkPoint.position - p)*0.5f;
		var dr = (grab.arm_L.link.linkPoint.position - p)*0.5f;
		p_l = q * Vector3.forward * 0.45f + p + dl;
		p_r = q * Vector3.forward * 0.45f + p + dr;
		
		grab.OnGrab(p_l, rb=>SetHandToBlockPoint(grab.arm_L, rb));
		grab.OnGrab(p_r, rb=>SetHandToBlockPoint(grab.arm_R, rb));
	}
	
	// protected void Deflect(L_Arm arm, Rigidbody rb) {
		// var c = rb.GetComponent<ConstraintBase>();
		// if(c == null) return;
		
		// SetHandToBlockPoint(arm, c);
	// }
	
	protected void SetHandToBlockPoint(L_Arm arm, Rigidbody rb) {
		var p = rb.worldCenterOfMass;
		// var p = constraint.rb.position;
		var offset = arm.link.linkPoint.parent.TransformDirection(arm.link.linkPoint.localPosition*0.25f);
		var disp = p - arm.link.linkPoint.position;
		
		if(Vector3.Angle(disp.Flat(), locomotion.transform.forward) > 65f)
			return;
		
		var targetPoint = disp + disp.normalized * 0.15f + arm.link.linkPoint.position;
		arm.link.targetPoint = targetPoint + offset + Vector3.up * 0.25f;
	}
}
