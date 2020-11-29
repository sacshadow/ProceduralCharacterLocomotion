using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class ConstraintWeapon : ConstraintBase {
	
	public bool follow = true;
	
	public override Vector3 GetMomentumBeforeCollision() {
		var v = velocityBeforeCollision * rb.mass;
		// var d = Vector3.Dot(v.normalized, attr.group.root.data.velocity);
		// var bv = d > 0 ? v.normalized * attr.totalMass * d : Vector3.zero;
		// bv *= Mathf.Lerp(0f, 0.1f, attr.stiffness-0.35f);
		
		// return v + bv;
		return v * 5f;
		
		// return velocityBeforeCollision * rb.mass + attr.group.root.data.velocity * attr.totalMass;
	}
	
	protected override void CalcuTorque(float deltaTime, float reciDeltaTime) {
		if(target == null || !follow) return;
	
		Vector3 torqueAxis;
		float torqueAngle;
		
		var targetRotation = target.rotation * Quaternion.Inverse(transform.rotation);
		targetRotation.ToAngleAxis(out torqueAngle, out torqueAxis);
		
		if(torqueAngle != 360f) {
			var rotationError = torqueAxis * FixEuler(torqueAngle);
			var angularVelocity = EulerMethod(rotationError, lastRotationError, attr.torqueModify*2, attr.torqueDiffModify, reciDeltaTime);
			
			angularVelocity = Vector3.ClampMagnitude(angularVelocity, attr.maxAngularVelocity);
			angularVelocity = Vector3.ClampMagnitude(angularVelocity, Mathf.PI * 20f);
			rb.AddTorque(angularVelocity * attr.keepTorqueWeight, ForceMode.VelocityChange);
			lastRotationError = rotationError;
		}
	}
	
	protected override void CalcuForce(float deltaTime, float reciDeltaTime) {
		if(target == null || !follow) return;
	
		var com = target.position + target.rotation * centerOfMass;
		var positionError = com - rb.worldCenterOfMass;
		var velocity = EulerMethod(positionError, lastPositionError, attr.forceModify, attr.forceDiffModify, reciDeltaTime);
		var diff = Vector3.ClampMagnitude(velocity - rb.velocity, attr.maxVelocity);
		var force = Vector3.ClampMagnitude(diff * reciDeltaTime * attr.keepForceWeight, 1200) * rb.mass;
		// var force = diff * reciDeltaTime * attr.keepForceWeight* rb.mass;
		rb.AddForce(force);
		lastPositionError = positionError;
	}
	
	protected override void FixedUpdate() {
		if(target == null) return;
	
		// rb.velocity = Vector3.ClampMagnitude(rb.velocity, attr.maxVelocity);
		base.FixedUpdate();
	}
	
	protected override void OnCollisionEnter(Collision collision) {}
	protected override void OnCollisionStay(Collision collision) {}
	protected override void OnCollisionExit(Collision collision) {}
}
