using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class DestroyAfterTime : MonoBehaviour {
	
	public float lifeTime = 1;
	
	private float t = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if(t > lifeTime)
			Destroy(gameObject);
	}
}
