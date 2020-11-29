using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Momentum  {
	
	public Dynamics dynamics;
	public Vector3 supportOffset, direction;
	
	public Analysis analysis {get {return dynamics.locomotion.analysis; }}
	
	public Momentum (Dynamics dynamics) {
		this.dynamics = dynamics;
	}
	
	public void Reset() {
		supportOffset = Vector3.zero;
		direction = -Vector3.up;
	}
	
	public void Simulate(Vector3 supportDirection, float deltaTime) {
		var plane = new Plane(Vector3.up, Vector3.up * analysis.targetHeight);
		var ray = new Ray(Vector3.up * analysis.disToGround, -supportDirection);
		float distance = 0f;
		var newOffset = Vector3.zero;
		
		if(plane.Raycast(ray, out distance))
			newOffset = Vector3.ClampMagnitude(ray.GetPoint(distance).Flat()/2f,0.25f);
		
		supportOffset = Vector3.Lerp(supportOffset, newOffset, 2f*deltaTime);
		direction = (supportOffset - Vector3.up * analysis.disToGround).normalized;
	}
	
}
