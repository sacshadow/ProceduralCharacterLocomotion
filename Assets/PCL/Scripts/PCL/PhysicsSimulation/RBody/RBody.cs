using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class RBody : MonoBehaviour {
	
	public float mass = 1, moiMulit = 0.036f;
	public float velocityLimit = 20f;
	public float angularVelocityLimit = Mathf.PI * 3f;
	
	[NonSerialized]
	public RData data, next;
	
	public bool debug = false;
	
	public bool weightShift = true;
	
	public bool isSimulating {get {return simulate; }}
	
	protected bool simulate = true;
	
	public void SetSimulation(bool state) {
		if(simulate != state) Reset();
		
		simulate = state;
	}
	
	public virtual void Reset() {
		data.SetMass(mass, mass * moiMulit);
		data.Reset(transform.position, transform.rotation);
		next.Copy(data);
	}
	
	public virtual void AddForce(Vector3 force) {
		if(simulate)
			next.force += force;
	}
	
	public virtual void AddTorque(Vector3 torque) {
		if(simulate)
			next.torque += torque;
	}
	
	public virtual void AddForceAtPosition(Vector3 position, Vector3 force) {
		AddForce(force);
		AddTorque(Vector3.Cross(position-data.position, force));
		
		// Debug.DrawRay(position, force, Color.red);
		// Debug.DrawRay(position, Vector3.Cross(position-data.position, force), Color.yellow);
		// Debug.Break();
	}
	
   
	public virtual void FinialApplyTransform() {
		if(simulate) {
			data.Copy(next);
			
			transform.position = data.position;
			transform.rotation = data.rotation;
		}
		else {
			data.velocity = (transform.position - data.position) / (PCLSimulation.deltaTime * Time.timeScale);
			data.position = transform.position;
			data.rotation = transform.rotation;
		}
	}
	
	void Awake() {
		data = new RData();
		next = new RData();
	}
   
}
