using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class ClashData {
	public List<Vector3> clashPoint;
	public float clashMass = 1;
	
	public ClashData() {
		clashPoint = new List<Vector3>();
	}
	
	public void Add(float mass, Vector3 point) {
		clashMass = Mathf.Max(clashMass, mass);
		clashPoint.Add(point);
	}
	
	public void Clear() {
		clashPoint.Clear();
		clashMass = 1;
	}
}

public class ConstraintAttribute : MonoBehaviour {
	public float
		torqueModify = 1, torqueDiffModify = 0.1f,
		forceModify = 1, forceDiffModify = 0.1f;
	public float 
		jointKeepForce = 1000;
		
	public float maxVelocity = 60, maxAngularVelocity = 1080;	
	
	[Range(0,1)]
	public float followWeight = 1;
	[Range(0,1)]
	public float stiffness = 0.1f;
	
	public AnimationCurve weightCurve = AnimationCurve.Linear(0,0,1,1);
	public AnimationCurve modifyCurve = AnimationCurve.Linear(0,0,1,1);
	
	public float collisionRecover = 0.05f;
	public float keepForceWeight = 1, keepTorqueWeight = 1, keepDriveWeight = 1;
	
	public float totalMass = 1;
	
	public RBGroup group;
	
	// public Dictionary<ConstraintAttribute, List<Vector3>> clashData;
	public Dictionary<ConstraintAttribute, ClashData> clashData;
	
	public void ClashWithOther(ConstraintAttribute other, float clashMass, Vector3 point) {
		if(!clashData.ContainsKey(other))
			// clashData[other] = new List<Vector3>();
			clashData[other] = new ClashData();
		
		clashData[other].Add(clashMass, point);
	}
	
	public void ClearClashData() {
		clashData.Clear();
	}
	
	protected void Awake() {
		clashData = new Dictionary<ConstraintAttribute, ClashData>();
	}
	
	protected void Update() {
		keepForceWeight = weightCurve.Evaluate(followWeight);
		// keepTorqueWeight = keepForceWeight;
		// keepDriveWeight = keepForceWeight;q
		// keepDriveWeight = 1;
		keepTorqueWeight = Mathf.Lerp(0.01f,1,keepForceWeight*4f);
		keepDriveWeight = Mathf.Lerp(0.1f,1,keepForceWeight*4f);
	}
}
