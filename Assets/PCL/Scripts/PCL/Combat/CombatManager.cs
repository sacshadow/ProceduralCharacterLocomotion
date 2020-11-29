using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;
using SDTK.Cameras;

using URD = UnityEngine.Random;


public static class CombatManager {
	public static string defaultClashSFX = "CollisionBody";
	
	public static void ShakeCamera(Vector3 point, Vector3 force) {
		var dis = Vector3.Distance(CameraManager.Instance.transform.position, point);
		
		if(dis < 5f)
			CameraManager.Instance.Shake(0.15f, Vector3.ClampMagnitude(force/800f,0.125f) * (5 - dis)/5);
	}
	
	public static void DispalyDamage(UnitAttribute attribute, float damage, Vector3 hitPoint) {
		if(CameraFollow.Instance.target == attribute.transform)
			BloodPopManager.PlayerPopDamage(damage, attribute.transform.position + Vector3.up * 0.75f, hitPoint);
		else
			BloodPopManager.PopDamage(damage, attribute.transform.position + Vector3.up * 0.75f, hitPoint);
	}
	
	public static void ApplyDamage(RBGroup group, float damage, Vector3 hitPoint, Vector3 hitForce) {
		var dmg = damage - group.attribute.defence;
		
		if(dmg > 1f)
			group.attribute.OnDamageRecord(dmg, hitPoint, hitForce);
	}
	
	public static void ApplyFallingDamage(RBGroup group, Vector3 fallingVelocity, float speedHitGround) {
		var s = -speedHitGround-1.5f;
		var damage = s * 4.5f + fallingVelocity.Flat().magnitude * 5.75f;
		// Debug.Log(damage);
		group.attribute.OnDamageRecord(damage, group.root.data.position, -fallingVelocity);
	}
	
	public static void ClashOnUnit(RBGroup lhs, RBGroup rhs, Vector3 clashPoint, Vector3 clashVelocity, Vector3 clashForce) {
		// CheckKnockDown(lhs, rhs, clashPoint, clashForce);
		CalculateClashDamage(lhs, rhs, clashPoint, clashVelocity, clashForce);
	}
	
	public static void SetKnockDown(RBGroup group) {
		group.attribute.isBalance = false;
	}
	
	private static void CalculateClashDamage(RBGroup lhs, RBGroup rhs, Vector3 clashPoint, Vector3 clashVelocity, Vector3 clashForce) {
		// if(rhs.root.data.velocity.sqrMagnitude < 2f)
			// return;
		
		var damage = Mathf.Clamp((clashVelocity - lhs.root.data.velocity).magnitude/2f - 1.5f,0, 6) * 1f * rhs.mass / lhs.mass;
		var clashSound = (clashForce.magnitude/lhs.mass/8f - 1.8f)/8f;
		if(clashSound > 0.15f)
			SFXPlayer.PlaySFX(defaultClashSFX, clashPoint, clashSound, URD.Range(0.75f, 1.25f));
		
		// Debug.Log(damage);
		
		if(damage > 30)
			damage = 30 + (damage - 30) * 0.25f;
		
		if(damage > 1)
			ApplyDamage(lhs, damage, clashPoint, clashForce);
		
	}
	
	private static void CheckKnockDown(RBGroup lhs, RBGroup rhs, Vector3 clashPoint, Vector3 clashForce) {
		var f = clashForce.magnitude;
		var effectModify = lhs.attribute.isOnGround && lhs.attribute.isBalance ? 1.5f : 1f;
		
		if(f * effectModify / lhs.mass > 4.2f)
			SetKnockDown(lhs);
		else if(!rhs.attribute.isBalance && f / lhs.mass > 1.5f)
			SetKnockDown(lhs);
		else if(!rhs.attribute.isBalance && Vector3.Distance(clashPoint, lhs.constraintSkeleton.neck[0].position) < 0.25f)
			SetKnockDown(lhs);
	}
	
}
