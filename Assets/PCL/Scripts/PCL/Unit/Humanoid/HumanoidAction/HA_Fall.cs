using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Fall : HumanoidAction {
	
	public Vector3 dropVelocity = Vector3.zero;
	protected DFA idle, jump_down, jump_fall, jump_land;
	protected KS_GroundMoveSequence groundMovement;
	
	
	
	public HA_Fall() {
		idle = DFAManager.Find("idle");
		jump_down = DFAManager.Find("jump_down");
		jump_fall = DFAManager.Find("jump_fall");
		jump_land = DFAManager.Find("jump_land");
		
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
	
	
	public override IEnumerator ActionUpdate() {
		dropVelocity = velocity;
		kinematics.CrossFadeAnimate(jump_down, 1f);
		while(attribute.isInAir && velocity.y > 0) {
			yield return null;
		}
		kinematics.CrossFadeAnimate(jump_fall, 3f);
		
		while(TimeToLand() > 0.3f) {
			dropVelocity = velocity;
			yield return null;
		}
		
		kinematics.CrossFadeStateAndAnimate(groundMovement, jump_land, 3f);
		
		while(attribute.isInAir) {
			dropVelocity = velocity;
			yield return null;
		}
	}
	
	private float TimeToLand() {
		if(!detection.down.isHit) return 100;
		
		var a = 0.5f * 9.81f;
		var b = -velocity.y;
		var c = detection.down.hit.distance;
		
		var rt = Mathf.Sqrt(b*b-4*a*c);
		var t = (-b + rt)/a/2;
		return t;
	}
	
}
