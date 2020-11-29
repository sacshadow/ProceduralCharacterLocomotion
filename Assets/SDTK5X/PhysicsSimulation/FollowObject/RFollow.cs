using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class RFollow : MonoBehaviour {

	public RPoint[] point;
	
	private Vector3 lastPoint;
	
	private Transform localParent;
	private Renderer render;
	
	// Use this for initialization
	// IEnumerator Start() {
	void Start() {
		// localParent = transform.parent.parent.parent.parent;
		
		// while(!Input.GetKeyDown(KeyCode.T)) {
			// yield return null;
		// }
		
		// yield return new WaitForSeconds(1f);
		
		lastPoint = transform.position;
		InitFirstPoint();
		Loop.Between(1,point.Length).Do(InitRPoint);
		render = transform.root.GetComponentInChildren<Renderer>();
		
		// yield return null;
		
		// while(true) {
			
			
			
			// yield return null;
		// }
		
	}
	
	void Update() {
		if(Time.timeScale >0 && render.isVisible)
			SetUpdate();
	}
	
	// Update is called once per frame
	void SetUpdate() {
		// if(Time.timeScale <=0)
			// return;
		
		InitFirstPoint();
		Loop loop = Loop.Between(1,point.Length);
		
		loop.Do(InitPoint);
		loop.Do(CaluForce);
		loop.Do(MovePoint);
	}
	
	private void InitRPoint(int index) {
		// point[index].localParent = localParent;
		point[index].Init(point[index-1]);
	}
	
	private void InitFirstPoint() {
		point[0].position = transform.position;
		point[0].velocity = (point[0].position - lastPoint) /Time.deltaTime;
		point[0].acceleration = Vector3.zero;
		
		lastPoint = point[0].position;
	}
	
	private void InitPoint(int index) {
		point[index].CaluInit(point[index-1]);
	}

	private void CaluForce(int index) {
		point[index].CaluForce(point[index-1]);
	}
	
	private void MovePoint(int index) {
		point[index].Move(point[index-1]);
	}
	
}
