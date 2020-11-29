using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class PlayerInput {
	
	
	public KeyState
		action_0, action_1, action_2, action_3, pickUp, run, dodge, jump;
	
	public PlayerInput() {
		action_0 = action_1 = action_2 = action_3 = pickUp = run = dodge = jump = new KeyState();
	}
	
	public KeyState[] GetState() {
		return Loop.Array(action_0, action_1, action_2, action_3, pickUp, run, dodge, jump);
	}
	
	public virtual void UpdateAxis(Vector3 position, Action<Vector3, Vector3> OnInput) {
		
	}
	
}
