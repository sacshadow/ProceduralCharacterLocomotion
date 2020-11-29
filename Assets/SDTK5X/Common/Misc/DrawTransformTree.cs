using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public class DrawTransformTree : MonoBehaviour {
#if UNITY_EDITOR	
	public Gradient gradient = new Gradient();
	public float radio = 0.025f;
	public bool drawGizmos = true;
	
	private int treeDepth = 1;
	private int depth;
	
	void OnDrawGizmos() {
		if(!drawGizmos)
			return;
		
		depth = 0;
		DrawChild(this.transform, 0);
		
		if(depth != treeDepth) {
			treeDepth = depth;
		}
	}
	
	private void DrawChild(Transform t, int d) {
		var c = gradient.Evaluate((float)d / treeDepth);
		var p = t.position;
		var f = t.forward * radio;
		var r = t.right * radio;
		var u = t.up * radio;
		
		depth = Mathf.Max(depth, d+1);
		
		var dp = new Vector3[]{
			p + f, p - f,
			p + r, p + u,
			p - r, p - u,
		};
		
		Gizmos.color = c;
		Loop.Count(4).Do(x=>{
			Gizmos.DrawLine(dp[0], dp[2 +x]);
			Gizmos.DrawLine(dp[1], dp[2 +x]);
			Gizmos.DrawLine(dp[2 +x], dp[2+(x+1)%4]);
		});
		
		
		
		foreach(Transform e in t) {
			if(!e.gameObject.activeSelf)
				continue;
		
			var ep = e.position;
			Gizmos.color = c;
			Loop.ForEach(dp, x => Gizmos.DrawLine(x, ep));
			
			DrawChild(e, d+1);
		}
	}
#endif
}
