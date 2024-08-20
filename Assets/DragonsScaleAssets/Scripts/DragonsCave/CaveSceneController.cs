using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PickableSpot
{
    public int id;
    public Transform pivot;
    public bool used;
    public PickableInstance pickable;
}

public class CaveSceneController : MonoBehaviour
{
    [SerializeField] private List<PickableSpot> pickableSpots;
    [SerializeField] private Transform dragonPivot;
    private DragonController dragonObj;
    [SerializeField] private GameObject dragonSign;

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
        SetPickables();
        SetDragon();
    }

    private void Update()
    {
        if (CaveManager.Instance.dragon.data == null && CaveManager.Instance.currentTimeDragonSpawner < 10f)
        {
            dragonSign.SetActive(true);
        }
        else
        {
            dragonSign.SetActive(false);
        }
    }


    public void SetPickables()
    {
        foreach (PickableInstance pickableInstance in CaveManager.Instance.pickableInstances)
        {
            SpawnPickable(pickableInstance);
        }
    }

    public void SetDragon()
    {
        if (CaveManager.Instance.dragon.data != null)
        {
            dragonObj = Instantiate(CaveManager.Instance.dragon.data.prefab, dragonPivot);
        }
    }

    public void DespawnDragon()
    {
        AudioManager.Instance.StopDragonSFX();
        // MUERES
        Destroy(dragonObj.gameObject);
    }


    public void SpawnPickable(PickableInstance pickable)
    {
        List<PickableSpot> availableSpots = pickableSpots.FindAll((x) => x.used == false);
        PickableSpot spot = availableSpots[Random.Range(0, availableSpots.Count)];
        spot.pickable = pickable;
        spot.id = pickable.index;
        spot.used = true;
        PicakbleObject obj = Instantiate(pickable.pickablePrefab, spot.pivot);
        obj.pivotID = pickable.index;
    }

    public void RemovePickable(int pivotID)
    {
        PickableSpot spot = pickableSpots.FirstOrDefault((x) => x.id == pivotID);
        PickableInstance instance = CaveManager.Instance.pickableInstances.Find((x) => x.index == spot.id);
        spot.pickable = null;
        spot.id =-1;
        spot.used =false;
        CaveManager.Instance.pickableInstances.Remove(instance);
    }

    
}
