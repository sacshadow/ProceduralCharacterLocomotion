using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class CameraManager : InstanceBehaviour<CameraManager> {
	
	public Camera cam;
	public Transform listener;
	
	private Vector3 shakeAmount;
	private float shakeTime;
	private bool isShake = false;
	
	public static void SetToTransform(Transform target) {
		SetToTransform(target, target);
	}
	public static void SetToTransform(Transform target, Transform listenerTarget) {
		Instance.transform.SetParent(target);
		Instance.transform.localPosition = Vector3.zero;
		Instance.transform.localRotation = Quaternion.identity;
		
		Instance.listener.SetParent(listenerTarget);
		Instance.listener.localPosition = Vector3.zero;
	}
	
	public static void Lerp(Transform from, Transform to, float lerp) {
		var trans = Instance.transform;
		trans.position = Vector3.Lerp(from.position, to.position, lerp);
		trans.rotation = Quaternion.Lerp(from.rotation, to.rotation, lerp);
	}
	
	public static IEnumerator CamLerp(Func<Vector3> GetAimPoint, Func<Vector3> GetTargetPoint, float lerpTime = 1f) {
		var trans = Instance.transform;
		var orgPoint = trans.position;		
		var orgDirection = trans.forward;
		
		var t = 0f;
		
		while(t<lerpTime) {
			t += Time.deltaTime;
			
			var targetPoint = GetTargetPoint();
			var targetDir = (GetAimPoint() - trans.position).normalized;
			
			trans.position = Vector3.Lerp(orgPoint, targetPoint, t/lerpTime);
			trans.rotation = Quaternion.LookRotation(Vector3.Lerp(orgDirection, targetDir, t/lerpTime));
			
			
			yield return null;
		};
	}
	
	public void Shake(float shakeTime, Vector3 shakeAmount) {
		this.shakeAmount = Vector3.ClampMagnitude(shakeAmount, 0.15f);
		this.shakeTime = shakeTime + Time.time;
		isShake = true;
	}
	
	void Update() {
		if(shakeTime > Time.time)
			transform.localPosition = Vector3.Scale(URD.insideUnitSphere, shakeAmount);
		else if(isShake)
			EndShake();
	}
	
	private void EndShake() {
		isShake = false;
		transform.localPosition = Vector3.zero;
	}
	
}
