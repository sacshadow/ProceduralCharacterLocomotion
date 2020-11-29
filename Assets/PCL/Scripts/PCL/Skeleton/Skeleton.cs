using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public class Skeleton : MonoBehaviour {
	public Transform[] spine, leftArm, rightArm, leftLeg, rightLeg, neck;
	
	public Transform[] feetL, feetR;
	
	public WeightRate weightRate;
	
	
	public List<Transform> GetList() {
		var rt = new List<Transform>(spine);
		rt.AddRange(leftArm);
		rt.AddRange(rightArm);
		rt.AddRange(leftLeg);
		rt.AddRange(rightLeg);
		rt.AddRange(neck);
		return rt;
	}
	
	public int GetBoneCount() {
		return spine.Length + leftArm.Length + rightArm.Length + leftLeg.Length + rightLeg.Length + neck.Length;
	}
	
	public void ForEachBone(Action<Transform> Process) {
		for(int i=0; i<spine.Length; i++) Process(spine[i]);
		for(int i=0; i<leftArm.Length; i++) Process(leftArm[i]);
		for(int i=0; i<rightArm.Length; i++) Process(rightArm[i]);
		for(int i=0; i<leftLeg.Length; i++) Process(leftLeg[i]);
		for(int i=0; i<rightLeg.Length; i++) Process(rightLeg[i]);
		for(int i=0; i<neck.Length; i++) Process(neck[i]);
	}
	
	public void ForIndexEachBone(Action<int, Transform> Process) {
		var j = 0;
		for(int i=0; i<spine.Length; i++, j++) Process(j,spine[i]);
		for(int i=0; i<leftArm.Length; i++, j++) Process(j,leftArm[i]);
		for(int i=0; i<rightArm.Length; i++, j++) Process(j,rightArm[i]);
		for(int i=0; i<leftLeg.Length; i++, j++) Process(j,leftLeg[i]);
		for(int i=0; i<rightLeg.Length; i++, j++) Process(j,rightLeg[i]);
		for(int i=0; i<neck.Length; i++, j++) Process(j,neck[i]);
	}
	
	public void ForIndexEachSpine(Action<int, Transform> Process) {
		for(int i=0; i<spine.Length; i++) Process(i,spine[i]);
	}
	
	public void ForIndexEachArm(Action<int, Transform> Process) {
		var j = spine.Length;
		for(int i=0; i<spine.Length; i++, j++) Process(j,leftArm[i]);
		for(int i=0; i<spine.Length; i++, j++) Process(j,rightArm[i]);
	}
	public void ForIndexEachSpineAndArm(Action<int, Transform> Process) {
		var j = 0;
		for(int i=0; i<spine.Length; i++, j++) Process(j,spine[i]);
		for(int i=0; i<leftArm.Length; i++, j++) Process(j,leftArm[i]);
		for(int i=0; i<rightArm.Length; i++, j++) Process(j,rightArm[i]);
	}
	
	public Vector3 GetCOM() {
		Vector3 p = Vector3.zero;
		float r = 0;
		
		Action<Vector3,float> Sum = (v,f)=> {
			p+=v*f;
			r+=f;
		};
		
		SumWeight(spine, weightRate.spine, Sum);
		SumWeight(leftArm, weightRate.leftArm, Sum);
		SumWeight(rightArm, weightRate.rightArm, Sum);
		SumWeight(leftLeg, weightRate.leftLeg, Sum);
		SumWeight(rightLeg, weightRate.rightLeg, Sum);
		SumWeight(neck, weightRate.neck, Sum);
		
		if(r != 0)
			return p/r;
		return spine[0].position;
	}
	
	private void SumWeight(Transform[] t, float[] rate, Action<Vector3,float> Process) {
		for(int i=0; i<t.Length; i++) Process(t[i].position, rate[i]);
	}
	
}
