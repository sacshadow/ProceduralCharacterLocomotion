using System;
using System.Collections;
using System.Collections.Generic;
// using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class ConstructUnit : MonoBehaviour {
	// public Skeleton constrainter_prefab;
	// public RBGroup unit_prefab;
	
	public Rigidbody sphere_prefab;
	public Camera cam;
	
	[Range(8,40)]
	public float speed = 20;
	[Range(1,40)]
	public float mass = 5;
	
	public bool shot = false, autoInit = false;
	
	private RBGroup focus;
	
	// private void Init(bool camFollow) {
		// var temp = GT.Instantiate(constrainter_prefab, transform.position, transform.rotation);
		// var u = GT.Instantiate(unit_prefab, transform.position, transform.rotation);
		// var cList = temp.GetList();
		// var bList = u.GetComponentInChildren<Skeleton>().GetList();
		
		// var constraint = new List<ConstraintBase>();
		
		// for(int i=0; i<cList.Count; i++) {
			// var cb = cList[i].GetComponent<ConstraintBase>();
			// if(cb == null) continue;
			
			// cb.target = bList[i];
			// constraint.Add(cb);
		// }
		
		// u.AddConstraint(temp, constraint.ToArray());
		// u.Begin();
		
		
		
		// if(camFollow) {
			// focus = u;
			// PlayerControl.Instance.target = temp.transform;
		// }
	// }
	
	// IEnumerator Start() {
		// yield return new WaitForSeconds(0.1f);
		// if(autoInit) Init(true);
	// }
	
	private void Shot() {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		
		var temp = GT.Instantiate(sphere_prefab, ray.origin, Quaternion.identity);
		temp.mass = mass;
		temp.velocity = ray.direction * speed;
	}
	
	// private void Reset() {
		// if(PlayerControl.Instance.target != null)
			// Destroy(PlayerControl.Instance.target.gameObject);
			
		// if(focus == null) return;	
		
		// focus.transform.position = transform.position;
		// focus.GetComponent<HumanoidControl>().state.behaviour.Reset();
	// }
	
	void Update() {
		// IPT.KeyDown(KeyCode.F1, ()=>Init(true));
		// IPT.KeyDown(KeyCode.F2, ()=>Init(false));
		
		// IPT.KeyDown(KeyCode.F5, Reset);
		
		if(shot) {
			// IPT.LMB_Down(Shot);
			IPT.KeyDown(KeyCode.Z, Shot);
		}
		
		// IPT.KeyDown(KeyCode.C, ()=>PCLSimulation.Instance.timeScale = 0.1f);
		// IPT.KeyDown(KeyCode.V, ()=>PCLSimulation.Instance.timeScale = 1f);
		IPT.KeyDown(KeyCode.B, ()=>Debug.Break());
	}
	
}
