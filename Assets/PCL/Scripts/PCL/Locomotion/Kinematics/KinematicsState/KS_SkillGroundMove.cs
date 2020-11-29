using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_SkillGroundMove : KS_LimbStanceMove {
	
	public float time = 1f;
	public float speed = 1f;
	
	protected float t = 0f;
	
	// public override void SetPercentage(float percentage) {
		// t = time * percentage;
	// }
	
	public override void Reset() {
		base.Reset();
		t = 0;
	}
	
	protected override (float speed, float percent) CalculateSpeedAndPercentage(float deltaTime) {
		t += deltaTime * speed;
		return (speed, Mathf.Clamp(t/time,0, 1));
	}
	
	// protected override void CalculateActionPercentage(float deltaTime) {
		// t += Time.deltaTime * speed;
		// percentage = Mathf.Clamp(t/time,0, 1);
	// }
	
}
