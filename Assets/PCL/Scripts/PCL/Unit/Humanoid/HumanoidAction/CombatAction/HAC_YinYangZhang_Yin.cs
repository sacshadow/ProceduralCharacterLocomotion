using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAC_YinYangZhang_Yin : HumanoidAction {
	
	public DFA stance;
	
	public float moveSpeed = 3.2f;
	
	protected L_Arm[] hand;
	protected KS_YinYangZhang yinYangZhang;
	
	public HAC_YinYangZhang_Yin () {
		stance = DFAManager.Find("stance_3");
		yinYangZhang = new KS_YinYangZhang();
	}
	
	public RBody GetGrabTarget() {
		return yinYangZhang.grabTarget;
	}
	
	public override void End() {
		base.End();
		constraintAttribute.stiffness = 0.1f;
		yinYangZhang.grabTarget = null;
	}
	
	public override IEnumerator ActionUpdate() {
		kinematics.SetState(yinYangZhang);
		animate.CrossFade(stance, 0.5f);
		
		constraintAttribute.stiffness = 0.25f;
		// constraintAttribute.stiffness = 0.0f;
		
		while(attribute.airTime < 0.5f) {
			behaviour.Towards(state.inputDirection);
			// behaviour.Move(GetDirection() * moveSpeed);
			behaviour.Move( state.inputMove * moveSpeed);
			
			yield return null;
		}
	}
	
	protected Vector3 GetDirection() {
		var rt = state.inputMove;
		
		if(yinYangZhang.grabTarget != null) {
			var dt = yinYangZhang.grabTarget.data;
			var disp = (dt.position - rdata.position).Flat();
			var vd = Vector3.Cross(dt.velocity.normalized, disp.normalized);
			var dir = Vector3.Cross(disp, Vector3.up).normalized;
			rt += dir * Mathf.Sign(vd.y) * Mathf.Lerp(2,0,disp.magnitude*4f);
			rt = Vector3.ClampMagnitude(rt,1);
		}
		
		return rt;
	}
}
