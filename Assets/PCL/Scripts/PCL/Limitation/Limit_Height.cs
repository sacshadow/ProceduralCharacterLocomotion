using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Limit_Height : LimitBase {
	public Transform target;
	public float minY = -0.25f, maxY = 0.25f;
	
	public override Vector3 ModiyPoint(Vector3 point) {
		var rt = point;
		rt.y = Mathf.Clamp(point.y - target.position.y, minY, maxY) + target.position.y;
		return rt;
	}
	
}
