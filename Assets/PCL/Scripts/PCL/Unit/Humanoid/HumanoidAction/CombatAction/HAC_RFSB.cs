using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_RFSB : HumanoidAction {
	
	public DFA rfsb;
	public float speed = 3f;
	
	protected KS_RFSB ks_rfsb;
	
	public HAC_RFSB() {
		rfsb = DFAManager.Find("push_0");
		ks_rfsb = new KS_RFSB{standeredMoveSpeed = 1.8f, limbCheckRadio = 0.15f, moveSpeedOffest = 0.4f};
	}
	
	// public override void Begin() {
		
	// }
	
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		var dir = state.inputDirection;
		var move = dir * speed;
		
		
		ks_rfsb.speed = 1.5f;
		ks_rfsb.time = rfsb.timeLength;
		kinematics.SetState(ks_rfsb);
		animate.CrossFade(rfsb, 0.15f);
		
		constraintAttribute.stiffness = 0.25f;
		
		while(ks_rfsb.percentage < 0.3f) {
			var v = rfsb.Get(ks_rfsb.percentage).velocity;
			behaviour.Towards(dir);
			behaviour.Move(-move/2f + v);
			yield return null;
		}
		
		rdata.velocity += move;
		while(ks_rfsb.percentage < 0.6f) {
			constraintAttribute.stiffness = ks_rfsb.percentage + 0.15f;
			
			var v = rfsb.Get(ks_rfsb.percentage).velocity;
			
			behaviour.Towards(dir);
			behaviour.Move(move + v);
			
			yield return null;
		}
		
		while(ks_rfsb.percentage < 1f) {
			var v = rfsb.Get(ks_rfsb.percentage).velocity;
			
			behaviour.Towards(dir);
			behaviour.Move(Vector3.zero + v);
			
			yield return null;
		}
	}
}
