using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_FightStance : HumanoidAction {
	
	public DFA stance;
	public float stanceMoveSpeed = 4f;
	public float heightOffset = 0;
	
	protected KS_DefenceGroundMove defenceGroundMove;
	protected KS_LimbGroundMove groundMovement;
	
	public HAC_FightStance() {
		defenceGroundMove = new KS_DefenceGroundMove();
		groundMovement = new KS_LimbStanceMove{standeredMoveSpeed = 1.4f, limbCheckRadio = 0.15f, moveSpeedOffest = 0.4f};
		defenceGroundMove.groundMovement = groundMovement;
	}
	
	public void SetStance(DFA stance, Vector3 feetOffset_L, Vector3 feetOffset_R, float heightOffset) {
		this.stance = stance;
		if(state != null)
			animate.CrossFade(stance, 0.5f);
		groundMovement.feetOffset_L = feetOffset_L;
		groundMovement.feetOffset_R = feetOffset_R;
	}
	
	public override void Begin() {
		// kinematics.SetState(groundMovement);
		kinematics.SetState(defenceGroundMove);
		if(stance != null)
			animate.CrossFade(stance, 0.5f);
	}
	
	public override void End() {
		base.End();
		analysis.heightOffset = 0;
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		analysis.heightOffset = heightOffset;
		constraintAttribute.stiffness = 0f;
		while(attribute.airTime < 0.5f) {
			behaviour.Towards(state.inputDirection);
			behaviour.Move(state.inputMove * stanceMoveSpeed);
			yield return null;
		}
	}
	
}
