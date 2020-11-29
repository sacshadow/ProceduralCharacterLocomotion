using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_PianNing : HumanoidAction {
	
	
	public DFA throw_L, throw_R;
	public RBody throwTarget;
	public KS_PianNing pianNing;
	
	public HAC_PianNing() {
		throw_L = DFAManager.Find("throw_guojianshuai_L");
		throw_R = DFAManager.Find("throw_guojianshuai_R");
		
		pianNing = new KS_PianNing();
		pianNing.feetOffset_L = new Vector3(-0.2f, 0,  0.05f);
		pianNing.feetOffset_R = new Vector3( 0.2f, 0,  -0.05f);
	}
	
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		var dir = Vector3.Dot(locomotion.transform.right, state.inputDirection);
		
		pianNing.offset = dir > 0 ? 0.25f : 0.75f;
		kinematics.SetState(pianNing);
		animate.CrossFade(dir > 0 ? throw_L : throw_R, 0.25f);
		pianNing.speed = 1.25f;
		
		pianNing.SetTarget(throwTarget, state.inputDirection);
		behaviour.ForceTowards(state.inputDirection);
		
		constraintAttribute.stiffness = 0.65f;
		
		while(pianNing.percentage < 1 && throwTarget == pianNing.grabed) {
			// behaviour.Towards(state.inputDirection);
			behaviour.ForceTowards(state.inputDirection);
			behaviour.Move(state.inputMove * 0.5f);
			
			yield return null;
		}
	}
	
	
	
}
