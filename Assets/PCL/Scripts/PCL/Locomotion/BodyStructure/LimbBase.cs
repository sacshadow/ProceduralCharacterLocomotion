using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class LimbBase : MonoBehaviour {
	
	
	public BoneIK ik;
	public RBody rbody;
	public LimbLink link;
	
	public LayerMask collideMask = 1;
	public float landRadio = 0.05f;
	public float maxDistance = 1f;
	
	public LimbMode mode = LimbMode.ANIMATE;
	
	public Vector3 offset;
	public Vector2 halfSize = new Vector2(0.04f, 0.075f);
	
	public LimitBase limit;
	
	[NonSerialized]
	public Vector3 currentPosition, resultPosition, animatePosition;
	[NonSerialized]
	public Quaternion currentRotation, resultRotation, animateRotation;
	
	[NonSerialized]
	public Vector3 targetPosition;
	[NonSerialized]
	public Quaternion targetRotation;
	
	[NonSerialized]
	public bool isOnGround = false;
	
	public Action<float>[] PoseProcessMode;
	public Action<float> PoseProcessUpdate;
	
	public bool debug = false;
	
	protected float modeChangeTime = 0;
	protected float modeChangeInterval = 0.3f;
	
	public virtual void Reset() {
		
	}
	
	public virtual void SetPose(float deltaTime) {
		PoseProcessUpdate(deltaTime);
	}
	
	public virtual void SetLimbOffset(Vector3 offset, Vector3 velocity) {
		var d = velocity.normalized * Vector3.Dot(offset, velocity.normalized);
		var o = offset - d;
		offset = o.Flat() * 1.5f;
	}
	
	public virtual void UpdateAfterIK() {
		
	}
	
	public virtual void CheckState(LayerMask mask) {
		// Debug.Log("in CheckState");
	
		UpdateAnimatePose();
	
		if(!isOnGround || mode == LimbMode.ANIMATE) {
			currentPosition = resultPosition = transform.position;
			currentRotation = resultRotation = transform.rotation;
		}
		
		var p = ik.transform.position;
		var dir = currentPosition - p;
		var dis = maxDistance - landRadio;
		
		Cast.SphereCast(p, dir, landRadio, dis, mask, LandOnGround, HangInAir);
	}
	
	public void GetGroundPoint(List<Vector3> point) {
		if(isOnGround)
			point.AddRange(GetLandPoints());
	}
	
	public virtual void SetCurrentPose(Vector3 position, Quaternion rotation, float deltaTime) {
		var p = ik.transform.position;
		var tdisp = p - position;
		
		resultPosition = p - Vector3.ClampMagnitude(tdisp, maxDistance);
		resultRotation = rotation;
	}
	
	public virtual void SaveTargetPose(Vector3 position, Quaternion rotation) {
		var p = position;
		if(limit != null)
			p = limit.ModiyPoint(p);
			
		this.targetPosition = p;
		this.targetRotation = rotation;
	}
	
	public virtual void KeepPose(float deltaTime) {
		SetCurrentPose(targetPosition, targetRotation, deltaTime);
	}
	
	[ContextMenu("Set Distance")]
	public void SetDistance() {
		maxDistance = Vector3.Distance(ik.transform.position, transform.position);
	}
	
	public virtual void SetMode(LimbMode mode, float fadeTime = 0.25f) {
		this.mode = mode;
		
		PoseProcessUpdate = PoseProcessMode[(int)mode];
		
		if(rbody != null)
			rbody.SetSimulation(mode == LimbMode.PHYSICS);
			
		ik.ikUpdate = mode == LimbMode.BONE_IK || mode == LimbMode.PHYSICS;
	}
	
	protected virtual void UpdateAnimatePose() {
		var trans = ik.GetEndIKTransform();
		animatePosition = trans.position;
		animateRotation = trans.rotation;
		
		link.targetPoint = animatePosition;
		Debug.DrawLine(link.targetPoint, rbody.data.position, Color.red);
	}
	
	protected virtual void SetLimbPose(Vector3 position, Quaternion rotation, float deltaTime) {
		Pose(position, rotation);
	}
	
	protected virtual void UpdateAnimate(float deltaTime) {
		SetLimbPose(animatePosition, animateRotation, deltaTime);
	}
	
	protected virtual void UpdateBoneIK(float deltaTime) {
		SetLimbPose(resultPosition, resultRotation, deltaTime);
	}
	
	protected virtual void UpdateAutoIK(float deltaTime) {
		
	}
	
	protected virtual void UpdatePhysics(float deltaTime) {
		// SetLimbPose(rbody.data.position,animateRotation, deltaTime);
		transform.position = currentPosition = rbody.data.position;
	}
	
	protected virtual void Pose(Vector3 position, Quaternion rotation) {
		transform.position = currentPosition = position;
		transform.rotation = currentRotation = rotation;
	}
	
	protected virtual void LandOnGround(RaycastHit hit) {
		var p = ik.transform.position;
		var dir = p - currentPosition;
		
		if(dir.magnitude >= hit.distance) {
			currentPosition = hit.point;
			transform.position = hit.point;
			isOnGround = true;
		}
		else
			isOnGround = false;
	}
	
	protected virtual void HangInAir() {
		isOnGround = false;
	}
	
	protected virtual Vector3[] GetLandPoints() {
		var p = transform.position;
		var f = transform.forward * halfSize.y;
		var s = transform.right * halfSize.x;
		
		return new Vector3[]{
			p + f + s,
			p + f - s,
			p - f - s,
			p - f + s,
		};
	}
	
	protected virtual void Awake() {
		PoseProcessMode = new Action<float>[] {
			UpdateAnimate,
			UpdateBoneIK,
			UpdateAutoIK,
			UpdatePhysics,
		};
		PoseProcessUpdate = UpdateAnimate;
		link = rbody.GetComponent<LimbLink>();
	}
	
	protected virtual void Start() {
		currentPosition = transform.position;
		currentRotation = transform.rotation;
		SetMode(mode, 0);
	}
	
	protected virtual void OnDrawGizmos() {
		Gizmos.color = Color.red;
		if(ik != null)
			Gizmos.DrawLine(ik.transform.position, transform.position);
		
		var ps = GetLandPoints();
		
		Gizmos.color = Color.white;
		for(int i=0; i<4; i++) {
			Gizmos.DrawLine(ps[i], ps[(i+1)%4]);
		}
	}
}
