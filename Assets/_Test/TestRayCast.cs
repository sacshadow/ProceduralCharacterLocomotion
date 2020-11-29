using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class TestRayCast : MonoBehaviour {
	
	public float distance = 1, radio = 0.5f;

	// Start is called before the first frame update
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		Cast.SphereCast(transform.position, transform.forward, radio, distance, ~0, OnHit);
	}
	
	private void OnHit(RaycastHit hit) {
		Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
	}
}
