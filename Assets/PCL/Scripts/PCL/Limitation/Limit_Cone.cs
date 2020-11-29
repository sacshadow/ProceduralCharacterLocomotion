using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Limit_Cone : LimitBase {
	
	public float angle = 60;
	public float maxDis = 1, minDis = 0.15f;

	public override Vector3 ModiyPoint(Vector3 point) {
		var disp = point - transform.position;
		var a = Vector3.Angle(disp, transform.forward);
		
		if(a < angle)
			return point;
		
		var aixs = Vector3.Cross(transform.forward, disp).normalized;
		var q = Quaternion.AngleAxis(angle, aixs);
		
		return q * transform.forward * disp.magnitude + transform.position;
	}
	
	void OnDrawGizmos() {
		Gizmos.matrix = transform.localToWorldMatrix;
		
		var d0 = Quaternion.Euler(angle,0,0) * Vector3.forward * minDis;
		var d1 = Quaternion.Euler(angle,0,0) * Vector3.forward * maxDis;
		
		
		Gizmos.DrawLine(d0, d1);
		Gizmos.DrawLine(Quaternion.Euler(0,0,90) * d0, Quaternion.Euler(0,0,90) * d1);
		Gizmos.DrawLine(Quaternion.Euler(0,0,180) * d0, Quaternion.Euler(0,0,180) * d1);
		Gizmos.DrawLine(Quaternion.Euler(0,0,270) * d0, Quaternion.Euler(0,0,270) * d1);
		
		
		var last0 = d0;
		var last1 = d1;
		
		for(int i=1; i<25; i++) {
			var q = Quaternion.Euler(0,0,i*15);
			var n0 = q * d0;
			var n1 = q * d1;
			
			Gizmos.DrawLine(last0, n0);
			Gizmos.DrawLine(last1, n1);
			last0 = n0;
			last1 = n1;
		}
	}
	
}
