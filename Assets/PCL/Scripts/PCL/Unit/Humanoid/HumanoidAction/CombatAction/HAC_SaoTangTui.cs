using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_SaoTangTui : HumanoidAction {
	
	public DFA saoTangTui;
	public float moveSpeed = 2.5f;
	
	public HAC_SaoTangTui() {
		saoTangTui = DFAManager.Find("kick_6");
	}
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		var animate = saoTangTui;
		var ks = new KS_Animate(animate, false);
		ks.speed = 0.75f;
		
		kinematics.CrossFadeStateAndAnimate(ks, animate);
		constraintAttribute.stiffness = 1f;
		
		var dir = state.inputDirection;
		var move = dir * moveSpeed;
		
		while(ks.percentage < 0.65f) {
			behaviour.Towards(dir);
			behaviour.Move(move);
			yield return null;
		}
		
		// var dir = state.inputDirection;
		
		while(ks.percentage < 0.85f) {
			behaviour.Towards(dir);
			behaviour.Move(Vector3.zero);
			yield return null;
		}
		
		// lastComboTime = Time.time;
	}
}
