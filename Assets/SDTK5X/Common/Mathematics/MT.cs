using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//Maths Tools
public static class MT {
	
	public static float Repeat01(float value) {
		return Mathf.Repeat(value, 1);
	}
	
	public static int SumOf(params int[] value) {
		return Loop.Calculate(value, (x,y)=>x+y);
	}
	
	public static int AverageOf(params int[] value) {
		return SumOf(value)/value.Length;
	}
	
	public static float SumOf(params float[] value) {
		return Loop.Calculate(value, (x,y)=>x+y);
	}
	
	public static float AverageOf(params float[] value) {
		return SumOf(value)/value.Length;
	}
	
	public static float Min(params float[] value) {
		var rt = value[0];
		for(int i=1; i<value.Length; i++)
			rt = value[0] < rt ? value[0] : rt;
		return rt;
	}
	
	public static float Max(params float[] value) {
	var rt = value[0];
		for(int i=1; i<value.Length; i++)
			rt = value[0] > rt ? value[0] : rt;
		return rt;
	}
	
}
