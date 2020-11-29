using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_FallDown : HumanoidAction {
	public static string collisionGroundSound = "CollideGround";
	
	public Vector3 fallingVelocity;
	public float speedHitGround = 0;
	
	protected Vector3 com;
	protected float weight = 1f;
	protected List<Rigidbody> body;
	
	public override void Begin() {
		if(behaviour.constraintSkeleton != null)
			body = Loop.SelectEach(behaviour.constraintSkeleton.spine, x=>x.GetComponent<Rigidbody>());
		else
			body = new List<Rigidbody>();
		
		com = behaviour.constraintSkeleton.GetCOM();
		weight = 1f;
		fallingVelocity = Vector3.zero;
	}
	
	public override IEnumerator ActionUpdate() {
		if(behaviour.constraintSkeleton == null) yield break;
		
		yield return null;
		fallingVelocity = UpdateDiff();
		
		
		locomotion.bodyStructure.SetLimbMode(LimbMode.PHYSICS, LimbMode.PHYSICS, 0.3f);
		
		yield return state.StartCoroutine(WaitWhile(t=>fallingVelocity.y >= -0.25f));
		yield return state.StartCoroutine(WaitWhile(t=>fallingVelocity.y < -0.25f));
		
		CombatManager.ApplyFallingDamage(behaviour, fallingVelocity, speedHitGround);
		SFXPlayer.PlaySFX(collisionGroundSound, position, 
			-speedHitGround/8f, URD.Range(0.75f, 1.25f));
		
		var downTime = Mathf.Max(fallingVelocity.magnitude/2f - 2f, -speedHitGround/4f - 2f)/2.5f;
		downTime = Mathf.Min(downTime, 4f);
		
		yield return state.StartCoroutine(WaitWhileDraged(t=>fallingVelocity.magnitude > 0.5f || t < downTime));
		// if(weight > 0)
		SetComplateDown();
		
		yield return state.StartCoroutine(WaitWhile(t=> weight > 0));
	}
	
	protected IEnumerator WaitWhile(Func<float, bool> Condition) {
		float t = 0;
		while(Condition(t) || Time.timeScale <= 0) {
			t += Time.deltaTime;
			fallingVelocity = UpdateDiff();
			speedHitGround = Mathf.Min(fallingVelocity.y, speedHitGround);
			StopBodyKeep();
			yield return null;
		}
	}
	
	protected IEnumerator WaitWhileDraged(Func<float, bool> Condition) {
		float t = 0, interal = 0, shock = -speedHitGround + fallingVelocity.Flat().magnitude;
		
		while(Condition(t) || Time.timeScale <= 0) {
			t += Time.deltaTime;
			fallingVelocity = UpdateDiff();
			speedHitGround = Mathf.Min(fallingVelocity.y, speedHitGround);
			StopBodyKeep();
			
			interal += Time.deltaTime;
			if(shock > 2f && interal > 0.05f) {
				BloodPopManager.SetDeathBlood(behaviour.constraintSkeleton.GetCOM(),shock/30f+0.25f);
				interal = 0;
			}
			
			yield return null;
		}
	}
	
	protected Vector3 UpdateDiff() {
		if(Time.timeScale <=0) return Vector3.zero;
		
		var bodyVelocity = Vector3.zero;
		body.ForEach(x=>bodyVelocity += x.velocity);
		
		var nCom = behaviour.constraintSkeleton.GetCOM();
		var rt = (nCom - com)*50/Time.timeScale + bodyVelocity/body.Count;
		com = nCom;
		Debug.DrawRay(nCom, bodyVelocity, Color.red);
		return rt;
	}
	
	protected void StopBodyKeep() {
		if(weight < 0) {
			var c = com;
			var t = com;
			Cast.LineRay(c + Vector3.up, -Vector3.up, 2, behaviour.climbMask, hit=>t = hit.point + Vector3.up * 0.3f);
			var p = Vector3.Lerp(behaviour.transform.position, t, 4*Time.deltaTime);
			behaviour.locomotion.body.controller.Move(p-behaviour.transform.position);
			return;
		}
	
		weight -= (2f + GetAngle()/30f) * Time.deltaTime;
		if(locomotion.body.data.velocity.magnitude > 4)
			weight -= 2*Time.deltaTime;
		
		if(weight < 0.5f)
			locomotion.body.SetSimulation(false);
		
		if(weight < 0)
			SetComplateDown();
		else
			SetFollowWeight(weight);
	}
	
	protected void SetComplateDown() {
		// behaviour.ForEachRB(x=>x.SetSimulation(false));
		locomotion.body.SetSimulation(false);
		attribute.isStandup = false;
		SetFollowWeight(0);
	}
	
	protected void SetFollowWeight(float weight) {
		if(constraintAttribute != null) constraintAttribute.followWeight = weight;
	}
	
	private float GetAngle() {
		if(behaviour.constraintSkeleton == null)
			return 0;
	
		var root = behaviour.constraintSkeleton.spine[0];
		var waist = behaviour.constraintSkeleton.spine[1];
		var chest = behaviour.constraintSkeleton.spine[2];
		
		return Vector3.Angle(root.up + waist.up + chest.up, Vector3.up);
	}
}
