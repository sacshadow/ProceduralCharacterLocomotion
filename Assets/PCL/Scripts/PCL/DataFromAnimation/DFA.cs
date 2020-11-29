using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class DFA : ScriptableObject {
	public const float frameInterval = 0.02f;

	public float timeLength = 1;
	public FData[] data;
	
	public FData Get(float percent) {
		try {
			var index = Mathf.Min((int)(timeLength * percent / frameInterval), data.Length);
			return data[index];
		} catch (System.Exception e) {
			Debug.Log(percent);
			throw e;
		}
	}
}
