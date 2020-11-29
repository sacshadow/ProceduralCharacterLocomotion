using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Limit_Distance : LimitBase {
	
	public Transform[] jointChain;
	
	public float minDistance = 0.15f;
	public float jointLength = 1f;
	
	public override Vector3 ModiyPoint(Vector3 point) {
		var disp = point - jointChain[0].position;
		return disp.normalized * Mathf.Clamp(disp.magnitude, minDistance, jointLength) + jointChain[0].position;
	}
	
	void Awake() {
		if(jointChain != null && jointChain.Length > 1)
			jointLength = UT.ChainDistance(jointChain);
	}
}
