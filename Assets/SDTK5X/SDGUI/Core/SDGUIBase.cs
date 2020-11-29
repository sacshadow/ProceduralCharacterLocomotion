using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using  SDTK.GUI.Tools;

public abstract class SDGUIBase : MonoBehaviour {

	public abstract int width{get;}
	public abstract int height{get;}
	
	public virtual SDGUIBase getParent{
		get{
			if(parent == null)
				General.RelationCheck(this);
			return parent;
		}
	}
	
	[SerializeField]
	protected SDGUIBase parent;
	
	public virtual void OnParentSizeChange(SDGUIBase parentGUI){
	}
	public virtual void OnChildAwake(SDGUIBase childGUI){
		childGUI.OnParentSizeChange(this);
	}
	
	protected virtual void Remove(SDGUIBase child){
	}
	
	void OnDestroy(){
		if(parent != null)
			parent.Remove(this);
	}
	
}
