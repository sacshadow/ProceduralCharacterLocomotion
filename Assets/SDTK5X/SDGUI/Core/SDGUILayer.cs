using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDTK.GUI.Tools;

public class SDGUILayer : SDGUIBase {

	public override int width{
		get{return parent.width;}
	}
	public override int height{
		get{return parent.height;}
	}
	
	private Transform myTransform;
	
	[SerializeField]
	private List<SDGUIElement> element = new List<SDGUIElement>();
	
	
	public virtual void SetParent(SDGUIManager nManager){
		transform.parent = nManager.transform;
		parent = nManager;
	}
	
	public override void OnParentSizeChange(SDGUIBase parentGUI){
		foreach(SDGUIElement e in element){
			e.OnParentSizeChange(this);
		}
	}
	
	
	// Use this for initialization
	void OnEnable () {
		myTransform = this.transform;
		General.RelationCheck(this);
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.localPosition = Vector3.zero;
		myTransform.localRotation = Quaternion.identity;
		myTransform.localScale = Vector3.one;
	}
}
