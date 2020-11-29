using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

public class PCLSimulation : InstanceBehaviour<PCLSimulation> {
	public const float deltaTime = 0.02f;
	public static Vector3 gravity = Vector3.up * -9.81f;
	public static float reciDeltaTime = 50;
	public static List<RBGroup> rbGroup = new List<RBGroup>();
	
	public int count = 0; 
	public AnimationCurve tsModify = AnimationCurve.Linear(0,0,1,1);
	
	[Range(0,1)]
	public float timeScale = 1;
	
	public static void Add(RBGroup rbg) {
		if(!rbGroup.Contains(rbg))
			rbGroup.Add(rbg);
	}
	
	public static void Remove(RBGroup rbg) {
		rbGroup.Remove(rbg);
	}
	
	
	public static void LevelInit() {
		
	}
	
	public void CalculateStep() {
		ForEachRBGroup(FrameInit);
		ForEachRBGroup(LocomotionUpdate);
		ForEachRBGroup(LinkUpdate);
		ForEachRBGroup(CalculateMotion);
		ForEachRBGroup(ApplyLimitation);
		ForEachRBGroup(ApplyTransform);
		ForEachRBGroup(IKUpdate);
		ForEachRBGroup(ConstructUpdate);
	}
	
	protected override void Awake() {
		base.Awake();
		LevelInit();
	}
	
	private void FrameInit(RBGroup group) {group.FrameInit();}
	private void LocomotionUpdate(RBGroup group) {group.LocomotionUpdate();}
	private void LinkUpdate(RBGroup group) {group.LinkUpdate();}
	private void CalculateMotion(RBGroup group) {group.CalculateMotion();}
	private void ApplyLimitation(RBGroup group) {group.ApplyLimitation();}
	private void ApplyTransform(RBGroup group) {group.ApplyTransform();}
	private void IKUpdate(RBGroup group) {group.ApplyBoneIK();}
	private void ConstructUpdate(RBGroup group) {group.ConstraintUpdate(deltaTime);}
	
	private void Update() {
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = deltaTime * timeScale;
	}
	
	private void FixedUpdate() {
		count = rbGroup.Count;
		
		if(Time.timeScale <=0) return;
		
		CalculateStep();
	}
	
	private void ForEachRBGroup(Action<RBGroup> Process) {
		for(int i=0; i<rbGroup.Count; i++) Process(rbGroup[i]);
	}
	
}
