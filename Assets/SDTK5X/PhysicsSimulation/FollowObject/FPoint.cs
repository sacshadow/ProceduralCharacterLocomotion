using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//Float point
public class FPoint : RPoint {
	
	public float floatForce = 10;
	
	public override void CaluForce(RPoint next) {
		base.CaluForce(next);
		
		acceleration += Rand.insideUnitSphere * floatForce;
	}
	
}
