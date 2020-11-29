using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDTK.Cameras {
	public class CameraControl : InstanceBehaviour<CameraControl> {
		
		public CameraFollow cameraFollow;
		
		public float horizontalSpeed = 720, verticalSpeed = 360, modify = 1;
		public float pClampMin = -87.5f, pClampMax = 87.5f;
		
		public bool inverseX = false, inverseY = false;
		
		public bool pitch = true, yaw = true;
		
		
		public void Process(float deltaTime) {
			var input = IPT.AxisXY("Mouse X", "Mouse Y");
			if(!pitch) input.y = 0;
			if(!yaw) input.x = 0;
			
			cameraFollow.rotate += horizontalSpeed * modify * input.x * deltaTime * (inverseX ? -1:1);
			cameraFollow.pitch += verticalSpeed * modify * input.y * deltaTime * (inverseY ? 1:-1);
			cameraFollow.pitch = Mathf.Clamp(cameraFollow.pitch, pClampMin, pClampMax);
			
			cameraFollow.FollowTarget(deltaTime);
		}
		
		void Start() {
			cameraFollow.enabled = false;
		}
		
		void LateUpdate() {
			Process(Time.deltaTime);
		}
		
		
	}
}
