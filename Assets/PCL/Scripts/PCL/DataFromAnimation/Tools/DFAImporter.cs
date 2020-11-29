using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
	using UnityEditor;
#endif
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class DFAImporter : MonoBehaviour {
	public static List<string> moveKeyword = new List<string>{"idle","walk","run","step"};
	
#if UNITY_EDITOR
	public Animator animator;
	public Skeleton skeleton;
	
	// public Dictionary<string, DFA> dfa;
	
	
	private bool isProcess = false;
	
	public void Do(Func<IEnumerator> Process) {
		if(isProcess) return;
		StartCoroutine(IDo(Process));
	}
	
	public IEnumerator IDo(Func<IEnumerator> Process) {
		isProcess = true;
		yield return StartCoroutine(Process());
		isProcess = false;
		Debug.Log("Process Finished " + Time.time.ToString("f2"));
	}
	
	private IEnumerator UpdateAll() {
		foreach(var kvp in DFAManager.data) {
			yield return StartCoroutine(GetDataFromAnimation(kvp.Key, kvp.Value));
		}
	}
	
	private IEnumerator UpdateEmpty() {
		foreach(var kvp in DFAManager.data) {
			if(kvp.Value.data == null || kvp.Value.data.Length == 0)
				yield return StartCoroutine(GetDataFromAnimation(kvp.Key, kvp.Value));
		}
	}
	
	private IEnumerator GetDataFromAnimation(string animationName, DFA temp) {
		var count = Mathf.CeilToInt(temp.timeLength/DFA.frameInterval);
		var ifi = 1f/DFA.frameInterval;
		temp.data = new FData[count];
		
		Debug.Log("Play " + animationName);
		animator.Play(animationName, -1, 0);
		yield return null;
		
		var boneCount = skeleton.GetBoneCount();
		var lastCom = skeleton.GetCOM();
		
		for(int i = 0; i<count; i++) {
			temp.data[i] = new FData();
			var dt = temp.data[i];
			var t = i * DFA.frameInterval/temp.timeLength;
			animator.Play(animationName, -1, t);
			// animator.PlayInFixedTime(animationName, -1, t);
			yield return null;
			
			var com = skeleton.GetCOM();
			dt.velocity = (com - lastCom) * ifi;
			dt.keepHeight = com.y;
			lastCom = com;
			
			dt.localRotation = new Quaternion[boneCount];
			skeleton.ForIndexEachBone((index, bone)=>dt.localRotation[index] = bone.localRotation);
			dt.leftHand = skeleton.leftArm[3].position - skeleton.leftArm[1].position;
			dt.rightHand = skeleton.rightArm[3].position - skeleton.rightArm[1].position;
			dt.leftFeet = skeleton.leftLeg[2].position - skeleton.leftLeg[0].position;
			dt.rightFeet = skeleton.rightLeg[2].position - skeleton.rightLeg[0].position;
			
			yield return null;
		}
		
		if(IsLoop(animationName))
			temp.data[0].velocity = Vector3.Lerp(temp.data[1].velocity, Last.Of(temp.data).velocity, 0.5f);
		
		EditorUtility.SetDirty(temp);
	}
	
	private bool IsLoop(string animationName) {
		var s = animationName.Split('_')[0];
		// Debug.Log(animationName + " " + s);
		return moveKeyword.Contains(s);
	}
	
	void Start() {
		// dfa = Resources.LoadAll("DataFromAnimation", typeof(DFA)).Select(x=>x as DFA).ToDictionary(x=>x.name);
		DFAManager.Init();
	}
	
	void OnGUI() {
		if(GUILayout.Button("UpdateAll"))
			Do(UpdateAll);
		if(GUILayout.Button("UpdateEmpty"))
			Do(UpdateEmpty);	
	}
	
	
#endif	
}
