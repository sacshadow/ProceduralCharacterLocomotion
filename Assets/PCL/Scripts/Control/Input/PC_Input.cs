using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class PC_Input : PlayerInput {
	
	public PC_Input() {
		action_0 = new KeyState(KeyCode.Mouse0);
		action_1 = new KeyState(KeyCode.Mouse1);
		action_2 = new KeyState(KeyCode.E);
		action_3 = new KeyState(KeyCode.R);
		
		pickUp = new KeyState(KeyCode.F);
		run = new KeyState(KeyCode.LeftShift);
		dodge = new KeyState(KeyCode.LeftControl);
		jump = new KeyState(KeyCode.Space);
	}
	
	public override void UpdateAxis(Vector3 position, Action<Vector3, Vector3> OnInput) {
		OnInput(
			Vector3.ClampMagnitude(IPT.AxisXZ("Horizontal", "Vertical"), 1),
			IPT.GetMouseDirectionXZ(position, CameraManager.Instance.cam));
	}
	
}
