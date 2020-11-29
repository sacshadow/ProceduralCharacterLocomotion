using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_StickMovement : KinematicsState {
	
	public KinematicsState state;
	
	public WP_Stick stick;
	public L_Arm arm_L, arm_R;

	
	public KS_StickMovement(WP_Stick stick, KinematicsState state) {
		this.stick = stick;
		this.state = state;
	}
	
	public override void Reset() {
		state.kinematics = this.kinematics;
		state.Reset();
		
		arm_L = locomotion.bodyStructure.arm_L as L_Arm;
		arm_R = locomotion.bodyStructure.arm_R as L_Arm;
		
		arm_L.SetMode(LimbMode.PHYSICS);
		arm_R.SetMode(LimbMode.PHYSICS);
	}
	
	public override (float speed, float percent) GetSpeedAndPercentage(float deltaTime) {
		return state.GetSpeedAndPercentage(deltaTime);
	}
	
	public override void Simulate(float deltaTime) {
		ProcessHandMovement(deltaTime);
		state.Simulate(deltaTime);
	}
	
	protected void ProcessHandMovement(float deltaTime) {
		var handPos = stick.stickLink.GetHoldPoints();
		
		arm_R.link.targetPoint = handPos[0];
		arm_L.link.targetPoint = handPos[1];
	}
}
