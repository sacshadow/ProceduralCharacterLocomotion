using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class DataClass {
	public string className, classInert;
	
	public List<DataParam> dataParam;
	
	public DataClass(string classInfo) {
		var info = classInfo.Split(' ');
		className = info[0];
		if(info.Length > 1)
			classInert = info[1];
		else
			classInert = "GData";
		
		dataParam = new List<DataParam>();
	}
	
}

public class DataParam {
	public ParamInfo paramInfo;
	public string paramType, paramName;
	
	public virtual DataParam Set(ParamInfo paramInfo, string[] paramData) {
		this.paramInfo = paramInfo;
		paramType = paramData[paramInfo.offset + 0];
		paramName = paramData[paramInfo.offset + 1];
		return this;
	}
	
	public string DeclareParam() {
		return Replace(paramInfo.declare);
	}
	
	public string WriteData() {
		return Replace(paramInfo.write);
	}
	
	public virtual string ReadData() {
		return Replace(paramInfo.read);
	}
	
	protected string Replace(string format) {
		return format.Replace("#paramType", paramType).Replace("#paramName", paramName);
	}
	
}

public class DictParam : DataParam {
	public string id;
	
	public override DataParam Set(ParamInfo paramInfo, string[] paramData) {
		base.Set(paramInfo, paramData);
		id = paramData[paramInfo.offset + 2];
		return this;
	}
	
	public override string ReadData() {
		return Replace(paramInfo.read).Replace("#id",id);
	}
}

public class ParamInfo {
	
	public string declare, write, read;
	public int offset = 0;
	
	public ParamInfo(string declare, string write, string read, int offset = 0) {
		this.declare = declare;
		this.write = write;
		this.read = read;
		this.offset = offset;
	}
	
	public virtual DataParam Set(string[] paramData) {
		return new DataParam().Set(this, paramData);
	}
}

public class DictInfo : ParamInfo {
	
	public DictInfo(string declare, string write, string read) : base(declare, write, read,1) {
		
	}
	
	public override DataParam Set(string[] paramData) {
		return new DictParam().Set(this, paramData);
	}
}
