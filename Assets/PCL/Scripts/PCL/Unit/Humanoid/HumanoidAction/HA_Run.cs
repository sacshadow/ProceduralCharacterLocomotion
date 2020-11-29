using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Run : HumanoidAction {
	
	public float runSpeed = 5.5f, stepSpeed = 4.2f;
	
	public float percentage {get {return kinematics.percentage; }}
	
	protected DFA[] runAnim;
	protected DFA[] step;
	protected KS_GroundMoveSequence groundMovement;
	protected KS_StepMovement stepMovement;
	protected int moveDir = -1;
	
	public HA_Run() {
		runAnim = DFAManager.Find("idle", "walk_forward", "run_forward");
		step = DFAManager.Find("step_forward","step_right","step_backward","step_left");
		
		groundMovement = new KS_GroundMoveSequence {
			groundMove = new KS_GroundMovement[] {
				new KS_LimbGroundMove{standeredMoveSpeed = 0.8f, shiftDownSpeed = 0f,   shiftUpSpeed = 1f,   limbCheckRadio = 0.15f},
				new KS_LimbGroundMove{standeredMoveSpeed = 1.2f, shiftDownSpeed = 0.9f, shiftUpSpeed = 2f,   limbCheckRadio = 0.1f,   moveSpeedOffest = 0.45f, offset = -0.2f},
				new KS_LimbGroundMove{standeredMoveSpeed = 2.2f, shiftDownSpeed = 1.8f, shiftUpSpeed = 3.8f, limbCheckRadio = 0.075f, moveSpeedOffest = -0.01f, offset = -0.25f},
				new KS_LimbGroundMove{standeredMoveSpeed = 3.2f, shiftDownSpeed = 3.2f, shiftUpSpeed = 6f,   limbCheckRadio = 0.05f,  moveSpeedOffest = -0.01f, offset = -0.35f},
				new KS_LimbGroundMove{standeredMoveSpeed = 4.5f, shiftDownSpeed = 5.8f, shiftUpSpeed = 100f, limbCheckRadio = 0.05f,  moveSpeedOffest = -0.01f, offset = -0.45f},
			}
		};
		
		stepMovement = new KS_StepMovement{standeredMoveSpeed = 2.2f, limbCheckRadio = 0.15f,   moveSpeedOffest = 0.5f, offset = 0f};
	}
	
	public override void Begin() {
		
	}
	
	public override void End() {
		base.End();
		analysis.heightOffset = 0;
	}
	
	public override IEnumerator ActionUpdate() {
		
		while(Legal(attribute.airTime < 0.5f)) {
			if(IsMoveForward())
				yield return state.StartCoroutine(RunForward());
			else
				yield return state.StartCoroutine(StepMove());
			
			yield return null;
		}
		
	}
	
	public IEnumerator StepMove() {
		moveDir = -1;
		
		// var percentage = kinematics.percentage;
		// kinematics.SetState(stepMovement);
		SetKinematics(stepMovement);
		// stepMovement.percentage = percentage;
		
		while(Legal(!IsMoveForward() && attribute.airTime < 0.5f)) {
			// var dot = (Vector3.Dot(rdata.velocity.normalized, state.inputMove) + 0.5f) /2f;
			
			var dot = Vector3.Dot(locomotion.transform.forward, state.inputMove);
			dot = dot < -0.5f ? Mathf.Lerp(1,0,Mathf.Abs(rdata.angularVelocity.y/2f)) : 1;
			
			behaviour.Towards(state.inputDirection);
			// behaviour.Move(state.inputMove * dot * stepSpeed);
			behaviour.Move(state.inputMove * dot * stepSpeed);
			
			SetStepClip();
			
			yield return null;
		}
	}
	
	public IEnumerator RunForward() {
		var runDirection = locomotion.direction.forward;
		animate.CrossFade(runAnim[0], 0.3f);
		var t = 0f;
		var index = 0;
		
		analysis.heightOffset = 0.2f;
		
		// var percentage = kinematics.percentage;
		
		// groundMovement.percentage = percentage;
		
		// kinematics.SetState(groundMovement);
		SetKinematics(groundMovement);
		
		// Debug.Log("IN RUN");
		while(Legal(IsMoveForward() && t < 0.25f && attribute.airTime < 0.5f)) {
			index = SwitchAnimateBySpeed(index);
		
			if(state.inputMove.sqrMagnitude > 0.01f) {
				t = 0;
				runDirection = Vector3.Slerp(runDirection, state.inputMove, 4*Time.deltaTime);
			}
			else
				t += Time.deltaTime;
			
			// var dot = Mathf.Clamp01(Vector3.Dot(runDirection.normalized, locomotion.transform.forward));
			var dot = (Vector3.Dot(runDirection.normalized, locomotion.transform.forward) + 1) /2f;
			
			var diff = detection.flipPoint.hit.point.y - analysis.groundPoint.y;
			var y = detection.flip.isHit ? diff*3f : 0;
			analysis.heightOffset = prediction.futurePoint.isHit ? 
				prediction.futurePoint.hit.point.y - analysis.groundPoint.y : 0;
			
			behaviour.Towards(runDirection.normalized);
			behaviour.Move(runDirection * dot * runSpeed + Vector3.up * y);
			
			yield return null;
		}
		
		// Debug.Log("OUT RUN");
	}
	
	protected virtual void SetKinematics(KinematicsState state) {
		kinematics.SetState(state);
	}
	
	protected bool IsMoveForward() {
		return Vector3.Angle(state.inputDirection, state.inputMove) < 60f;
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
		
		if(crtDir == 1) {
			stepMovement.feetOffset_L = new Vector3(-0.15f, 0, -0.05f);
			stepMovement.feetOffset_R = new Vector3(0.15f, 0, 0.05f);
			stepMovement.stepOffset_L = 0.35f;
			stepMovement.stepOffset_R = 0.0f;
			stepMovement.moveSpeedOffest = 0f;
		}
		else if(crtDir == 2) {
			stepMovement.feetOffset_L = new Vector3(-0.075f, 0, -0.15f);
			stepMovement.feetOffset_R = new Vector3(0.075f, 0, 0.15f);
			stepMovement.stepOffset_L = 0.25f;
			stepMovement.stepOffset_R = 0.0f;
			stepMovement.moveSpeedOffest = 0.25f;
		}
		else if(crtDir == 3) {
			stepMovement.feetOffset_L = new Vector3(-0.15f, 0, -0.05f);
			stepMovement.feetOffset_R = new Vector3(0.15f, 0, 0.05f);
			stepMovement.stepOffset_L = 0.0f;
			stepMovement.stepOffset_R = 0.35f;
			stepMovement.moveSpeedOffest = 0f;
		}
	}
	
	private int SwitchAnimateBySpeed(int index) {
		var id = GetIDBySpeed(velocity.Flat().magnitude);
		if(id == index) return index;
		
		animate.CrossFade(runAnim[id], 1f);
		return id;
	}
	
	private int GetIDBySpeed(float speed) {
		if(speed < 1.2f) return 0;
		if(speed < 2f) return 1;
		return 2;
	}
}
