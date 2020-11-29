using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KinematicsState {
	
	public LimbMode armMode = LimbMode.ANIMATE;
	public LimbMode legMode = LimbMode.ANIMATE;
	
	public Kinematics kinematics;
	public float percentage {get {return kinematics.percentage; }}
	public Locomotion locomotion {get {return kinematics.locomotion; }}
	public UnitAttribute attribute {get {return kinematics.locomotion.attribute; }}
	public BodyStructure bodyStructure {get {return kinematics.locomotion.bodyStructure; }}
	public LayerMask mask {get {return kinematics.locomotion.behaviour.climbMask; }}
	public float mass {get {return kinematics.locomotion.body.data.mass; }}
	public Vector3 position {get {return kinematics.locomotion.body.data.position; }}
	public Quaternion rotation {get {return kinematics.locomotion.transform.rotation; }}
	public Vector3 velocity {get {return kinematics.locomotion.body.data.velocity; }}
	public Vector3 angularVelocity {get {return kinematics.locomotion.body.data.angularVelocity; }}
	
	public virtual void Reset() {
		bodyStructure.SetLimbMode(armMode, legMode, 0.25f);
	}
	
	public virtual (float speed, float percent) GetSpeedAndPercentage(float deltaTime) {
		return (1, Mathf.Repeat(percentage + deltaTime, 1));
	}
	
	public virtual void Simulate(float deltaTime) {
		
	}
	
	public virtual void FinalModify() {
	
	}
}
