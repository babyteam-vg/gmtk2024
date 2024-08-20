using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PickableInstance
{
    public PicakbleObject pickablePrefab;
    public int index;
}
[Serializable]
public class DragonInstance
{
    public DragonData data;
    public float currentTime;
}

[Serializable]
public class DragonInLevel
{
    public DragonData dragonData;
    public int weight;
}
[Serializable]
public class DragonChooser
{
    public List<DragonInLevel> dragonsInLevel = new List<DragonInLevel>();
    public DragonData GetDragon()
    {
        int randomIndex = Random.Range(1, 101);
        int sumDragWeight =0;
        foreach (DragonInLevel dragon in dragonsInLevel)
        {
            sumDragWeight += dragon.weight;
            if (randomIndex <= sumDragWeight)
            {
                return dragon.dragonData;
            }
        }
        return null;
    }
}

public class CaveManager : MonoBehaviour
{
    public static CaveManager Instance { get; private set; }
    [SerializeField] private List<DragonChooser> dragonsDatasByLevel =new List<DragonChooser>();
    [SerializeField] private List<PicakbleObject> pickablesPrefabs =new List<PicakbleObject>();
    
    public DragonInstance dragon =null;
    [SerializeField] private float timeToSpawnDragon;
    [SerializeField] private float margenTimeToSpawnDragon;
    public float currentTimeDragonSpawner;
    
    public List<PickableInstance> pickableInstances =new List<PickableInstance>();
    private int currentIndex;
    [SerializeField] private int totalRecogibles;
    [SerializeField] private float timeToSpawnPickable;
    private float currentTimePickableSpawner;

    [SerializeField] private AudioClip dragonArriveSound;
    [SerializeField] private AudioClip dragonExitSound;
    
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            DontDestroyOnLoad(this);
            Instance = this; 
        } 
    }

    private void Start()
    {
        currentTimeDragonSpawner = timeToSpawnDragon;
        currentTimePickableSpawner = timeToSpawnPickable;
    }

    private void Update()
    {
       if(dragon.data != null)
        {
            dragon.currentTime -= Time.deltaTime;
            if (dragon.currentTime <= 0)
            {
                DespawnDragon();
            }
        }
       else
       {
           currentTimeDragonSpawner -= Time.deltaTime;
           
           if (currentTimeDragonSpawner <= 0)
           {
               SpawnRandomDragon();
           }
          
       }

        if (currentTimePickableSpawner >= 0)
        {
            currentTimePickableSpawner -= Time.deltaTime;
        }
        else
        {
            currentTimePickableSpawner = timeToSpawnPickable;
            SpawnRandomPickable();
            
        }
    }

    private void SpawnRandomPickable()
    {
        if (totalRecogibles > pickableInstances.Count)
        {
            PickableInstance instance = new PickableInstance();
            instance.pickablePrefab = pickablesPrefabs[Random.Range(0, pickablesPrefabs.Count)];
            currentIndex += 1;
            instance.index = currentIndex;
            pickableInstances.Add(instance);
            if (CaveSceneController.Instance != null) CaveSceneController.Instance.SpawnPickable(instance);
        }
    }

    private void DespawnPickable(PickableInstance instance)
    {
        PickableInstance x = pickableInstances.FirstOrDefault((x) => x.index == instance.index);
        if(x!= null) pickableInstances.Remove(x);
    }
    
    private void SpawnRandomDragon()
    {
        
        dragon.data = dragonsDatasByLevel[GameManager.Instance.GetCurrentLevel()].GetDragon();
        dragon.currentTime = dragon.data.sleepTotalTime;
        AudioManager.Instance.PlayDragonSFX(dragonArriveSound,true);
        
        if (CaveSceneController.Instance != null)
        {
            CaveSceneController.Instance.SetDragon();
            PlayerDiesByDragon();
        }
    }

    private void DespawnDragon()
    {
        if (CaveSceneController.Instance != null)
        {
            CaveSceneController.Instance.DespawnDragon();
            PlayerDiesByDragon();
        }

        dragon.data = null;
        currentTimeDragonSpawner = Random.Range(timeToSpawnDragon - margenTimeToSpawnDragon, timeToSpawnDragon + margenTimeToSpawnDragon);
        AudioManager.Instance.PlayDragonSFX(dragonExitSound,true);
    }

    public void PlayerDiesByDragon()
    {
        
    }
}
