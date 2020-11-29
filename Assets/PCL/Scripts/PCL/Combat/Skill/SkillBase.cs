using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class SkillBase {
	
	
	public Locomotion locomotion;
	
	public virtual void Setup(Locomotion locomotion) {
		this.locomotion = locomotion;
		
		
	}
	
	public virtual void Process(float deltaTime) {
		
		
	}
	
}
