using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Roll_Stick : HA_Roll {
	
	public WP_Stick stick;
	
	public HA_Roll_Stick(WP_Stick stick) : base() {
		this.stick = stick;
	}
	
	public override void Begin() {
		base.Begin();
		stick.follow = false;
	}
	
	public override void End() {
		base.End();
		stick.follow = true;
	}
	
}
