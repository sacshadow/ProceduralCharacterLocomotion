﻿using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HC_WJDR : HAIBase {
	
	protected override FightStyleBase InitUnarmedFightStyle() {
		return new FS_YinYangZhang();
	}
	
}
