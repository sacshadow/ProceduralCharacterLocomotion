using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_RFSB : KS_SkillGroundMove {
	
	public S_Grab grab;
	
	public KS_RFSB () {
		grab = new S_Grab();
	}
	
	public override void Reset() {
		base.Reset();
		grab.Setup(locomotion);
		grab.arm_L.SetMode(LimbMode.PHYSICS);
		grab.arm_R.SetMode(LimbMode.PHYSICS);
	}
	
	public override void Simulate(float deltaTime) {
		// Debug.Break();
		
		var fdata = kinematics.animate.current.GetCurrentFrame();
		var q = locomotion.transform.rotation;
		
		var p_l = q * fdata.leftHand + grab.arm_L.link.linkPoint.position;
		var p_r = q * fdata.rightHand + grab.arm_R.link.linkPoint.position;
		
		grab.arm_L.link.targetPoint = p_l;
		grab.arm_R.link.targetPoint = p_r;
	
		if(percentage > 0.2f || percentage < 0.8f) {
			grab.OnGrab(p_l, rb=>Push(grab.arm_L, rb));
			grab.OnGrab(p_r, rb=>Push(grab.arm_R, rb));
		}
		
		base.Simulate(deltaTime);
	}
	
	protected void Push(L_Arm arm, Rigidbody rb) {
		var p = rb.worldCenterOfMass;
		// var p = constraint.rb.position;
		var disp = p - arm.link.linkPoint.position;
		var offset = arm.link.linkPoint.parent.TransformDirection(arm.link.linkPoint.localPosition*0.25f);
		var targetPoint = disp + disp.normalized * 0.15f + arm.link.linkPoint.position;
		arm.link.targetPoint = targetPoint + offset;
		
		var c = rb.GetComponent<ConstraintBase>();
		if(c != null && disp.magnitude < 0.25f) PushUnit(arm, c);
	}
	
	protected void PushUnit(L_Arm arm, ConstraintBase constraint) {
		var body = constraint.attr.group.root;
		
		var v = locomotion.body.data.velocity;
		var av = (arm.rbody.data.position - locomotion.analysis.supportPoint).normalized * (v.magnitude + 1);
		body.AddForceAtPosition(arm.rbody.data.position, av * locomotion.body.data.mass * 4f);
		// constraint.ApplyDeform(v*constraint.rb.mass,arm.rbody.data.position, 1f);
		
		var disp = constraint.rb.worldCenterOfMass - arm.rbody.data.position;
		constraint.ApplyDeform(disp.normalized * constraint.rb.mass, arm.rbody.data.position, 1);
	}
}
