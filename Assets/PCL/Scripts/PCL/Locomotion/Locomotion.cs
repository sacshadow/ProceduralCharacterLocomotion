using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Locomotion : MonoBehaviour {
	
	public DynamicsData dynamicsData;
	
	public HumanoidBehaviour behaviour;
	public Transform direction;
	public Skeleton skeleton;
	public BodyStructure bodyStructure;
	
	public Detection detection;
	public Analysis analysis;
	public Prediction prediction;
	public Dynamics dynamics;
	public Kinematics kinematics;
	
	public float animValue=0;
	
	[NonSerialized]
	public PCLBody body;
	
	[NonSerialized]
	public Vector3 targetMovement, targetFaceDirection = Vector3.forward, lookDirection = Vector3.forward;
	
	public UnitAttribute attribute {get {return behaviour.attribute; }}
	
	public Func<Vector3> OverrideDirection;
	
	public Vector3 GetDirection() {
		if(OverrideDirection != null) return OverrideDirection();
		return targetFaceDirection;
	}
	
	public void Init() {
		body = behaviour.GetComponent<PCLBody>();
		detection = new Detection(this);
		analysis = new Analysis(this);
		prediction = new Prediction(this);
		dynamics = new Dynamics(this);
		kinematics = new Kinematics(this);
	}
	
	public void Reset() {
		detection.Reset();
		analysis.Reset();
		dynamics.Reset();
		kinematics.Reset();
		
		targetMovement = Vector3.zero;
		targetFaceDirection = transform.forward;
		lookDirection = transform.forward;
		direction.rotation = transform.rotation;
	}
	
	public void Simulate(float deltaTime) {
		detection.CheckEnvironment(body.data.velocity.magnitude * 0.15f);
		analysis.Process();
		prediction.Process();
		dynamics.Simulate(deltaTime);
		kinematics.Simulate(deltaTime);
	}
	
	public void UpdateAfterIK() {
		bodyStructure.UpdateAfterIK();
		behaviour.constraintSkeleton.leftLeg[2].localRotation = skeleton.leftLeg[2].localRotation;
		behaviour.constraintSkeleton.rightLeg[2].localRotation = skeleton.rightLeg[2].localRotation;
	}
	
	// DEBUG_INFO
// #if UNITY_EDITOR
	// void OnGUI() {
		// GUILayout.BeginVertical("","box");
		// GUILayout.Label(body.data.velocity.Flat().magnitude.ToString("f2"));
		// GUILayout.EndVertical();
	// }
// #endif
}
