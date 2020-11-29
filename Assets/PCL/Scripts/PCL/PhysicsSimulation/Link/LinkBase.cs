using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class LinkBase : MonoBehaviour {
	
	public List<LimitBase> limit;
	public RBody target, link;
	
	public virtual void Reset() {
	
	}
	
	public virtual void FrameInit() {
		
	}
	
	public virtual void ApplyForce(float deltaTime, float reciDeltaTime) {
		if(!target.isSimulating) return;
	}
	
	public virtual void ApplyLimitations() {
		if(!target.isSimulating) return;
		
		for(int i=0; i<limit.Count; i++) ApplyLimit(limit[i].ModiyPoint);
	}
	
	protected virtual void ApplyLimit(Func<Vector3, Vector3> Process) {
		target.next.position = Process(target.next.position);
	}
	
	protected virtual void OnDrawGizmos() {
		
	}
	
}
