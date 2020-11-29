using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDTK.Cameras {
	public class CameraFollow : InstanceBehaviour<CameraFollow> {
		
		public Transform axis, handler, cameraPoint;
		public Transform target;
		public LayerMask cameraCollideMask = ~0;
		public float collideSize = 0.25f, collideDamp = 12f;
		
		public float rotate = 0, pitch = 15, distance = 5;
		[NonSerialized]
		public Vector3 axisOffset, cameraOffset; 
		
		protected float cpDis;
		
		public static Vector3 Convert(Vector3 input) {
			if(Instance != null) return Instance.transform.TransformDirection(input);
			return input;
		}
		
		public void AutoInit() {
			axisOffset = axis.localPosition;
			cameraOffset = cameraPoint.localPosition;
			rotate = transform.eulerAngles.y;
			pitch = axis.localEulerAngles.x;
			distance = -handler.localPosition.z;
			cpDis = distance;
		}
		
		public void ResetCPDis() {
			cpDis = distance;
		}
		
		public void SetTarget(Transform target) {
			this.target = target;
		}
		
		public void FollowTarget(float deltaTime) {
			if(target != null)
				transform.position = target.position;
			
			transform.rotation = Quaternion.Euler(0,rotate,0);
			axis.localPosition = axisOffset;
			axis.localRotation = Quaternion.Euler(pitch,0,0);
			handler.localPosition = Vector3.forward * -distance;
			
			var startPoint = axis.TransformPoint(cameraOffset);
			var direction = -axis.forward;
			// var endPoint = handler.TransformPoint(cameraOffset);
			
			Cast.SphereCast(startPoint, direction, collideSize, distance, cameraCollideMask, hit=>SetCpDis(hit.distance, deltaTime), ()=>SetCpDis(distance, deltaTime));
			
			cameraPoint.position = startPoint + direction * cpDis;
		}
		
		protected void SetCpDis(float distance, float deltaTime) {
			if(cpDis > distance)
				cpDis = distance;
			else
				cpDis = Mathf.Lerp(cpDis, distance, collideDamp * deltaTime);
		}
		
		protected override void Awake() {
			base.Awake();
			AutoInit();
		}
		
		void LateUpdate() {
			FollowTarget(Time.deltaTime);
		}
		
	}
}
