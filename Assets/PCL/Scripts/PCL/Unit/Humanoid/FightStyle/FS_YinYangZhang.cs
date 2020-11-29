using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class FS_YinYangZhang : FightStyleBase {
	
	public HAC_FightStance fightStance;
	
	public HAC_Punch punch;
	public HAC_Kick kick;
	public HAC_DoubleKick doubleKick;
	public HAC_KaoDa kaoDa;
	
	public HAC_YinYangZhang_Yin yinZhang;
	
	public HAC_RFSB rfsb;
	public HAC_PianNing pianNing;
	
	
	public FS_YinYangZhang() {
		fightStance = new HAC_FightStance();
		fightStance.SetStance(DFAManager.Find("stance_2"), new Vector3(-0.15f,0,-0.25f), new Vector3(0.05f, 0, 0.35f), -0.3f);
		
		punch = new HAC_Punch();
		kick = new HAC_Kick();
		kaoDa = new HAC_KaoDa();
		doubleKick = new HAC_DoubleKick();
		
		yinZhang = new HAC_YinYangZhang_Yin();
		
		rfsb = new HAC_RFSB();
		pianNing = new HAC_PianNing();
		
		movement = MoveStyleBase.Default();
	}
	
	public override HumanoidAction GetFightStance() {
		return fightStance;
	}
	
	public override IEnumerator FightStanceA() {
		state.SetDefault(fightStance);
		state.Play(fightStance);
		
		while(control.Legal(fightStance.isProcess || punch.isProcess)) {
			// control.UpdateInput();
			UpdateInput();
			
			if(input.action_0.IsDown() && !punch.isProcess)
				state.Play(punch);
			
			if(input.action_0.IsPress()) {
				if(input.jump.IsDown())
					yield return control.StartCoroutine(control.DoStateLegal(kick));
				
				if(input.run.IsDown() && attribute.airTime < 0.5f)
					yield return control.StartCoroutine(control.DoStateLegal(kaoDa));
			} 
			else if(!punch.isProcess && control.IsRun())
				break;
			else if(input.action_1.IsPress() && !punch.isProcess)
				break;
			
			if(input.jump.IsDown())
				yield return control.StartCoroutine(control.DoStateLegal(doubleKick));
			
			yield return null;
		}
		punch.isProcess = false;
		state.SetDefault(movement.defaultAction);
		state.Reset();
	}
	
	public override IEnumerator FightStanceB() {
		state.Play(yinZhang);
		while(control.Legal(yinZhang.isProcess && input.action_1.IsPress())) {
			// control.UpdateInput();
			UpdateInput();
			
			if(input.action_0.IsDown() || input.run.IsDown()) {
				pianNing.throwTarget = yinZhang.GetGrabTarget();
			
				var f = Vector3.Dot(state.behaviour.locomotion.transform.forward, state.inputDirection);
				if(f > 0.35f)
					yield return control.StartCoroutine(control.DoStateLegal(rfsb));
				else
					yield return control.StartCoroutine(control.DoStateLegal(pianNing));
			}
			
			yield return null;
		}
		state.Reset();
	}
	
	
	
}
