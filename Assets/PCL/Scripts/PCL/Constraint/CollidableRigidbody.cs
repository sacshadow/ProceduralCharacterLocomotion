using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class CollidableRigidbody : MonoBehaviour {
	
	public Rigidbody rb;
	public Vector3 velocityBeforeCollision;
	
	public virtual float GetCollisionModify() {
		return 1;
	}
	
	public virtual Vector3 GetMomentumBeforeCollision() {
		return velocityBeforeCollision * rb.mass;
	}
	
	protected virtual void Awake() {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate() {
		this.velocityBeforeCollision = rb.velocity;
	}
}
