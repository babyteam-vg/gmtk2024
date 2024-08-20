using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragonController : MonoBehaviour
{
    public DragonInstance instanceData;
    [SerializeField] private AudioClip sleepSound;
    [SerializeField] private GameObject zSign;
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
        CaveManager.Instance.dragon.currentTime -= noiseValue;
    }

    private void Start()
    {
        AudioManager.Instance.PlayDragonSFX(sleepSound,false);
    }

    public void Update()
    {
        zSign.transform.localScale = new Vector3(1,1,0) * (CaveManager.Instance.dragon.currentTime/CaveManager.Instance.dragon.data.sleepTotalTime)+ Vector3.forward;
    }
}
