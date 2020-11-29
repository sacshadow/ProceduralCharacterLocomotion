using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Prediction {
	public const float checkHeight = 4;
	public const float checkRadio = 0.075f;
	
	public Locomotion locomotion;
	public RData data {get {return locomotion.body.data; }}
	
	public bool isOn = true;
	public float predictTime = 0.3f;
	
	public CastResult futurePoint;
	// public Vector3 futureLandPoint;
	
	
	public Prediction (Locomotion locomotion) {
		this.locomotion = locomotion;
		futurePoint = new CastResult();
	}
	
	public virtual Vector3 PredictFinalVelocity(Vector3 offset) {
		var v = (futurePoint.hit.point + offset - data.position) / predictTime;
		// Debug.DrawRay(data.position, v, Color.blue);
	
		if(isOn && futurePoint.isHit)
			return (futurePoint.hit.point + offset - data.position) / predictTime;
		else
			return Vector3.zero;
	}
	
	public virtual void Process() {
		GetFuturePosition();
		// PredictPoint();
	}
	
	protected virtual void GetFuturePosition() {
		var p = data.position;
		var v = data.velocity;
		// var a = data.force / data.mass;
		var t = predictTime;
		
		// var futurePosition = p + v * t + 0.5f * a * t * t;
		var futurePosition = p + v * t;
		
		Cast.SphereCast(futurePosition, -Vector3.up, checkRadio, checkHeight, locomotion.behaviour.climbMask, 
			futurePoint.OnHitSuccess, futurePoint.OnHitFail);
	}
	
	// protected virtual void PredictPoint() {
		// if(!futurePoint.isHit)
			// return;
	// }
}
