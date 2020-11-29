using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class PCLBody : RBody {
	
	public Transform rotationAxis;
	public CharacterController controller;
	
	public override void Reset() {
		data.SetMass(mass, mass * moiMulit);
		data.Reset(transform.position, rotationAxis.rotation);
		next.Copy(data);
	}
	
	public override void FinialApplyTransform() {
		if(simulate && Time.timeScale > 0) {
			var orgPosition = data.position;
			data.Copy(next);
			
			// transform.position = data.position;
			controller.Move(data.position - transform.position);
			data.velocity = (transform.position - orgPosition)/(PCLSimulation.deltaTime * Time.timeScale);
			data.position = transform.position;
			rotationAxis.rotation = data.rotation;
			
			// Debug.Log(rotationAxis.eulerAngles);
			
			
			// Debug.Log("changeAV " + changeAV);
			
			
		} else {
			if(Time.timeScale <= 0) return;
			
			data.velocity = (transform.position - data.position) / (PCLSimulation.deltaTime * Time.timeScale);
			data.position = transform.position;
			data.rotation = rotationAxis.rotation;
		}
	}
}
