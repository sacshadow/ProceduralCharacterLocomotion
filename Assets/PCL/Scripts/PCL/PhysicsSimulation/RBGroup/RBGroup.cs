using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class RBGroup : MonoBehaviour {
	
	public Color debugColor = Color.blue;
	public UnitAttribute attribute;
	
	// [NonSerialized]
	public List<BoneIK> boneIK;
	// [NonSerialized]
	public List<RBody> rb;
	// [NonSerialized]
	public List<LinkBase> link;
	// [NonSerialized]
	public List<ConstraintBase> constraint;
	
	public RBody root;
	
	[NonSerialized]
	public Skeleton constraintSkeleton;
	[NonSerialized]
	public ConstraintAttribute constraintAttribute;
	
	public float mass {get {return attribute.mass; }}
	
	protected float dt, rdt;
	protected Vector3 com;
	
	public virtual Vector3 GetGroundPoint() {
		Vector3 rt = root.data.position;
		
		constraintSkeleton.ForEachBone(x=> {if(x.position.y < rt.y) rt = x.position; });
		
		return rt;
	}
	
	public virtual void AddSimulateObjects(RBody[] bArray, LinkBase[] lArray) {
		rb.AddRange(bArray);
		link.AddRange(lArray);
	}
	public virtual void AddSimulateObjects(List<RBody> bList, List<LinkBase> lList) {
		rb.AddRange(bList);
		link.AddRange(lList);
	}
	public virtual void RemoveSimulateObjects(RBody[] bArray, LinkBase[] lArray) {
		Loop.ForEach(bArray, x=> rb.Remove(x));
		Loop.ForEach(lArray, x=> link.Remove(x));
	}
	public virtual void RemoveSimulateObjects(List<RBody> bList, List<LinkBase> lList) {
		bList.ForEach(x=> rb.Remove(x));
		lList.ForEach(x=> link.Remove(x));
	}
	
	public virtual void AddConstraint(ConstraintBase constraint) {
		this.constraint.Add(constraint);
	}
	public virtual void RemoveConstraint(ConstraintBase constraint) {
		this.constraint.Remove(constraint);
	}
	
	public virtual void AddSkeletonConstraint(Skeleton skeleton, List<ConstraintBase> constraint) {
		this.constraint = constraint;
		this.constraintSkeleton = skeleton;
		this.constraintAttribute = skeleton.GetComponent<ConstraintAttribute>();
		constraintAttribute.group = this;
		constraintAttribute.totalMass = mass;
		
		ForEachConstraint(x=>x.Setup());
		
		ApplyMass(skeleton.spine, skeleton.weightRate.spine);
		ApplyMass(skeleton.leftArm, skeleton.weightRate.leftArm);
		ApplyMass(skeleton.rightArm, skeleton.weightRate.rightArm);
		ApplyMass(skeleton.leftLeg, skeleton.weightRate.leftLeg);
		ApplyMass(skeleton.rightLeg, skeleton.weightRate.rightLeg);
		ApplyMass(skeleton.neck, skeleton.weightRate.neck);
	}
	
	public virtual void Reset() {
		ForEachRB(x=>x.Reset());
		ForEachConstraint(x=>x.Reset());
		ForEachLink(x=>x.Reset());
		
		constraintAttribute.ClearClashData();
	}
	
	public virtual void Begin() {
		boneIK.ForEach(x=>{ x.update = false; x.InitData();});
		Reset();
	}
	
	protected virtual void ApplyMass(Transform[] t, float[] r) {
		for(int i=0; i<t.Length; i++) {
			if(r[i] > 0.001f) {
				var rb = t[i].GetComponent<Rigidbody>();
				if(rb != null)
					rb.mass = r[i] * mass;
			}
		}
	}
	
	public virtual void AddLink(LinkBase l) {
		link.Add(l);
	}
	
	public virtual void RemoveLink(LinkBase l) {
		link.Remove(l);
	}
	
	public virtual void AddRB(RBody r) {
		rb.Add(r);
	}
	public virtual void RemoveRB(RBody r) {
		rb.Remove(r);
	}
	
	public virtual void FrameInit() {
		dt = PCLSimulation.deltaTime * Time.timeScale;
		rdt = 1f/dt;
		
		ForEachRB(RigidbodySimulation.Init);
		ForEachLink(x=>x.FrameInit());
		ForEachConstraint(x=>x.FrameInit());
	}
	
	public virtual void LocomotionUpdate() {
		
		
	}
	
	public virtual void LinkUpdate() {
		ForEachLink(x=>x.ApplyForce(dt, rdt));
	}
	
	public virtual void CalculateMotion() {
		ForEachRB(x=>RigidbodySimulation.CalculateMotion(x,dt));
	}
	
	public virtual void ApplyLimitation() {
		// com = GetCOM();
		ForEachLink(x=>x.ApplyLimitations());
	}
	public virtual void ApplyTransform() {
		// ShiftWeight();
	
		ForEachRB(x=>RigidbodySimulation.ApplyTransform(x,dt));
	}
	
	public virtual void ApplyBoneIK() {
		ForEachIK(x=>x.Update_IK());
	}
	
	public virtual void ConstraintUpdate(float deltaTime) {
		ForEachConstraint(x=>x.CalcuDistributeForce(deltaTime));
		ForEachConstraint(x=>x.CalcuDistribute(deltaTime));
		ForEachConstraint(x=>x.FrameUpdate(deltaTime));
	}
	
	public virtual Vector3 GetCOM() {
		var totalWeight = 0f;
		var com = Vector3.zero;
		
		ForEachRB(x=>{
			if(x.weightShift) {
				totalWeight += x.next.mass;
				com += x.next.position * x.next.mass;
			}
		});
		
		return com / totalWeight;
	}
	
	public virtual void ForEachIK(Action<BoneIK> Process) {
		for(int i=0; i<boneIK.Count; i++) Process(boneIK[i]);
	}
	
	public virtual void ForEachRB(Action<RBody> Process) {
		for(int i=0; i<rb.Count; i++) Process(rb[i]);
	}
	
	public virtual void ForEachLink(Action<LinkBase> Process) {
		for(int i=0; i<link.Count; i++) Process(link[i]);
	}
	
	public virtual void ForEachConstraint(Action<ConstraintBase> Process) {
		for(int i=0; i<constraint.Count; i++) Process(constraint[i]);
	}
	
	public virtual void OnEnable() {
		PCLSimulation.Add(this);
	}
	
	public virtual void OnDisable() {
		PCLSimulation.Remove(this);
	}
	
	public virtual void OnDestory() {
		PCLSimulation.Remove(this);
	}
	
	protected virtual void ShiftWeight() {
		var shiftCOM = GetCOM();
		var offset = com - shiftCOM;
		ForEachRB(x=>{if(x.weightShift) x.next.position += offset;});
		
	}
}
