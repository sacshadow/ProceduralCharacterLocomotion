using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace SDTK.ProjectAssetInfo{

	public enum ESIconType{NULL,MENUITEM, SCRIPT, ASSET, PREFAB, MATERIAL}
	
	public class ESIcon{
		public string niceName;
		public ESIconType type;
		public string path;
		public string picID;
		public string tooltip;
		
		public ESIcon(){
			this.niceName="";
			this.type=ESIconType.NULL;
			this.path="";
			this.picID="";
			this.tooltip="";
		}
		public ESIcon(string name, string path,ESIconType type,string tooltip){
			this.niceName=name;
			this.path=path;
			this.type=type;
			this.tooltip=tooltip;
			
			switch(type){
				case ESIconType.SCRIPT: picID=AssetDatabase.AssetPathToGUID("Assets/SDTK/EditorShelf/Resources/script.png");break;
				case ESIconType.MENUITEM: picID=AssetDatabase.AssetPathToGUID("Assets/SDTK/EditorShelf/Resources/EditorScript.png");break;
				case ESIconType.PREFAB: picID=AssetDatabase.AssetPathToGUID("Assets/SDTK/EditorShelf/Resources/prefab.png");break;
				default: picID="";break;
			}
		}
		
		public void OnSelect(){
			switch(type){
				case ESIconType.NULL:
				case ESIconType.MENUITEM: return;
			}
			
			if(path==null || path.Length==0)
				throw new Exception("data error");
			
			string assetPath=AssetDatabase.GUIDToAssetPath(path);
			Selection.activeObject=AssetDatabase.LoadAssetAtPath(assetPath,typeof(UnityEngine.Object));
		}
		
		public void OnApply(){
			if(path==null || path.Length==0)
				throw new Exception("data error");
			
			if(type==ESIconType.NULL)
				throw new Exception("Icon does not have any data");
			else if(type==ESIconType.SCRIPT)
				AddComponents();
			else if(type==ESIconType.MENUITEM)
				EditorApplication.ExecuteMenuItem(path);
			else if(type==ESIconType.PREFAB)
				InstancePrefab();
		}
		
		public void OnDrag(){
			
		}
		
		public static implicit operator GUIContent(ESIcon icon){
			GUIContent rt=new GUIContent(icon.niceName, icon.tooltip);
			if(icon.picID==null || icon.picID.Length<0)
				return rt;
			
			string path=AssetDatabase.GUIDToAssetPath(icon.picID);
			Texture2D pic=AssetDatabase.LoadAssetAtPath(path,typeof(Texture2D)) as Texture2D;
			
			rt.image=pic;
			return rt;
		}
		
		private void AddComponents(){
			Undo.RecordObjects(Selection.gameObjects,"AddComponent "+path);
		
			// foreach(GameObject go in Selection.gameObjects)
				// UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(go, "Assets/SDTK5X/Common/Editor/_DataAsset/ProjectAssetInfo.cs (85,5)", path);
		}
		
		private void InstancePrefab(){
			
			
			string prefabPath=AssetDatabase.GUIDToAssetPath(path);
			
			GameObject prefab=AssetDatabase.LoadAssetAtPath(prefabPath,typeof(GameObject)) as GameObject;
			
			if(prefab==null)
				throw new Exception("Lose prefab");
			
			UnityEngine.Object obj=PrefabUtility.InstantiatePrefab(prefab);
			Selection.activeObject=obj;
			EditorApplication.ExecuteMenuItem("GameObject/Move To View");
		}
	}
	
	
	

}
