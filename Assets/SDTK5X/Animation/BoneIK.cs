using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

// [ExecuteInEditMode]
public abstract class BoneIK : MonoBehaviour {
	
	public Vector3 vox = -Vector3.forward;
	
	public Transform target;
	
	public bool norInverse = false;
	public bool voxInverse = false;
	public bool update = false, ikUpdate = true;
	
	public bool drawVox = false;
	// public IKAxis ikAxis = IKAxis.X;
	// public bool negAxis = false;
	
	public Vector3 rotateOffset0 = new Vector3(0,0,0);
	public Vector3 rotateOffset1 = new Vector3(0,0,0);
	
	public IKVoxBase voxCalculater;
	
	[ContextMenu("SetIK")]
	public void SetIK() {
		InitData();
		update = true;
	}
	
	public virtual void SetToPoint(Vector3 point) {
		target.transform.position = point;
	}
	
	public abstract void InitData();
	
	public abstract float GetLength();
	
	public abstract Vector3 COM();
	
	public abstract Vector3 GetLocalBound(Transform root, float minRadio = 0.035f);
	
	public abstract Transform GetEndIKTransform();
	
	public abstract bool IsLegal();
	
	public abstract void Update_IK();
	
	
	
	protected virtual void Start() {
		InitData();
	}
	
	protected virtual void LateUpdate () {
		if(!update)
			return;
		if(IsLegal())
			Update_IK();
	}
	
	protected bool AnyEmpty(params Transform[] obj) {
		for(int i=0; i<obj.Length; i++) {
			if(obj[i] == null) return true;
		}
		return false;
	}
	
	protected float Distance(Transform a, Transform b) {
		return Vector3.Distance(a.position, b.position);
	}
	
	protected void DrawLine(params Transform[] trans) {
		for(int i=0; i<trans.Length-1; i++)
			Gizmos.DrawLine(trans[i].position, trans[i+1].position);
	}
	
	
	
}
