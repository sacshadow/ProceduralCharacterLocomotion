using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Roll : HumanoidAction {
	
	public float rollSpeed = 4.5f;
	
	protected DFA jump_fall;
	protected DFA[] roll;
	protected DFA rollAnimation;
	protected Vector3 keepDir;
	
	public HA_Roll() {
		jump_fall = DFAManager.Find("jump_fall");
		roll = DFAManager.Find("roll_front", "roll_right", "roll_back", "roll_left");
	}
	
	public override void End() {
		base.End();
		analysis.balanceCheck = true;
		analysis.heightOffset = 0;
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		if(attribute.airTime < 0.25f)
			yield return state.StartCoroutine(AccelerateOnGround());
		
		SetAnimationAndDirection();
		
		kinematics.CrossFadeAnimate(rollAnimation, 0.15f);
		locomotion.bodyStructure.ForEachLimb(l=>l.ik.ikUpdate = false);
		constraintAttribute.stiffness = 1f;
		analysis.balanceCheck = false;
		analysis.heightOffset = 0f;
		
		var t = 0f;
		var v = velocity.Flat().normalized * rollSpeed;
		
		while(t<rollAnimation.timeLength) {
			t += Time.deltaTime;
			
			behaviour.Towards(keepDir);
			behaviour.Move(v);
			
			yield return null;
		}
		
		kinematics.CrossFadeAnimate(jump_fall, 0.5f);
		constraintAttribute.stiffness = 0.1f;
		
		t = 0;
		while(t<0.25f) {
			t += Time.deltaTime;
			behaviour.Towards(state.inputDirection);
			// behaviour.Move(Vector3.zero);
			behaviour.Move(v);
			yield return null;
		}
		analysis.balanceCheck = true;
	}
	
	private IEnumerator AccelerateOnGround() {
		var v = locomotion.direction.forward * rollSpeed;
		
		if(state.inputMove.sqrMagnitude > 0.01f)
			v = state.inputMove.Flat().normalized * rollSpeed;
		
		analysis.heightOffset = -0.25f;
		
		while(attribute.airTime < 0.2f && Vector3.Dot(v.normalized, velocity.normalized) < 0.75f) {
			behaviour.Towards(state.inputDirection);
			behaviour.Move(v*10f);
			yield return null;
		}
		
		var d = Vector3.Dot(v, Vector3.ClampMagnitude(velocity.Flat(), rollSpeed));
		
		if(d < 0.95f && attribute.isOnGround)
			locomotion.body.data.velocity += v*(1.5f-d);
	}
	
	private void SetAnimationAndDirection() {
		var trans = attribute.airTime < 0.2f ? locomotion.direction : locomotion.transform;
		var v = velocity.Flat().normalized;
		
		var rollDir = GetDir(roll, v, trans.forward, trans.right);
		
		rollAnimation = roll[rollDir];
		keepDir =  Quaternion.Euler(0,-rollDir * 90, 0) * v;
	}
}
