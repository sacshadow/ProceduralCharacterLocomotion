/*
	游戏常用工具集合
	2012-08-03
*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SDTK{
	public class Tools {
		
		//判断是否符合C#类名命名规范（英文和数字only）
		public static bool IsLegalScriptName_CS(string csName){
			Regex r = new Regex("^[A-Z][a-zA-Z0-9_]+$",RegexOptions.Singleline);
			Match m = r.Match(csName);
			return m.Success;
		}
		
		//获取新组件 T
		public static T GetNewComponent<T>() where T: Component{
			return GetNewComponent<T>(null);
		}
		public static T GetNewComponent<T>(GameObject go) where T: Component{
			GameObject temp=go;
			if(temp ==null )
				temp=new GameObject(typeof(T).ToString());
			
			return temp.AddComponent<T>();
		}	
		
		//获取一个新WireLine
		public static WireLine GetNewLine(Vector3[] points, float width, Material mat){
			WireLine line=GetNewComponent<WireLine>();
			
			line.Setup(points, width, mat);
			return line;
		}
		
		//插入新得分至游戏得分榜
		public static List<int> InsertScore(List<int> org, int newScore){
			return InsertScore(org,newScore,10);//默认长度10
		}
		public static List<int> InsertScore(List<int> org, int newScore, int maxLength){
			int i=0;
			
			if(org==null)
				throw new Exception("空链表");
			if(maxLength<=0)
				throw new Exception("最大长度必须大于0");
			
			while(i<org.Count){//比对元素
				if(newScore>org[i])
					break;
				i++;
			}
			
			if(i<maxLength)//是否需要插入新得分
				org.Insert(i,newScore);
			
			while(org.Count>maxLength){//剔除多余元素
				org.RemoveAt(org.Count-1);
			}
			return org;
		}
		
	}
}