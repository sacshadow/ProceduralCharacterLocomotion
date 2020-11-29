using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public class BloodPop : MonoBehaviour {
	
	public TextMesh[] ui;
	public Color[] c;
	
	public AnimationCurve alpha = AnimationCurve.Linear(0,1,1,0);
	public AnimationCurve scale = AnimationCurve.Linear(0,1,1,0);
	public float lifeTime = 0.75f;
	public float popSpeed = 1f;
	
	private float t;
	
	public void Set(float damage) {
		var s = damage.ToString("f0");
		for(int i=0; i<ui.Length; i++) {
			ui[i].text = s;
		}
	}
	
	void Start() {
		transform.rotation = CameraManager.Instance.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
		t += Time.deltaTime;
		
		transform.rotation = CameraManager.Instance.transform.rotation;
		transform.Translate(Vector3.up * popSpeed * Time.deltaTime);
		transform.localScale = Vector3.one * scale.Evaluate(t/lifeTime);
		
		for(int i=0; i<ui.Length; i++) {
			var nc = c[i];
			nc.a = alpha.Evaluate(t/lifeTime);
			ui[i].color = nc;
		}
		if(t > lifeTime)
			Destroy(gameObject);
	}
}
