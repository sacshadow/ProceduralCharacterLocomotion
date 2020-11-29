using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HumanoidAction {
	
	public HumanoidState state;
	
	public HumanoidBehaviour behaviour {get {return state.behaviour; }}
	public ConstraintAttribute constraintAttribute {get {return behaviour.constraintAttribute; }}
	public UnitAttribute attribute {get {return state.behaviour.attribute; }}
	public Locomotion locomotion {get {return state.behaviour.locomotion; }}
	public Detection detection {get {return state.behaviour.locomotion.detection; }}
	public Analysis analysis {get {return state.behaviour.locomotion.analysis; }}
	public Prediction prediction {get {return state.behaviour.locomotion.prediction; }}
	public Kinematics kinematics {get {return state.behaviour.locomotion.kinematics; }}
	public Animate animate {get {return state.behaviour.locomotion.kinematics.animate; }}
	
	public Transform transform {get {return state.behaviour.locomotion.transform; }}
	public Transform direction {get {return state.behaviour.locomotion.direction; }}
	
	public Vector3 position {get {return state.transform.position; }}
	public Quaternion rotation {get {return state.behaviour.locomotion.direction.rotation; }}
	
	public RData rdata {get {return state.behaviour.locomotion.body.data; }}
	public Vector3 velocity {get {return state.behaviour.locomotion.body.data.velocity; }}
	public Vector3 angularVelocity {get {return state.behaviour.locomotion.body.data.angularVelocity; }}
	
	public bool isProcess = false;
	
	public virtual void Begin() {
		
	}
	
	public virtual void End() {
		
	}
	
	public virtual void Stop() {
		isProcess = false;
		End();
	}
	
	public virtual IEnumerator Process() {
		Begin();
		isProcess = true;
		yield return state.StartCoroutine(ActionUpdate());
		isProcess = false;
		End();
	}
	
	public virtual IEnumerator ActionUpdate() {
		yield return null;
	}
	
	//TOOL
	/*******************************************************************************/
	
	protected bool Legal(bool state) {
		return attribute.isAlive && attribute.isStandup && attribute.isBalance && state;
	}
	
	protected int GetDir(DFA[] crtMoveMode, Vector3 dir, Vector3 forward, Vector3 right) {
		// var f = locomotion.transform.forward.Flat().normalized;
		// var s = locomotion.transform.right.Flat().normalized;
		return UT.GetDirectionIndex(dir, forward, right, crtMoveMode.Length);
	}
	
}
