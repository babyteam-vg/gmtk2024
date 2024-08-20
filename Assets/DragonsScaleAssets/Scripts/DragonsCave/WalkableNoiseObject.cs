using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class WalkableNoiseObject : MonoBehaviour
{

    public float defaultNoiseStrong;
    [SerializeField] private bool noiseOnStep;
    [SerializeField] private AudioClip noiseSound;
    public delegate void NoiseEvent(float strong);
    public static event NoiseEvent OnNoiseEvent;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out PlayerMovementController player))
        {
            player.stepEvent.AddListener(MakeNoiseOnStep);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent(out PlayerMovementController player))
        {
            player.stepEvent.RemoveListener(MakeNoiseOnStep);
        }
    }

    private void MakeNoiseOnStep(bool soft)
    {
        if (noiseOnStep)
        {
            MakeNoise(soft);
        }
    }
    
    public void MakeNoise(bool soft= false)
    {
        float value = defaultNoiseStrong;
        if (soft) value = value / 3;
            AudioManager.Instance.PlaySFX(noiseSound,value/defaultNoiseStrong);
        
        OnNoiseEvent?.Invoke(value);
    }
}
