using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;


public class UnitAttribute : MonoBehaviour {
	public const float bloodPopInterval = 0.125f;
	
	public float mass = 80;
	public float hp = 200, hpMax = 200, hpCap = 45, hpRecover = 10, hpCap_2 = 100;
	public float defence = 0;
	
	public bool
		isAlive = true,
		isBalance = true,
		isStandup = true,
		isOnGround = true;
	
	[NonSerialized]
	public Vector3
		position, velocity, angularVelocity, direction;
	[NonSerialized]
	public Quaternion rotation;
	
	[NonSerialized]
	public float pitch = 1f;
	
	[NonSerialized]
	public float lastLandTime = 0;
	
	[NonSerialized]
	public float clashShock = 0;
	
	public Action<float, Vector3> DamageCallback;
	
	public bool isDead {get {return !isAlive; }}
	public bool isFallDown {get {return !isStandup; }}
	public bool isInAir {get {return !isOnGround; }}
	public float airTime {get {return Time.time - lastLandTime; }}
	
	protected float frameDamage, hitInterval, lastHitTime;
	protected Vector3 hitPoints, hitForce;
	protected int hitCount;
	
	public void OnDamageRecord(float damage, Vector3 point) {
		OnDamageRecord(damage, point, Vector3.zero);
	}
	
	public void OnDamageRecord(float damage, Vector3 point, Vector3 hitForce) {
		if(isDead || damage < 0.5f) return;
	
		this.frameDamage += damage;
		this.hitPoints += point;
		this.hitForce += hitForce;
		hitCount++;
		
		if(Time.time - lastHitTime > bloodPopInterval + 0.05f)
			hitInterval = 0.05f;
	}
	
	public void SetOnGround(bool state) {
		if(state)
			lastLandTime = Time.time;
		isOnGround = state;
	}
	
	public void Kill() {
		isBalance = isAlive = false;
	}
	
	protected void ProcessDamage() {
		if(isDead || frameDamage <= 0) return;
		
		hitInterval -= Time.deltaTime;
		if(hitInterval <= 0)
			DispalyDamage();
	}
	
	protected void DispalyDamage() {
		var point = hitPoints / hitCount;
		
		PlaySFX(frameDamage, point);
		
		if(frameDamage < 1)
			return;
		
		CombatManager.ShakeCamera(point, hitForce);
		CombatManager.DispalyDamage(this, frameDamage, point);
		
		hp -= frameDamage;
		if(hp < 0 && frameDamage >10)
			Kill();
		
		if(DamageCallback != null)
			DamageCallback(frameDamage, hitForce);
		
		frameDamage = 0;
		hitPoints = hitForce = Vector3.zero;
		hitCount = 0;
		
		hitInterval = bloodPopInterval;
		lastHitTime = Time.time;
	}
	
	protected void PlaySFX(float damage, Vector3 point) {
		if(damage > 40 && damage > URD.value * 100)
			SFXPlayer.PlaySFX("HurtHard", point, 0.4f + damage/240, pitch);
		else if(damage > 5 && damage > URD.value * 45)
			SFXPlayer.PlaySFX("HurtHard", point, 0.2f + damage/160, pitch);
	}
	
	protected void Recover() {
		if(isDead || hp < 0) return;
		
		clashShock = Mathf.Clamp01(clashShock - Time.deltaTime * 4f);
		
		if(hp < hpCap)
			hp += hpRecover * Time.deltaTime;
		else if(hp < hpCap_2)
			hp += 2.5f*Time.deltaTime;
	}
	
	protected void Update() {
		if(isDead) isBalance = false;
		ProcessDamage();
		Recover();
	}
	
}
