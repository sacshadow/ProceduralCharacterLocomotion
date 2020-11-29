using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_LimbGroundMove : KS_GroundMovement {
	
	public float moveSpeedOffest = 0.5f;
	public float checkDistanceModify = 0.5f;
	public float limbCheckRadio = 0.05f;
	public float offset = 0f;
	public Vector3 feetOffset_L = new Vector3(-0.075f, 0, 0);
	public Vector3 feetOffset_R = new Vector3( 0.075f, 0, 0);
	protected float stepTime;
	
	public override void Reset() {
		base.Reset();
		
		SetLimbPose(locomotion.bodyStructure.leg_L);
		SetLimbPose(locomotion.bodyStructure.leg_R);
	}
	
	protected override void CheckLimbPose() {
		var fv = horiztalMoveSpeed + rotateSpeed + verticalSpeed;
		if(fv > 0.15f)
			stepTime = standeredMoveSpeed/2 / fv;
		else
			stepTime = 1;
		
		CalculateFeetMoveOffset();
		
		SetLimbTargetPose(locomotion.bodyStructure.leg_L, Resample(offset + percentage + 0.5f));
		SetLimbTargetPose(locomotion.bodyStructure.leg_R, Resample(offset + percentage));
	}
	
	protected virtual void CalculateFeetMoveOffset() {
		var localDir = locomotion.transform.InverseTransformDirection(velocity).Flat().normalized;
		var x = localDir.x*localDir.x*0.25f;
		var y = localDir.x * 0.15f;
		locomotion.bodyStructure.leg_L.offset = new Vector3(-x,0, y) + feetOffset_L;
		locomotion.bodyStructure.leg_R.offset = new Vector3( x,0,-y) + feetOffset_R;
	}
	
	protected override void AnimateLimbMove(float deltaTime) {
		MoveEachLimb(locomotion.bodyStructure.leg_L, Resample(percentage + 0.5f), deltaTime);
		MoveEachLimb(locomotion.bodyStructure.leg_R, Resample(percentage), deltaTime);
	}
	
	protected virtual void MoveEachLimb(LimbBase limb, float value, float deltaTime) {
		if(value >= 1)
			limb.KeepPose(deltaTime);
			// MoveLimb(limb,1,deltaTime);
		else
			MoveLimb(limb, value, deltaTime);
		// SetLimbPose(limb);
	}
	
	protected virtual void SetLimbPose(LimbBase limb) {
		// var checkPosition = rotation * limb.offset + position;
		var checkPosition = limb.transform.position;
		checkPosition.y = position.y;
		var checkDis = limb.maxDistance + 0.5f;
		var landPosition = checkPosition - Vector3.up * checkDis;
		
		Cast.SphereCast(checkPosition, -Vector3.up, 0.05f, checkDis, mask, hit=>landPosition = hit.point);
		limb.SaveTargetPose(landPosition, rotation);
	}
	
	protected virtual float Resample(float value) {
		var p = Mathf.Repeat(value,1);
		return Mathf.Lerp(0,1,p*2);
	}
	
	protected virtual void MoveLimb(LimbBase limb, float value, float deltaTime) {
		var landPosition = limb.targetPosition;
		var speed = Mathf.Max(velocity.magnitude*2.5f, standeredMoveSpeed*1.5f);
		var cp = Vector3.MoveTowards(limb.currentPosition, landPosition + Vector3.up * 0.1f * Mathf.Sin(value * Mathf.PI), speed*deltaTime);
		var cr = Quaternion.Lerp(limb.currentRotation, limb.targetRotation, 4*deltaTime);
		
		limb.SetCurrentPose(cp, cr, 1);
		SetLimbPose(limb);
	}
	
	protected virtual void SetLimbTargetPose(LimbBase limb, float value) {
		if(value < 1)
			CalculatePose(limb, value);
	}
	
	protected virtual void CalculatePose(LimbBase limb, float value) {
		var checkRootPosition = position + velocity * stepTime * (1 - value + moveSpeedOffest);
		var checkPosition = GetTargetRotation(value) * limb.offset + checkRootPosition;
		
		var checkDis = limb.maxDistance + checkDistanceModify;
		var checkDir = kinematics.locomotion.dynamics.GetMomentumDirection();
		var landPosition = checkPosition - Vector3.up * checkDis;
		
		checkDir.x *= 0.5f;
		checkDir.z *= 0.5f;
		
		// Cast.SphereCast(checkPosition, -Vector3.up, limbCheckRadio, checkDis, mask, hit=>landPosition = hit.point);
		Cast.SphereCast(checkPosition, checkDir, limbCheckRadio, checkDis, mask, hit=>landPosition = hit.point);
		limb.SaveTargetPose(landPosition, rotation);
	}
	
	protected virtual Quaternion GetTargetRotation(float value) {
		return Quaternion.Euler(angularVelocity * stepTime * (1-value)) * rotation;
	}
}
