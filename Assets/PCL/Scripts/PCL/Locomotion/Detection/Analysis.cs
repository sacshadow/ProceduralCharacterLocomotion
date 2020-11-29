using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Analysis {
	public const float defaultTargetHeight = 1.15f;
	public const float maxDistToGround = 2;
	
	public Locomotion locomotion;
	public HumanoidBehaviour behaviour {get {return locomotion.behaviour; }}
	public BodyStructure bodyStructure {get {return locomotion.bodyStructure; }}
	public Detection detection {get {return locomotion.detection; }}
	public Animate animate {get {return locomotion.kinematics.animate; }}
	
	public bool balanceCheck = true;
	
	public float targetHeight = 1.15f, supportTargetHeight = 1.15f;
	public float heightOffset = 0;
	public Vector3 targetVelocity;
	
	public List<Vector3> hull;
	public Vector3 groundPoint, supportPoint;
	public float 
		lastGroundHeigth = 0,
		disToGround = 2;
		
	protected float unbalanceTime = 0, stepbackTime = 0;
	
	public Analysis (Locomotion locomotion) {
		this.locomotion = locomotion;
		hull = new List<Vector3>();
	}
	
	public void Reset() {
		unbalanceTime = 0;
		stepbackTime = 0;
	}
	
	public Vector3 GetSupportPoint() {
		var p = locomotion.body.data.position;
		if(SDTK.Geometry.GeometryPlane.IsPointInPolygon(hull, p))
			return GetBalanceSupportPoint(p);
		else
			return GetClosetPoint(hull, p);
	}
	
	public void Process() {
		GetAnimateState();
		CalculateSupportPoint();
	}
	
	protected void GetAnimateState() {
		animate.GetCurrentFrame(SetAnimateData, ResetAnimateData);
	}
	
	protected void CalculateSupportPoint() {
		Loop.ForEach(bodyStructure.limb, l=>l.CheckState(behaviour.climbMask));
		var landPoint = bodyStructure.GetGroundPoint();
		if(landPoint.Count == 0)
			SetAirState();
		else
			SetLandState(landPoint);
		
		// Debug.Log("landPoint" + landPoint.Count);
		
		CheckBodyState();
	}
	
	protected void CheckBodyState() {
		if(!balanceCheck || behaviour.attribute.isDead) return;
		
		// if(behaviour.attribute.airTime > 0.45f) {
		if(behaviour.attribute.airTime > 0.05f) {
			if(behaviour.attribute.isBalance)
				behaviour.attribute.isBalance = GetBodyAngle(0.05f) < 75f;
			return;
		}
		
		
		var balance = true;
		if(detection.down.HitLessThan(0.35f)) balance = false;
		else if(GetBodyAngle(0.05f) > 90f) balance = false;
		
		if(GetBodyAngle(0.25f) > 45f) unbalanceTime += PCLSimulation.deltaTime;
		else unbalanceTime = 0;
		if(Vector3.Dot(locomotion.direction.forward, locomotion.body.data.velocity) < -4f) 
			stepbackTime += PCLSimulation.deltaTime;
		else
			stepbackTime = 0;
		
		if(unbalanceTime > GetKeepTime()) balance = false;
		else if(stepbackTime > 0.35f) balance = false;
		
		// if(balance == false) {
			// Debug.Log("detection " + detection.down.HitLessThan(0.35f));
			// Debug.Log("GetBodyAngle 25 " + GetBodyAngle(0.25f));
			// Debug.Log("GetBodyAngle 05 " + GetBodyAngle(0.05f));
			// Debug.Log("velocity " + Vector3.Dot(locomotion.direction.forward, locomotion.body.data.velocity));
			// Debug.Log("stepbackTime " + stepbackTime);
		// }
		
		behaviour.attribute.isBalance = balance;
		if(!balance)
			balanceCheck = false;
	}
	
	protected float GetKeepTime() {
		var rotate = locomotion.body.data.angularVelocity;
		var d = -Vector3.Dot(locomotion.direction.right, rotate.normalized);
		return Mathf.Lerp(0.2f, 0.1f, d);
	}
	
	protected float GetBodyAngle(float timeAfter) {
		var rotate = RigidbodySimulation.CalculateRotation(locomotion.body, timeAfter);
		var up = rotate * Vector3.up;
		return Vector3.Angle(Vector3.up, up);
	}
	
	protected void SetAnimateData(FData data) {
		targetHeight = data.keepHeight;
		targetVelocity = data.velocity;
		
		var t = locomotion.transform;
		var lv = t.InverseTransformDirection(locomotion.body.data.velocity);
		bodyStructure.SetAnimateOffset(data,lv);
	}
	
	protected void ResetAnimateData() {
		targetHeight = defaultTargetHeight;
		targetVelocity = Vector3.zero;
	}
	
	protected Vector3 GetBalanceSupportPoint(Vector3 position) {
		var p = position;
		p.y = groundPoint.y;
		return p;
	}
	
	protected Vector3 GetClosetPoint(List<Vector3> hullPoint, Vector3 position) {
		var p = hullPoint[0];
		var d = (p - position).Flat().sqrMagnitude;
		
		for(int i=1; i<hullPoint.Count; i++) {
			var dis = (hullPoint[i] - position).Flat().sqrMagnitude;
			if(dis < d) {
				p = hullPoint[i];
				d = dis;
			}
		}
		return p;
	}
	
	protected void SetAirState() {
		hull.Clear();
		
		if(detection.down.isHit)
			groundPoint = detection.down.hit.point;
		else
			groundPoint = locomotion.body.data.position - Vector3.up * maxDistToGround;
		
		supportPoint = groundPoint;
		lastGroundHeigth = groundPoint.y;
		disToGround = locomotion.body.data.position.y - lastGroundHeigth;
		supportTargetHeight = targetHeight  + heightOffset;
		
		behaviour.attribute.SetOnGround(false);
	}
	
	protected void SetLandState(List<Vector3> landPoint) {
		hull = GetLandPointHull(landPoint);
		groundPoint = LowerPointOf(hull);
		supportPoint = GetSupportPoint();
		lastGroundHeigth = groundPoint.y;
		disToGround = locomotion.body.data.position.y - lastGroundHeigth;
		supportTargetHeight = targetHeight  + heightOffset;
		
		behaviour.attribute.SetOnGround(true);
		
		var p = locomotion.transform.position + Vector3.right * 2f;
		Debug.DrawLine(p, p + Vector3.up * targetHeight, Color.red);
	}
	
	protected List<Vector3> GetLandPointHull(List<Vector3> landPoint) {
		var hullIndex = ConvexHull.CalculateOnXZPlane(landPoint);
		
		for(int i=0; i<hullIndex.Count; i++) {
			Debug.DrawLine(landPoint[hullIndex[i]], landPoint[hullIndex[(i+1)%hullIndex.Count]]);
		}
		
		return Loop.SelectEach(hullIndex,x=>landPoint[x]);
	}
	
	protected Vector3 LowerPointOf(List<Vector3> landPoint) {
		var rt = landPoint[0];
		for(int i=1; i<landPoint.Count; i++) {
			if(landPoint[i].y < rt.y)
				rt = landPoint[i];
		}
		return rt;
	}
	
}
