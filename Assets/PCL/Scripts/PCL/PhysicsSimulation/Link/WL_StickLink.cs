using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class WL_StickLink : WeaponLink {
	
	public Transform holdPoint;
	public Vector3 offset = Vector3.up * 0.25f;
	
	public WL_HoldLink weightPoint;
	public Limit_HoldPoint weightHoldLimit;
	
	public Vector3 holdPosition, holdDirection;
	public float distanceLimit = 0.4f;
	
	public float disToCOM = 0.5f;
	public float holdSperate = 0.35f;
	
	public float errorModify = 0.5f, lastErrorModify = 0.1f;
	
	protected Vector3 lastError;
	
	public void SetPose(Vector3 holdPosition, Vector3 holdDirection) {
		this.holdPosition = holdPosition;
		this.holdDirection = holdDirection;
		
		weightHoldLimit.comDistance = disToCOM;
		weightPoint.position = holdPosition + holdDirection * disToCOM;
	}
	
	public Vector3[] GetHoldPoints() {
		var forward = weightPoint.transform.forward * holdSperate/2f;
		return new Vector3[] {target.data.position + forward, target.data.position - forward, };
	}
	
	public override void AttachTo(HumanoidBehaviour behaviour) {
		base.AttachTo(behaviour);
		link = behaviour.root;
		holdPoint = behaviour.locomotion.skeleton.spine[2];
		this.limit.Add(behaviour.locomotion.bodyStructure.bodyLimit);
		this.limit.Add(behaviour.locomotion.bodyStructure.swingLimit);
		weightPoint.limit.Add(behaviour.locomotion.bodyStructure.bodyLimit);
	}
	
	public override void Detach(HumanoidBehaviour behaviour) {
		base.Detach(behaviour);
		this.limit.Remove(behaviour.locomotion.bodyStructure.bodyLimit);
		this.limit.Remove(behaviour.locomotion.bodyStructure.swingLimit);
		weightPoint.limit.Remove(behaviour.locomotion.bodyStructure.bodyLimit);
	}
	
	public override void ApplyLimitations() {
		base.ApplyLimitations();
		var hp = holdPoint.TransformPoint(offset);
		var disp = target.next.position - hp;
		target.next.position = Vector3.ClampMagnitude(disp, distanceLimit) + hp;
		
		target.next.rotation = Quaternion.LookRotation(disp.normalized, Vector3.up);
		
		Debug.DrawLine(target.next.position, hp);
	}
	
	public override void ApplyForce(float deltaTime, float reciDeltaTime) {
		if(!target.isSimulating) return;
		
		EulerMethod(reciDeltaTime);
	}
	
	protected void EulerMethod(float reciDeltaTime) {
		var data = target.data;
		
		var error = holdPosition - data.position;
		var perdictVelocity = error * errorModify + (error - lastError) * lastErrorModify * reciDeltaTime;
		lastError = error;
		
		var force = Vector3.ClampMagnitude(perdictVelocity, 20) * reciDeltaTime * data.mass;
		target.AddForce(force);
		link.AddForce(-force);
	}
	
	
}
