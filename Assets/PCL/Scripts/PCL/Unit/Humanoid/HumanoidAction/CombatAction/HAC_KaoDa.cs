using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_KaoDa : HumanoidAction {
	
	public DFA kaoDa;
	public float dashSpeed = 4f;
	
	public float percentage = 0;
	
	protected KS_SkillGroundMove groundMovement;
	
	public HAC_KaoDa() {
		kaoDa = DFAManager.Find("kf_kao_0");
		groundMovement = new KS_SkillGroundMove{standeredMoveSpeed = 1.8f, limbCheckRadio = 0.15f, moveSpeedOffest = 0.4f};
		
	}
	
	public override void Begin() {
		kinematics.SetState(groundMovement);
		groundMovement.speed = 1;
	}
	
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
	}
	
	
	public override IEnumerator ActionUpdate() {
		var dir = state.inputDirection;
		var move = dir * dashSpeed;
		
		animate.CrossFade(kaoDa, 0.15f);
		
		groundMovement.time = kaoDa.timeLength;
		// groundMovement.offset = 0.5f;
		
		groundMovement.feetOffset_L = new Vector3(-0.1f, 0,  0.15f);
		groundMovement.feetOffset_R = new Vector3( 0.1f, 0,  -0.45f);
		
		constraintAttribute.stiffness = 1.2f;
		
		while(groundMovement.percentage < 0.7f) {
			behaviour.Towards(dir);
			behaviour.Move(move);
			
			yield return null;
		}
		
		if(attribute.airTime < 0.25f)
			Dash(move, 2.5f);
		
		while(groundMovement.percentage < 1f) {

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
