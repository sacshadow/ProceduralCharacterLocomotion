using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDTK.GUI.Tools;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class SDGUIManager : SDGUIBase {
	public static SDGUIManager Instance{
		get{
			if(singleton == null)
				singleton = Object.FindObjectOfType(typeof(SDGUIManager)) as SDGUIManager;
			if(singleton == null)
				singleton = SDGUI.Create(8);
			return singleton;
		}
	}
	private static SDGUIManager singleton;
	
	
	public override int width{get{return screenWidth;}}
	public override int height{get{return screenHeight;}}
	
	[SerializeField]
	private UICameraSet cam = new UICameraSet();
	
	private GizmosGrid grid = new GizmosGrid();
	private int screenWidth = -1, screenHeight = -1;
	private Transform myTransform;
	
	[SerializeField]
	private BoxCollider boxCollider;
	[SerializeField]
	private List<SDGUILayer> guiLayer = new List<SDGUILayer>();
	
	public override SDGUIBase getParent{
		get{return null;}
	}
	
	public void AddLayer(SDGUILayer nLayer){
		nLayer.SetParent(this);
		if(guiLayer.Contains(nLayer))
			Debug.LogWarning("Already Contains Layer "+nLayer.name);
		else
			guiLayer.Add(nLayer);
	}
	
	void Start(){
		if(singleton != null && singleton!=this){
			this.gameObject.SetActive(false);
			Debug.LogWarning("This scene already contains a gui manager");
		}
		singleton = this;
	
		OnEnable();
	}
	
	void OnEnable(){
		if(name == "GameObject")
			name = "SDGUIManager";
		
		if(cam == null)
			cam = new UICameraSet();
		
		if(guiLayer == null || guiLayer.Count ==0)
			SDGUI.AddLayer("Base");
		
		myTransform = this.transform;
		boxCollider = GetComponent<Collider>() as BoxCollider;
		
		cam.Init(myTransform);
		OnScreenSizeChange();
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.localScale = Vector3.one*0.01f;
		myTransform.rotation = Quaternion.identity;
		
		if(screenWidth != Screen.width || screenHeight != Screen.height)
			OnScreenSizeChange();
	}
	
	private void OnScreenSizeChange(){
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		FitCollider();
		cam.FitToScreen(screenWidth, screenHeight);
		grid.SetScreenSize(screenWidth, screenHeight);
		
		foreach(SDGUILayer gLayer in guiLayer){
			gLayer.OnParentSizeChange(this);
		}
	}
	
	private void FitCollider(){
		boxCollider.size = new Vector3(screenWidth,screenHeight,0);
		boxCollider.center = new Vector3(screenWidth/2f,screenHeight/2f,0);
	}
	
	void OnDrawGizmos(){
		boxCollider.hideFlags = HideFlags.HideInInspector;
		grid.GridOnGizmos(transform.position);
	}
	
}
