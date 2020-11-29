using UnityEngine;
using System.Collections;

public class Test1 : MonoBehaviour {

	[SerializeField]
	private int k = 10;
	
	// Use this for initialization
	void Start () {
		Invoke("",k);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
