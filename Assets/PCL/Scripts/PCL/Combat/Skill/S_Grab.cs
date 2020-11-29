using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class S_Grab : SkillBase {
	public static int interactable = LayerMask.NameToLayer("Interactable");
	
	public L_Arm arm_L, arm_R;
	public LayerMask grabMask;
	public Rigidbody grabed_L, grabed_R;
	public Transform root;
	public float checkRadio = 0.4f;
	
	public override void Setup(Locomotion locomotion) {
		base.Setup(locomotion);
		arm_L = locomotion.bodyStructure.arm_L as L_Arm;
		arm_R = locomotion.bodyStructure.arm_R as L_Arm;
		grabMask = locomotion.behaviour.grabMask;
		grabed_L = grabed_R = null;
		root = locomotion.behaviour.constraintAttribute.transform;
	}
	
	public override void Process(float deltaTime) {
		grabed_L = GrabCheck(arm_L.rbody.data.position);
		grabed_R = GrabCheck(arm_R.rbody.data.position);
		if(grabed_L != null) Debug.DrawLine(arm_L.rbody.data.position, grabed_L.position, Color.yellow);
		if(grabed_R != null) Debug.DrawLine(arm_R.rbody.data.position, grabed_R.position, Color.yellow);
	}
	
	public void ForEachArmGrabed(Action<L_Arm, Rigidbody> Process) {
		if(grabed_L != null) Process(arm_L, grabed_L);
		if(grabed_R != null) Process(arm_R, grabed_R);
	}
	
	public void OnGrab(Vector3 point, Action<Rigidbody> Process) {
		var rt = GrabCheck(point);
		if(rt != null) Process(rt);
	}
	
	protected Rigidbody GrabCheck(Vector3 point) {
		Rigidbody rt = null;
		var dist = 1000f;
		var col = Physics.OverlapSphere(point, checkRadio, grabMask);
		var forward = locomotion.transform.forward;
		var position = locomotion.transform.position;
		
		for(int i=0; i<col.Length; i++) {
			var c = col[i];
			if(c.transform.root == root) continue;
			
			var dir = c.transform.position - position;
			if(Vector3.Angle(forward, dir) > 100) continue;
			
			IF_Rigidbody(c, r=>{
				var d = Vector3.Distance(r.position, point);
				if(d < dist) {
					dist = d;
					rt = r;
				};
				Debug.DrawLine(r.position, point);
			});
		}
		
		return rt;
	}
	
	protected void IF_Rigidbody(Collider c, Action<Rigidbody> Process) {
		var r = GetRigidbody(c);
		if(r != null) Process(r);
	}
	
	protected Rigidbody GetRigidbody(Collider c) {
		if(c.gameObject.layer == interactable) return FindRB(c.transform);
		return c.GetComponent<Rigidbody>();
	}
	
	protected Rigidbody FindRB(Transform t) {
		if(t == null)
			return null;
		
		var rt = t.GetComponent<Rigidbody>();
		if(rt != null)
			return rt;
		
		return FindRB(t.parent);
	}
}
