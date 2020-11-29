using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Detection {
	
	public Transform detectTrans;
	public LayerMask moveCollideMask;
	public Locomotion locomotion;
	
	public CastResult down, flip, climb, flipPoint, climbPoint;
	
	public bool canFlip {get {return LegalHit(flip, flipPoint, 0.6f, 0.5f); }}
	public bool canClimb {get {return LegalHit(climb, climbPoint, 1f, 0.5f); }}
	
	public void Reset() {
		down.isHit = flip.isHit = climb.isHit = flipPoint.isHit = climbPoint.isHit = false;
	}
	
	public Detection(Locomotion locomotion) {
		this.detectTrans = locomotion.direction;
		this.moveCollideMask = locomotion.behaviour.climbMask;
		down = new CastResult();
		flip = new CastResult();
		climb = new CastResult();
		flipPoint = new CastResult();
		climbPoint = new CastResult();
	}
	
	public void CheckGround() {
		var rad = 0.075f;
		var dist = 2.4f;
		Cast.SphereCast(detectTrans.position, -Vector3.up, rad, dist, moveCollideMask,
			down.OnHitSuccess, down.OnHitFail);
	}
	
	public void CheckEnvironment(float moveCheckDis) {
		flipPoint.isHit = false;
		climbPoint.isHit = false;
		
		CheckGround();
		CheckFlip(moveCheckDis);
		CheckClamp(moveCheckDis);
		
		if(climb.isHit)
			CheckClampPoint();
		else if(flip.isHit)
			CheckFlipPoint();
	}
	
	public void CheckFlip(float moveCheckDis) {
		var rad = 0.15f;
		var dist = 0.5f + moveCheckDis;
		Cast.SphereCast(detectTrans.position + Vector3.up * -0.8f, detectTrans.forward, rad, dist, moveCollideMask,
			flip.OnHitSuccess, flip.OnHitFail);
	}
	
	public void CheckClamp(float moveCheckDis) {
		var rad = 0.15f;
		var dist = 0.35f  + moveCheckDis;
		Cast.SphereCast(detectTrans.position + Vector3.up * 0.5f, detectTrans.forward, rad, dist, moveCollideMask,
			climb.OnHitSuccess, climb.OnHitFail);
	}
	
	public void CheckFlipPoint() {
		var rad = 0.05f;
		var dist = 1.25f;
		Cast.SphereCast(flip.hit.point + Vector3.up * 1.25f, -Vector3.up, rad, dist, moveCollideMask,
			flipPoint.OnHitSuccess, flipPoint.OnHitFail);
	}
	
	public void CheckClampPoint() {
		var rad = 0.05f;
		var dist = 0.55f;
		Cast.SphereCast(climb.hit.point + Vector3.up * 0.55f, -Vector3.up, rad, dist, moveCollideMask,
			climbPoint.OnHitSuccess, climbPoint.OnHitFail);
	}
	
	private bool LegalHit(CastResult result, CastResult point, float dis, float dot) {
		return result.isHit && point.HitLessThan(dis) && Vector3.Dot(point.hit.normal, Vector3.up) > dot;
	}
	
	
	
}
