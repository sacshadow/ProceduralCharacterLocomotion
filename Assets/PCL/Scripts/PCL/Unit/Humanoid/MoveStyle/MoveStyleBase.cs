using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class MoveStyleBase {
	
	public HA_DefaultAction defaultAction;
	public HA_Run run;
	public HA_Jump jump;
	public HA_Fall fall;
	public HA_Roll roll;
	public HA_Flip flip;
	public HA_Climb climb;
	public HA_FallDown fallDown;
	public HA_Gitup gitup;
	
	public static MoveStyleBase Default() {
		return new MoveStyleBase{
			defaultAction = new HA_DefaultAction(),
			run = new HA_Run(),
			jump = new HA_Jump(),
			fall = new HA_Fall(),
			roll = new HA_Roll(),
			flip = new HA_Flip(),
			climb = new HA_Climb(),
			fallDown = new HA_FallDown(),
			gitup = new HA_Gitup()
		};
	}
	
	public static MoveStyleBase Stick(WP_Stick stick) {
		return new MoveStyleBase{
			defaultAction = new HAC_StickStance(stick),
			run = new HA_Run_Stick(stick),
			jump = new HA_Jump(),
			fall = new HA_Fall(),
			roll = new HA_Roll_Stick(stick),
			flip = new HA_Flip(),
			climb = new HA_Climb(),
			fallDown = new HA_FallDown(),
			gitup = new HA_Gitup()
		};
	}
	
	
}
