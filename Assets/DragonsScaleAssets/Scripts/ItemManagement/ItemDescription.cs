using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;


/**
 * The type of an item.
 */
public enum ItemType
{
    Pickaxe,
    Shovel,
    BattleAxe,
    Sword,
    Shield,
}


/**
 * An item in-game.
 */
[Serializable]
public class ItemDescription : ScriptableObject
{
    /**
     * ID used to identify this item.
     */
    public string baseId = "";

    /**
     * The item type as requested by a customer ("I want a pickaxe!").
     */
    public ItemType itemType = ItemType.Pickaxe;

    /**
     * The name of the item as represented in-game.
     */
    public string displayName;

    /**
     * The base value this item can be sold for. Can be 0.
     */
    public int baseMonetaryValue;

    /**
     * The bonus monetary value this item gives to a crafted item when used as an ingredient.
     */
    public int ingredientValue = 0;

    /**
     * A dictionary with all the items in the game by ID.
     */
    public static Dictionary<string, ItemDescription> KnownItems = new();


    public ItemDescription()
    {
        if (baseId == "")
        {
            throw new Exception("Item must have an ID");
        }

        if (!KnownItems.ContainsKey(baseId))
        {
            KnownItems.Add(baseId, this);      
        }
    }

    [CanBeNull]
    public ItemDescription GetItemWithId(string itemId)
    {
        return KnownItems.GetValueOrDefault(itemId);
    }

}
