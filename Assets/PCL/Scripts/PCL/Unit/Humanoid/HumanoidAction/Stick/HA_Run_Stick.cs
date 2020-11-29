using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Run_Stick : HA_Run {

	public KS_StickMovement stickMovement;
	
	public HA_Run_Stick(WP_Stick stick) : base() {
		stickMovement = new KS_StickMovement(stick, null);
	}
	
	protected override void SetKinematics(KinematicsState state) {
		stickMovement.stick.SetStance(stickMovement.stick.stance.normalStance[1]);
		stickMovement.state = state;
		kinematics.SetState(stickMovement);
	}
	
}
