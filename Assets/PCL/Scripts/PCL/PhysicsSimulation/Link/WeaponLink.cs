using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public abstract class WeaponLink : LinkBase {
	
	public virtual void AttachTo(HumanoidBehaviour behaviour) {
		behaviour.AddSimulateObjects(GetComponentsInChildren<RBody>(), GetComponentsInChildren<LinkBase>());
	}
	
	public virtual void Detach(HumanoidBehaviour behaviour) {
		behaviour.AddSimulateObjects(GetComponentsInChildren<RBody>(), GetComponentsInChildren<LinkBase>());
	}
	
}
