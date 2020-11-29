using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class DropLink : LinkBase {
	
	public Transform linkPoint;
	
	public Vector3 GetDropDirection() {
		return (linkPoint.position - transform.position).normalized;
	}
	
	public override void ApplyForce(float deltaTime, float reciDeltaTime) {
		if(!target.isSimulating) return;
	
		var dtr = target.data;
		
		var disp = dtr.position - linkPoint.position;
		var dir = disp.normalized;
		
		
		var force = dir * (0 - disp.magnitude) * 40 + (link.data.velocity - dtr.velocity) * 1;
		// rhs.AddForce(force + Vector3.up * supportForce * dtr.mass);
		target.AddForce(force);
	}
	
}
