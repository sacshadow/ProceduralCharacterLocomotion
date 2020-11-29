using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public static class DataFromAnimationSetups {
	
	[MenuItem("PCL/Create Data From Animation")]
	public static void CreateDFA() {
		var dfai = UnityEngine.Object.FindObjectOfType(typeof(DFAImporter)) as DFAImporter;	
		var clip = dfai.animator.runtimeAnimatorController.animationClips;
		
		Loop.ForEach(clip, c=>SetClip(c,dfai));
		AssetDatabase.Refresh();
		Debug.Log("Done");
	}
	private static void SetClip(AnimationClip c, DFAImporter dfai) {
		var path = "Assets/PCL/Resources/DataFromAnimation/"+c.name+".asset";
		
		DFA dfa;
		
		if(SDTK.DataRW.IsDataExists(Application.dataPath + "/PCL/Resources/DataFromAnimation/"+c.name+".asset"))
			dfa = AssetDatabase.LoadAssetAtPath(path, typeof(DFA)) as DFA;
		else
			dfa = CreateNewClip(path);
			
		// clip.frameRate = Mathf.RoundToInt(c.frameRate);
		dfa.timeLength = c.length;
		EditorUtility.SetDirty(dfa);
	}
	private static DFA CreateNewClip(string path) {
		var dfa = ScriptableObject.CreateInstance<DFA>();
		AssetDatabase.CreateAsset(dfa, path);
		
		return dfa;
	}
	
}
