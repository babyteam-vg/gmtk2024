using System;
using System.Collections.Generic;


public struct ItemSlot
{
    public Item item;
    public  int amount;
}


/**
 * Manages the items that a player.
 */
[Serializable]
public class Inventory
{
    private readonly Dictionary<Item, int> _itemSlots = new();

    public event Action<ItemSlot> ItemValueChanged;

    public void AddItem(Item itemDescription, int amount = 1)
    {
        if (!_itemSlots.TryAdd(itemDescription, amount))
        {
            _itemSlots[itemDescription] += amount;
        }
    }

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
        ItemSlot slot = new()
        {
            item = item,
            amount = _itemSlots[item]
        };
        ItemValueChanged?.Invoke(slot);
    }
}