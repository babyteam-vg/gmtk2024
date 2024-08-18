using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragonController : MonoBehaviour
{
    public DragonInstance instanceData;
    [SerializeField] private MeshRenderer _renderer;
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
        instanceData.currentTime -= noiseValue;
    }
}
