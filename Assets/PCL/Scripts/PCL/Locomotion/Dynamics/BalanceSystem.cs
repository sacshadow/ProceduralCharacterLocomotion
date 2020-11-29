using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class BalanceSystem {
	
	private Dynamics dynamics;
	private DynamicsData data;
	
	private Vector3 lastRotationError;
	
	private Transform transform {get {return dynamics.locomotion.transform; }}
	
	public BalanceSystem(Dynamics dynamics) {
		this.dynamics = dynamics;
		this.data = dynamics.data;
	}
	
	public virtual void Reset() {
		lastRotationError = Vector3.zero;
	}
	
	public virtual Vector3 GetTorque(Vector3 targetFaceDirection, float deltaTime) {
		var reciDeltaTime = 1f/deltaTime;
		var targetRotation = Quaternion.LookRotation(targetFaceDirection);
		Vector3 torqueAxis;
		float torqueAngle;
		
		targetRotation = targetRotation * Quaternion.Inverse(transform.rotation);
		targetRotation.ToAngleAxis(out torqueAngle, out torqueAxis);
		
		if(torqueAngle == 360f)
			return Vector3.zero;
		
		var rotationError = torqueAxis * FixEuler(torqueAngle);
		var angularVelocity = 
			(rotationError * data.eulerModify + (rotationError - lastRotationError)
			* data.lastEulerModify * reciDeltaTime) * data.eulerAmount;
		
		// angularVelocity = Vector3.ClampMagnitude(angularVelocity, 540) * Mathf.Deg2Rad;
		angularVelocity = Vector3.ClampMagnitude(angularVelocity, 720) * Mathf.Deg2Rad;
		
		var torque = (angularVelocity - dynamics.body.data.angularVelocity) * data.finialModify * reciDeltaTime;
		lastRotationError = rotationError;
		return Vector3.ClampMagnitude(torque, data.maxBalanceTorque);
	}
	
	private float FixEuler (float angle) {
		if(angle > 180f)
			return angle - 360;
		return angle;
	}
}
