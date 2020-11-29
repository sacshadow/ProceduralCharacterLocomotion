using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class WP_Stick : WeaponBase {
	
	public WL_StickLink stickLink;
	
	public ConfigurableJoint jointL, jointR;
	public Transform stanceTrans;
	
	public override FightStyleBase GetFightStyle() {
		return new WFS_Stick(this);
	}
	
	public override void Attach(HumanoidBehaviour behaviour) {
		
		transform.SetParent(behaviour.constraintSkeleton.rightArm[3]);
		
		var lh = behaviour.constraintSkeleton.leftArm[3].GetComponent<Rigidbody>();
		var rh = behaviour.constraintSkeleton.rightArm[3].GetComponent<Rigidbody>();
		
		jointL = AddJoint(lh,rb,behaviour.constraintSkeleton.leftArm[4].localPosition);
		jointR = AddJoint(rh,rb,behaviour.constraintSkeleton.rightArm[4].localPosition);
		
		jointL.angularZMotion = ConfigurableJointMotion.Locked;
		jointL.anchor = Vector3.forward * -0.8f;
		jointR.zMotion = ConfigurableJointMotion.Free;
		
		
		base.Attach(behaviour);
	}
	
	public override void Detach() {
		base.Detach();
		Destroy(jointL);
		Destroy(jointR);
	}
	
	public void SetStance(Transform stance) {
		this.stanceTrans = stance;
	}
	
	protected override void Init(Action<WeaponLink, WeaponStance> Process) {
		stickLink = GT.Instantiate(Resources.Load<WL_StickLink>("WeaponLink/WL_StickLink"));
		Process(stickLink, GT.Instantiate(Resources.Load<WeaponStance>("WeaponStance/StickStance")));
		target = stickLink.weightPoint.transform;
		SetStance(stance.normalStance[0]);
	}
	
	
	
	void Update() {
		if(stanceTrans != null)
			stickLink.SetPose(stanceTrans.position, stanceTrans.up);
	}
	
	
}
