using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class LookAtDirection : MonoBehaviour {
	
	public Transform targetDirection, neck, head;
	
	public float
		minX = -15, maxX = 60,
		clampY = 80;
	
	public float weight = 1;
	
	public float followWeight = 720, damp = 40f;
	public float maxRotateSpeed = 620f;
	private Vector2 angle, av;
	
	public void Reset() {
		targetDirection.localRotation = Quaternion.identity;
	}
	
	public void SetLookDirection(Vector3 forward) {
		targetDirection.rotation = Quaternion.LookRotation(forward);
		Update_IK();
	}

	public void Update_IK() {
		var targetAngle = Vector2.Lerp(Vector2.zero, GetAngle(), weight);
		var aa = (targetAngle - angle) * followWeight - av * damp;
		var change = aa * Time.deltaTime;
		angle += Vector2.ClampMagnitude(av + 0.5f * change, maxRotateSpeed) * Time.deltaTime;
		av += change;
		
		av.x = Mathf.Clamp(av.x, minX, maxX);
		av.y = Mathf.Clamp(av.y, -clampY, clampY);
		
		var q = Quaternion.Euler(angle.x/2f, angle.y/2f, 0);
		neck.localRotation = head.localRotation = q;
	}
	
	private Vector2 GetAngle() {
		var lp = transform.InverseTransformDirection(targetDirection.forward);
		var flat = lp.Flat();
		lp.x = 0;
		
		var x = -lp.GetDirectionInAngle(Vector3.forward, Vector3.up);
		if(Vector3.Dot(lp, Vector3.forward) < 0)
			x = 0;
		else
			x = Mathf.Clamp(x, minX, maxX);
	
		var y = flat.GetDirectionInAngle(Vector3.forward, Vector3.right);
		y = Mathf.Clamp(y, -clampY, clampY);
		
		return new Vector2(x,y);
	}
	
	// void LateUpdate() {
		// LookAtDirection();
	// }
	
}
