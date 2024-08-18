using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPileController : MonoBehaviour
{
    [SerializeField] private float maxHight;
    [SerializeField] private float divisorForScale;
    [SerializeField] private float pow;
    
    void Start()
    {
        this.transform.position = Vector3.up*MapValue(GameManager.Instance.playerData.coins/divisorForScale)*maxHight;
    }
    
    float MapValue(float x)
    {
        if (x > 1)
        {
            return 1;
        }
        else if (x < 0)
        {
            return 0;
        }
        else
        {
            return 1 - Mathf.Pow(1 - x, pow);
        }
        
    }
    
}
