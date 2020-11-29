using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;
namespace UnityLogic {
	public class LogicPanel : LogicObject {
		public class PTag {
			public string tagName;
			public Tag tag;
			
			public PTag(string tagName, Tag tag) {
				this.tagName = tagName;
				this.tag = tag;
			}
		}
		
		public Env env;
		
		public List<PTag> panelTag;
		
		public LogicPanel(Env env) {
			this.env = env;
			this.panelTag = new List<PTag>();
			this.panelTag.Add(new PTag("标签1", new Tag("标签1")));
		}
		
		
	}
}
