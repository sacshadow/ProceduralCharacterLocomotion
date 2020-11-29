using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class LimbLink : LinkBase {
	
	public Transform linkPoint;
	
	public Vector3 targetPoint {
		get {
			return mTargetPoint;
		}
		set {
			mTargetPoint = value;
		}
	}
	
	[Range(0,1)]
	public float weight = 1;
	public float controlModify = 1f, lastModify = 0.5f;
	public float damp = 0.3f;
	
	public Action<LimbLink,float,float> ForceOverride;
	
	// public bool keepLimbLength = true;
	// public float keepDistance = 0.95f;
	// public float keepStrength = 10;
	public float limbLength = 1f;
	
	protected Vector3 lastError;
	protected Vector3 mTargetPoint;
	
	public override void Reset() {
		lastError = Vector3.zero;
		ForceOverride = null;
		// ForceOverride = ControlHandMove.Empty;
	}
	
	public override void ApplyForce(float deltaTime, float reciDeltaTime) {
		if(!target.isSimulating || weight == 0) {
			lastError = Vector3.zero;
			return;
		}
		
		if(ForceOverride != null)
			ForceOverride(this, deltaTime, reciDeltaTime);
		else
			EulerMethod(deltaTime, reciDeltaTime);
		
		// if(keepLimbLength)
			// KeepLimbLength();
	}
	
	protected virtual void EulerMethod(float deltaTime, float reciDeltaTime) {
		var tp = Vector3.ClampMagnitude(targetPoint - linkPoint.position, limbLength) + linkPoint.position;
		var error = tp - target.data.position;
		var control = error * controlModify + (error - lastError)*lastModify * reciDeltaTime;
		var force = control * target.data.mass * reciDeltaTime * weight;
		
		lastError = error;
		
		
		target.AddForce(force);
		
		// Debug.DrawLine(targetPoint, linkPoint.position, Color.red);
		
		// link.AddForceAtPosition(linkPoint.position, -force);
	}
	
	// protected virtual void KeepLimbLength() {
		// var disp = linkPoint.position - target.data.position;
		// var force = disp.normalized * (disp.magnitude - keepDistance) * keepStrength * target.data.mass;
		// force += Vector3.up * 4f;
		// force -= target.data.velocity * damp * target.data.mass;
		// target.AddForce(force);
	// }
	
	protected override void ApplyLimit(Func<Vector3, Vector3> Process) {
		target.next.position = Process(target.next.position);
	}
	
	
}
