using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public static class ConvexHull {
	
	
	public static List<int> CalculateOnXZPlane(List<Vector3> position) {
		if(position.Count < 3)
			throw new System.Exception("Point Length must more than 2 : Current " + position.Count);
		
		var pList = Loop.SelectEach(position, x=>x.Flat());
		var org = GetBottomLeft(pList);
		
		List<int> list = Sort(pList,org);
		List<int> rt = GetConvexHull(pList,org,list);
		
		// DebugSort(pList,org,list);
		// DebugHull(pList,rt);
		
		return rt;
	}
	
	private static int GetBottomLeft(List<Vector3> pList) {
		var rt = 0;
		
		for(int i = 1; i<pList.Count; i++) {
			if(pList[i].z < pList[rt].z)
				rt = i;
			else if(pList[i].z == pList[rt].z && pList[i].x < pList[rt].x)
				rt = i;
		}
		
		return rt;
	}
	
	private static List<int> Sort(List<Vector3> pList, int org) {
		var rt = Loop.Count(pList.Count).Select(x=>x);
		var dot = Loop.SelectEach(pList, x=>Vector3.Dot((x-pList[org]).normalized, Vector3.right));
		
		rt.RemoveAt(org);
		
		rt.Sort((lhs,rhs)=>{
			if(dot[lhs] == dot[rhs]) return 0;	
			return dot[lhs]<dot[rhs] ? 1 : -1;	
		});
		
		return rt;
	}
	
	private static void DebugSort(List<Vector3> pList, int org, List<int> list) {
		var c = 1f/list.Count;
		
		for(int i=0; i<list.Count; i++) {
			Debug.DrawLine(pList[org], pList[list[i]], new Color(i*c, i*c, i*c, 1));
		}
	}
	
	private static List<int> GetConvexHull(List<Vector3> pList, int org, List<int> list) {
		var rt = new List<int>();
		var c = 0;
		
		rt.Add(org);
		rt.Add(list[c]);
		
		for(int i=1; i< list.Count; i++) {
			bool testHull = true;
			var dir = pList[list[i]] - pList[list[c]];
			
			for(int j = i+1; j<list.Count; j++) {
				var nd = pList[list[j]] - pList[list[i]];
				
				if(Vector3.Cross(dir,nd).y>0) {
					testHull = false;
					break;
				}
			}
			
			if(testHull) {
				rt.Add(list[i]);
				c = i;
			}
		}
		
		return rt;
	}
	
	private static void DebugHull(List<Vector3> pList, List<int> list) {
		for(int i =0; i<list.Count; i++) {
			Debug.DrawLine(pList[list[i]], pList[list[(i+1)%list.Count]]);
		}
	}
	
}
