using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

// [ExecuteInEditMode]
public class Torso : MonoBehaviour {
	
	public Transform pelvis, waist, chest;
	
	[Range(-35f, 35f)]
	public float bend, twist, tilt;
	
	[Range(-45f, 45f)]
	public float screw;
	
	public Vector3 com;
	
	public bool autoUpdate = false;
	
	[NonSerialized]
	public Quaternion localRotation;
	
	public void Reset() {
		Angle(0,0,0);
		bend = twist = tilt = 0;
		autoUpdate = false;
		localRotation = Quaternion.identity;
	}
	
	public float GetDiff() {
		var a = transform.parent.position;
		var b = transform.position;
		return  Vector3.Distance(a,b) - (a.y - b.y);
	}
	
	public void Angle(float x, float y, float z) {
		// transform.localRotation = Quaternion.Lerp(transform.localRotation,
			// Quaternion.Euler(x,y,z), 8f * Time.deltaTime);
		localRotation = Quaternion.Euler(x,y,z);
	}
	
	public void Bend(float angle) {
		// bend = Mathf.Lerp(bend, angle, 8f*Time.deltaTime);
		bend = angle;
		bend = Clamp(bend);
	}
	
	public void Twist(float angle) {
		// twist = Mathf.Lerp(twist, angle, 8f*Time.deltaTime);
		twist = angle;
		twist = Clamp(twist);
	}
	
	public void Tilt(float angle) {
		// tilt = Mathf.Lerp(tilt, angle, 8f*Time.deltaTime);
		tilt = angle;
		tilt = Clamp(tilt);
	}
	
	public void Screw(float angle) {
		// screw = Mathf.Lerp(screw, angle, 8f*Time.deltaTime);
		screw = angle;
	}
	
	public void SetRotation() {
		if(pelvis == null || waist == null || chest == null)
			return;
		
		pelvis.localRotation = LP(pelvis.localRotation, Quaternion.Euler(bend, twist - screw, tilt));
		waist.localRotation = LP(waist.localRotation, Quaternion.Euler(bend, twist + screw, tilt));
		chest.localRotation = LP(chest.localRotation, Quaternion.Euler(bend, twist + screw * 2f, tilt));
		
		com = (pelvis.position + waist.position + chest.position) /3f;
		
		Debug.DrawLine(com, com+transform.forward * 0.2f, Color.blue);
		Debug.DrawLine(com, com+transform.up * 0.2f, Color.green);
		Debug.DrawLine(com, com+transform.right * 0.2f, Color.red);
		
		transform.localRotation = LP(transform.localRotation, localRotation);
		
		// transform.position = transform.parent.position + transform.position - com;
	}
	
	private float Clamp(float value) {
		return Mathf.Clamp(value, -35, 35);
	}
	
	private Quaternion LP(Quaternion org, Quaternion target) {
		return Quaternion.Lerp(org, target, 8f * 0.02f * Time.timeScale);
	}
	
	// Use this for initialization
	// void Start () {
		
	// }
	
	// Update is called once per frame
	// void Update () {
		
	// }
}
