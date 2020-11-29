using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;


[CreateAssetMenuAttribute(fileName = "WeightRate", menuName = "PCL/WeightRate")]
public class WeightRate : ScriptableObject {
	
	public float[] 
		spine, 
		leftArm, 
		rightArm, 
		leftLeg, 
		rightLeg, 
		neck;
	
}
