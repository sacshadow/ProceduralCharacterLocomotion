using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public static class Formula  {
	
	public static (float,float) Quadratic(float a, float b, float c) {
		var root = Mathf.Sqrt(4f*a*c);
		var k = -b + 2*a;
		return (k+root, k-root);
	}
	
}
