using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public class AIF_Line : AI_AreaInfulence {

	public float width = 10;
	public AnimationCurve forceModify = AnimationCurve.Linear(0,1,1,0);
	
	public override Vector3 GetInfulenceOnPoint(Vector3 point) {
		var localPos = transform.InverseTransformPoint(point);
		
		if(localPos.z > keepDis || localPos.z < -1)
			return Vector3.zero;
		if(Mathf.Abs(localPos.x * 2) > width)
			return Vector3.zero;
		
		return transform.forward * keepForce * forceModify.Evaluate(localPos.z/keepDis);
	}
	
	protected override void OnDrawGizmos() {
		base.OnDrawGizmos();
		Gizmos.matrix = transform.localToWorldMatrix;
		
		Gizmos.DrawCube(Vector3.zero, new Vector3(width,1,1));
		Gizmos.DrawWireCube(Vector3.forward * keepDis/2f, new Vector3(width,1,keepDis));
	}
	
}
