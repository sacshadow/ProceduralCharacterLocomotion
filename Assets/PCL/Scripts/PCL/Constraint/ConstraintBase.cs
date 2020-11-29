using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public abstract class ConstraintBase : CollidableRigidbody {
	public const float collideForceModify = 25f;
	
	public Transform target;
	public ConstraintAttribute attr;
	public ConstraintBase[] linked;
	
	public float minModify = 0;
	
	[HideInInspector]
	public Vector3 collideForce = Vector3.zero;
	[HideInInspector]
	public Action<Collision> OnCollisionImpact;
	
	protected Vector3 centerOfMass;
	protected int collideCount = 0;
	
	protected Vector3 lastRotationError, lastPositionError;
	protected Vector3 modifyAcc = Vector3.zero;
	protected float mdf = 1f, modify =1f;
	
	protected Transform root;
	
	public override Vector3 GetMomentumBeforeCollision() {
		var v = velocityBeforeCollision * rb.mass;
		var d = Vector3.Dot(v.normalized, attr.group.root.data.velocity);
		var bv = d > 0 ? v.normalized * attr.totalMass * d : Vector3.zero;
		bv *= Mathf.Lerp(0f, 0.1f, attr.stiffness-0.35f);
		
		return v + bv;
		
		// return velocityBeforeCollision * rb.mass + attr.group.root.data.velocity * attr.totalMass;
	}
	
	public override float GetCollisionModify() {
		return attr.stiffness;
	}
	
	public virtual void Setup() {
		centerOfMass = rb.centerOfMass;
		root = transform.root;
		mdf =1f;
	}
	
	public virtual void Reset() {
		rb.Sleep();
		
		lastRotationError = lastPositionError = Vector3.zero;
		transform.position = target.position;
		transform.rotation = target.rotation;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		
		mdf =1f;
	}
	
	public virtual void ApplyDeform(Vector3 force, Vector3 point, float modify = 1) {
		ApplyAsHit(force, point, modify);
		rb.AddForceAtPosition(force*20, point);
	}
	
	public virtual void ApplyAsHit(Vector3 force, Vector3 point, float modify = 1) {
		collideForce += force * modify / rb.mass / collideForceModify;
	}
	
	public virtual void FrameInit() {
		collideForce = Vector3.ClampMagnitude(collideForce, 20);
	}
	
	public virtual void CalcuDistributeForce(float deltaTime) {
		if(linked.Length == 0) return;
		Loop.ForEach(linked,x=>modifyAcc += x.collideForce - collideForce);
		modifyAcc = modifyAcc/linked.Length;
	}
	public virtual void CalcuDistribute(float deltaTime) {
		collideForce += modifyAcc * deltaTime * 25f;
		if(collideForce.sqrMagnitude > 0.0001f)
			collideForce -= collideForce.normalized * 2f * deltaTime;
		collideForce = collideForce * Mathf.Pow(attr.collisionRecover, deltaTime);
	}
	
	public virtual void FrameUpdate(float deltaTime) {
		var d = collideForce.magnitude / rb.mass * 16f;
		mdf = Mathf.Min(1-d, mdf + 2*deltaTime);
		mdf = Mathf.Max(mdf,0.1f);
		// if(collideCount >0) mdf = 0.1f;
		
		modify =  Mathf.Max(attr.modifyCurve.Evaluate(mdf), minModify);
		modify = Mathf.Lerp(modify,1,attr.stiffness);
		
		CalcuTorque(deltaTime, PCLSimulation.reciDeltaTime);
		CalcuForce(deltaTime, PCLSimulation.reciDeltaTime);
	}
	
	protected virtual void CalcuTorque(float deltaTime, float reciDeltaTime) {
		Vector3 torqueAxis;
		float torqueAngle;
		
		var targetRotation = target.rotation * Quaternion.Inverse(transform.rotation);
		targetRotation.ToAngleAxis(out torqueAngle, out torqueAxis);
		
		if(torqueAngle != 360f) {
			var rotationError = torqueAxis * FixEuler(torqueAngle);
			var angularVelocity = EulerMethod(rotationError, lastRotationError, attr.torqueModify, attr.torqueDiffModify, reciDeltaTime);
			
			angularVelocity = Vector3.ClampMagnitude(angularVelocity, attr.maxAngularVelocity * modify);
			angularVelocity = Vector3.ClampMagnitude(angularVelocity, Mathf.PI * 6f);
			rb.AddTorque(angularVelocity * attr.keepTorqueWeight, ForceMode.VelocityChange);
			lastRotationError = rotationError;
		}
	}
	
	protected virtual void CalcuForce(float deltaTime, float reciDeltaTime) {
		var com = target.position + target.rotation * centerOfMass;
		var positionError = com - rb.worldCenterOfMass;
		var velocity = EulerMethod(positionError, lastPositionError, attr.forceModify, attr.forceDiffModify, reciDeltaTime);
		var diff = Vector3.ClampMagnitude(velocity - rb.velocity, attr.maxVelocity * modify);
		var force = Vector3.ClampMagnitude(diff * reciDeltaTime * attr.keepForceWeight, 300) * rb.mass;
		// var force = diff * reciDeltaTime * attr.keepForceWeight* rb.mass;
		rb.AddForce(force);
		lastPositionError = positionError;
	}
	
	protected virtual Vector3 EulerMethod(Vector3 error, Vector3 lastError, float errorModify, float differenceModify, float rdt) {
		return error * errorModify + (error - lastError) * differenceModify * rdt;
	}
	
	protected float FixEuler (float angle) {
		if(angle > 180f)
			return angle - 360;
		return angle;
	}
	
	protected override void FixedUpdate() {
		rb.velocity = Vector3.ClampMagnitude(rb.velocity, attr.maxVelocity);
		base.FixedUpdate();
	}
	
	protected virtual void OnCollisionEnter(Collision collision) {
		collideCount ++;
		
		if(collision.transform.root != root) {
			CalculateCollideForce(collision);
			 
			if(OnCollisionImpact != null)
				OnCollisionImpact(collision);
		}
	}
	
	protected virtual void OnCollisionStay(Collision collision) {
		if(collision.transform.root != root)
			CalculateCollideForce(collision);
	}
	
	protected virtual void OnCollisionExit(Collision collision) {
		collideCount --;
	}
	
	protected virtual void CalculateCollideForce(Collision collision) {
		var collideNormal = collision.contacts[0].normal;
		var momentum = Vector3.zero;
		var crb = collision.transform.GetComponent<CollidableRigidbody>();
		
		if(crb == null)return;
		
		// if(crb != null )
			momentum = crb.GetMomentumBeforeCollision()/rb.mass/collideForceModify;
		// else
			// momentum = -GetMomentumBeforeCollision()/collideForceModify/4f;
	
		// Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal,Color.yellow);
		// Debug.Break();
		var d = Vector3.Dot(collideNormal, momentum.normalized);
		if(d > 0)
			collideForce += Vector3.ClampMagnitude(momentum * d, 10);
		
		var cb = crb as ConstraintBase;
		if(cb == null || cb.attr.group.attribute.isDead)
			return;
		if(momentum.magnitude<0.05f)
			return;
		
		for(int i=0; i<collision.contacts.Length; i++) {
			attr.ClashWithOther(cb.attr, crb.rb.mass, collision.contacts[i].point);
		}
	}
	
	protected virtual void OnDrawGizmos() {
		if(linked == null || linked.Length == 0) return;
		if(rb == null) return;
		
		var p = transform.position + collideForce;
		Gizmos.color = Color.red;
		Loop.ForEach(linked, x=>Gizmos.DrawLine(p, x.transform.position + x.collideForce));
		
		Gizmos.color = new Color(1,0,0,0.2f);
		Gizmos.DrawLine(p, transform.position);
		
		// p = rb.worldCenterOfMass;
		
		// Gizmos.DrawLine(p,p+transform.right*0.15f);
		
		// Gizmos.color = Color.green;
		// Gizmos.DrawLine(p,p+transform.up*0.15f);
		
		// Gizmos.color = Color.blue;
		// Gizmos.DrawLine(p,p+transform.forward*0.15f);
	}
	
	protected virtual void OnWillRenderObject() {
		
	}
}
