using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class LimitBase : MonoBehaviour {
	
	public virtual Vector3 ModiyPoint(Vector3 point) {
		return point;
	}
	
	public virtual Vector3 LimitForce(Vector3 point) {
		return Vector3.zero;
	}

	
}
