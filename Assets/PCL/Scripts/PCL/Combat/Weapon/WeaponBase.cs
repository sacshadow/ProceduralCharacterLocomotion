using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public abstract class WeaponBase : ConstraintWeapon {
// public abstract class WeaponBase : MonoBehaviour {
	
	// public float mass = 2f;
	// public Rigidbody rb;
	public WeaponLink weaponLink;
	public HumanoidBehaviour behaviour;
	public WeaponStance stance;
	
	// public float em = 2, lem = 0.5f, erm = 2, lerm = 0.5f;
	
	protected Vector3 lastError, lastErrorR;
	
	public abstract FightStyleBase GetFightStyle();
	
	public virtual void Attach(HumanoidBehaviour behaviour) {
		if(weaponLink != null) Detach();
		
		this.behaviour = behaviour;
		Init(AttachToBehaviour);
		
		// target = stickLink.weightPoint.transform;
		attr = behaviour.constraintAttribute;
		Setup();
		behaviour.AddConstraint(this);
		collideForce = Vector3.zero;
	}
	
	public virtual void Detach() {
		behaviour.RemoveConstraint(this);
		weaponLink.Detach(behaviour);
		Destroy(weaponLink.gameObject);
		Destroy(stance.gameObject);
		transform.SetParent(null);
		target = null;
		Setup();
	}
	
	protected abstract void Init(Action<WeaponLink, WeaponStance> Process);
	
	protected void AttachToBehaviour(WeaponLink link, WeaponStance stance) {
		this.weaponLink = link;
		this.stance = stance;
		
		weaponLink.AttachTo(behaviour);
		weaponLink.transform.SetParent(behaviour.locomotion.direction);
		
		stance.transform.SetParent(behaviour.locomotion.direction);
		stance.transform.localPosition = Vector3.zero;
		stance.transform.localRotation = Quaternion.identity;
		
		behaviour.locomotion.bodyStructure.OnAttachWeapon(this);
		if(behaviour.constraintAttribute != null)
			transform.SetParent(behaviour.constraintAttribute.transform);
		
	}
	
	protected ConfigurableJoint AddJoint(Rigidbody lhs, Rigidbody rhs, Vector3 offset) {
		var joint = rhs.gameObject.AddComponent<ConfigurableJoint>();
		joint.connectedBody = lhs;
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = Vector3.zero;
		joint.connectedAnchor = offset;
		joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Locked;
		
		return joint;
	}
}
