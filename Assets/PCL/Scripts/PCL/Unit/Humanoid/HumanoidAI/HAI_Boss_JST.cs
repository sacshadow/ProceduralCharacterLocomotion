using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAI_Boss_JST : HAI_Bandit {
	
	protected FS_JingSheTui fs_JingSheTui;
	
	protected override FightStyleBase InitUnarmedFightStyle() {
		fs_JingSheTui = new FS_JingSheTui();
		return fs_JingSheTui;
	}
	
	protected override IEnumerator Attack() {
		if(target == null) yield break;
		// if(target.attribute.isFallDown) yield break;
		
		state.SetDefault(fightStyle.GetFightStance());
		// state.Reset();
		yield return null;
		
		if(attackWeight > 1)
			yield return StartCoroutine(ApproachToTarget(attackDis, 0.36f, 3f));
		
		if(Legal(DisToTarget(0.5f) < attackDis + 0.25f)) {
			
			SFXPlayer.PlaySFX("Yell", transform.position + Vector3.up, 0.5f, attribute.pitch);
			// var t = 0f;
			// while(Legal(t<0.3f)) {
				// t+= Time.deltaTime;
				
				// ModifyedMove(GetTargetDirection()*0.25f);
				// yield return null;
			// }
			
			yield return StartCoroutine(PerformAttack());
		}
		
		state.SetDefault(fightStyle.movement.defaultAction);
		// state.Reset();
	}
	
	protected override IEnumerator PerformAttack() {
		var rd = URD.value;
		if(DisToTarget()>2f && URD.value > 0.65f) yield return StartCoroutine(FlyKick());
		else if(DisToTarget()>1.5f && URD.value > 0.85f) yield return StartCoroutine(Combo(1.75f, fs_JingSheTui.doubleKick));
		else if(DisToTarget()<1f && URD.value > 0.45f) yield return StartCoroutine(Combo(1f, fs_JingSheTui.saoTangTui, fs_JingSheTui.doubleKick));
		else if(rd > 0.75f) yield return StartCoroutine(Combo(1.1f, fs_JingSheTui.punch,  fs_JingSheTui.punch, fs_JingSheTui.doubleKick));
		else if(rd > 0.55f) yield return StartCoroutine(Combo(1.1f, fs_JingSheTui.punch, fs_JingSheTui.kick, fs_JingSheTui.kick));
		else if(rd > 0.35f) yield return StartCoroutine(Combo(1.2f, fs_JingSheTui.kick, fs_JingSheTui.doubleKick));
		else yield return StartCoroutine(NormalCombo());
	}
	
	protected IEnumerator FlyKick() {
		state.Play(fs_JingSheTui.feiTi);
		while(Legal(fs_JingSheTui.feiTi.isProcess)) {
			yield return null;
		}
	}
	
	protected IEnumerator Combo(float distance, params HumanoidAction[] comboAction) {
		
		while(Legal(DisToTarget(0.25f) < distance)) {
			ModifyMovement(GetTargetDirection());
			yield return null;
		}
		for(int i=0; Legal(i<comboAction.Length); i++) {
			Move(GetTargetDirection());
			yield return StartCoroutine(DoStateLegal(comboAction[i]));
		}
		if(Legal())
			yield return StartCoroutine(DashBack());
	}
	
	protected override IEnumerator DashBack() {
		ModifyedMove(-behaviour.locomotion.direction.forward);
		if(URD.value > 0.75f)
			yield return StartCoroutine(DoStateLegal(roll));
		else {
			var t = 0f;
			state.Play(run);
			
			while(Legal(t<1.25f)) {
				t += Time.deltaTime;
				ModifyedMove(-behaviour.locomotion.direction.forward*4);
				yield return null;
			}
			state.Reset();
		}
	}
	
	protected IEnumerator NormalCombo() {
		var combo = URD.Range(1,8) - 4;
		combo = Mathf.Max(combo, 1);
		
		for(int i =0; Legal(i<combo); i++) {
			HumanoidAction skill;
			
			if(DisToTarget() > 1.5f && URD.value > 0.65f)
				skill = fs_JingSheTui.doubleKick;
			else if(URD.value > 0.65f)
				skill = fs_JingSheTui.punch;
			else if(URD.value > 0.15f)
				skill = fs_JingSheTui.kick;
			else
				skill = fs_JingSheTui.saoTangTui;
			
			// var t = 0f;
			// while(Legal(t<1f) && DisToTarget(0) < 1.5f) {
				// t += Time.deltaTime;
				// ModifyMovement(-GetTargetDirection()*2);
				// yield return null;
			// }
			
			state.Play(skill);
			var dir = GetTargetDirection();
			Move(GetTargetDirection());
			while(Legal(skill.isProcess)) {
				// dir = GetTargetDirection();
				// if(DisToTarget(0.5f) > 1.25f) {
					Move(GetTargetDirection());
				// }
				// else if(DisToTarget(0.15f) < 0.5f)
					// ModifyMovement(-behaviour.locomotion.direction.forward);
				// else
					// ModifyMovement(dir);
				
				yield return null;
			};
			
			// yield return StartCoroutine(DashBack());
		}
		if(Legal(IsTargetApproach()))
			yield return StartCoroutine(DashBack());
	}
}
