using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_StepMovement : KS_LimbGroundMove {
	
	
	public float stepOffset_L = 0.15f;
	public float stepOffset_R = 0.0f;
	
	protected override void CheckLimbPose() {
		var fv = horiztalMoveSpeed + rotateSpeed + verticalSpeed;
		if(fv > 0.15f)
			stepTime = standeredMoveSpeed/2 / fv;
		else
			stepTime = 1;
		
		CalculateFeetMoveOffset();
		
		SetLimbTargetPose(locomotion.bodyStructure.leg_L, Resample(offset + percentage + stepOffset_L));
		SetLimbTargetPose(locomotion.bodyStructure.leg_R, Resample(offset + percentage + stepOffset_R));
	}
	
	protected override void AnimateLimbMove(float deltaTime) {
		MoveEachLimb(locomotion.bodyStructure.leg_L, Resample(percentage + stepOffset_L), deltaTime);
		MoveEachLimb(locomotion.bodyStructure.leg_R, Resample(percentage + stepOffset_R), deltaTime);
	}
	
	protected override float Resample(float value) {
		var p = Mathf.Repeat(value,1);
		return Mathf.Lerp(0,1,p*1.5f);
	}
}
