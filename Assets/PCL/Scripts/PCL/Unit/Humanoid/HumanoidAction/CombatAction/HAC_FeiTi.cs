using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_FeiTi : HumanoidAction {
	
	public DFA feiTi;
	public float dashSpeed = 5f;
	
	public float percentage = 0;
	
	protected KS_SkillGroundMove groundMovement;
	
	public HAC_FeiTi() {
		feiTi = DFAManager.Find("kick_5");
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
		
		animate.CrossFade(feiTi, 0.15f);
		
		groundMovement.time = feiTi.timeLength;
		// groundMovement.offset = 0.5f;
		
		groundMovement.feetOffset_L = new Vector3(-0.1f, 0,  0f);
		groundMovement.feetOffset_R = new Vector3( 0.1f, 0,  0f);
		
		constraintAttribute.stiffness = 1.2f;
		
		while(groundMovement.percentage < 0.25f) {
			behaviour.Towards(dir);
			behaviour.Move(move);
			
			yield return null;
		}
		
		locomotion.bodyStructure.SetLimbMode(LimbMode.ANIMATE, LimbMode.ANIMATE, 0.05f);
		if(attribute.airTime < 0.25f)
			Dash(move, 4.5f);
		
		while(groundMovement.percentage < 1f) {

			behaviour.Towards(dir);
			behaviour.Move(Vector3.zero);
			
			yield return null;
		}
	}
	
	private void Dash(Vector3 dashMove, float max = 5f) {
		var velocityChange = dashMove - rdata.velocity;
		rdata.velocity += Vector3.ClampMagnitude(velocityChange, max) + Vector3.up * 3.4f;
	}
	
}
