using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_DoubleKick : HumanoidAction {
	
	public DFA doubleKick;
	public float moveSpeed = 4f;
	
	public HAC_DoubleKick() {
		doubleKick = DFAManager.Find("kick_2");
	}
	
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		var dir = state.inputDirection;
		// var move = dir * moveSpeed;
		
		var ks = new KS_Animate(doubleKick, false);
		ks.speed = 1f;
		
		kinematics.CrossFadeStateAndAnimate(ks, doubleKick);
		constraintAttribute.stiffness = 0.8f;
		
		while(ks.percentage < 0.15f) {
			behaviour.Towards(dir);
			behaviour.Move(dir*moveSpeed);
			yield return null;
		}
		
		if(attribute.airTime < 0.25f)
			Dash(state.inputDirection * moveSpeed + Vector3.up * 2f, 2f);
		
		while(ks.percentage < 1) {
			behaviour.Towards(dir);
			behaviour.Move(Vector3.zero);
			yield return null;
		}
	}
	
	private void Dash(Vector3 dashMove, float max = 3f) {
		var velocityChange = dashMove - rdata.velocity;
		rdata.velocity += Vector3.ClampMagnitude(velocityChange, max);
	}
	
}
