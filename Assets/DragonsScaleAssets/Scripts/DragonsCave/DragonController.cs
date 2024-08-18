using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public float dragonTotalTime;
    public float currentTimer;

    void OnEnable()
    {
        WalkableNoiseObject.OnNoiseEvent += OnNoiseEventHandle;
    }

    void OnDisable()
    {
        WalkableNoiseObject.OnNoiseEvent -= OnNoiseEventHandle;
    }

    void OnNoiseEventHandle(float noiseValue)
    {
        
    }
}
