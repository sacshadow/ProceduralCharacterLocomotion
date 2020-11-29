using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using URD = UnityEngine.Random;

public class ScreenBlood : InstanceBehaviour<ScreenBlood> {
	
	public Image screenBlood;
	public Color c1, c2;
	
	public AnimationCurve modify = AnimationCurve.Linear(0,0,1,1);
	
	public AudioSource heartBeat_0, heartBeat_1;
	
	public float drySpeed = 0.5f;
	public float value = 0;
	
	public static void Splash(float dmg) {
		// Debug.Log(dmg);
		if(singleton == null) return;
		
		if(Instance.value < 0.1f)
			Instance.value = 0.1f;
		
		Instance.value += dmg/40f;
	}
	
	public void ScreenBloodDisplay( ) {
		if(PlayerControl.Instance.unit == null)
			return;
			
		var a = PlayerControl.Instance.unit.attribute;
		
		var v = Mathf.Clamp01(Mathf.Clamp01(a.hp/200) - value);
		
		screenBlood.color = Color.Lerp(c1, c2, v);
		
		heartBeat_0.volume = Mathf.Lerp(1,0,v*2f - 0.25f);
		heartBeat_1.volume = Mathf.Lerp(1,0,v*2f);
		
	}
	
	void Start() {
		screenBlood.color = c2;
	}
	
	// Update is called once per frame
	void Update () {
		// if(value < 0)
			// return;
		value = Mathf.Clamp01(value - Time.deltaTime * drySpeed);	
		
		ScreenBloodDisplay();
		
		// screenBlood.color = Color.Lerp(c2, c1, modify.Evaluate(value));
	}
}
