using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class DevelopManager : MonoBehaviour {
	
	public UnitControl unitControl;
	// public UnitControl unitControl_prefab;
	// public Skeleton constrainter_prefab;
	
	// public WeaponBase weapon_prefab;
	
	// public bool usePrefab = true;
	
	
	// private UnitControl TempInit() {
		// var temp = GT.Instantiate(constrainter_prefab, transform.position, transform.rotation);
		
		// var cList = temp.GetList();
		// var bList = unitControl.GetComponentInChildren<Skeleton>().GetList();
		// var u = unitControl.GetComponent<RBGroup>();
		
		// var constraint = new List<ConstraintBase>();
		
		// for(int i=0; i<cList.Count; i++) {
			// var cb = cList[i].GetComponent<ConstraintBase>();
			// if(cb == null) continue;
			
			// cb.target = bList[i];
			// constraint.Add(cb);
		// }
		
		// u.AddSkeletonConstraint(temp, constraint);
		
		// unitControl.input = new PC_Input();
		// unitControl.Init();
		// unitControl.SetPose(transform.position, transform.forward);
		
		// return unitControl;
	// }
	
	// private UnitControl CreateUnit() {
		// var unit = GT.Instantiate(unitControl_prefab, transform.position, Quaternion.identity);
		// var temp = GT.Instantiate(constrainter_prefab, transform.position, Quaternion.identity);
		
		// var cList = temp.GetList();
		// var bList = unit.GetComponentInChildren<Skeleton>().GetList();
		// var u = unit.GetComponent<RBGroup>();
		
		// var constraint = new List<ConstraintBase>();
		
		// for(int i=0; i<cList.Count; i++) {
			// var cb = cList[i].GetComponent<ConstraintBase>();
			// if(cb == null) continue;
			
			// cb.target = bList[i];
			// constraint.Add(cb);
		// }
		
		// u.AddSkeletonConstraint(temp, constraint);
		
		// unit.input = new PC_Input();
		
		// unit.isControl = true;
		// unit.Init();
		// unit.SetPose(transform.position, transform.forward);
		
		// return unit;
	// }
	
	// private void AttachWeapon(HumanoidControl control) {
		// var temp = GT.Instantiate(weapon_prefab);
		// control.PickupWeapon(temp);
	// }
	
	
	// Start is called before the first frame update
	void Start() {
		// if(usePrefab) {
			// SetCamFollow(CreateUnit());
			if(unitControl != null)
				Destroy(unitControl.gameObject);
		// }
		// else
			// SetCamFollow(TempInit());
	}
	
	// Update is called once per frame
	// void Update() {
		// if(SDTK.Cameras.CameraFollow.Instance.target != null)
			// TestFunc();
	// }
	
	private void TestFunc() {
		var u = SDTK.Cameras.CameraFollow.Instance.target.GetComponent<UnitControl>();
		
		if(Input.GetKeyDown(KeyCode.X)) {
			u.attribute.isBalance = false;
			(u as HumanoidControl).state.behaviour.locomotion.analysis.balanceCheck = false;
		}
	}
	
	private void SetCamFollow(UnitControl control) {
		SDTK.Cameras.CameraFollow.Instance.target = control.transform;
	}
}
