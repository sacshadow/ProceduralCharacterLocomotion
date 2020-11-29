using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDTK.GUI.Tools;

[ExecuteInEditMode]
[RequireComponent( typeof( BoxCollider))]
public class SDGUIElement : SDGUIBase {

	public int sizeX=100, sizeY=100;
	public float xPosition=0, yPosition=0;
	
	public override int width{get{return sizeX;}}
	public override int height{get{return sizeY;}}
	
	private Transform myTransform;
	private BoxCollider boxCollider;
	
	public virtual SDGUIElement this[string elementName]{
		get{return null;}
	}
	
	//~ private List
	
	public void RecalculatePosition(){
		SDGUIBase parentGUIControl = transform.parent.GetComponent<SDGUIBase>();
		
		Vector3 v = transform.localPosition;
		int x = Mathf.RoundToInt(v.x);
		int y = Mathf.RoundToInt(v.y);
		
		xPosition = Mathf.Clamp01((float)x/parentGUIControl.width);
		yPosition = Mathf.Clamp01((float)y/parentGUIControl.height);
		
		OnParentSizeChange(parentGUIControl);
	}
	
	public override void OnParentSizeChange(SDGUIBase parentGUI){
		int newX = Mathf.RoundToInt(parentGUI.width*xPosition);
		int newY = Mathf.RoundToInt(parentGUI.height*yPosition);
		
		transform.localPosition = new Vector3(newX,newY,0);
	}
	
	// Use this for initialization
	void OnEnable() {
		myTransform = transform;
		boxCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		General.RelationCheck(this);
		
		myTransform.localScale = Vector3.one;
		myTransform.rotation = Quaternion.identity;
	}
	
	void OnDrawGizmos(){
		boxCollider.hideFlags = HideFlags.HideInInspector;
		boxCollider.size = new Vector3(width,height,0);
		boxCollider.center = new Vector3(width/2f,height/2f,0);
	}
}
