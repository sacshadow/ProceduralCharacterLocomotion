using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class TestApplyForce : LinkBase {

	public Vector3 offset = Vector3.up;
	public Vector3 force = Vector3.forward * 80f;
	
	public override void ApplyForce(float deltaTime, float reciDeltaTime) {
		if(Input.GetKeyDown(KeyCode.P)) {
			// rbody.AddForceAtPosition(rbody.data.position + offset, force);
			target.AddForce(force);
			target.AddTorque(Vector3.Cross(offset, force));
		}
	}
	
}
