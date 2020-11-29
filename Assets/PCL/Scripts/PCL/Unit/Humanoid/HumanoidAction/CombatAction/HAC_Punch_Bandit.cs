using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_Punch_Bandit : HumanoidAction {

	
	public DFA[] punch;
	public float moveSpeed = 2.2f;
	
	protected int index = 0;
	protected float lastComboTime = 0;
	protected KS_SkillGroundMove groundMovement;
	protected bool combo = false;
	
	public HAC_Punch_Bandit() {
		this.punch = DFAManager.Find("punch_0", "punch_1");
		groundMovement = new KS_SkillGroundMove{standeredMoveSpeed = 1.8f, limbCheckRadio = 0.15f, moveSpeedOffest = 0.4f};
		groundMovement.feetOffset_L = new Vector3(-0.075f, 0, -0.45f);
		groundMovement.feetOffset_R = new Vector3( 0.075f, 0,  0.15f);
	}
	
	public override void Begin() {
		kinematics.SetState(groundMovement);
		
		combo = Time.time - lastComboTime < 0.5f;
		
		if(combo)
			index = (index + 1) % punch.Length;
		else
			index = 0;
		
		groundMovement.speed = combo ? 1.85f : 1.65f;
		groundMovement.time = punch[index].timeLength;
		animate.CrossFade(punch[index], 0.15f);
		
	}
	
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
	}
	
	public override IEnumerator ActionUpdate() {
		// var t = 0f;
		
		constraintAttribute.stiffness = 0.85f;
		
		while(groundMovement.percentage < 0.35f) {
			behaviour.Towards(state.inputDirection);
			behaviour.Move(state.inputMove * moveSpeed);
			
			yield return null;
		}
		
		var dir = state.inputDirection;
		var move = dir * moveSpeed;
		
		if(attribute.airTime < 0.25f && attribute.clashShock < 0.05f)
			Dash(dir * (combo ? 3f : 4f), 3.5f);
			// Dash(dir * (combo ? 2f : 2.5f), 1.5f);
		
		while(groundMovement.percentage < 0.95f) {
			behaviour.Towards(dir);
			behaviour.Move(move);
			
			yield return null;
		}
		
		lastComboTime = Time.time;
	}
	
	private void Dash(Vector3 dashMove, float max = 3f) {
		var velocityChange = dashMove - rdata.velocity;
		rdata.velocity += Vector3.ClampMagnitude(velocityChange, max);
	}
	
}
