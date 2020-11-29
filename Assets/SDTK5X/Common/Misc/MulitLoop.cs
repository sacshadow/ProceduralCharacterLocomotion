using UnityEngine;
using System;
using System.Collections;

public class MulitLoop {
	
	public int xBegin =0, xEnd = 0;
	public int yBegin =0, yEnd = 0;
	
	public static MulitLoop Count(int xIndex, int yIndex) {
		return new MulitLoop(xIndex, yIndex);
	}
	
	public MulitLoop() {}
	public MulitLoop(int xEnd, int yEnd) {
		this.xEnd = xEnd;
		this.yEnd = yEnd;
	}
	
	public MulitLoop(int xBegin, int xEnd, int yBegin, int yEnd) {
		this.xBegin = xBegin;
		this.xEnd = xEnd;
		this.yBegin = yBegin;
		this.yEnd = yEnd;
	}
	
	
	public void Do(Action<int, int> Process) {
		for(int y = yBegin; y<yEnd; y++) {
			for(int x = xBegin; x<xEnd; x++) {
				Process(x,y);
			}
		}
	}
	
}
