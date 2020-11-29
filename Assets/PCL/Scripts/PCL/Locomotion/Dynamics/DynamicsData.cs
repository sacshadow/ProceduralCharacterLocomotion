using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

[CreateAssetMenuAttribute(fileName = "DynamicsData", menuName = "PCL/DynamicsData")]
public class DynamicsData : ScriptableObject {
	[Header("Mobile")]
	public float maxHeight = 1.25f;
	public float
		supportControl = 280f,
		keepControl = 10f,
		motionControlModify = 2.8f,
		torqueControlModify = 5f,
		maxTorque = 100f, maxAccH = 20f, maxAccV = 2400;
	
	public AnimationCurve keepModify = AnimationCurve.Linear(0,1,1,0);
	
	[Header("balance")]
	public float balanceAngle = 45;
	
	public float
		eulerModify = 8,
		lastEulerModify = 0.25f,
		eulerAmount = 1,
		finialModify = 1,
		maxBalanceTorque = 40;
	
}
