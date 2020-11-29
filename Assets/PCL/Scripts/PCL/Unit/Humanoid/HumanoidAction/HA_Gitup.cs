using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HA_Gitup : HumanoidAction {
	
	public DFA idle, gitup_f, gitup_b;
	
	public HA_Gitup() {
		idle = DFAManager.Find("idle");
		gitup_f = DFAManager.Find("gitup_front");
		gitup_b = DFAManager.Find("gitup_back");
	}
	
	public override void End() {
		base.End();
		analysis.balanceCheck = true;
		
	}
	
	public override IEnumerator ActionUpdate() {
		float dir = 1;
		behaviour.ForEachRB(x=>x.SetSimulation(false));
		
		if(constraintAttribute != null)
			dir = InitToCons();
		else
			dir = InitToPosition();
		
		constraintAttribute.stiffness = 0.15f;
		analysis.balanceCheck = false;
		
		yield return null;
		
		var animate = dir>0?gitup_f:gitup_b;
		kinematics.CrossFadeAnimate(animate, 1f);
		locomotion.bodyStructure.ForEachLimb(l=>l.ik.ikUpdate = false);
		
		yield return null;
		
		behaviour.ForEachRB(x=>x.Reset());
		behaviour.ForEachRB(x=>x.SetSimulation(true));
		analysis.balanceCheck = false;
		attribute.isBalance = true;
		
		var t = 0f;
		var speed = 1f/ 1f;
		
		while(t<0.1f) {
			// kinematics.state.SetPercentage(0.01f);
			t += speed * Time.deltaTime;
			SetFollowWeight(Mathf.Lerp(0f,0.35f,t*10f));
			behaviour.Move(Vector3.zero);
			var d = analysis.targetHeight - analysis.disToGround;
			rdata.velocity.y += d*0.5f;
			
			yield return null;
		}
		
		analysis.balanceCheck = false;
		attribute.isBalance = true;
		
		while(t<1f) {
			// kinematics.state.SetPercentage(Mathf.Clamp(t-0.1f,0.01f,0.9f));
			t += speed * Time.deltaTime;
			SetFollowWeight(Mathf.Lerp(0.35f,1,(t-0.1f)*5));
			
			var u = Mathf.Max(0.4f - analysis.disToGround, 0);
			behaviour.Move(locomotion.direction.rotation * animate.Get(Mathf.Clamp01(t)).velocity.Flat() + Vector3.up * u);
			
			yield return null;
		}
		
		kinematics.CrossFadeAnimate(idle, 0.3f);
		SetFollowWeight(1);
		constraintAttribute.stiffness = 0.1f;
		
		analysis.balanceCheck = true;
		attribute.isBalance = true;
		attribute.isStandup = true;
		
		yield return null;
	}
	
	protected float InitToCons() {
		var c = behaviour.constraintSkeleton.GetCOM();
		Cast.LineRay(c + Vector3.up, -Vector3.up, 2, behaviour.climbMask, 
			hit=>transform.position = hit.point + Vector3.up * 0.4f,
			()=>transform.position = c);
		
		var up = Last.Of(behaviour.constraintSkeleton.spine).position - behaviour.constraintSkeleton.spine[0].position;
		var forward = Vector3.zero;
		Loop.ForEach(behaviour.constraintSkeleton.spine, x=>{
			forward+=x.forward;
		});
		
		up = up.Flat().normalized;
		
		if(forward.y > 0) up = -up;
		
		behaviour.Towards(up);
		locomotion.transform.rotation = Quaternion.LookRotation(up, Vector3.up);
		locomotion.direction.rotation = locomotion.transform.rotation;
		// locomotion.body.Reset();
		behaviour.ForEachRB(x=>x.Reset());
		
		return forward.y;
	}
	
	protected float InitToPosition() {
		Cast.LineRay(transform.position, -Vector3.up, 2, behaviour.climbMask, hit=>transform.position = hit.point + Vector3.up * 0.15f);
		return 1;
	}
	
	protected void SetFollowWeight(float weight) {
		if(constraintAttribute != null) constraintAttribute.followWeight = weight;
	}
}
