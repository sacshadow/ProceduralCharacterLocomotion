/*
	测量工具集合
*/

using UnityEngine;
using System;
using System.Collections;

namespace SDTK.Geometry{
	
	public class Measure {
		
		//返回2D多边形面积, 
		//公式  2 A(P) = sum_{i=0}^{n} ( x_i  (y_{i+1} - y_{i-1}) )
		public static float PolygonArea(Vector3[] point){
			int n=point.Length;
			float area=0;
			if(point.Length <=2)
				throw new Exception("Empty Array");
			
			for(int i=0; i< point.Length; i++){
				area+=point[i].x*(point[(i+1)%n].z-point[(i-1+n)%n].z);
			}
			return area/2f;
		}
		
		//中心点
		public static Vector3 Centroid(Vector3[] point){
			Vector3 centroid=Vector3.zero;
			if(point.Length ==0)
				Debug.LogWarning("Empty Array");
			
			foreach(Vector3 p in point)
				centroid+=p;
			
			return centroid/(float)point.Length;
		}
		
		//点 point 到线段 ( end, start) 的最短距离
		public static float CloseDistanceToSegment(Vector3 point, Vector3 start, Vector3 end){
			return ClosePointOnSegment(point, start, end).magnitude;
		}
		
		////点 point 到线段 ( end, start) 的最短距离,  个别情况下不准确，当需要判断距离点最近线段时理论上可以节省计算开销
		public static float QuickDisToSegment(Vector3 point, Vector3 start, Vector3 end){
			float l1,l2,l3, s, a;
			l1=(point-start).magnitude;
			l2=(point-end).magnitude;
			l3=(start-end).magnitude;
			
			if(l1>l3)
				return l2;
			
			if(l2>l3)
				return l1;
			
			s=(l1+l2+l3)/2f;
			a=Mathf.Sqrt(s*(s-l1)*(s-l2)*(s-l3));
			return (a/l3)*2;
		}
		
		//点 point 到线段 ( end, start) 的最近点
		public static Vector3 ClosePointOnSegment(Vector3 point, Vector3 start, Vector3 end){
			Vector3 p=start+ Vector3.Project(point-start, end-start);
			if(IsPointInSegmentBounding(p,start,end))
				return p;
			else
				return (point-start).sqrMagnitude < (point-end).sqrMagnitude ? start : end;
		}
		
		//判断点是否在线段的包围盒里
		public static bool IsPointInSegmentBounding(Vector3 point, Vector3 start, Vector3 end){
			return ((point.x-start.x)*(point.x-end.x)<=0 && (point.y-start.y)*(point.y-end.y)<=0 && (point.z -start.z)*(point.z-end.z)<=0);
		}
		
		
		
		//返回向量(point , start) 相对于向量 (end, start) 的方向 逆时针返回值 > 0, 顺时针返回值 < 0, 如果点point在直线上返回 0;
		//对于三维向量忽视 y轴
		public static float PointOfSide(Vector3 forward, Vector3 dir){
			return GetSide(forward.x,forward.z,dir.x,dir.z);
		}
		public static float PointOfSide(Vector3 point, Vector3 start, Vector3 end){
			Vector3 p=end-start, q=point-start;
			return GetSide(p.x, p.z, q.x, q.z);
		}
		public static float PointOfSide(Vector2 forward, Vector2 dir){
			return GetSide(forward.x,forward.y,dir.x,dir.y);
		}
		public static float PointOfSide(Vector2 point, Vector2 start, Vector2 end){
			Vector2 p=end-start, q=point-start;
			return GetSide(p.x, p.y, q.x, q.y);
		}
		private static float GetSide(float px, float py, float qx, float qy){
			return px*qy-qx*py;
		}
	}
}
