using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class WL_HoldLink : WeaponLink {
	
	public Transform holdPoint;
	public Vector3 position;
	public Quaternion rotation;
	
	public float errorModify = 0.5f, lastErrorModify = 0.1f;
	
	protected Vector3 lastError;
	
	public override void ApplyForce(float deltaTime, float reciDeltaTime) {
		if(!target.isSimulating) return;
		
		EulerMethod(reciDeltaTime);
	}
	
	public override void ApplyLimitations() {
		base.ApplyLimitations();
		// var up = (target.next.position - holdPoint.position).normalized;
		// target.next.rotation = Quaternion.LookRotation(up, holdPoint.forward) * Quaternion.Euler(90,0,0);
		var f = (target.next.position - holdPoint.position).normalized;
		target.next.rotation = Quaternion.LookRotation(f, holdPoint.forward);
	}
	
	protected void EulerMethod(float reciDeltaTime) {
		var data = target.data;
		
		var error = position - data.position;
		var perdictVelocity = error * errorModify + (error - lastError) * lastErrorModify * reciDeltaTime;
		lastError = error;
		
		var force = Vector3.ClampMagnitude(perdictVelocity, 20) * reciDeltaTime * data.mass;
		target.AddForce(force);
		link.AddForce(-force);
	}
	
}
