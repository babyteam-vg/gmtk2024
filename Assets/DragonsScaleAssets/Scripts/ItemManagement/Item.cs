using System;
using System.Collections.Generic;
using System.Linq;

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

    /**
     * The value this item is sold for.
     */
    public int MonetaryValue =>
        Description.baseMonetaryValue + _craftedWith.Sum(ingredient => ingredient.Description.ingredientValue);
}