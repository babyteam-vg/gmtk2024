using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public abstract class Item: ScriptableObject
{
    
}
*/

[Serializable]
public class PlayerData
{

    public int coins;
    public Inventory inventory;
}



public class GameManager: MonoBehaviour
{

    public PlayerData playerData;
    public static GameManager Instance { get; private set; }

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            DontDestroyOnLoad(this.gameObject);
            Instance = this; 
        } 
    }


    private void Start()
    {
        //LoadData();
    }


    public void SaveData()
    {
        string dataToSave = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("key", dataToSave);
        PlayerPrefs.Save();
    }
    public void LoadData()
    {
        if (PlayerPrefs.HasKey("key"))
        {
            string dataloaded = PlayerPrefs.GetString("key");
            playerData = JsonUtility.FromJson<PlayerData>(dataloaded);
        }
        else
        {
            playerData = new PlayerData();
            SaveData();
        }
    }

    public int GetCurrentLevel()
    {
        if (playerData.coins>= 100000)
        {
            return 4;
        }
        else if (playerData.coins>= 25000)
        {
            return 3;
        }
        else if (playerData.coins>= 5000)
        {
            return 2;
        }
        else if (playerData.coins>= 1000)
        {
            return 1;
        }
        else
        {
            return 0;
        }
        
    }
}
