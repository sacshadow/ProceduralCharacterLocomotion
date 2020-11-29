using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public abstract class UnitControl : MonoBehaviour {
	
	// [HideInInspector]
	public bool isControl = false, isPlaying = true;
	public UnitAttribute attribute;
	public PlayerInput input;
	
	public virtual void SetPlaying(bool state) {
		isPlaying = state;
	}
	
	public virtual void SetMove(Vector3 move, Vector3 direction) {
		
	}
	
	public virtual void SetControl(bool state) {
		isControl = state;
		Reset();
	}
	
	public virtual void SetPose(Vector3 position, Vector3 direction) {
		transform.position = position;
		transform.rotation = Quaternion.LookRotation(direction.Flat().normalized);
	}
	
	public virtual void Init() {
		
	}
	
	public virtual void Reset() {
		StopAllCoroutines();
		
		if(PlayerControl.Instance.unit == this && !isControl)
			PlayerControl.Instance.unit = null;
			
		if(attribute.isDead)
			return;	
			
		if(isControl) {
			PlayerControl.Instance.unit = this;
			StartCoroutine(OnPlayerControl());
		}else
			StartCoroutine(AIControl());
	}
	
	public virtual void Remove() {
		
	}
	
	protected virtual IEnumerator OnPlayerControl() {
		yield return null;
	}
	
	protected virtual IEnumerator AIControl() {
		yield return null;
	}
}
