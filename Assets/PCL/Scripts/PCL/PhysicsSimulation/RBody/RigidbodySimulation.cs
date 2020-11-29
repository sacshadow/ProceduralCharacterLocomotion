using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public static class RigidbodySimulation {
    
	public static void Init(RBody rb) {
		if(!rb.isSimulating) return;
	
		rb.data.mass = rb.mass;
		rb.data.momentOfInertia = rb.mass * rb.moiMulit;
	
		rb.next.Copy(rb.data);
		rb.next.force = PCLSimulation.gravity * rb.next.mass;
		rb.next.torque = Vector3.zero;
	}
	
	public static void CalculateMotion(RBody rb, float deltaTime) {
		if(!rb.isSimulating) return;
	
		var changeV = rb.next.force / rb.next.mass * deltaTime;
		var changeAV = rb.next.torque / rb.next.momentOfInertia * deltaTime;
	
		rb.next.velocity = Vector3.ClampMagnitude(rb.next.velocity + changeV, rb.velocityLimit);
		rb.next.position += (rb.data.velocity + changeV * 0.5f) * deltaTime;
		
		rb.next.angularVelocity = Vector3.ClampMagnitude(rb.next.angularVelocity + changeAV, rb.angularVelocityLimit);
		rb.next.rotation = Rotate(rb.next.rotation, (rb.data.angularVelocity + changeAV * 0.5f) * deltaTime);
	}
	
	public static Vector3 CalculatePosition(RBody rb, float deltaTime) {
		var changeV = rb.next.force / rb.next.mass * deltaTime;
		return rb.next.position + (rb.data.velocity + changeV * 0.5f) * deltaTime;
	}
	
	public static Quaternion CalculateRotation(RBody rb, float deltaTime) {
		var changeAV = rb.next.torque / rb.next.momentOfInertia * deltaTime;
		return Rotate(rb.next.rotation, (rb.data.angularVelocity + changeAV * 0.5f) * deltaTime);
	}
	
	public static void ApplyTransform(RBody rb, float deltaTime) {
		if(rb.isSimulating) {
			var disp = rb.next.position - rb.data.position;
			rb.next.velocity = disp / deltaTime;
		}
		
		rb.FinialApplyTransform();
	}
	
	private static Quaternion Rotate(Quaternion rotation, Vector3 v) {
		var q = new Quaternion(v.x,v.y,v.z,0);
		q *= rotation;
		rotation.x += q.x * 0.5f;
		rotation.y += q.y * 0.5f;
		rotation.z += q.z * 0.5f;
		rotation.w += q.w * 0.5f;
		return rotation;
	}
}
