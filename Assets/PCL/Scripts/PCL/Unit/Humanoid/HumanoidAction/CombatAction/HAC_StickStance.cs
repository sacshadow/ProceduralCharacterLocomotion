using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_StickStance : HA_DefaultAction {
	
	public DFA stance;
	public KS_StickHold stickHold;
	public float stanceMoveSpeed = 2.4f;
	public WP_Stick stick;
	
	public HAC_StickStance (WP_Stick stick) {
		this.stick = stick;
		
		stance = DFAManager.Find("stance_2");
		stickHold = new KS_StickHold(stick);
	}
	
	public override void Begin() {
		stick.SetStance(stick.stance.normalStance[0]);
		kinematics.SetState(stickHold);
		animate.CrossFade(stance, 0.5f);
	}
	
	public override IEnumerator ActionUpdate() {
		while(true) {
			behaviour.Towards(state.inputDirection);
			behaviour.Move(state.inputMove * stanceMoveSpeed);
			yield return null;
		}
	}
}
