/*
	自动截图，保存至 d:///GameScreenShots
	最后修改 2011-7-25
*/
using UnityEngine;
using System;
using System.Collections;
using SDTK;

[AddComponentMenu ("SDTK/Functional/Screen Shot")]
[RequireComponent(typeof(Camera))]
public class ScreenShot : MonoBehaviour {
	private static ScreenShot singleton;//单例
	public static ScreenShot Instance{//获得单例
		get{
			if(!singleton)
				GetSingleton();
			
			return singleton;
		}
	}
	
	private bool isWaiting=false;//是否开始截图
	
	private static void GetSingleton(){//实例化
		Camera[] cameras=Camera.allCameras;
		Camera cam;
		
		if(cameras.Length==0)
			throw new Exception("Camera not Find");
		
		cam=cameras[0];
		for(int i=1; i<cameras.Length; i++ ){
			if(cameras[i].depth<cam.depth){
				cam=cameras[i];
			}
		}
		
		singleton=cam.gameObject.AddComponent<ScreenShot>();
	}
	
	void Awake(){
		//~ Debug.Log("Enable");
		
		if(singleton!=null && singleton!=this)
			DestroyImmediate(this);
		else
			singleton=this;
	}
	
	public void DoScreenShot(string savePath){//截图
		if(isWaiting)
			return;
		
		isWaiting=true;
		StartCoroutine(WaitToScreenShot(savePath));
	}
	
	private IEnumerator WaitToScreenShot(string savePath){
		yield return new WaitForEndOfFrame();
		
		string[] pathSegment=Application.dataPath.Split('/');
		string name=pathSegment[pathSegment.Length-2];
		
		Texture2D screenShot=new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		screenShot.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0,false);
		screenShot.Apply();
		
		try{
			FileSaveInfo info=new FileSaveInfo(name,"png",savePath);
			string finialPath=DataRW.SaveBytesToFile(screenShot.EncodeToPNG(),info);
		
			Debug.Log(name+"截图完毕; "+finialPath);
		}
		catch(Exception e){
			Debug.LogError(e.Message);
		}
		finally{
			isWaiting=false;
			DestroyImmediate(screenShot);
		}
	}
	
}
