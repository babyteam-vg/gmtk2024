using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
        EmitItemChange(item);
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
    /// <param name="item">
    /// The item to take.
    /// </param>
    /// <param name="amount">
    /// How many items to take.
    /// </param>
    /// <returns>
    /// True if the item was taken.
    /// </returns>
    public bool SpendItem(Item item, int amount = 1)
    {
        if (!_itemSlots.TryGetValue(item, out int slot))
        {
            return false;
        }

        if (slot < amount)
        {
            return false;
        }

        _itemSlots[item] -= amount;

        if (_itemSlots[item] <= 0)
        {
            _itemSlots.Remove(item);
        }

        EmitItemChange(item);

        return true;
    }

    /// <summary>
    /// Checks if an item is in the inventory.
    /// </summary>
    /// <param name="item">
    /// The item to check.
    /// </param>
    /// <param name="minAmount">
    /// The minimum amount of the item to check for.
    /// </param>
    /// <returns>
    /// True if the item is in the inventory.
    /// </returns>
    private bool HasItem(Item item, int minAmount = 1)
    {
        return _itemSlots.GetValueOrDefault(item, 0) >= minAmount;
    }

    /// <summary>
    /// Clears the inventory.
    /// </summary>
    public void Clear()
    {
        foreach (Item item in ListItems())
        {
            _itemSlots[item] = 0;
            EmitItemChange(item);
        }
        _itemSlots.Clear();
    }

    /// <summary>
    /// Lists all items in the inventory.
    /// </summary>
    /// <returns>
    /// All items in the inventory.
    /// </returns>
    public List<Item> ListItems()
    {
        return new List<Item>(_itemSlots.Keys);
    }

    /// <summary>
    /// Lists all items in the inventory with their amounts.
    /// </summary>
    /// <returns>
    /// All items in the inventory.
    /// </returns>
    public IEnumerable<ItemCount> ListItemsCount()
    {
        return _itemSlots.Select(item => new ItemCount
        {
            Item = item.Key,
            Amount = item.Value
        });
    }

    /// <summary>
    /// Emits a change in the amount of an item in the inventory.
    /// </summary>
    /// <param name="item">
    /// The item to change.
    /// </param>
    private void EmitItemChange(Item item)
    {
        int amount = _itemSlots.GetValueOrDefault(item, 0);
        ItemCount count = new()
        {
            Item = item,
            Amount = amount
        };
        ItemValueChanged?.Invoke(count);
    }

    /// <summary>
    /// If all of the ingredients are present in the inventory, they will be removed and  the result item will
    /// be added to the inventory.
    ///
    /// Warning: Do not attempt to transmuate humans.
    /// </summary>
    /// <param name="ingredients">
    /// The ingredients to transmute.
    /// </param>
    /// <param name="result">
    /// The result of the transmutation.
    /// </param>
    /// <returns></returns>
    public bool Transmutate(
        List<Item> ingredients,
        Item result
    )
    {
        Dictionary<Item, int> ingredientsCount = new();
        foreach (Item item in ingredients)
        {
            ingredientsCount.TryAdd(item, 0);
            ingredientsCount[item] += 1;
        }

        foreach (Item item in ingredientsCount.Keys)
        {
            if (!HasItem(item, ingredientsCount[item]))
            {
                return false;
            }
        }

        foreach (Item item in ingredientsCount.Keys)
        {
            SpendItem(item, ingredientsCount[item]);
        }

        AddItem(result);
        return true;
    }
}