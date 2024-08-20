using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum CraftingRecipeIngredientType
{
    Empty,
    Item,
    ItemType
}

[Serializable]
public class CraftingRecipeIngredient
{
    [SerializeField]
    [Tooltip(
        "Item means it will check if the item is allowed. ItemType means it will check if the item type is allowed.")]
    public CraftingRecipeIngredientType type = CraftingRecipeIngredientType.Empty;

    [Space] public ItemDescription allowedItem;
    public ItemType allowedItemType;

    public bool IsCompatible(Item item)
    {
        if (item == null)
        {
            return type == CraftingRecipeIngredientType.Empty;
        }

        if (type == CraftingRecipeIngredientType.ItemType)
        {
            return allowedItemType == item.Description.itemType;
        }

        return allowedItem == item.Description;
    }
}


[Serializable]
public class CraftingConditionalResult
{
    public ItemDescription itemToCheck;
    public int minAmountOfItemToCheck = 1;
    public ItemDescription resultItem;
}

[Serializable, CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    [FormerlySerializedAs("resultItem")] public ItemDescription defaultResultItem;

    [Tooltip("When crafting, a special item can be crafted if it complied with any of the following conditions. The first one that matches will be used. If nothing matches, the default result will be used.")]
    public List<CraftingConditionalResult> conditionalResults = new();

    [Header("Top Row")]
    public CraftingRecipeIngredient ingredientTL;
    public CraftingRecipeIngredient ingredientTC;
    public CraftingRecipeIngredient ingredientTR;

    [Header("Center Row")]
    public CraftingRecipeIngredient ingredientML;
    public CraftingRecipeIngredient ingredientMC;
    public CraftingRecipeIngredient ingredientMR;

    [Header("Bottom Row")]
    public CraftingRecipeIngredient ingredientBL;
    public CraftingRecipeIngredient ingredientBC;
    public CraftingRecipeIngredient ingredientBR;


    /// <summary>
    /// Returns the result item for the given list of ingredients. Calling this functions assumes that the ingredients
    /// are enough to craft the result.
    /// </summary>
    /// <param name="ingredients"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Item GetResultFor(List<Item> ingredients)
    {
        Dictionary<ItemDescription, int> counts = new();
        foreach (Item item in ingredients)
        {
            if (!counts.TryAdd(item.Description, 1))
            {
                counts[item.Description]++;
            }
        }

        foreach (CraftingConditionalResult conditionalResult in conditionalResults)
        {
            if (conditionalResult.itemToCheck == null)
            {
                continue;
            }

            ItemDescription item = conditionalResult.itemToCheck;

            if (
                counts.TryGetValue(item, out int amount) &&
                amount >= conditionalResult.minAmountOfItemToCheck
            )
            {
                return new Item(conditionalResult.resultItem, ingredients);
            }
        }

        return new Item(defaultResultItem, ingredients);
    }
}