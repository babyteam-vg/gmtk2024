using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An item in-game.
/// </summary>
[Serializable]
public class Item
{
    public readonly ItemDescription Description;
    private readonly IReadOnlyList<Item> _craftedWith;

    public Item(ItemDescription description, IReadOnlyList<Item> craftedWith = null)
    {
        _craftedWith = craftedWith ?? new List<Item>();
        Description = description;
    }

    /// The value this item is sold for.
    public int MonetaryValue =>
        Description.baseMonetaryValue + _craftedWith.Sum(ingredient => ingredient.Description.ingredientValue);


    /// <summary>
    /// Gets the hash code.
    /// </summary>
    /// <returns>
    /// The hash code.
    /// </returns>
    public override int GetHashCode()
    { 
        if (_craftedWith == null)
        {
            return Description.baseId.GetHashCode();
        }
        string hashString = _craftedWith
            .OrderBy(item => item.Description.baseId)
            .Aggregate(Description.baseId, (current, item) => current + ";" + item.Description.baseId);

        return hashString.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is not Item) return false;

        return GetHashCode() == obj.GetHashCode();
    }

    public override string ToString()
    {
        return Description.baseId;
    }
}