using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Limit_Cap : LimitBase {
	
	public float radio = 0.3f, height = 1f;
	public float keepOutRadio = 0.5f, keepOutForce = 10;
	
	
	public override Vector3 LimitForce(Vector3 point) {
		if(keepOutRadio < radio) return Vector3.zero;
		
		var a = keepOutForce - radio;
		var disp = point - transform.position;
		var ay = Mathf.Abs(disp.y);
		
		if(ay > height/2f + a) return Vector3.zero;
		
		if(ay < Mathf.Max((height/2f - radio), 0))
			return Repel(disp.Flat());
		else
			return Repel(disp);
	}
	
	public override Vector3 ModiyPoint(Vector3 point) {
		var disp = point - transform.position;
		if(InRange(disp.y))
			return Clamp(GetRadio(Mathf.Abs(disp.y)), disp) + transform.position;
		else
			return point;
	}
	
	private Vector3 Repel(Vector3 disp) {
		var dis = disp.magnitude;
		if(dis < keepOutRadio)
			return disp.normalized * (keepOutRadio - dis) * keepOutForce;
		
		return Vector3.zero;
	}
	
	private Vector3 Clamp(float r, Vector3 disp) {
		var d = disp.Flat();
		d = d.normalized * Mathf.Max(r,d.magnitude);
		d.y = disp.y;
		return d;
	}
	
	private bool InRange(float y) {
		return Mathf.Abs(y) < height/2f;
	}
	
	private float GetRadio(float y) {
		var hf = Mathf.Max((height/2f - radio), 0);
		
		if(y>height/2f)
			return 0;
		if(y<hf)
			return radio;
		else {
			var r = (y-hf)/radio;
			return Mathf.Sqrt(1 - r*r);
		}
	}
	
	protected virtual void OnDrawGizmos() {
		DrawCap(radio);
		
		if(keepOutRadio > radio) {
			Gizmos.color = new Color(1,1,1,0.25f);
			DrawCap(keepOutRadio);
		}
	}
	
	private void DrawCap(float r) {
		var hf = Vector3.up * Mathf.Max(height/2f - radio,0);
		var up = hf + transform.position;
		var dp = -hf + transform.position;
		
		Gizmos.DrawWireSphere(up, r);
		Gizmos.DrawWireSphere(dp, r);
		
		var f = Vector3.forward * r;
		var s = Vector3.right * r;
		
		Gizmos.DrawLine(up+f,dp+f);
		Gizmos.DrawLine(up-f,dp-f);
		Gizmos.DrawLine(up+s,dp+s);
		Gizmos.DrawLine(up-s,dp-s);
	}
}
