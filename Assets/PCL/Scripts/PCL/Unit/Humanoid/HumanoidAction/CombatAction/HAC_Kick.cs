using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_Kick : HumanoidAction {
	
	public DFA[] kick_forward, kick_spin;
	public float moveSpeed = 2f;
	
	protected int index = 0;
	protected float lastComboTime = 0;
	protected bool combo = false;
	
	public HAC_Kick() {
		kick_forward = DFAManager.Find("kick_0","kick_4");
		kick_spin = DFAManager.Find("kick_3","kick_1");
	}
	
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		var animate = GetKickAnimate();
		var ks = new KS_Animate(animate, false);
		ks.speed = 1f;
		
		kinematics.CrossFadeStateAndAnimate(ks, animate);
		constraintAttribute.stiffness = 0.65f;
		
		while(ks.percentage < 0.25f) {
			behaviour.Towards(state.inputDirection);
			behaviour.Move(state.inputMove);
			yield return null;
		}
		
		var dir = state.inputDirection;
		
		while(ks.percentage < 1) {
			behaviour.Towards(dir);
			behaviour.Move(Vector3.zero);
			yield return null;
		}
		
		lastComboTime = Time.time;
	}
	
	private DFA GetKickAnimate() {
		if(Mathf.Abs(rdata.angularVelocity.y) < 1) {
			var combo = Time.time - lastComboTime < 0.5f;
			if(combo)
				index = (index + 1) % kick_forward.Length;
			else
				index = 0;
				
			if(attribute.airTime < 0.25f && attribute.clashShock < 0.05f)
				Dash(state.inputDirection * (combo ? 1.5f : 1.5f) + Vector3.up * 1.5f, 1f);
				
			return kick_forward[index];
		}
		else
			return rdata.angularVelocity.y < 0 ? kick_spin[0] : kick_spin[1];
	}
	
	private void Dash(Vector3 dashMove, float max = 3f) {
		var velocityChange = dashMove - rdata.velocity;
		rdata.velocity += Vector3.ClampMagnitude(velocityChange, max);
	}
	
}
