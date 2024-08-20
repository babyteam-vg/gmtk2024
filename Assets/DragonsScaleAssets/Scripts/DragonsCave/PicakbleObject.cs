using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicakbleObject : MonoBehaviour
{
    public int pivotID;
    [SerializeField] private ItemDescription _itemDescription;
    [SerializeField] private bool noiseOnPick;

    // Update is called once per frame
    public void PickObject()
    {
        GameManager.Instance.playerData.inventory.AddItem(new Item(_itemDescription),1);
        if (transform.TryGetComponent(out WalkableNoiseObject noise) && noiseOnPick)
        {
            noise.MakeNoise(true);
        }
        CaveSceneController.Instance.RemovePickable(pivotID);
        //Remove del Cave
        Destroy(this.gameObject);
    }

    
}
