using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicakbleObject : MonoBehaviour
{
    public int currentIndex;
    [SerializeField] private ItemDescription _itemDescription;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void PickObject()
    {
        GameManager.Instance.playerData.inventory.AddItem(new Item(_itemDescription),1);
        //Remove del CaveScene
        //Remove del Cave
        Destroy(this.gameObject);
    }

    
}
