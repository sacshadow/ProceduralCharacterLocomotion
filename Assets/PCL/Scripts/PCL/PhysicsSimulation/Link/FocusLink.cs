using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class FocusLink : LinkBase {
	
	public Transform linkPoint, chest;
	public LimitBase applyForce;
	
	public override void ApplyForce(float deltaTime, float reciDeltaTime) {
		if(!target.isSimulating) return;
	
		var lookDir = (transform.position - chest.position).Flat();
		target.next.rotation = Quaternion.LookRotation(lookDir);
		
		// Debug.DrawLine(transform.position, linkPoint.position, Color.red);
		
		var holdPosition = linkPoint.position;
		var dtl = link.data;
		var dtr = target.data;
		
		
		var disp = holdPosition - dtr.position;
		var dir = disp.normalized;
		
		var htVelo = dtl.velocity;
		var relativeMovement = (htVelo - dtr.velocity) * dtr.mass;
		
		// var force = dir * ((disp.magnitude) * 3200 - Vector3.Dot(dir,relativeMovement * -0.05f)) + Vector3.up * 9.81f * target.data.mass;
		var force = dir * ((disp.magnitude) * 3200 - Vector3.Dot(dir,relativeMovement * -0.05f));
		
		
		// WORKED
		// var force = disp * 800 + (relativeMovement*0.1f + Vector3.up) * 9.81f * target.data.mass;
		
		
		force += applyForce.LimitForce(target.data.position) * target.data.mass * 0.25f;
		
		target.AddForce(force);
		link.AddForce(-force);
	}
	
	protected override void ApplyLimit(Func<Vector3, Vector3> Process) {
		target.next.position = Process(target.next.position);
	}
	
}
