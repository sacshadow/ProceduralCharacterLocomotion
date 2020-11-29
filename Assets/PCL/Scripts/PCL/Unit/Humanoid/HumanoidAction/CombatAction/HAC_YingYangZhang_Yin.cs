using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_YingYangZhang_Yin : HumanoidAction {
	
	public DFA stance;
	
	public float moveSpeed = 2f;
	
	protected L_Arm[] hand;
	protected KS_YinYangZhang yinYangZhang;
	
	public HAC_YingYangZhang_Yin () {
		stance = DFAManager.Find("stance_3");
		yinYangZhang = new KS_YinYangZhang();
	}
	
	public RBody GetGrabTarget() {
		return yinYangZhang.grabTarget;
	}
	
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		kinematics.SetState(yinYangZhang);
		animate.CrossFade(stance, 0.5f);
		
		constraintAttribute.stiffness = 0.0f;
		
		while(attribute.airTime < 0.5f) {
			behaviour.Towards(state.inputDirection);
			behaviour.Move(state.inputMove * moveSpeed);
			
			yield return null;
		}
	}
}
