using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public class AI_AreaInfulence : MonoBehaviour {
	
	public static List<AI_AreaInfulence> area = new List<AI_AreaInfulence>();
	
	public float keepDis = 2f;
	public float keepForce = 1.5f;
	public Color color = new Color(1,1,0,0.5f);
	
	public virtual Vector3 GetInfulenceOnPoint(Vector3 point) {
		return Vector3.zero;
	}
	
	void OnEnable() {
		if(!area.Contains(this))
			area.Add(this);
	}
	
	void OnDisable() {
		if(area != null)
			area.Remove(this);
	}
	
	
	protected virtual void OnDrawGizmos() {
		Gizmos.color = color;
	}
	
}
