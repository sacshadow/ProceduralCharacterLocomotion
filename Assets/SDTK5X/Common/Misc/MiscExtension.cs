using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public static class MiscExtension {

	
	public static float GetDirectionInAngle(this Vector3 direction, Vector3 forward, Vector3 right) {
		return Vector3.Angle(direction, forward) * Mathf.Sign(Vector3.Dot(direction, right));
	}
	
	public static Vector3 Flat(this Vector3 orgDirection) {
		var d = orgDirection;
		d.y = 0;
		return d;
	}
	
}
