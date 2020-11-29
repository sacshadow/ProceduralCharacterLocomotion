using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Gear {
	public readonly static float[] ratio = new float[] {
		0,1/5f,1/4f,1/3f,1/2f,
		1,2,3,4,5,
	};
	
	[Range(0,9)]
	public int shift = 5;
	public float value = 0;
	
	[NonSerialized]
	public bool isSimulate = true;
	[NonSerialized]
	public float speed = 0;
	
	public virtual void Reset() {
		value = 0;
		shift = 5;
		speed = 0;
	}
	
	public void SetSimulate(bool state) {
		isSimulate = state;
		Reset();
	}
	
	public virtual void Run(float systemValue, float systemSpeed, float deltaTime) {
		var targetValue = Mathf.Repeat(systemValue * ratio[shift],1);
		
		if(targetValue < value)
			targetValue += 1;
		
		speed = systemSpeed * ratio[shift];
		
		var dis = targetValue - value;
		var spdChange = 1.5f + 0.8f * speed + dis * 0.3f;
		
		value = Mathf.MoveTowards(value, targetValue, spdChange * deltaTime);
		value = Mathf.Repeat(value,1);
		
	}
	
	public virtual void Simulate(float deltaTime) {
		if(isSimulate) RunSimulation(deltaTime);
	}
	
	protected virtual void RunSimulation(float deltaTime) {
		
	}
}
