using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using SDTK.Cameras;

using URD = UnityEngine.Random;

public abstract class FightStyleBase {

	public MoveStyleBase movement;

	public HumanoidControl control;
	public UnitAttribute attribute {get {return control.attribute; }}
	public PlayerInput input {get {return control.input; }}
	
	public HumanoidState state {get {return control.state; }}
	public HumanoidBehaviour behaviour {get {return control.state.behaviour; }}
	public Locomotion locomotion {get {return control.state.behaviour.locomotion; }}
	public Transform direction {get {return control.state.behaviour.locomotion.direction; }}
	public Detection detection {get {return control.state.behaviour.locomotion.detection; }}
	
	public FightStyleBase() {
		movement = MoveStyleBase.Default();
	}
	
	public abstract HumanoidAction GetFightStance();
	
	// public virtual void GetReplaceDefaultAction(Action<HumanoidAction> Callback) {
	// }
	
	public virtual IEnumerator FightStanceA() {
		yield return null;
	}
	
	public virtual IEnumerator FightStanceB() {
		yield return null;
	}
	
	public virtual IEnumerator FightStanceC() {
		yield return null;
	}
	
	public virtual IEnumerator FightStanceD() {
		yield return null;
	}
	
	protected void UpdateInput() {
		if(!control.isControl) return;
		
		var camPoint = CameraFollow.Instance.transform;
		
		control.input.UpdateAxis(camPoint.position - camPoint.forward*0.25f, 
			(move, face)=>
				control.state.UpdateInput(CameraFollow.Convert(move), 
				PlayerControl.CheckEnemyNearby(camPoint.position, face)));
	}
	
}
