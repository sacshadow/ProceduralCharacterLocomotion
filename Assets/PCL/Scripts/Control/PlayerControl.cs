using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class PlayerControl : InstanceBehaviour<PlayerControl> {
	
	public UnitControl unit;
	
	public static Vector3 CheckEnemyNearby(Vector3 position, Vector3 aimDir) {
		
		var d = aimDir;
		var close = 360f;
		
		for(int i=0; i<HAIBase.ai.Count; i++) {
			var t = HAIBase.ai[i];
			
			if(t.attribute.isDead)
				continue;
			if(!t.attribute.isBalance)
				continue;
			
			var disp = (t.behaviour.root.data.position - position).Flat();
			
			if(disp.magnitude > 4.5f)
				continue;
				
			var angle = Vector3.Angle(aimDir, disp);
			if(angle < 45 && angle < close) {
				close = angle;
				d = disp.normalized;
			}
		}
		
		return d;
	}
	
}
