using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class PlayData {
	public DFA dfa;
	public float percentage = 0;
	
	public FData GetFrame(float percentage) {
		this.percentage = percentage;
		return dfa.Get(this.percentage);
	}
	public FData GetCurrentFrame() {
		return dfa.Get(this.percentage);
	}
}

public class Animate : Gear {
	public Skeleton skeleton;
	public PlayData current, fade;
	
	public float fadeTime = -1;
	public float fadeAmount = 0;
	
	public Animate(Skeleton skeleton) {
		this.skeleton = skeleton;
		current = new PlayData();
		fade = new PlayData();
	}
	
	public override void Reset() {
		base.Reset();
		fadeTime = -1;
		fadeAmount = 0;
	}
	
	public void GetCurrentFrame(Action<FData> Process, Action NoFrameData) {
		if(current.dfa != null) 
			Process(current.GetFrame(current.percentage));
		else
			NoFrameData();
	}
	
	public void Play(DFA anim) {
		Play(anim, 0);
	}
	public void Play(DFA anim, float normalizedTime) {
		fade.dfa = null;
		current.dfa = anim;
	}
	
	public void CrossFade(DFA anim, float fadeTime = 0.15f) {
		CrossFade(anim, fadeTime, 0);
	}
	public void CrossFade(DFA anim, float fadeTime, float normalizedTime) {
		if(current.dfa == anim) return;
	
		fade.dfa = current.dfa;
		current.dfa = anim;
		
		if(fade.dfa == null) return;
		
		this.fadeTime = fadeTime;
		this.fadeAmount = 0;
	}
	
	public virtual void SetToFrameData(FData frame) {
		skeleton.ForIndexEachBone((index,bone)=>bone.localRotation = frame.localRotation[index]);
	}
	
	public virtual void FadeToFrameData(FData frame, FData fadeFrame, float deltaTime) {
		fadeAmount += deltaTime;
		var percent = fadeAmount/fadeTime;
		
		// var value = Mathf.SmoothStep(0,1,percent);
		var value = Mathf.Sin(percent * Mathf.PI/2f);
		
		// Action<int,Transform> Process = (index, bone) =>
			// bone.localRotation = Quaternion.Lerp(
				// fadeFrame.localRotation[index], frame.localRotation[index], value);
		
		Action<int,Transform> Process = (index, bone) =>
			bone.localRotation = Quaternion.Lerp(
				bone.localRotation, frame.localRotation[index], value);
		
		skeleton.ForIndexEachBone(Process);
		
		if(percent >= 1)
			fadeTime = -1;
	}
	
	public void Process(float percentage, float deltaTime) {
		if(current.dfa == null || skeleton == null) return;
		
		if(percentage > 1)
			throw new System.Exception("Animate Percentage out of range " + percentage);
		
		var frame = current.GetFrame(percentage);
		
		if(fadeTime > 0)
			FadeToFrameData(frame, fade.GetFrame(fade.percentage), deltaTime);
		else
			SetToFrameData(frame);
	}
	
}
