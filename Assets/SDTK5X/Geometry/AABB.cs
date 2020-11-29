/*
	2D 轴坐标包围盒
	Axis Aligned Bounding Box
*/
using UnityEngine;
using System.Collections;

namespace SDTK.Geometry{
	public class AABB {
		public float right, left, top, bottom;
		
		//实例化, 生成包围盒
		public AABB(Vector2[] point){
			if(point.Length==0)
				Debug.LogError("Empty Array");
			
			right=left=point[0].x;
			top=bottom=point[0].y;
			
			for(int i=1; i< point.Length; i++){
				right=Mathf.Max(right, point[i].x);
				left=Mathf.Min(left,point[i].x);
				top=Mathf.Max(top,point[i].y);
				bottom=Mathf.Max(bottom,point[i].y);
			}
		}
		
		//判断点 point 是否在范围内
		public bool IsPointInBox(Vector2 point){
			return ((point.x-right)*(point.x-left)<=0 && (point.y-top)*(point.y-bottom)<=0);
		}
		
		//获得区域大小
		public float GetBoundArea(){
			return (right-left)*(top-bottom);
		}
		
		//判断点是否在线段范围内
		public static bool IsPointInSegment(Vector2 point, Vector2 start, Vector2 end){
			return ((point.x-start.x)*(point.x-end.x)<=0 && (point.y-start.y)*(point.y-end.y)<=0);
		}
		
		//判断两个包围盒是否相交
		public static bool IsOverlap(AABB a, AABB b){
			return !(a.right < b.left ||a.left>b.right ||  a.top< b.bottom || a.bottom>b.top);
		}
	}
}