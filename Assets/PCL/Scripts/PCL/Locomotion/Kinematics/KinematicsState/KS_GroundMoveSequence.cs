using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_GroundMoveSequence : KinematicsState {
	
	public KS_GroundMovement[] groundMove;
	
	public int current;
	// public float percentage = 0;
	
	
	public override void Reset() {
		Loop.ForEach(groundMove, g=>g.kinematics = this.kinematics);
		Loop.ForEach(groundMove, g=>g.Reset());
		current = 0;
		// percentage = 0;
	}
	
	public override (float speed, float percent) GetSpeedAndPercentage(float deltaTime) {
		return groundMove[current].GetSpeedAndPercentage(deltaTime);
	}
	
	public override void Simulate(float deltaTime) {
		Shift(velocity.Flat().magnitude);
		groundMove[current].Simulate(deltaTime);
	}
	
	protected virtual void Shift(float speed) {
		if(speed < groundMove[current].shiftDownSpeed && current > 0)
			current --;
		else if(speed > groundMove[current].shiftUpSpeed && current < groundMove.Length-1)
			current ++;
	}
	
}
