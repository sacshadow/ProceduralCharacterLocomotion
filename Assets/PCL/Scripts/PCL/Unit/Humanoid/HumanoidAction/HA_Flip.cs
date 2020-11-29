using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Flip : HumanoidAction {
	
	protected DFA jump_over_F, jump_over_R, jump_over_L;
	protected KS_GroundMoveSequence groundMovement;
	
	public HA_Flip() {
		jump_over_F = DFAManager.Find("jump_over_f");
		// jump_over_F = DFAManager.Find("jump_over");
		
		groundMovement = new KS_GroundMoveSequence {
			groundMove = new KS_GroundMovement[] {
				new KS_LimbGroundMove{standeredMoveSpeed = 0.8f, shiftDownSpeed = 0f, shiftUpSpeed = 1f, limbCheckRadio = 0.15f},
				new KS_LimbGroundMove{standeredMoveSpeed = 1.8f, shiftDownSpeed = 0.9f, shiftUpSpeed = 2f, limbCheckRadio = 0.1f, moveSpeedOffest = 0.4f},
				new KS_LimbGroundMove{standeredMoveSpeed = 2.6f, shiftDownSpeed = 1.8f, shiftUpSpeed = 3.6f, limbCheckRadio = 0.075f, moveSpeedOffest = 0.3f},
				new KS_LimbGroundMove{standeredMoveSpeed = 4.5f, shiftDownSpeed = 3.2f, shiftUpSpeed = 6f, limbCheckRadio = 0.05f, moveSpeedOffest = 0.2f},
				new KS_LimbGroundMove{standeredMoveSpeed = 6.4f, shiftDownSpeed = 5.8f, shiftUpSpeed = 100f, limbCheckRadio = 0.05f, moveSpeedOffest = 0.2f},
			}
		};
	}
	
	public override void Begin() {
		// var p = kinematics.percentage;
		kinematics.SetState(groundMovement);
		// groundMovement.percentage = p;
	}
	
	public override IEnumerator ActionUpdate() {
		var dir = -(detection.flip.hit.normal.Flat()).normalized;
		
		var fp = detection.flipPoint.hit.point;
		var sp = position - Vector3.up * analysis.targetHeight;
		var cp = position - Vector3.up * analysis.targetHeight;
		var t = 0f;
		var interval = jump_over_F.timeLength + 0.1f;
		
		// kinematics.CrossFadeAnimate(jump_over_F, 0);
		animate.CrossFade(jump_over_F, 0);
		
		while(t < interval && cp.y < fp.y && attribute.airTime < 0.25f) {
			t += Time.deltaTime;
			
			var tp = Vector3.Lerp(sp,fp, t/jump_over_F.timeLength);
			// Debug.DrawLine(fp, p, Color.red);
			behaviour.Towards(dir);
			
			cp = position - Vector3.up * analysis.targetHeight;
			
			if(Time.timeScale > 0)
				behaviour.Move((tp - cp)/Time.deltaTime + Vector3.up);
			
			yield return null;
		}
	}
	
	
	
}
