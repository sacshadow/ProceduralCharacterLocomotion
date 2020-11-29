using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColorRandom : MonoBehaviour {
	
	public Renderer targetRender;
	public Gradient colorGradient;
	public string key = "_Color";
	public int index = 0;
	
	private MaterialPropertyBlock block;
	
	void Awake() {
		block = new MaterialPropertyBlock();
		block.SetColor(key, colorGradient.Evaluate(Random.Range(0,8)/8f));
	}
	
	void Update() {
		targetRender.SetPropertyBlock(block, index);
	}
	
	
}
