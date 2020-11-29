using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAI_Bandit : HAIBase {
	
	
	public float attackDis = 1.5f;
	public float keepDis = 3.5f;
	
	public float dodgeRate = 0.35f;
	
	public float
		attackWeight = 2,
		closeWeight = 1,
		keepAwayWeight = 1,
		idleWeight = 1;
	
	protected FS_Default fs_banditDefault;
	
	protected Func<IEnumerator>[] AIAction;
	
	protected override FightStyleBase InitUnarmedFightStyle() {
		fs_banditDefault = new FS_Default();
		return fs_banditDefault;
	}
	
	protected override void AI_Init() {
		AIAction = new Func<IEnumerator>[]{Attack, KeepClose, KeepAway, Idle,};
	}
	
	protected override IEnumerator AI_OnGroundDefault() {
		while(Legal(state.defaultAction.isProcess)) {
		
			yield return StartCoroutine(AIAction[GetRandAction()]());
		
			yield return null;
		}
	}
	
	protected virtual IEnumerator Attack() {
		if(target == null) yield break;
		if(target.attribute.isFallDown && URD.value > 0.25f) yield break;
		
		state.SetDefault(fightStyle.GetFightStance());
		// state.Reset();
		yield return null;
		
		if(attackWeight > 1)
			yield return StartCoroutine(ApproachToTarget(attackDis, 0.36f, 3f));
		
		if(Legal(DisToTarget(0.5f) < attackDis + 0.25f)) {
			
			SFXPlayer.PlaySFX("Yell", transform.position + Vector3.up, 0.5f, attribute.pitch);
			var t = 0f;
			while(Legal(t<0.3f)) {
				t+= Time.deltaTime;
				
				ModifyedMove(GetTargetDirection()*0.25f);
				yield return null;
			}
			
			yield return StartCoroutine(PerformAttack());
		}
		
		state.SetDefault(fightStyle.movement.defaultAction);
		// state.Reset();
	}
	
	protected virtual IEnumerator KeepClose() {
		float t = URD.Range(0.5f, 3f);
		float dodgeCheckInterval = 0;
		
		while(Legal(t>0)) {
			t -= Time.deltaTime;
		
			dodgeCheckInterval -= Time.deltaTime;
			if(dodgeCheckInterval <0 && IsTargetApproach()) {
				if(URD.value < dodgeRate)
					yield return StartCoroutine(DashBack());
				dodgeCheckInterval = 0.25f;
			}
			
			
			var dir = GetTargetDirection();
			var move = ModifyMovement(dir);
			
			if((DisToTarget(0) > 3f && Vector3.Angle(dir, move) < 45) && !run.isProcess)
				state.Play(run);
			else if(DisToTarget(0) < 4f && !defaultAction.isProcess)
				state.Reset();
			
			if(DisToTarget(0.5f) > keepDis)
				ModifyedMove(GetTargetDirection());
			else {
				t = Mathf.Min(t, 0.5f);
				ModifyedMove(Vector3.zero);
			}
			
			yield return null;
		}
		// if(Legal())
			// state.Reset();
	}
	
	protected virtual IEnumerator KeepAway() {
		float t = URD.Range(0.25f, 2f);
		float kd = keepDis * URD.Range(1f, 1.5f);
		float dodgeCheckInterval = 0;
		
		while(Legal(t>0)) {
			t -= Time.deltaTime;
			dodgeCheckInterval -= Time.deltaTime;
			if(dodgeCheckInterval <0 && IsTargetApproach()) {
				if(URD.value < dodgeRate)
					yield return StartCoroutine(DashBack());
				dodgeCheckInterval = 0.25f;
			}
			
			if(DisToTarget(1f) < kd)
				ModifyedMove(-GetTargetDirection()*0.5f);
			else	
				ModifyedMove(Vector3.zero);
				
			yield return null;
		}
	}
	
	protected virtual IEnumerator Idle() {
		float t = URD.Range(0.25f, 1.25f);
		float dodgeCheckInterval = 0;
		float kd = URD.Range(1.75f, 2.25f);
		
		state.SetDefault(defaultAction);
		// state.Reset();
		
		while(Legal(t > 0)) {
			t -= Time.deltaTime;
			dodgeCheckInterval -= Time.deltaTime;
			
			if(dodgeCheckInterval <0 && IsTargetApproach()) {
				if(URD.value < dodgeRate)
					yield return StartCoroutine(DashBack());
				dodgeCheckInterval = 0.25f;
			}
			if(DisToTarget(0.75f) < kd)
				ModifyedMove(-GetTargetDirection()*0.5f);
			else
				ModifyedMove(Vector3.zero);
			
			yield return null;
		}
	}
	
	protected virtual IEnumerator PerformAttack() {
		var combo = URD.Range(1,8) - 4;
		combo = Mathf.Max(combo, 1);
		
		for(int i =0; Legal(i<combo); i++) {
			HumanoidAction skill;
			if(URD.value > 0.35f)
				skill = fs_banditDefault.punch;
			else
				skill = fs_banditDefault.kick;
			
			
			var t = 0f;
			while(Legal(t<1f) && DisToTarget(0f) < 1.1f) {
				t += Time.deltaTime;
				// ModifyMovement(-GetTargetDirection()*2);
				// Move(-GetTargetDirection()*2);
				Move(-behaviour.locomotion.direction.forward*2);
				yield return null;
			}
			// t = 0f;
			// while( t< 3f && Vector3.Angle(GetTargetDirection(), behaviour.locomotion.direction.forward) > 45f) {
				// t += Time.deltaTime;
				// ModifyMovement(GetTargetDirection());
				// yield return null;
			// }
			
			state.Play(skill);
			var dir = GetTargetDirection();
			while(Legal(skill.isProcess)) {
				// dir = GetTargetDirection();
				if(DisToTarget(0.5f) > 1.25f) {
					Move(GetTargetDirection());
				}
				else if(DisToTarget(0.15f) < 0.5f)
					Move(-behaviour.locomotion.direction.forward);
					// ModifyMovement(-behaviour.locomotion.direction.forward);
				else
					Move(dir);
					// ModifyMovement(dir);
				
				yield return null;
			};
		}
		if(Legal(IsTargetApproach() && URD.value < dodgeRate))
			yield return StartCoroutine(DashBack());
	}
	
	protected virtual IEnumerator DashBack() {
		var t = 0f;
		state.Play(run);
		
		while(Legal(t<1.25f)) {
			t += Time.deltaTime;
			ModifyedMove(-behaviour.locomotion.direction.forward*4);
			yield return null;
		}
		state.Reset();
	}
	
	protected IEnumerator ApproachToTarget(float distance, float predict, float maxTime, bool tryDodge = true) {
		float t = maxTime;
		float dodgeCheckInterval = 0;
		
		while(Legal(t > 0 && DisToTarget(predict) > distance)) {
			t -= Time.deltaTime;
			dodgeCheckInterval -= Time.deltaTime;
			
			// if(tryDodge && dodgeCheckInterval <0 && IsTargetApproach()) {
				// if(URD.value < dodgeRate)
					// yield return StartCoroutine(DashBack());
				// dodgeCheckInterval = 0.25f;
			// }
			
			if(DisToTarget(0.25f) > 4f && !run.isProcess)
				state.Play(run);
			else if(DisToTarget(0.25f) < 4f && !state.defaultAction.isProcess)
				state.Reset();
			
			// var dir = GetTargetDirection();
			// var move = Vector3.Lerp(ModifyMovement(dir), dir, t/maxTime - 0.75f);
			
			// Move(move);
			ModifyedMove(GetTargetDirection());
			
			yield return null;
		}
		
	}
	
	protected virtual float[] GetStep() {
		return new float[]{attackWeight, closeWeight, keepAwayWeight, idleWeight,};
	}
	protected virtual int GetRandAction() {
		var step = GetStep();
		var value = URD.Range(0,Loop.Calculate(step, 0f, (a,b)=>a+b));
		
		for(int i=0; i<step.Length; i++) {
			if(value < step[i])
				return i;
			value -= step[i];
		}
		
		return step.Length -1;
	}
}
