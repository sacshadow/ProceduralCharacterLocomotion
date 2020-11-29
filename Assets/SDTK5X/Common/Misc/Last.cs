using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public static class Last {

	public static T Of<T>(T[] array) {
		return array[array.Length-1];
	}
	public static T Of<T>(List<T> list) {
		return list[list.Count-1];
	}
}

public static class SecendLast {
	public static T Of<T>(T[] array) {
		return array[array.Length-2];
	}
	public static T Of<T>(List<T> list) {
		return list[list.Count-2];
	}
}