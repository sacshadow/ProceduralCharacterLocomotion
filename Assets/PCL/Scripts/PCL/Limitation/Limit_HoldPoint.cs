using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Limit_HoldPoint : LimitBase {

	public Transform holdPoint;
	public float comDistance = 0.5f;
	
	public override Vector3 ModiyPoint(Vector3 point) {
		var disp = point - holdPoint.position;
		var dist = Mathf.Clamp(disp.magnitude, comDistance-0.05f, comDistance + 0.05f);
		var p = disp.normalized * dist + holdPoint.position;
		return p;
	}
	
}
