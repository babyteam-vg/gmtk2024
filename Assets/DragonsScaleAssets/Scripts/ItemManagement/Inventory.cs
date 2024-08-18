using System;
using System.Collections.Generic;


public struct ItemCount
{
    public Item Item;
    public int Amount;
}


/// <summary>
/// A storage for items.
/// </summary>
[Serializable]
public class Inventory
{
    private readonly Dictionary<Item, int> _itemSlots = new();

    public event Action<ItemCount> ItemValueChanged;

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">
    /// The item to add.
    /// </param>
    /// <param name="amount">
    /// The amount of the item to add.
    /// </param>
    public void AddItem(Item item, int amount = 1)
    {
        if (!_itemSlots.TryAdd(item, amount))
        {
            _itemSlots[item] += amount;
        }
    }

    /// <summary>
    /// Counts how many of an item are in the inventory.
    /// </summary>
    /// <param name="item">
    /// The item to count.
    /// </param>
    public int CountItem(Item item)
    {
        return _itemSlots.GetValueOrDefault(item, 0);
    }

    /// <summary>
    /// Takes an item from the inventory if there is enough.
    /// </summary>
    /// <param name="itemDescription">
    /// The item to take.
    /// </param>
    /// <param name="amount">
    /// How many items to take.
    /// </param>
    /// <returns>
    /// True if the item was taken.
    /// </returns>
    public bool SpendItem(Item itemDescription, int amount = 1)
    {
        if (!_itemSlots.TryGetValue(itemDescription, out int slot))
        {
            return false;
        }

        if (slot < amount)
        {
            return false;
        }

        _itemSlots[itemDescription] -= amount;

        if (_itemSlots[itemDescription] <= 0)
        {
            _itemSlots.Remove(itemDescription);
        }

        return true;
    }

    public void Clear()
    {
        _itemSlots.Clear();
    }

    public List<Item> ListItems()
    {
        return new List<Item>(_itemSlots.Keys);
    }

    private void EmitItemChange(Item item)
    {
        ItemCount count = new()
        {
            Item = item,
            Amount = _itemSlots[item]
        };
        ItemValueChanged?.Invoke(count);
    }
}