/*
	
	多边形三角化
	一个可以改进的点
	一个bug 暂时不处理
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SDTK.Geometry{
	
	//链表
	public class LinkList{
		public Vector3 position;//位置
		public int index;//序号
		public LinkList previous;//上一个节点
		public LinkList next;//下一个节点
		
		public int NextCount{//向下数个数
			get{
				if(null != next)
					return next.NextCount+1;
				return 0;
			}
		}
		
		public LinkList LastNode{//最后一个节点
			get{
				if(null != next)
					return next.LastNode;
				return this;
			}
		}
		
		public int PreviousCount{//向上数个数
			get{
				if(null !=previous)
					return previous.PreviousCount+1;
				return 0;
			}
		}
		
		public LinkList(Vector3 position, int index){//实例化
			this.position=position;
			this.index=index;
		}
		
		public LinkList CopyLink(){//复制一个节点
			LinkList rt=new LinkList(position, index);
			rt.previous=previous;
			rt.next=next;
			return rt;
		}
		
		public LinkList GetIndexNode(int i){//获取index 为 i的节点
			if(index == i)
				return this;
			else if(next ==null)
				return null;
			else 
				return next.GetIndexNode(i);
		}
	}
	
	//对简单多边形进行三角化
	public class Triangulation {
		
		private static List<int> result;//结果
		
		//三角化多边形
		//point以逆时针顺序提供
		public static int[] TriangulationPolygon(Vector3[] point){
			result=new List<int>();
			
			if(null == point || point.Length<2){//小于4个点,无法计算
				Debug.LogError("Data is Illegal!");
				return null;
			}
			
			if(point.Length==3){
				return new int[]{0,1,2};
			}
			
			GetTriangle(CreateLinkList(point));//递归调用GetTriangle
			
			return result.ToArray();
		}
		
		//分解三角形
		private static void GetTriangle(LinkList root){
			int i=root.NextCount;
			int n,m;
			if(i==2){//只有2条边， 直接返回这个三角形
				result.Add(root.next.index);
				result.Add(root.next.next.index);
				result.Add(root.index);

				return;
			}
			
			List<int> sort=SortLinkPoint(root);//按空间排序以保证获得凸多边形顶点
			LinkList startPoint=root,node;
			
			while(i >=2){
				n=sort[0];
				node=startPoint.GetIndexNode(n);//计算凸点
				if(null == node)
					Debug.LogError("index "+ n +" not find");
				sort.RemoveAt(0);
				
				m=Process(startPoint, node);
				if(m>=0){//Todo: 减少排序次数， 可以直接从第一次排序中提取出分开成两个列表的排序而不需要重新排序； 期望下次有时间解决
					LinkList org=root;
					LinkList newList=SeparateLinkList(org,Mathf.Min(m,n),Mathf.Max(m,n));
					GetTriangle(org);
					GetTriangle(newList);
					
					return;
				}
				else if(node==startPoint){//如果 node是第一个元素， 踢出， start现在是第二个元素
					startPoint=node.next;
					startPoint.previous=null;
				}
				
				i--;
			}
		}
		
		//p 是否在射线夹角 (p2,p1, p2, p3)之外
		private static bool IsPointOutofAngle(LinkList p, LinkList p1, LinkList p2, LinkList p3){
			return Measure.PointOfSide(p.position,p1.position,p2.position)>0 && Measure.PointOfSide(p.position, p2.position, p3.position) >0;
		}
		
		//处理生成三角形, 如果成功返回-1， 否则返回失败的点
		private static int Process(LinkList startPoint, LinkList node){
			LinkList p1, p2, t1, t2, stp;
			
			p1= (null != node.previous?node.previous: node.LastNode);//上一个点
			p2= (null != node.next?node.next: startPoint);//下一个点
			stp=p2.next;
			if(stp==null)
				stp=startPoint;
			
			if(startPoint.NextCount==2){//如果只有3个点,返回201
				result.Add(p2.index);
				result.Add(p1.index);
				result.Add(node.index);
				return -1;
			}
			
			t1=(null != p1.previous?p1.previous: node.LastNode);
			
			if(IsPointOutofAngle(t1,node,p1,p2))//特殊检测1
				return t1.index;
			
			while(t1!=stp){//检测线段相交
				t2=t1;
				t1=(null != t2.previous?t2.previous: node.LastNode);
				
				if(Geometry.GTools.CrossLineTest(t1.position,t2.position,p1.position,p2.position))//Todo: 应该遍历后返回第一个不相交线段, 因为暂时假设没有那么复杂的多边形, 只返回第一个并假设是不相交的
					return t1.index;
			}
			
			if(IsPointOutofAngle(t1,p1,p2,node))//特殊检测2
				return t1.index;
			
			//通过检测, 赋值201
			result.Add(p2.index);
			result.Add(p1.index);
			result.Add(node.index);
		
			p1.next=(null !=node.previous?node.next:null);
			p2.previous=(null !=node.next?node.previous:null);
			//~ Debug.Log("Push "+p1.index+" "+p2.index+"out "+node.index);
			return -1;
		}
		
		private static LinkList curentList;
		
		//排序,确保每次处理的都是凸点
		private static List<int> SortLinkPoint(LinkList root){
			List<int> sort=new List<int>();
			curentList=root;
			LinkList node=root;
			
			while(node!=null){
				sort.Add(node.index);
				node=node.next;
			}
			
			sort.Sort(CompareLink);
			string s="";
			for(int i=0; i<sort.Count; i++){
				s+=sort[i].ToString()+" ";
			}
			Debug.Log(s);
			return sort;
		}
		
		//比对委托函数
		private static int CompareLink(int i, int j){
			Vector3 v=curentList.GetIndexNode(i).position-curentList.GetIndexNode(j).position;
			
			if(v.z>0)
				return -1;
			if(v.z==0 && v.x>0)
				return -1;
			
			return 1;
		}
		
		//生成链表
		private static LinkList CreateLinkList(Vector3[] point){
			LinkList root=new LinkList(point[0],0);
			LinkList last=root,node;
			for(int i=1; i<point.Length; i++){
				node=new LinkList(point[i], i);
				last.next=node;
				node.previous=last;
				last=node;
			}
			return root;
		}
		
		//获取链表上的指定节点 i
		private static LinkList GetNode(LinkList root, int i){
			LinkList node=root;
			int index=0;
			while(index<i && node !=null){
				node=node.next;
				index++;
			}
			
			return node;
		}
		
		
		//debug用
		//~ private static string ShowIndex(LinkList root){
			//~ string rt=root.index.ToString()+"; ";
			//~ LinkList next=root.next;
			//~ while(next!=null){
				//~ rt+=next.index.ToString()+"; ";
				//~ next=next.next;
			//~ }
			
			//~ return rt;
			
		//~ }
		
		//断开原始链表为两个新链表
		//-----i---j----- next->
		//-----i.
		//     .i---j.
		//          .j-----next->
		//-----ij----- //new old
		//i---j//rt list
		private static LinkList SeparateLinkList(LinkList root, int i, int j){
			//~ Debug.Log(ShowIndex(root)+"; "+i+" "+j);
			LinkList breakPoint1=root.GetIndexNode(i);
			LinkList separateList=breakPoint1.CopyLink();
			LinkList breakPoint2=root.GetIndexNode(j);
			LinkList newLast=breakPoint2.CopyLink();
			
			breakPoint1.next=breakPoint2;
			breakPoint2.previous=breakPoint1;
			
			separateList.previous=null;
			newLast.previous.next=newLast;
			newLast.next=null;
			
			//~ Debug.Log(ShowIndex(root)+"; "+ShowIndex(separateList));
			
			return separateList;
		}
		
	}
}
