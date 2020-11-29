using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_YinYangZhang : KinematicsState {
	
	public KS_LimbStanceMove groundMovement;
	
	public S_Grab grab;
	public RBody momentum;
	// public float percentage = 0;
	
	public float kaihe = 0.5f;
	public float speed = 0;
	
	public RBody grabTarget;
	
	public KS_YinYangZhang () {
		groundMovement = new KS_LimbStanceMove{standeredMoveSpeed = 1.4f, limbCheckRadio = 0.2f, moveSpeedOffest = 0.0f};
		
		grab = new S_Grab();
	}
	
	public override void Reset() {
		groundMovement.kinematics = this.kinematics;
		groundMovement.Reset();
		// percentage = 0f;
		grab.Setup(locomotion);
		momentum = locomotion.bodyStructure.bodyMomentum;
		
		grab.arm_L.SetMode(LimbMode.PHYSICS);
		grab.arm_R.SetMode(LimbMode.PHYSICS);
		
		grabTarget = null;
		
		kaihe = 0.15f;
		speed = 0f;
	}
	
	public override (float speed, float percent) GetSpeedAndPercentage(float deltaTime) {
		return groundMovement.GetSpeedAndPercentage(deltaTime);
	}
	
	public override void Simulate(float deltaTime) {
		StepOffset();
		ProcessHandMovement(deltaTime);
		YinZhang(deltaTime);
		
		groundMovement.Simulate(deltaTime);
	}
	
	protected void YinZhang(float deltaTime) {
		grab.Process(deltaTime);
		grab.ForEachArmGrabed(GrabOrDeflect);
	}
	
	protected void StepOffset() {
		var v = locomotion.transform.InverseTransformDirection(locomotion.body.data.velocity);
		// s = Mathf.Abs(s);
		var x = Mathf.Lerp(0.1f, 0.35f, 1-Mathf.Abs(v.z)/2f);
		var z = Mathf.Lerp(0.05f, 0.175f, 1-Mathf.Abs(v.z)/2f);
		
		groundMovement.feetOffset_L = new Vector3(-x, 0, -z);
		groundMovement.feetOffset_R = new Vector3(x, 0, z);
	}
	
	protected void ProcessHandMovement(float deltaTime) {
		grabTarget = null;
	
		var dfa = kinematics.animate.current;
		var fdata = dfa.GetCurrentFrame();
		// var q = locomotion.direction.rotation;
		var q = locomotion.transform.rotation;
		
		var hand_L = grab.arm_L.rbody;
		var hand_R = grab.arm_R.rbody;
		
		
		var v = locomotion.transform.InverseTransformDirection(locomotion.body.data.velocity);
		var s =	v.z - Mathf.Abs(v.x)/2f
			- locomotion.body.data.angularVelocity.magnitude;
		
		kaihe = Mathf.Clamp01(kaihe - (s - speed) * 0.1f);
		speed = s;
		
		var hm = Mathf.Lerp(0.025f, 0.35f, Mathf.Abs(v.x)/2f);
		var circumference = Mathf.Lerp(hm,0.5f,Mathf.Sin(kaihe*Mathf.PI/2f));
		
		var o = new Vector3(0,Mathf.Lerp(+0.15f, -0.25f, kaihe), Mathf.Lerp(-0.05f, -0.25f, kaihe));
		var offset = momentum.data.position - locomotion.body.data.position;
		// var offset = Vector3.zero;
		
		var shift = 0.15f;
		var a = (percentage + shift) * 2 * Mathf.PI;
		var b = (percentage + shift + 0.5f) * 2 * Mathf.PI;
		
		var d =Mathf.Lerp(0.25f, 0f, kaihe);
		var l = Mathf.Lerp(0.5f, 1.5f, kaihe);
		
		var lh = new Vector3( Mathf.Cos(a) *l +d, Mathf.Sin(a), 0) * circumference + o;
		var rh = new Vector3(-Mathf.Cos(b) *l -d, Mathf.Sin(b), 0) * circumference + o;
		
		grab.arm_L.link.targetPoint = q * (fdata.leftHand + lh) + grab.arm_L.link.linkPoint.position + offset * 0.2f;
		grab.arm_R.link.targetPoint = q * (fdata.rightHand + rh) + grab.arm_R.link.linkPoint.position + offset * 0.2f;
	}
	
	protected void GrabOrDeflect(L_Arm arm, Rigidbody rb) {
		var c = rb.GetComponent<ConstraintBase>();
		if(c != null) Deflect(arm, c);
		else Grab(arm, rb);
	}
	
	
	
	protected void Grab(L_Arm arm, Rigidbody rb) {
		
	}
	protected void Deflect(L_Arm arm, ConstraintBase constraint) {
		var handDis = SetHandToBlockPoint(arm, constraint);
	
		if(handDis > 0.25f) return;
	
		var target = constraint.attr.group.root;
		var dist = (target.data.position - position).Flat().magnitude;
		
		grabTarget = target;
		
		// if(target.data.velocity.y < 1.25f && dist < 0.65f)
		// if(target.data.velocity.y < 1.25f || dist < 0.5f)
			Drag(arm, constraint, target);
		// else
			// Expand(arm, constraint, target);
	}
	
	protected float SetHandToBlockPoint(L_Arm arm, ConstraintBase constraint) {
		// var p = constraint.rb.worldCenterOfMass;
		var p = constraint.rb.position;
		var disp = p - arm.link.linkPoint.position;
		var offset = arm.link.linkPoint.parent.TransformDirection(arm.link.linkPoint.localPosition*0.25f);
		var targetPoint = disp + disp.normalized * 0.15f + arm.link.linkPoint.position;
		arm.link.targetPoint = targetPoint + offset + Vector3.up*0.15f;
		
		Debug.DrawLine(arm.link.linkPoint.position, targetPoint, new Color(0,1,1,1));
		
		////////////////////////////////////////////////////////////////////////////////////
		
		var rb = constraint.rb;
		var d = arm.rbody.data.position - arm.link.linkPoint.position;
		
		d = d - d.normalized * -0.15f + arm.link.linkPoint.position;
		
		disp = p - d;
		var relativeVelocity = rb.velocity - arm.rbody.data.velocity;
		
		var f = (disp * 0.5f - relativeVelocity * 0.5f) * rb.mass;
		
		constraint.ApplyDeform(-f, d, 1f);
		
		// return (p - arm.rbody.data.position).magnitude;
		return (p - arm.rbody.data.position).magnitude;
	}
	
	protected void Drag(L_Arm arm, ConstraintBase constraint, RBody target) {
		var flatV = target.data.velocity.Flat();
		var handDis = arm.rbody.data.position - locomotion.body.data.position;
		var selfV = locomotion.body.data.velocity + Vector3.Cross(locomotion.body.data.angularVelocity, handDis);
		
		var disp = target.data.position - locomotion.body.data.position - locomotion.direction.forward * 0.4f;
		// var dir = Vector3.Cross(target.data.position - locomotion.body.data.position, Vector3.up).normalized 
			// - locomotion.direction.forward * 0.4f;
		
		var f = -disp * 8000f
		// var f = -dir * 6000f
			+ (selfV - flatV*0.5f) * 2000f;
		f = f * locomotion.body.data.mass / target.data.mass;
		// f = Vector3.ClampMagnitude(f, 600f);
		
		// var m = Mathf.Lerp(500f, 900f, Mathf.Abs(locomotion.body.data.angularVelocity.y) + flatV.magnitude - disp.magnitude);		
		var m = Mathf.Lerp(100f , 600f, Mathf.Abs(locomotion.body.data.angularVelocity.y)*0.5f + flatV.magnitude*2f - disp.magnitude*2f);		
		f = Vector3.ClampMagnitude(f, m);
		
		var dir = (locomotion.body.data.position - target.data.position).normalized;
		var fd = Vector3.Dot(dir, f);
		// if(fd > 0)
			f -= dir * fd * 0.5f;
		
		// Debug.DrawRay(arm.rbody.data.position, f);
		
		// target.AddForceAtPosition(arm.rbody.data.position, f);
		target.AddForce(f);
		target.AddTorque(Vector3.Cross(arm.rbody.data.position - target.data.position, f/5f));
		// locomotion.body.AddForce(-f*0.25f);
		// locomotion.body.AddForce(f * 0.25f);
		
		// Debug.Break();
	}
	
	protected void Expand(L_Arm arm, ConstraintBase constraint, RBody target) {
		var self = locomotion.body;
		var flatV = target.data.velocity.Flat() - self.data.velocity.Flat();
		var disp = (target.data.position - self.data.position).Flat();
		var dir = disp.normalized;
		var dist = disp.magnitude;
		
		var tva = dir * Vector3.Dot(dir, flatV);
		var tvb = flatV - tva;
		
		var tm = target.data.mass;
		// var kf = Vector3.zero;
			
		// if(dist < 0.8f)
			// kf = dir * (0.8 - dist) * 0.75f * tm;
		
		var f = (-tva + tvb * 0.5f + locomotion.targetFaceDirection) * tm * 10f;
		f = Vector3.ClampMagnitude(f, self.mass * 3.2f);
		
		target.AddForce(f);
		self.AddForce(-f * 0.5f);
		
	}
	
}
