using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using URD = UnityEngine.Random;

public class ChangeHairStyle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var t = Rand.PickFrom(HairStyleCollection.Instance.banditHairStyle);
		
		t = GT.Instantiate(t, transform.position, transform.rotation);
		t.SetParent(transform);
	}
	
	
}
