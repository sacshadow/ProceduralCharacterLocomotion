using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_DefaultAction : HumanoidAction {
	
	public DFA idle;
	public DFA[] walk;
	public float moveSpeed = 2.8f;
	
	protected int moveDir;
	protected KS_DefenceGroundMove defenceMovement;
	protected KS_GroundMoveSequence groundMovement;
	
	public HA_DefaultAction() {
		idle = DFAManager.Find("idle");
		walk = DFAManager.Find(
			"walk_forward","walk_frontRight","walk_right","walk_backRight",
			"walk_backward","walk_backLeft", "walk_left","walk_frontLeft");
		
		defenceMovement = new KS_DefenceGroundMove();
		groundMovement = new KS_GroundMoveSequence {
			groundMove = new KS_GroundMovement[] {
				new KS_LimbGroundMove{standeredMoveSpeed = 0.8f, shiftDownSpeed = 0f, shiftUpSpeed = 1f, limbCheckRadio = 0.15f},
				new KS_LimbGroundMove{standeredMoveSpeed = 1.8f, shiftDownSpeed = 0.9f, shiftUpSpeed = 2.4f, limbCheckRadio = 0.1f, moveSpeedOffest = 0.4f, offset = -0.2f},
				new KS_LimbGroundMove{standeredMoveSpeed = 2.6f, shiftDownSpeed = 1.8f, shiftUpSpeed = 3.6f, limbCheckRadio = 0.075f, moveSpeedOffest = 0.3f, offset = -0.25f},
				new KS_LimbGroundMove{standeredMoveSpeed = 4.5f, shiftDownSpeed = 3.2f, shiftUpSpeed = 6f, limbCheckRadio = 0.05f, moveSpeedOffest = 0.2f, offset = -0.35f},
				new KS_LimbGroundMove{standeredMoveSpeed = 6.4f, shiftDownSpeed = 5.8f, shiftUpSpeed = 100f, limbCheckRadio = 0.05f, moveSpeedOffest = 0.2f, offset = -0.45f},
			}
		};
		defenceMovement.groundMovement = groundMovement;
	}
	
	public override void Begin() {
		// var percentage = groundMovement.percentage;
		kinematics.SetState(groundMovement);
		// kinematics.SetState(defenceMovement);
		// groundMovement.percentage = percentage;
	}
	
	public override IEnumerator ActionUpdate() {
		while(Legal(OnGround())) {
			if(LegalAndNotMove())
				yield return state.StartCoroutine(Idle());
			else if(Legal(OnGround()))
				yield return state.StartCoroutine(Move());
			
			yield return null;
		}
	}
	
	private IEnumerator Idle() {
		animate.CrossFade(idle, 0.3f);
		
		while(LegalAndNotMove()) {
			behaviour.Towards(state.inputDirection);
			behaviour.Move(Vector3.zero);
			
			yield return null;
		}
	}
	
	protected IEnumerator Move() {
		moveDir = -1;
		while(LegalAndMove()) {
			behaviour.Towards(state.inputDirection);
			behaviour.Move(state.inputMove * moveSpeed);
			analysis.heightOffset = prediction.futurePoint.isHit ? 
				Mathf.Clamp(prediction.futurePoint.hit.point.y - analysis.groundPoint.y, -0.15f, 0.4f) : 0;
			SetMoveClip();
			
			yield return null;
		}
	}
	
	protected bool OnGround() {
		return attribute.airTime < 0.35f;
	}
	protected bool LegalAndMove() {
		return Legal(OnGround()) && (state.inputMove.sqrMagnitude > 0.01f || IsMove());
	}
	protected bool LegalAndNotMove() {
		return Legal(OnGround()) && (state.inputMove.sqrMagnitude < 0.01f && !IsMove());
	}
	
	protected bool IsMove() {
		return velocity.sqrMagnitude > 0.015f;
	}
	
	protected void SetMoveClip() {
		if(groundMovement.current == 0 && moveDir != -1)
			SetToIdle();
		else {
			var f = locomotion.transform.forward.Flat().normalized;
			var s = locomotion.transform.right.Flat().normalized;
			var v = velocity.Flat().normalized;
			var crtDir = GetDir(walk, v, f, s);
			if(crtDir != moveDir)
				SetMoveClip(walk, crtDir);
		}
	}
	
	protected void SetToIdle() {
		animate.CrossFade(idle, 1.5f);
		moveDir = -1;
	}
	
	protected virtual void SetMoveClip(DFA[] crtMoveMode, int crtDir) {
		moveDir = crtDir;
		animate.CrossFade(crtMoveMode[crtDir], 0.3f);
	}
}
