using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class TestBound : MonoBehaviour {
	
	private Transform[] t;
	
	private void TestConvexHull2D() {
		ConvexHull.CalculateOnXZPlane(Loop.SelectEach(t,x=>x.position));
	}
	
	private void GetConvexHull() {
		var p = GetPoints();
		
		if(p.Length < 3) return;
		
		int org = GetLeftBottom(p);
		List<int> list = Sort(p,org);
		var c = 1f/list.Count;
		
		for(int i=0; i<list.Count; i++) {
			Debug.DrawLine(p[org], p[list[i]], new Color(i*c, i*c, i*c, 1));
		}
		
		List<int> rt = GetConvexHull(p,org,list);
		
		for(int i =0; i<rt.Count; i++) {
			Debug.DrawLine(p[rt[i]], p[rt[(i+1)%rt.Count]]);
		}
	}
	
	private Vector3[] GetPoints() {
		return Loop.SelectArray(t, x=>x.position.Flat());
	}
	
	private int GetLeftBottom(Vector3[] p) {
		var rt = 0;
		
		for(int i = 0; i<p.Length; i++) {
			if(p[i].z < p[rt].z) {
				// if(p[i].x < p[rt].x)
					rt = i;
			} else if(p[i].z == p[rt].z) {
				if(p[i].x < p[rt].x)
					rt = i;
			}
		}
		
		return rt;
	}
	
	private List<int> Sort(Vector3[] p, int org) {
		var rt = Loop.Count(p.Length).Select(x=>x);
		rt.RemoveAt(org);
		
		rt.Sort((lhs,rhs)=>{
			var l = Vector3.Dot((p[lhs] - p[org]).normalized, Vector3.right);
			var r = Vector3.Dot((p[rhs] - p[org]).normalized, Vector3.right);
			
			if(l == r) return 0;
			
			return l<r ? 1 : -1;
			
		});
		
		return rt;
	}
	
	private List<int> GetConvexHull(Vector3[] p, int org, List<int> a) {
		var rt = new List<int>();
		
		rt.Add(org);
		rt.Add(a[0]);
		
		var c = 0;
		
		for(int i=1; i< a.Count; i++) {
			bool test = true;
			var dir = p[a[i]] - p[a[c]];
			
			for(int j = i+1; j<a.Count; j++) {
				var nd = p[a[j]] - p[a[i]];
				
				if(Vector3.Cross(dir,nd).y>0) {
					test = false;
					break;
				}
			}
			
			if(test) {
				rt.Add(a[i]);
				c = i;
			}
		}
		
		return rt;
	}
	
	
	// Start is called before the first frame update
	void Start() {
		t = GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update() {
		// GetConvexHull();
		TestConvexHull2D();
	}
}
