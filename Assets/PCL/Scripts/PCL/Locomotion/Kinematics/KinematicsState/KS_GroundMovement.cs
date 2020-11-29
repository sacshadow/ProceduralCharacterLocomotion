using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_GroundMovement : KinematicsState {
	public const float verticalMoveModify = 1.25f;
	public const float rotateMoveModify = 1.5f;
	
	public float 
		standeredMoveSpeed = 2f,
		shiftDownSpeed = 1.25f,
		shiftUpSpeed = 2.5f;
	
	protected float horiztalMoveSpeed = 0;
	protected float verticalSpeed = 0;
	protected float rotateSpeed = 0;
	
	public override void Reset() {
		bodyStructure.SetLimbMode(LimbMode.ANIMATE, LimbMode.BONE_IK, attribute.isInAir ? 1.5f : 0.5f);
	
		horiztalMoveSpeed = 0;
		verticalSpeed = 0;
		rotateSpeed = 0;
	}
	
	public override (float speed, float percent) GetSpeedAndPercentage(float deltaTime) {
		FrameInit();
		return CalculateSpeedAndPercentage(deltaTime);
	}
	
	public override void Simulate(float deltaTime) {
		CheckLimbPose();
		AnimateLimbMove(deltaTime);
	}
	
	protected virtual void FrameInit() {
		horiztalMoveSpeed = velocity.Flat().magnitude;
		verticalSpeed = Mathf.Abs(velocity.y) * verticalMoveModify * standeredMoveSpeed;
		rotateSpeed = Mathf.Abs(angularVelocity.y) / Mathf.PI * standeredMoveSpeed * rotateMoveModify;
	}
	
	protected virtual (float speed, float percent) CalculateSpeedAndPercentage(float deltaTime) {
		var v = Mathf.Min(horiztalMoveSpeed + verticalSpeed + rotateSpeed, standeredMoveSpeed*2);
		var speed = v / standeredMoveSpeed;
		var p = Mathf.Repeat(percentage + speed*deltaTime, 1);
		return (speed, p);
	}
	
	protected virtual void CheckLimbPose() {
		
	}
	
	protected virtual void AnimateLimbMove(float deltaTime) {
		
	}
	
}
