using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

//Utility Tool
public static class UT {
	
	public static int GetDirectionIndex(Vector3 direction, Vector3 forward, Vector3 right, int segment) {
		var segmentDegree = 360f / segment;
		var angle = direction.GetDirectionInAngle(forward, right);
		if(angle<0) angle+=360;
		var value = angle/segmentDegree;
		return Mathf.RoundToInt(value)%segment;
	}
	
	public static float ChainDistance(params Vector3[] point) {
		if(point.Length < 2) throw new Exception("Can not measure less than 2 point");
		
		var rt = 0f;
		for(int i=1; i<point.Length; i++) rt += Vector3.Distance(point[i-1], point[i]);
		return rt;
	}
	public static float ChainDistance(params Transform[] trans) {
		if(trans.Length < 2) throw new Exception("Can not measure less than 2 point");
	
		var rt = 0f;
		for(int i=1; i<trans.Length; i++) rt += Vector3.Distance(trans[i-1].position, trans[i].position);
		return rt;
	}
	
}
