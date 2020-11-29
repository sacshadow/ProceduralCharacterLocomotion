using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_Animate : KinematicsState {
	
	public float timeLength = 1;
	public float time = 0;
	public float speed = 1;
	public bool loop = false;
	
	// public float percentage {get {return time/timeLength; }}
	
	public KS_Animate(DFA dfa) {
		timeLength = dfa.timeLength;
		if(timeLength <=0)
			throw new System.Exception("Unexcept time length " + timeLength + " from " + dfa.name);
	}
	
	public KS_Animate(DFA dfa, bool loop) {
		this.loop = loop;
		timeLength = dfa.timeLength;
		if(timeLength <=0)
			throw new System.Exception("Unexcept time length " + timeLength + " from " + dfa.name);
	}
	
	
	public override void Reset() {
		bodyStructure.SetLimbMode(armMode, legMode, 0f);
		time = 0;
		kinematics.percentage = 0;
	}
	
	
	public override (float speed, float percent) GetSpeedAndPercentage(float deltaTime) {
		time = kinematics.percentage * timeLength;
		
		if(loop)
			time = Mathf.Repeat(time+deltaTime*speed, timeLength);
		else
			time = Mathf.Clamp(time+deltaTime*speed, 0, timeLength);
	
		return (speed/timeLength, time/timeLength);
	}
	
	
}
