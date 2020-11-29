using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class BodyStructure : MonoBehaviour {
	
	public Locomotion locomotion;
	public RBody bodyMomentum;
	public LimbBase arm_L, arm_R, leg_L, leg_R;
	
	public LimitBase bodyLimit, swingLimit;
	
	public WeaponBase weapon;
	
	[NonSerialized]
	public LimbBase[] limb;
	
	public virtual void OnAttachWeapon(WeaponBase weapon) {
		if(this.weapon != null) this.weapon.Detach();
		this.weapon = weapon;
	}
	
	public virtual void SetLimbMode(LimbMode armMode, LimbMode legMode, float fade) {
		arm_L.SetMode(armMode, fade);
		arm_R.SetMode(armMode, fade);
		leg_L.SetMode(legMode, fade);
		leg_R.SetMode(legMode, fade);
	}
	
	public void ForEachLimb(Action<LimbBase> Process) {
		for(int i=0; i<limb.Length; i++) Process(limb[i]);
	}
	
	public virtual void SetAnimateOffset(FData data, Vector3 velocity) {
		leg_L.SetLimbOffset(data.leftFeet, velocity);
		leg_R.SetLimbOffset(data.rightFeet, velocity);
	}
	
	public virtual List<Vector3> GetGroundPoint() {
		var rt = new List<Vector3>();
		ForEachLimb(l=>l.GetGroundPoint(rt));
		if(locomotion.detection.down.HitLessThan(0.5f))
			rt.AddRange(GetBodyOnGroundPoint());
		return rt;
	}
	
	public virtual void SetPose(float deltaTime) {
		ForEachLimb(l=>l.SetPose(deltaTime));
	}
	
	public virtual void UpdateAfterIK() {
		ForEachLimb(l=>l.UpdateAfterIK());
	}
	
	// public virtual void Reset() {
		// ForEachLimb(l=>l.Reset());
	// }
	
	protected Vector3[] GetBodyOnGroundPoint() {
		var p = locomotion.detection.down.hit.point;
		var f = locomotion.direction.forward * 0.15f;
		var s = locomotion.direction.right * 0.15f;
		return new Vector3[] {
			p + f + s,
			p + f - s,
			p - f + s,
			p - f - s,
		};
	}
	
	protected virtual void Awake() {
		limb = new LimbBase[]{arm_L, arm_R, leg_L, leg_R};
	}
	
	
}
