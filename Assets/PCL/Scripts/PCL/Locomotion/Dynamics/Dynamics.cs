using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Dynamics {
	
	public Locomotion locomotion;
	public DynamicsData data;
	public MobileSystem mobileSystem;
	public BalanceSystem balanceSystem;
	public Momentum momentum;
	
	public Vector3 torque, acceleration, supportAcceleration;
	
	public float clashValue = 0;
	
	public PCLBody body {get {return locomotion.body; }}
	public HumanoidBehaviour behaviour {get {return locomotion.behaviour; }}
	public UnitAttribute attribute {get {return locomotion.behaviour.attribute; }}
	protected Vector3 targetMovement {get {return locomotion.targetMovement; }}
	protected Vector3 targetFaceDirection {get {return locomotion.GetDirection(); }}
	
	public Vector3 GetMomentumDirection() {
		if(attribute.isOnGround)
			// return momentum.direction;
			return -locomotion.transform.up;
		else
			return -Vector3.up;
	}
	
	public Dynamics(Locomotion locomotion) {
		this.locomotion = locomotion;
		data = locomotion.dynamicsData;
		mobileSystem = new MobileSystem(this);
		balanceSystem = new BalanceSystem(this);
		momentum = new Momentum(this);
	}
	
	public virtual void Reset() {
		mobileSystem.Reset();
		balanceSystem.Reset();
		momentum.Reset();
	}
	
	public virtual void Simulate(float deltaTime) {
		if(attribute.isOnGround && attribute.isBalance)
			OnGroundMove(deltaTime);
		else
			OnAirMove(deltaTime);
			
		AddForceFromConstraintDeform();
	}
	
	protected virtual void OnGroundMove(float deltaTime) {
		acceleration = CalculateTargetAcceleration(deltaTime);
		torque = CalculateTargetTorque(deltaTime);
		var supportDirection = locomotion.analysis.supportPoint - body.data.position;
		
		Debug.DrawRay(body.data.position, Vector3.up * acceleration.y, Color.green);
		// Debug.DrawRay(body.data.position, acceleration.Flat(), Color.blue);
		// Debug.DrawRay(body.data.position, GetMomentumDirection(), Color.red);
		
		var sd = supportDirection;
		
		if(sd.y == 0)
			supportAcceleration = acceleration.Flat();
		else {
			if(sd.y < 0)
				sd = -sd;
			var k = acceleration.y / sd.y;
			var w = Vector3.Dot(acceleration.normalized, sd.normalized);
			// w = w * (2-w);
			supportAcceleration = acceleration.Flat()*w + sd * k;
			supportAcceleration = Vector3.ClampMagnitude(supportAcceleration, data.maxAccH + data.maxAccV);
			
			momentum.Simulate(supportDirection, deltaTime);
		}
		
		Debug.DrawLine(body.data.position, locomotion.analysis.supportPoint, Color.red);
		
		//Motion force
		// body.AddForce(acceleration * body.data.mass);
		body.AddForce(supportAcceleration * body.data.mass);
		body.AddTorque(torque);
		
		//Reaction force due to force apply on support point;
		body.AddForce(Vector3.Cross(torque * 20f, supportDirection));
		body.AddTorque(-Vector3.Cross(supportDirection, supportAcceleration * body.data.mass * 0.025f));
	}
	
	protected virtual void AddForceFromConstraintDeform() {
		if(behaviour.constraintSkeleton == null) return;
		
		var com = behaviour.constraintSkeleton.GetCOM();
		// body.AddForce(Vector3.ClampMagnitude(com - body.data.position, 0.1f*PCLSimulation.deltaTime)*body.data.mass);
		// body.AddForce((com - body.data.position)*200*body.data.mass);
		
		// body.AddForce((com - body.data.position)*2*body.data.mass);
		
		// var v = Vector3.Dot(locomotion.targetMovement, body.data.velocity);
		// var dist = (com - body.data.position).magnitude;
		// var modify = Mathf.Lerp(1,400, dist/20f);
		// var modify = Mathf.Lerp(1,200,v);
		
		// var t = 0.25f;
		// var k = (com + behaviour.constraintAttribute.GetComponent<Rigidbody>().velocity * t) - (body.data.position + body.data.velocity);
		// var modify = k.magnitude * 20f;
		
		var modify = Mathf.Lerp(2f, 400, attribute.clashShock);
		
		
		body.AddForce((com - body.data.position) * modify * body.data.mass);
		
		behaviour.ForEachConstraint(AddConstraintCollideForce);
		
		if(behaviour.constraintAttribute.clashData.Count > 0) {
			foreach(var kvp in behaviour.constraintAttribute.clashData)
				ClashSolver.Process(behaviour, kvp.Key.group, kvp.Value);
			behaviour.constraintAttribute.clashData.Clear();
		}
	}
	
	// protected virtual void ClashKnockDown() {
		// attribute.isBalance = false;
	// }
	
	protected virtual void AddConstraintCollideForce(ConstraintBase constraint) {
		var f = constraint.collideForce * constraint.rb.mass;
		// var impact = f * 4f;
		var impact = f * 1f;
		// var amount = 5f;
		var amount = 2.5f;
		// var amount = 10f;
		
		body.AddForce(impact * amount);
		body.AddTorque(Vector3.Cross(constraint.transform.position - body.data.position, f * amount));
	}
	
	protected virtual void OnAirMove(float deltaTime) {
		torque = CalculateTargetTorque(deltaTime);
		body.AddTorque(torque/10f);
	}
	
	protected virtual Vector3 CalculateTargetTorque(float deltaTime) {
		return balanceSystem.GetTorque(targetFaceDirection, deltaTime);
	}
	
	protected virtual Vector3 CalculateTargetAcceleration(float deltaTime) {
		return mobileSystem.GetAcceleration(targetMovement, deltaTime);
	}
	
	
	
	
	
	
}
