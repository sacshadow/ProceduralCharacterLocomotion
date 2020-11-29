using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HumanoidBehaviour : RBGroup {
	
	public LayerMask ground = 1<<10, climbMask = ~0, grabMask = 1<<14;
	public Locomotion locomotion;
	
	// [NonSerialized]
	public Vector3 targetFaceDirection = Vector3.forward;
	[NonSerialized]
	public float rotateSpeed;
	
	public override void LocomotionUpdate() {
		base.LocomotionUpdate();
		
		var dt = PCLSimulation.deltaTime * Time.timeScale;
		RotateToDirection(dt);
		locomotion.Simulate(dt);
	}
	
	public override void ApplyTransform() {
		base.ApplyTransform();
	}
	
	public override void ApplyBoneIK() {
		base.ApplyBoneIK();
		locomotion.UpdateAfterIK();
	}
	
	public override void Reset() {
		base.Reset();
		rotateSpeed = 0;
		targetFaceDirection = locomotion.transform.forward;
		locomotion.Reset();
	}
	public override void Begin() {
		boneIK = new List<BoneIK>(GetComponentsInChildren<BoneIK>());
		rb = new List<RBody>(GetComponentsInChildren<RBody>());
		link = new List<LinkBase>(GetComponentsInChildren<LinkBase>());
		
		base.Begin();
	}
	
	public virtual void Move(Vector3 velocity) {
		locomotion.targetMovement = velocity;
	}
	
	public virtual void Towards(Vector3 direction) {
		targetFaceDirection = direction;
	}
	
	public virtual void ForceTowards(Vector3 direction) {
		targetFaceDirection = direction;
		locomotion.direction.rotation = Quaternion.LookRotation(direction);
		locomotion.lookDirection = targetFaceDirection;
		locomotion.targetFaceDirection = locomotion.direction.forward;
	}
	
	protected virtual void RotateToDirection(float deltaTime) {
		var directionTransform = locomotion.direction;
		var q = Quaternion.Euler(0,rotateSpeed,0);
		var unitDir = q * directionTransform.forward;
		var unitRight = q * directionTransform.right;
		
		var angle = Vector3.Angle(targetFaceDirection, unitDir) * Mathf.Sign(Vector3.Dot(targetFaceDirection, unitRight));
		var targetSpeed = Mathf.Clamp(angle, -1080, 1080);
		var torque = (targetSpeed - rotateSpeed) * 2.8f - rotateSpeed * 12.5f;
		
		rotateSpeed += torque*deltaTime;
		// rotateSpeed = Mathf.Clamp(rotateSpeed, -360, 360);
		rotateSpeed = Mathf.Clamp(rotateSpeed, -1080, 1080);
		
		directionTransform.rotation =  Quaternion.Euler(0,rotateSpeed*deltaTime*60,0) * directionTransform.rotation;
		
		locomotion.lookDirection = targetFaceDirection;
		locomotion.targetFaceDirection = directionTransform.forward;
		// locomotion.targetFaceDirection = targetFaceDirection;
	}
	
	void Awake() {
		targetFaceDirection = transform.forward;
		locomotion.Init();
	}
}
