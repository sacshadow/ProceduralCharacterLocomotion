using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_PianNing : KS_SkillGroundMove {
	
	public S_Grab grab;
	
	public RBody grabed;
	
	protected RBody grabTarget;
	protected Vector3 orgDirection;
	protected Vector3 throwUp;
	protected Vector3 throwTowards;
	// protected float rotateDirection;
	// protected Vector3 throwAxis;
	
	protected int stage = 0;
	
	protected Action[] ThrowAction;
	
	// protected float sign = 1;
	protected Vector3 right;
	
	
	
	public KS_PianNing () {
		grab = new S_Grab();
		ThrowAction = new Action[]{
			GrabTarget,
			CheckSuccess,
			PushOut,
			WaitForFinish,
		};
	}
	
	public override void Reset() {
		base.Reset();
		grab.Setup(locomotion);
		grab.arm_L.SetMode(LimbMode.PHYSICS);
		grab.arm_R.SetMode(LimbMode.PHYSICS);
		stage = 0;
	}
	
	public override void Simulate(float deltaTime) {
		SetHand();
		if(grabTarget != null)
			ThrowTarget();
		
		base.Simulate(deltaTime);
	}
	
	public void SetTarget(RBody grabTarget, Vector3 throwTowards) {
		this.grabTarget = grabTarget;
		this.throwTowards = throwTowards;
		
		grabed = grabTarget;
		
		if(grabTarget != null)
			InitThrowParamas();
	}
	
	protected void InitThrowParamas() {
		orgDirection = locomotion.transform.forward.Flat();
		var dot = Vector3.Dot(locomotion.transform.forward, throwTowards);
		
		throwUp = Vector3.up * Mathf.Max(-dot,0);
		
		// sign =  Mathf.Sign(Vector3.Dot(GetGrabDir(grabed), locomotion.transform.right));
	}
	
	protected virtual void SetHand() {
		var fdata = kinematics.animate.current.GetCurrentFrame();
		var q = locomotion.transform.rotation;
		
		var p_l = q * fdata.leftHand + grab.arm_L.link.linkPoint.position;
		var p_r = q * fdata.rightHand + grab.arm_R.link.linkPoint.position;
		
		grab.arm_L.link.targetPoint = p_l;
		grab.arm_R.link.targetPoint = p_r;
		
		if(percentage > 0.3f)
			grabed = null;
		
		grab.OnGrab(p_l, rb=>Grab(grab.arm_L, rb));
		grab.OnGrab(p_r, rb=>Grab(grab.arm_R, rb));
		
	}
	
	protected virtual void ThrowTarget() {
		ThrowAction[stage]();
	}
	
	protected void GrabTarget() {
		// /*
		var throwAxis = Vector3.Cross(locomotion.direction.forward, Vector3.up);
	
		var applyPoint = grabTarget.data.rotation * Vector3.up * 0.25f + grabTarget.data.position;
		var disp = applyPoint - position;
		// var disp = grabTarget.data.position - position;
		var force = Vector3.Cross(disp.normalized, throwAxis) * 800f + locomotion.direction.forward * 50f 
			-disp * 1000 + throwUp * 500f;
		
		var cd = (locomotion.body.data.position - grabTarget.data.position).normalized;
		var cf = Vector3.Dot(cd, force);
		if(cf > 0)
			force -= cd * cf * 0.5f;
		
		grabTarget.AddForceAtPosition(applyPoint, force);
		
		locomotion.body.AddForce(-force*0.5f);
		
		Debug.DrawRay(applyPoint, force, Color.red);
		// */
		
		// var applyPoint = (grab.arm_L.link.targetPoint + grab.arm_R.link.targetPoint)/2f;
		// var disp = applyPoint - locomotion.body.data.position;
		// var velocity = locomotion.body.data.velocity + Vector3.Cross(locomotion.body.data.angularVelocity, disp);
		
		// var throwUp = Vector3.up * Mathf.Clamp01(Vector3.Dot((position - grabTarget.data.position).normalized, locomotion.direction.forward));
		// var throwAxis = Vector3.Cross(locomotion.direction.forward, Vector3.up);
		// var force = velocity * locomotion.body.data.mass / grabTarget.data.mass * 800f + throwAxis * 600f + throwUp * 600f;
		
		// grabTarget.AddForceAtPosition(applyPoint, force);
		// locomotion.body.AddForce(-force*0.25f);
		
		if(percentage > 0.3f)
			stage++;
	}
	
	protected void CheckSuccess() {
		
		var disp = locomotion.body.data.position - grabTarget.data.position;
		
		if(disp.magnitude < 0.5f)
			stage++;
		else
			stage += 2;
	}
	
	protected void PushOut() {
		// /*
		var throwAxis = Vector3.Cross(locomotion.direction.forward, Vector3.up);
		
		var applyPoint = grabTarget.data.rotation * Vector3.up * 0.25f + grabTarget.data.position;
		var disp = applyPoint - position;
		// var disp = grabTarget.data.position - position;
		
		var throwDir = Vector3.Cross(disp.normalized, throwAxis);
		throwDir *= throwDir.y > 0 ? 250 : 150;
		
		// var force = throwDir + locomotion.direction.forward * 1000f;
		var force = throwDir + locomotion.direction.forward * 2000f + Vector3.up * 2000f;
		// var force = throwDir + locomotion.direction.forward * 1000f + throwUp * 1000f;
		
		var cd = (locomotion.body.data.position - grabTarget.data.position).normalized;
		var cf = Vector3.Dot(cd, force);
		if(cf > 0)
			force -= cd * cf * 0.5f;
		
		grabTarget.AddForceAtPosition(applyPoint, force);
		locomotion.body.AddForce(-force.Flat()*0.5f);
		// */
		// locomotion.body.AddForce(-force * throwForce * 0.5f);
		
		// var applyPoint = grabTarget.data.rotation * Vector3.up * 0.25f + grabTarget.data.position;
		// var disp = applyPoint - position;
		
		// applyPoint = (grab.arm_L.link.targetPoint + grab.arm_R.link.targetPoint)/2f;
		// var velocity = locomotion.body.data.velocity + Vector3.Cross(locomotion.body.data.angularVelocity, disp);
		
		// var force = velocity * locomotion.body.data.mass / grabTarget.data.mass * 800f;
		
		// grabTarget.AddForceAtPosition(applyPoint, force);
		// locomotion.body.AddForce(-force*0.25f);
		
		if(disp.magnitude > 0.5f || percentage > 0.75f)
			stage++;
	}
	
	protected void WaitForFinish() {
		grabed = null;
	}
	
	private Vector3 GetGrabDir(RBody grabed) {
		return (grabed.data.position - locomotion.body.data.position).normalized;
	}
	
	private Vector3 GetRight(Vector3 dir) {
		var rt = ((dir.Flat() + locomotion.targetFaceDirection)/2f).normalized;
		return rt;
	}
	
	private Vector3 GetGrabDisp(RBody grabed) {
		return grabed.data.position - locomotion.body.data.position;
	}
	
	private Vector3 GetThrowDirAssist(RBody grabed) {
		var dir = locomotion.targetFaceDirection;
		
		// var ai = GetAimAIOhterThan(grab, dir);
		// if(ai != null)
			// return TargetAIMovement(ai);
		// else
			return dir;
	}
	
	protected virtual void Grab(L_Arm arm, Rigidbody rb) {
		var p = rb.worldCenterOfMass;
		// var p = constraint.rb.position;
		var offset = arm.link.linkPoint.parent.TransformDirection(arm.link.linkPoint.localPosition*0.25f);
		var disp = p - arm.link.linkPoint.position;
		
		var targetPoint = disp + disp.normalized * 0.15f + arm.link.linkPoint.position;
		arm.link.targetPoint = targetPoint + offset;
		
		var c = rb.GetComponent<ConstraintBase>();
		if(c != null) HoldTarget(arm, c);
		if(c != null && percentage > 0.3f) grabed = c.attr.group.root;
	}
	
	protected void HoldTarget(L_Arm arm, ConstraintBase constraint) {
		var disp = constraint.rb.worldCenterOfMass - arm.rbody.data.position;
		constraint.ApplyDeform(disp * constraint.rb.mass, arm.rbody.data.position, 100);
	}
}
