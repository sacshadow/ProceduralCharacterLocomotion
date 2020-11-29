using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HumanoidState : MonoBehaviour {
	public const float queueWaitTime = 0.3f;
	
	public HumanoidBehaviour behaviour;
	
	public HumanoidAction defaultAction, currentAction, next;
	
	// [NonSerialized]
	public Vector3 inputMove, inputDirection = Vector3.forward;
	
	public string currentActionType;
	
	private float queueInterTime;
	
	public void Stop() {
		StopAllCoroutines();
		defaultAction.Stop();
		if(currentAction != null) currentAction.Stop();
		currentAction = next = null;
		inputDirection = behaviour.locomotion.direction.forward;
		inputMove = Vector3.zero;
	}
	
	public void Reset() {
		Stop();
		PlayDefault();
	}
	
	public void UpdateInput(Vector3 move, Vector3 direction) {
		inputMove = move;
		inputDirection = direction;
	}
	
	public void SetDefault(HumanoidAction action) {
		if(defaultAction == action) return;
		
		var currentIsDefault = currentAction == defaultAction;
		
		if(defaultAction != null) 
			defaultAction.Stop();
		
		defaultAction = action;
		defaultAction.state = this;
		if(currentIsDefault)
			Reset();
	}
	
	public void Queue(HumanoidAction action) {
		if(currentAction == null || currentAction == defaultAction)
			Play(action);
		else {
			next = action;
			queueInterTime = Time.time;
		}
	}
	
	public void Play(HumanoidAction action) {
		if(currentAction != null)
			currentAction.Stop();
		StopAllCoroutines();
		StartCoroutine(IPlay(action));
	}
	
	public IEnumerator IPlay(HumanoidAction action) {
		action.state = this;
		currentAction = action;
		
		if(currentAction != null) currentActionType = currentAction.ToString();
		
		next = null;
		yield return StartCoroutine(action.Process());
		if(next != null && Time.time - queueInterTime < queueWaitTime)
			Play(next);
		else
			PlayDefault();
	}
	
	public void PlayDefault() {
		currentAction = defaultAction;
		if(currentAction != null) currentActionType = currentAction.ToString();
		next = null;
		StartCoroutine(defaultAction.Process());
	}
}
