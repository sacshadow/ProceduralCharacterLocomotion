using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Step : HumanoidAction {
	
	public DFA[] step;
	public float moveSpeed = 5f;
	
	// protected KS_GroundMoveSequence groundMovement;
	protected KS_StepMovement stepMovement;
	protected int moveDir = -1;
	
	public HA_Step() {
		step = DFAManager.Find("step_forward","step_right","step_backward","step_left");
		
		// groundMovement = new KS_GroundMoveSequence {
			// groundMove = new KS_GroundMovement[] {
				// new KS_LimbGroundMove{standeredMoveSpeed = 0.8f, shiftDownSpeed = 0f, shiftUpSpeed = 1f, limbCheckRadio = 0.15f},
				// new KS_LimbGroundMove{standeredMoveSpeed = 1.8f, shiftDownSpeed = 0.9f, shiftUpSpeed = 2.4f, limbCheckRadio = 0.1f, moveSpeedOffest = 0.4f, offset = -0.2f},
				// new KS_LimbGroundMove{standeredMoveSpeed = 2.6f, shiftDownSpeed = 1.8f, shiftUpSpeed = 3.6f, limbCheckRadio = 0.075f, moveSpeedOffest = 0.3f, offset = -0.25f},
				// new KS_LimbGroundMove{standeredMoveSpeed = 4.5f, shiftDownSpeed = 3.2f, shiftUpSpeed = 6f, limbCheckRadio = 0.05f, moveSpeedOffest = 0.2f, offset = -0.35f},
				// new KS_LimbGroundMove{standeredMoveSpeed = 6.4f, shiftDownSpeed = 5.8f, shiftUpSpeed = 100f, limbCheckRadio = 0.05f, moveSpeedOffest = 0.2f, offset = -0.45f},
			// }
		// };
		stepMovement = new KS_StepMovement{standeredMoveSpeed = 2.2f, limbCheckRadio = 0.15f,   moveSpeedOffest = 0.5f, offset = 0f};
	}
	
	public override void Begin() {
		kinematics.SetState(stepMovement);
		moveDir = -1;
	}
	
	public override void End() {
		base.End();
		analysis.heightOffset = 0;
	}
	
	public override IEnumerator ActionUpdate() {
		var dir = state.inputMove.normalized;
		if(dir.sqrMagnitude < 0.1f)
			dir = locomotion.direction.forward;
		var d = dir * moveSpeed;
		var t = 0f;
		
		if(attribute.airTime < 0.25f)
			Dash(d, 6f);
		
		// d.y = 2f;
		SetStepClip();
		
		// analysis.heightOffset -= 0.15f;
		
		while(Legal(t<0.6f &&  attribute.airTime < 0.5f)) {
			t+=Time.deltaTime;
			behaviour.Towards(state.inputDirection);
			behaviour.Move(d*0.5f);

			yield return null;
		}
	}
	
	protected void SetStepClip() {
		var f = locomotion.transform.forward.Flat().normalized;
		var s = locomotion.transform.right.Flat().normalized;
		var v = velocity.Flat().normalized;
		var crtDir = GetDir(step, v, f, s);
		if(crtDir != moveDir)
			SetStepClip(step, crtDir);
	}
	
	protected virtual void SetStepClip(DFA[] crtMoveMode, int crtDir) {
		moveDir = crtDir;
		animate.CrossFade(crtMoveMode[crtDir], 0.3f);
		
		if(crtDir == 0) {
			stepMovement.feetOffset_L = new Vector3(-0.075f, 0, -0.1f);
			stepMovement.feetOffset_R = new Vector3(0.075f, 0, 0.1f);
			stepMovement.stepOffset_L = 0.25f;
			stepMovement.stepOffset_R = 0.0f;
			stepMovement.moveSpeedOffest = -0.25f;
		}
		else if(crtDir == 1) {
			stepMovement.feetOffset_L = new Vector3(-0.1f, 0, -0.05f);
			stepMovement.feetOffset_R = new Vector3(0.1f, 0, 0.05f);
			stepMovement.stepOffset_L = 0.15f;
			stepMovement.stepOffset_R = 0.0f;
			stepMovement.moveSpeedOffest = -0.15f;
		}
		else if(crtDir == 2) {
			stepMovement.feetOffset_L = new Vector3(-0.075f, 0, -0.15f);
			stepMovement.feetOffset_R = new Vector3(0.075f, 0, 0.05f);
			stepMovement.stepOffset_L = 0.25f;
			stepMovement.stepOffset_R = 0.0f;
			stepMovement.moveSpeedOffest = 0.0f;
		}
		else if(crtDir == 3) {
			stepMovement.feetOffset_L = new Vector3(-0.1f, 0, -0.05f);
			stepMovement.feetOffset_R = new Vector3(0.1f, 0, 0.05f);
			stepMovement.stepOffset_L = 0.0f;
			stepMovement.stepOffset_R = 0.15f;
			stepMovement.moveSpeedOffest = -0.15f;
		}
	}
	
	private void Dash(Vector3 dashMove, float max = 3f) {
		var velocityChange = dashMove - rdata.velocity;
		rdata.velocity += Vector3.ClampMagnitude(velocityChange, max);
	}
}
