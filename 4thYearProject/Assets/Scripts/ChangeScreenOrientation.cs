﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScreenOrientation : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Screen.orientation = ScreenOrientation.Landscape;
    }
}
