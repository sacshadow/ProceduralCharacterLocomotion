using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

[Serializable]
public class RData {
	public bool init = false;
	
	public float mass = 1;
	// public Matrix4x4 momentOfInertia, inverseMOI;
	public float momentOfInertia = 0.036f, inverseMOI = 27.78f;
	
	public Vector3 position, velocity, force;
	
	public Quaternion rotation;
	public Vector3 angularVelocity, torque;
	
	public RData() {}
	
	public RData(RData org) {
		Copy(org);
	}
	
	public void SetMass(float mass, float momentOfInertia) {
		this.mass = mass;
		this.momentOfInertia = momentOfInertia;
		this.inverseMOI = 1f/momentOfInertia;
	}
	
	public void Reset(Vector3 position, Quaternion rotation) {
		init = true;
		this.position = position;
		this.rotation = rotation;
		
		velocity = force = angularVelocity = torque = Vector3.zero;
	}
	
	public void Copy(RData rd) {
		this.mass = rd.mass;
		this.momentOfInertia = rd.momentOfInertia;
		this.momentOfInertia = rd.momentOfInertia;
		
		this.position = rd.position;
		this.velocity = rd.velocity;
		this.force = rd.force;
		
		this.rotation = rd.rotation;
		this.angularVelocity = rd.angularVelocity;
		this.torque = rd.torque;
		
		init = true;
	}
   
   
}
