using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class Kinematics {
	
	public Locomotion locomotion;
	public Animate animate;
	public Gear arm, leg;
	
	public KinematicsState state;
	// public float percentage = 0;
	
	public List<Gear> gear;
	public float percentage = 0;
	public float speed = 0;
	
	public Kinematics (Locomotion locomotion) {
		this.locomotion = locomotion;
		gear = new List<Gear>();
		animate = new Animate(locomotion.skeleton);
		arm = new Gear();
		leg = new Gear();
		Add(animate);
		Add(arm);
		Add(leg);
	}
	
	public void Reset() {
		if(state != null)
			state.Reset();
		// animate.Reset();
		percentage = speed = 0;
		gear.ForEach(x=>x.Reset());
	}

	public void Add(Gear g) {gear.Add(g); }
	public void Remove(Gear g) {gear.Remove(g); }
	
	public void SetState(KinematicsState state) {
		this.state = state;
		state.kinematics = this;
		state.Reset();
		animate.Reset();
	}
	
	public void PlayAnimate(DFA dfa) {
		PlayStateAndAnimate(new KS_Animate(dfa), dfa);
	}
	
	public void CrossFadeAnimate(DFA dfa, float fadeTime = 0.5f) {
		CrossFadeStateAndAnimate(new KS_Animate(dfa), dfa, fadeTime);
	}
	
	public void PlayStateAndAnimate(KinematicsState state, DFA dfa) {
		SetState(state);
		animate.Play(dfa);
	}
	
	public void CrossFadeStateAndAnimate(KinematicsState state, DFA dfa, float fadeTime = 0.5f) {
		SetState(state);
		animate.CrossFade(dfa, fadeTime);
	}
	
	public void Simulate(float deltaTime) {
		var rt = state.GetSpeedAndPercentage(deltaTime);
		for(int i=0; i<gear.Count; i++) gear[i].Run(rt.percent, rt.speed, deltaTime);
		
		// percentage = state.Simulate(deltaTime);
		percentage = rt.percent;
		state.Simulate(deltaTime);
		animate.Process(percentage, deltaTime);
		RepositionCOM();
		locomotion.bodyStructure.SetPose(deltaTime);
		state.FinalModify();
	}
	
	protected void RepositionCOM() {
		var bodyCom = locomotion.skeleton.GetCOM();
		var disp = locomotion.body.data.position - bodyCom;
		locomotion.skeleton.transform.position = locomotion.skeleton.transform.position + disp;
	}
	
}
