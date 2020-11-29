using UnityEngine;
using System.Collections;
using SDTK.GUI.Tools;

namespace SDTK.GUI.Tools{

	[System.Serializable]
	public class UICameraSet {

		public Camera cam;
		
		[HideInInspector]
		public Transform transform{
			get{
				if(cam == null)
					return null;
				
				if(myTransform == null)
					myTransform = cam.transform;
				
				return myTransform;
			}
		}
		
		private Transform myTransform;
		
		private static Camera Get2DCamera(Transform t){
			Camera cam = (new GameObject("UI Camera")).AddComponent<Camera>();
			cam.transform.parent = t;
			cam.orthographic = true;
			cam.farClipPlane = 1;
			cam.nearClipPlane = -1;
			cam.clearFlags = CameraClearFlags.Depth;
			cam.gameObject.layer = t.gameObject.layer;
			
			return cam;
		}
		
		public void Init(Transform t){
			if(cam == null)
				cam = Get2DCamera(t);
		}
		
		public void FitToScreen(int width, int height){
			if(cam == null)
				throw new System.Exception("Missing UI Camera");
			
			cam.orthographicSize = height/100f/2f;
			transform.localPosition = new Vector3(width/2f, height/2f);
		}
		
	}
}
