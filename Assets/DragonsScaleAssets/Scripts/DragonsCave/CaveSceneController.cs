using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PickableSpot
{
    public Transform pivot;
    public PickableInstance pickable;
}

public class CaveSceneController : MonoBehaviour
{
    [SerializeField] private List<PickableSpot> pickableSpots;
    [SerializeField] private Transform dragonPivot;
    private DragonController dragonObj;

    public static CaveSceneController Instance { get; private set; }
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        {
            Instance = this; 
        } 
    }
    void Start()
    {
        SetDragon();
        SetPickables();
    }
    

    public void SetPickables()
    {
        
    }

    public void SetDragon()
    {
        if (CaveManager.Instance.dragon.data.prefab != null)
        {
            dragonObj = Instantiate(CaveManager.Instance.dragon.data.prefab, dragonPivot);
        }
    }

    public void DespawnDragon()
    {
        Destroy(dragonObj.gameObject);
    }

    public void AddPickable(PickableInstance pickable)
    {
        List<PickableSpot> availableSpots = pickableSpots.FindAll((x) => x.pickable.pickablePrefab == null);
        PickableSpot spot = availableSpots[Random.Range(0, availableSpots.Count)];
        spot.pickable = pickable;
        Instantiate(pickable.pickablePrefab, spot.pivot);
    }

    public void RemovePickable(PickableInstance pickable)
    {
        
    }

    
}
