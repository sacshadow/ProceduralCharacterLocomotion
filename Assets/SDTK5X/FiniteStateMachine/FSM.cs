using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

namespace SDTK {

	public class FSM<T> {
		
		public Action<T, Action>[] Process;
		public int point {get {return crtPoint; }}
		
		public bool isDone {get {return crtPoint >= Process.Length; }}
		
		protected int crtPoint = 0;
		
		public static Action<T, Action> WaitFor(FSM<T> otherProcess) {
			return (data, Next) => {if(otherProcess.Update(data)) Next();};
		}
		
		public void Push(params Action<T, Action>[] Process) {
			this.Process = Process;
		}
		
		public void Reset() {
			crtPoint = 0;
		}
		
		public void Next() {
			crtPoint++;
		}
		
		public bool Update(T data) {
			if(!isDone)
				Process[crtPoint](data, Next);
			return isDone;
		}
		
	}
}
