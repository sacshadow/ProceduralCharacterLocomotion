/*
	几何工具集合
	
*/

using UnityEngine;
using System.Collections;

namespace SDTK.Geometry{
	public static class GTools {//几何
		//线段相交状态: 相交, 不相交, 平行, 共线
		public enum IntersectState{INTERSECT=0, NOTINTERSECT, PARALLEL, COLLINEAR }
		
		//交点信息
		public class IntersectionPoint{
			public IntersectState state;
			public Vector2 crossPoint;
			
			public IntersectionPoint(){
				state=IntersectState.NOTINTERSECT;
			}
		}
		
		//检测在同一水平面上的两条线段是否相交
		//segment 1 (p1, p2), segment 2 (q1, q2)
		public static bool CrossLineTest(Vector3 p1, Vector3 p2, Vector3 q1, Vector3 q2){
			return CrossLineTest(new Vector2(p1.x,p1.z),new Vector2(p2.x,p2.z),new Vector2(q1.x,q1.z),new Vector2(q2.x,q2.z));
		}
		public static bool CrossLineTest(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2){
			AABB b1=new AABB(new Vector2[]{p1,p2}), b2=new AABB(new Vector2[]{q1,q2});
			float c1,c2,c3,c4;
			
			if(!AABB.IsOverlap(b1,b2))
				return false;
			
			c1=Measure.PointOfSide(p1,q1,q2);
			c2=Measure.PointOfSide(p2,q1,q2);
			c3=Measure.PointOfSide(q1,p1,p2);
			c4=Measure.PointOfSide(q2,p1,p2);
			
			if(c1*c2<0 && c3*c4<0)
				return true;
			
			if((c1==0 && c2 ==0) || (c3==0 && c4==0))
				Debug.LogError("Collinear Line Exist");
			
			return false;
		}
		
		//返回两条线段的交点, 以及他们是否相交 平行 或者共线
		//segment 1 (p1, p2), segment 2 (q1, q2)
		public static IntersectionPoint GetLineIntersection(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2){
			float r,s, n1,n2,d;
			float a=p2.x-p1.x, b=p2.y-p1.y;
			float x=q2.x-q1.x, y=q2.y-q1.y;
			float m=p1.y-q1.y,n=p1.x-q1.x;
			IntersectionPoint intersectionPoint=new IntersectionPoint();
			
			n1=m*x-n*y;
			d=a*y-b*x;
			
			if(d == 0){
				intersectionPoint.state=IntersectState.PARALLEL;
				if(n1 ==0)
					intersectionPoint.state=IntersectState.COLLINEAR;
				
				return intersectionPoint;
			}
			
			n2=m*a-n*b;
			
			r=n1/d;
			s=n2/d;
			
			if(r>0 && s>0 && r<1 && s<1)
				intersectionPoint.state=IntersectState.INTERSECT;
			
			intersectionPoint.crossPoint=p1+(p2-p1)*r;
			return intersectionPoint;
		}
		
		//返回线段在(x,z) 平面上的垂线
		public static Vector3 GetPerpendicular(Vector3 dir){
			return new Vector3(-dir.z,dir.y,dir.x);
		}
		public static Vector2 GetPerpendicular(Vector2 dir){
			return new Vector2(-dir.y,dir.x);
		}
		
		//折线偏移
		//isClose 是否封闭
		//如果封闭==ture  则 认为头尾相连
		public static Vector3[] GetOffsetShape(Vector3[] node, float width, bool isClose = false){
			Vector3[] rt;
			int n=isClose?node.Length: node.Length-1;
			int seg=node.Length;
			int i=isClose?0:1;
			
			if(isClose && n < 3)
				throw new System.Exception("Illegal Array");
			
			if(!isClose && n<1)
				throw new System.Exception("Illegal Array");
			
			rt=new Vector3[node.Length];
			
			for(; i<n; i++){
				rt[i]=GetOffsetPoint(node[i], node[(i+1)%seg],node[(i-1+seg)%seg],width);
			}
			
			if(isClose)
				return rt;
			
			rt[0]=node[0]+GetPerpendicular(node[1]-node[0]).normalized*width;
			rt[n]=node[n]+GetPerpendicular(node[n]-node[n-1]).normalized*width;
			
			return rt;
		}
		
		//折线点偏移
		public static Vector3 GetOffsetPoint(Vector3 org, Vector3 p1, Vector3 p2, float width) {
			Vector3 l1=(p1-org).normalized, l2=(p2-org).normalized;
			Vector3 nor=(l1+l2).normalized;
			
			float dis;
			
			if(Mathf.Approximately(nor.sqrMagnitude,0)){//if angle aob is almost 180 d , use the normal of line1 
				nor=new Vector3(-l1.z,0,l1.x).normalized;
			}
			
			dis=IntersectionDis(nor,l1,width);

			if(Measure.PointOfSide(p2, org, p1)>-0.00001f)// as >=0, 防止浮点精度导致的错误
				return org+nor*dis;
			else
				return org+nor*-dis;
		}
		private static float IntersectionDis(Vector3 nor,Vector3 dir, float width){//查找偏移点距中心距离
			float angle=Vector3.Angle(nor,dir);
			return width/(Mathf.Sin(angle*Mathf.Deg2Rad));
		}
		
	}
}







