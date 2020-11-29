using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

[RequireComponent(typeof(ConstraintBase))]
public class UnitImpact : MonoBehaviour {
	
	public float damageTakenModify = 10f;
	public float ignoreImpact = 5f;
	public float knockBack = 0.25f;
	public float shockModify = 1f;
	
	public string hitSFX = "PunchHard";
	public float sfxVolume = 1;
	
	[HideInInspector]
	public ConstraintBase constrainter;
	
	public RBGroup group {get {return constrainter.attr.group; }}
	public UnitAttribute attribute {get {return constrainter.attr.group.attribute; }}
	
	public float lastHitSoundTime = 0f;
	
	protected void Start() {
		constrainter = GetComponent<ConstraintBase>();
		constrainter.OnCollisionImpact = OnCollisionImpact;
	}
	
	protected void OnCollisionImpact(Collision collision) {
		if(attribute.isDead) return;
		
		var cb = collision.collider.GetComponent<CollidableRigidbody>();
		if(cb == null || cb.velocityBeforeCollision.magnitude < 3.2f) return;
		
		if(Vector3.Dot(cb.velocityBeforeCollision.normalized, collision.relativeVelocity.normalized) < 0.25f)
			return;
		
		var amount = cb.GetCollisionModify();
		var momentum = Vector3.ClampMagnitude(cb.GetMomentumBeforeCollision() * amount, 5 * cb.rb.mass);
		var energy = momentum.sqrMagnitude;
		
		// Debug.Log(energy);
		if(energy < ignoreImpact) return;
		
		var hs = Mathf.Pow(energy, 0.25f);
		var d = hs * damageTakenModify;
		
		if(d > 15)
			d = 15 + (d - 15) * 0.01f;
		// if(d > 20)
			// d = 20 + (d - 20) * 0.05f;
		
		var veloChange = momentum * 8f / group.root.data.mass;
		veloChange.y *= 0;
		
		group.root.data.velocity += Vector3.ClampMagnitude(veloChange, 6-group.root.data.velocity.magnitude);
		
		if(group.attribute.hp + group.attribute.defence < d)
			group.root.data.velocity += momentum / group.root.data.mass * 4;
		
		CombatManager.ApplyDamage(group, d, collision.contacts[0].point, momentum);
		// constrainter.ApplyDeform(momentum * 2f, collision.contacts[0].point, shockModify*2f);
		constrainter.ApplyDeform(momentum*8f, collision.contacts[0].point, shockModify*0.5f);
		
		if(Time.time - lastHitSoundTime > 0.175f) {
			SFXPlayer.PlaySFX(hitSFX, collision.contacts[0].point, d/17f, URD.Range(0.75f, 1.25f));
			lastHitSoundTime = Time.time;
		}
	}
	
}
