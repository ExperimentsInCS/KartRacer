﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start() called");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.1f, 0.0f));
    }
}
