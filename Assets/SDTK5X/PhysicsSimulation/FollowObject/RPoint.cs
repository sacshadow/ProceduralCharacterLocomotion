using UnityEngine;
//using UnityEditor;
using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

public class RPoint : MonoBehaviour {
	
	public float mass = 1f;
	public float gravity = -9.81f;
	
	public Vector3 keepPoint;
	// public float keepDis;
	public float spring = 10f;
	// public float kSpring = 10f;
	public float damp = 0.2f;
	public float airResistance = 0.2f;
	
	public Vector3 position, velocity, acceleration;
	public Vector3 offset;
	
	public Vector3 convertAngle = new Vector3(0,-90,0);
	
	public float limitMaxOffset = 0.15f;
	
	// public Transform localParent;
	
	// public Quaternion orgRotation;
	
	public virtual void Init(RPoint next) {
		position = transform.position;
		// orgRotation = transform.rotation;
		velocity = Vector3.zero;
		acceleration = Vector3.up*gravity;
		
		keepPoint = transform.localPosition;
		
		// keepDis = (next.position - this.position).sqrMagnitude;
	}
	
	
	public virtual void CaluInit(RPoint next) {
		acceleration = Vector3.up * gravity + velocity * -airResistance;
		// acceleration = Vector3.zero;
	}
	
	public virtual void CaluForce(RPoint next) {
		// Debug.DrawLine(next.transform.TransformPoint(keepPoint), this.position, Color.red);
		
		// Vector3 kOffset = next.position - this.position;
		offset = next.transform.TransformPoint(keepPoint) - this.position;
		// offset = offset.normalized * Mathf.Max(offset.sqrMagnitude,1);
		
		float oForce = -offset.sqrMagnitude * spring;
		// float kForce = (keepDis - kOffset.sqrMagnitude) * kSpring;
		
		Vector3 diff = next.velocity * Time.deltaTime * next.mass - this.velocity * Time.deltaTime * this.mass;
		// Vector3 force = offset.normalized * oForce + kOffset * kForce + diff;
		Vector3 force = offset.normalized * oForce + diff;
		// Vector3 force = offset.normalized * oForce;
		
		this.acceleration -= force;
		next.acceleration += force;
	}
	
	public virtual void Move(RPoint next) {
		acceleration = acceleration.normalized * Mathf.Min(acceleration.magnitude, 180);
		// acceleration.x *= localParent.localScale.x;
		
		velocity = (velocity + acceleration * Time.deltaTime) * (1 - damp);
		
		position += velocity * Time.deltaTime;
		
		Vector3 targetPoint = next.transform.TransformPoint(keepPoint);
		Vector3 limitOffset =  this.position - targetPoint;
		position = targetPoint + limitOffset.normalized * Mathf.Min(limitOffset.magnitude, limitMaxOffset);
		
		transform.position = position;
		// transform.rotation = Quaternion.LookRotation(next.position - this.position, next.transform.up) * Quaternion.Euler(0,-90 * localParent.localScale.x,0);
		transform.rotation = Quaternion.LookRotation(next.position - this.position, next.transform.up) * Quaternion.Euler(convertAngle);
	}
	
	
	
}
