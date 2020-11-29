using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class TestJoint : MonoBehaviour {
	
	public Rigidbody a, b, c;
	
	public ConfigurableJoint jointA, jointB;

	// Start is called before the first frame update
	void Start() {
		
	}
	
	private void Attach() {
		jointA = AddJoint(a,c);
		jointB = AddJoint(b,c);
		
		jointA.angularZMotion = ConfigurableJointMotion.Locked;
		jointB.zMotion = ConfigurableJointMotion.Free;
	}
	
	private ConfigurableJoint AddJoint(Rigidbody lhs, Rigidbody rhs) {
		var joint = rhs.gameObject.AddComponent<ConfigurableJoint>();
		joint.connectedBody = lhs;
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = Vector3.zero;
		joint.connectedAnchor = Vector3.zero;
		joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Locked;
		
		return joint;
	}
	
	private void Remove() {
		Destroy(jointA);
		Destroy(jointB);
	}
	
	// Update is called once per frame
	void Update() {
		IPT.KeyDown(KeyCode.Z, Attach);
		IPT.KeyDown(KeyCode.X, Remove);
	}
}
