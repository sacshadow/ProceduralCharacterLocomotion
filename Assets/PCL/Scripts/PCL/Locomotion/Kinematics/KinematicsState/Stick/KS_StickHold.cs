using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_StickHold : KinematicsState {
	
	public KS_LimbStanceMove groundMovement;
	
	public WP_Stick stick;
	
	public L_Arm arm_L, arm_R;
	
	public KS_StickHold (WP_Stick stick) {
		this.stick = stick;
		groundMovement = new KS_LimbStanceMove{standeredMoveSpeed = 1.4f, limbCheckRadio = 0.2f, moveSpeedOffest = 0.0f};
		// groundMovement.feetOffset_L = new Vector3(-0.075f, 0, -0.15f);
		// groundMovement.feetOffset_R = new Vector3( 0.075f, 0, 0.15f);
	}
	
	public override void Reset() {
		groundMovement.kinematics = this.kinematics;
		groundMovement.Reset();
		
		arm_L = locomotion.bodyStructure.arm_L as L_Arm;
		arm_R = locomotion.bodyStructure.arm_R as L_Arm;
		
		arm_L.SetMode(LimbMode.PHYSICS);
		arm_R.SetMode(LimbMode.PHYSICS);
	}
	
	public override (float speed, float percent) GetSpeedAndPercentage(float deltaTime) {
		return groundMovement.GetSpeedAndPercentage(deltaTime);
	}
	
	public override void Simulate(float deltaTime) {
		StepOffset();
		ProcessHandMovement(deltaTime);
		
		groundMovement.Simulate(deltaTime);
	}
	
	public override void FinalModify() {
		locomotion.skeleton.rightArm[3].localRotation = 
		locomotion.skeleton.rightArm[3].localRotation * Quaternion.Euler(135,0,0);
	}
	
	protected void StepOffset() {
		var v = locomotion.transform.InverseTransformDirection(locomotion.body.data.velocity);
		// s = Mathf.Abs(s);
		var x = Mathf.Lerp(0.1f, 0.35f, Mathf.Abs(v.x)/2f);
		var z = Mathf.Lerp(0.05f, 0.175f, 1 - Mathf.Abs(v.x)/2f);
		
		groundMovement.feetOffset_L = new Vector3(-x, 0, -z - 0.1f);
		groundMovement.feetOffset_R = new Vector3(x, 0, z);
	}
	
	protected void ProcessHandMovement(float deltaTime) {
		var handPos = stick.stickLink.GetHoldPoints();
		
		arm_R.link.targetPoint = handPos[0];
		arm_L.link.targetPoint = handPos[1];
	}
	
}
