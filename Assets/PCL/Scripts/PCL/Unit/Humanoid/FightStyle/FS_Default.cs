using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class FS_Default : FightStyleBase {
	
	public HAC_FightStance fightStance;
	public HAC_Punch_Bandit punch;
	public HAC_Kick kick;
	
	public FS_Default() {
		fightStance = new HAC_FightStance();
		fightStance.SetStance(DFAManager.Find("stance_2"), new Vector3(-0.15f,0,-0.25f), new Vector3(0.05f, 0, 0.35f), -0.3f);
		punch = new HAC_Punch_Bandit();
		kick = new HAC_Kick();
		movement = MoveStyleBase.Default();
	}
	
	public override HumanoidAction GetFightStance() {
		return fightStance;
	}
	
	public override IEnumerator FightStanceA() {
		state.SetDefault(fightStance);
		state.Play(fightStance);
		// if(punch.isProcess) punch.Stop();
		
		while(control.Legal(fightStance.isProcess || punch.isProcess)) {
			control.UpdateInput();
			
			if(input.action_0.IsDown() && !punch.isProcess)
				state.Play(punch);
			
			if(input.action_0.IsPress()) {
				if(input.jump.IsDown())
					yield return control.StartCoroutine(control.DoStateLegal(kick));
			} else if(control.IsRun() && !punch.isProcess)
				break;
			else if(input.action_1.IsPress() && !punch.isProcess)
				break;
			

			
			yield return null;
		}
		state.SetDefault(movement.defaultAction);
		state.Reset();
	}
	
}
