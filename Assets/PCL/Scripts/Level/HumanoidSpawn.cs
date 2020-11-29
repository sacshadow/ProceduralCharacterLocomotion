using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HumanoidSpawn : MonoBehaviour {
	
	public bool isPlayer = false;
	public bool auto = true;
	public HumanoidControl unitControl_prefab;
	public Skeleton constrainter_prefab;
	public WeaponBase weapon_prefab;
	
	public HumanoidControl Spawn() {
		var position = transform.position + Vector3.up * 1.2f;
		var unit = GT.Instantiate(unitControl_prefab, position, Quaternion.identity);
		var constrainter = GT.Instantiate(constrainter_prefab, position, Quaternion.identity);
		
		unit.state.behaviour.AddSkeletonConstraint(constrainter, GetConstraint(unit, constrainter));
		unit.Init();
		unit.SetPose(position, transform.forward);
		
		if(weapon_prefab != null)
			AttachWeapon(unit);
		
		return unit;
	}
	
	private List<ConstraintBase> GetConstraint(HumanoidControl unit, Skeleton constrainter) {
		var cList = constrainter.GetList();
		var bList = unit.state.behaviour.locomotion.skeleton.GetList();
		var constraint = new List<ConstraintBase>();
		
		for(int i=0; i<cList.Count; i++) {
			var cb = cList[i].GetComponent<ConstraintBase>();
			if(cb == null) continue;
			
			cb.target = bList[i];
			constraint.Add(cb);
		}
		return constraint;
	}
	
	private void AttachWeapon(HumanoidControl control) {
		var temp = GT.Instantiate(weapon_prefab);
		control.PickupWeapon(temp);
	}
	
	private void AutoSpawn() {
		if(isPlayer) SetCamFollow(Spawn());
		else Spawn();
	}
	
	private void SetCamFollow(UnitControl control) {
		SDTK.Cameras.CameraFollow.Instance.target = control.transform;
		control.isControl = true;
		control.input = new PC_Input();
		control.Reset();
	}
	
	// Start is called before the first frame update
	void Start() {
		if(auto) AutoSpawn();
	}
	
}
