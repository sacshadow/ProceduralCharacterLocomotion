using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Climb : HumanoidAction {
	
	protected DFA jump_wall_climb;
	protected KS_GroundMoveSequence groundMovement;
	
	public HA_Climb() {
		jump_wall_climb = DFAManager.Find("jump_wall_climb");
		
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
	
	public override void End() {
		base.End();
		
		analysis.balanceCheck = true;
		analysis.heightOffset = 0;
	}
	
	
	public override IEnumerator ActionUpdate() {
		var dir = -(detection.climb.hit.normal.Flat()).normalized;
		
		var fp = detection.climbPoint.hit.point + dir;
		var sp = position - Vector3.up;
		var cp = position - Vector3.up * analysis.targetHeight;
		var t = 0f;
		// var interval = jump_wall_climb.timeLength + 0.5f;
		var tv = (fp - cp)/Time.deltaTime;
		
		kinematics.CrossFadeAnimate(jump_wall_climb, 0.2f);
		// animate.CrossFade(jump_wall_climb, 0);
		
		// rdata.velocity = dir*5 + Vector3.up * 4;
		rdata.velocity = Vector3.Lerp(rdata.velocity, Vector3.up * 4 + dir * 0.5f, 6*Time.deltaTime);
		
		analysis.heightOffset = 4f;
		
		var p = 0.8f;
		
		var ap = 0f;
		
		analysis.balanceCheck = false;
		
		// while(t < interval || cp.y < fp.y) {
		// while(t < interval) {
		while(groundMovement.percentage < p && attribute.airTime < 0.25f && t < 1.2f) {
			t += Time.deltaTime;
			// var tp = Vector3.Lerp(sp,fp, t/jump_wall_climb.timeLength);
			
			behaviour.Towards(dir);
			
			var y = position.y - jump_wall_climb.Get(Mathf.Clamp01(ap)).keepHeight;
			ap = Mathf.Max((1 - (fp.y-y)/2f)*1f, 0);
			// kinematics.state.SetPercentage(ap);
			
			// groundMovement.percentage = Mathf.Min(1 - (fp.y-y)/1.75f, p+0.05f);
			
			
			cp = position - Vector3.up * analysis.targetHeight;
			
			if(position.y < fp.y) {
				var d = fp.y - position.y - 0.2f;
				var f = (d * 4 - velocity.y) * 2f;
				tv = Vector3.Lerp(tv, Vector3.up * f + dir, 2*Time.deltaTime);
				analysis.heightOffset = Mathf.Max(fp.y - position.y - velocity.y * 0.5f);
			} else {
				// tv = (tp - cp)/Time.deltaTime;
				tv = dir * 4 + Vector3.up * (fp.y + 0.35f - position.y) * 4 + Vector3.up;
				analysis.heightOffset = 0f;
			}
			
			behaviour.Move(tv);
			
			yield return null;
		}
		analysis.balanceCheck = false;
		
		kinematics.CrossFadeAnimate(jump_wall_climb, 0);
		t = ap * jump_wall_climb.timeLength;
		// kinematics.state.SetPercentage(ap);
		
		rdata.velocity = dir;
		analysis.heightOffset = 0f;
		
		while(t<jump_wall_climb.timeLength && attribute.airTime < 0.25f) {
			t += Time.deltaTime;
			behaviour.Move(dir*0.5f);
			yield return null;
		}
		
	}
	
}
