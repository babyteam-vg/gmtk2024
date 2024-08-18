using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// An item in-game.
/// </summary>
[Serializable]
public class Item
{
    public readonly ItemDescription Description;
    private readonly IReadOnlyList<Item> _craftedWith;

    public Item(ItemDescription description, IReadOnlyList<Item> craftedWith)
    {
        Description = description;
        _craftedWith = craftedWith;
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
        string hashString = _craftedWith
            .OrderBy(item => item.Description.baseId).ToList()
            .Aggregate(Description.baseId, (current, item) => current + (";" + item.Description.baseId));

        return hashString.GetHashCode();
    }
}