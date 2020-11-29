using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class MobileSystem {
	
	protected Dynamics dynamics;
	protected DynamicsData data;
	protected PCLBody body {get {return locomotion.body; }}
	protected Locomotion locomotion {get {return dynamics.locomotion; }}
	protected Detection detection {get {return dynamics.locomotion.detection; }}
	protected Analysis analysis {get {return dynamics.locomotion.analysis; }}
	protected Prediction prediction {get {return dynamics.locomotion.prediction; }}
	protected Transform transform {get {return dynamics.locomotion.transform; }}
	
	public MobileSystem(Dynamics dynamics) {
		this.dynamics = dynamics;
		this.data = dynamics.data;
	}
	
	public virtual void Reset() {
	
	}
	
	public virtual Vector3 GetAcceleration(Vector3 targetMovement, float deltaTime) {
		var targetVelocity = GetTargetVelocity(targetMovement);
		var velocityChange = targetVelocity - body.data.velocity;
		
		var finalAcceleration = GetMotionControl(targetVelocity, velocityChange);
		finalAcceleration.y = GetSupportControl(targetVelocity, velocityChange);
		
		return finalAcceleration;
	}
	
	protected virtual Vector3 GetTargetVelocity(Vector3 targetMovement) {
		var centerModify = (body.data.position - analysis.supportPoint).Flat() * 1.25f;
	
		var targetVelocity = targetMovement - centerModify;
		targetVelocity.y += analysis.targetVelocity.y + PredictVerticalVelocity();
		
		return targetVelocity;
	}
	
	protected virtual float GetSupportControl(Vector3 targetVelocity, Vector3 velocityChange) {
		var heightDiff = analysis.supportTargetHeight - analysis.disToGround;
		var acceleration = heightDiff * data.supportControl + velocityChange.y * data.keepControl;
		
		var control = GetVerticalAccelerationControl(acceleration, targetVelocity.y, heightDiff);
		// var control = acceleration;
		
		if(analysis.disToGround > data.maxHeight)
			control = 0;
		// else if(body.data.velocity.y > 3.2f)
			// control = 0;
		// else
		
		control *= Mathf.Lerp(1,0,(body.data.velocity.y-1.5f)/1.7f);
		control = Mathf.Clamp(control, 0, data.maxAccV);
		
		Debug.DrawLine(transform.position, analysis.groundPoint, Color.green);
		
		return control;
	}
	
	protected virtual float GetVerticalAccelerationControl(float acceleration, float targetVelocity, float d) {
		var a = acceleration;
		var u = body.data.velocity.y;
		var t = PredictTime(u,d);
		var v = u + (a-9.81f) * t;
		var c = 0;
		
		var check = d > 0 && analysis.heightOffset < 0.5f ? 0.45f : 0.05f;
		// var check = d > 0 && analysis.targetVelocity.y < 0.5f ? 0.45f : 0.05f;
		// var check = 0.3f;
		
		while(t < check && c<10) {
			c++;
			
			a *= v > targetVelocity ? 0.618f : 1.618f;
			v = u + (a-9.81f) * t;
			t = PredictTime(v,d);
		}
		
		return a;
	}
	
	protected float PredictTime(float u, float d) {
		if(u*d <= 0) return 0.05f;
		return d/u;
	}
	
	protected virtual Vector3 GetMotionControl(Vector3 targetVelocity, Vector3 velocityChange) {
		var a = velocityChange.Flat() * data.motionControlModify;
		// var u = body.data.velocity.Flat();
		// var t = 0.1f;
		// var v = u + a * t;
		// var c = 0;
		
		
		// while(c < 10 && Vector3.Dot(targetVelocity, v) < 0.99f) {
			// c++;
		
			// a *=  1.618f;
			// v = u + a * t;
		// }
		
		return Vector3.ClampMagnitude(a, data.maxAccH);
	}
	
	protected float PredictVerticalVelocity() {
		return prediction.PredictFinalVelocity(Vector3.up * analysis.targetHeight).y;
	}
	
}
