using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;


[Serializable]
public class FData {
	
	public float keepHeight;
	public Vector3 velocity;
	public Quaternion[] localRotation;
	public Vector3 leftHand, rightHand, leftFeet, rightFeet;
	
	
}
