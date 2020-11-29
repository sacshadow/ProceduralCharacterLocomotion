using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Jump : HumanoidAction {
	
	public float jumpSpeed = 5f;
	public float crouchHeight = 0.5f;
	
	protected DFA jump;
	protected KS_GroundMoveSequence groundMovement;
	
	public HA_Jump() {
		jump = DFAManager.Find("jump_up");
	
		groundMovement = new KS_GroundMoveSequence {
			groundMove = new KS_GroundMovement[] {
				new KS_LimbGroundMove{standeredMoveSpeed = 0.8f, shiftDownSpeed = 0f, shiftUpSpeed = 1f, limbCheckRadio = 0.15f},
				new KS_LimbGroundMove{standeredMoveSpeed = 1.4f, shiftDownSpeed = 0.9f, shiftUpSpeed = 2f, limbCheckRadio = 0.1f, moveSpeedOffest = 0.45f},
				new KS_LimbGroundMove{standeredMoveSpeed = 2.2f, shiftDownSpeed = 1.8f, shiftUpSpeed = 3.8f, limbCheckRadio = 0.075f, moveSpeedOffest = 0.36f, offset = -0.2f},
				new KS_LimbGroundMove{standeredMoveSpeed = 3.8f, shiftDownSpeed = 3.2f, shiftUpSpeed = 6f, limbCheckRadio = 0.05f, moveSpeedOffest = 0.2f, offset = -0.2f},
				new KS_LimbGroundMove{standeredMoveSpeed = 4.8f, shiftDownSpeed = 5.8f, shiftUpSpeed = 100f, limbCheckRadio = 0.05f, moveSpeedOffest = 0.1f, offset = -0.2f},
			}
		};
	}
	
	public override void Begin() {
		kinematics.SetState(groundMovement);
	}
	
	public override void End() {
		analysis.heightOffset = 0;
	}
	
	public override IEnumerator ActionUpdate() {
		var v = velocity;
		var t = 0f;
		v.y = -20f;
		
		behaviour.Towards(locomotion.direction.forward);
		
		while(attribute.isInAir) yield return null;
		
		animate.CrossFade(jump, 0.4f);
		analysis.heightOffset = -20f;
		
		var js = jumpSpeed * Mathf.Lerp(0.01f, 1, locomotion.body.data.velocity.Flat().magnitude);
		var ch = crouchHeight * Mathf.Lerp(1.5f, 1, locomotion.body.data.velocity.Flat().magnitude);
		
		while(attribute.airTime<0.75f && t<0.25f && analysis.disToGround > ch) {
			t += Time.deltaTime;
			behaviour.Move(v);
			yield return null;
		}
		
		// v.y = jumpSpeed;
		v.y = js;
		analysis.heightOffset = js;
		
		while(attribute.isOnGround) {
			behaviour.Move(v);
			yield return null;
		}
	}
}
