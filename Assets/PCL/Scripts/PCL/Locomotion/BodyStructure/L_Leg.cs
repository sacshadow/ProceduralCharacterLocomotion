using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class L_Leg : LimbBase {
	
	public Transform[] leg;
	public Transform tip, rear, ankle;
	
	public float maxOffsetKeepDistance = 0.5f;
	public float offsetKeepPose = 0.5f;
	
	
	public AnimationCurve disModify = AnimationCurve.Linear(0,1,1,0);
	public float feetLiftAngle = 90f;
	
	public AnimationCurve tipModify = AnimationCurve.Linear(0,0,1,1);
	public AnimationCurve rearModify = AnimationCurve.Linear(-1,1,0,0);
	public float tipAngle = 180, rearAngle = -90;
	
	public float legDisModify = -0.0155f;
	
	public float s;
	
	protected Vector3 feetOffset;
	protected Quaternion feetLocalRotation;
	
	public override void UpdateAfterIK() {
		if(ik.ikUpdate == true)
			UpdateFeet();
	}
	
	public override void KeepPose(float deltaTime) {
		var disp = targetPosition - leg[0].position;
		var dist = disp.magnitude;
		var k = CalculateOffsetKeepValue(disp.magnitude);
		var p = disp.normalized * Mathf.Lerp(dist,offsetKeepPose,k) + leg[0].position;
	
		SetCurrentPose(p, targetRotation, deltaTime);
	}
	
	protected float CalculateOffsetKeepValue(float distance) {
		if(distance < maxDistance) return 0;
		return (distance - maxDistance) / maxOffsetKeepDistance;
	}
	
	protected void UpdateFeet() {
		var p = transform.position;
		var disp = transform.InverseTransformDirection(leg[0].position - transform.position);
		var d = disp.magnitude;
		disp.x = 0;
		var a = disp.GetDirectionInAngle(Vector3.up, Vector3.forward);
		
		var ld = Vector3.Distance(leg[0].position, p);
		
		s = a + disModify.Evaluate(d/(ld + legDisModify)) * feetLiftAngle;
		SetPos(s/90);
		
		SetFeetAngle();
	}
	
	protected void SetPos(float angle) {
		tip.localRotation = Quaternion.Euler(tipModify.Evaluate(angle) * tipAngle,0,0);
		rear.localRotation = Quaternion.Euler(rearModify.Evaluate(angle) * rearAngle,0,0);
	}
	
	protected void SetFeetAngle() {
		var dist = Vector3.Distance(ankle.position, leg[2].position);
		var keepDis = 0.05f;
		
		if(dist < keepDis)
			// leg[2].rotation = Quaternion.Lerp(leg[2].rotation, ankle.rotation, 4*Time.deltaTime);
			leg[2].rotation = ankle.rotation;
		// else {
			// var orgRotation = leg[1].rotation * feetLocalRotation;
			// leg[2].rotation = Quaternion.Lerp(ankle.rotation, orgRotation, (dist-keepDis)/0.05f);
		// }
	}
	
	protected override void UpdatePhysics(float deltaTime) {
		// SetLimbPose(rbody.data.position,animateRotation, deltaTime);
		transform.position = currentPosition = rbody.data.position;
		leg[2].localRotation = feetLocalRotation;
	}
	
	protected override void UpdateAnimatePose() {
		var trans = ik.GetEndIKTransform();
		
		animatePosition = trans.TransformPoint(feetOffset);
		animateRotation = transform.rotation;
	}
	
	protected override void Awake() {
		base.Awake();
		feetOffset = ankle.InverseTransformPoint(transform.position);
		feetLocalRotation = leg[2].localRotation;
	}
	
}
