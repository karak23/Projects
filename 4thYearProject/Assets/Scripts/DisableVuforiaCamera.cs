﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class DisableVuforiaCamera : MonoBehaviour {

    public Camera camera;

    // Use this for initialization
    void Start()
    {
        if (camera.enabled)
            if (camera.GetComponent<VuforiaBehaviour>() != null)
                camera.GetComponent<VuforiaBehaviour>().enabled = false;
    }
}
