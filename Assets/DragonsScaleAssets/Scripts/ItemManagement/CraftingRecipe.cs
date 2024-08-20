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

[Serializable, CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public ItemDescription resultItem;

    [Header("Top Row")] public CraftingRecipeIngredient ingredientTL;

    [FormerlySerializedAs("ingredientTM")] [CanBeNull]
    public CraftingRecipeIngredient ingredientTC;

    public CraftingRecipeIngredient ingredientTR;

    [FormerlySerializedAs("ingredientCL")] [Header("Center Row")]
    public CraftingRecipeIngredient ingredientML;

    [FormerlySerializedAs("ingredientCM")] public CraftingRecipeIngredient ingredientMC;
    [FormerlySerializedAs("ingredientCR")] public CraftingRecipeIngredient ingredientMR;

    [Header("Bottom Row")] public CraftingRecipeIngredient ingredientBL;
    [FormerlySerializedAs("ingredientBM")] public CraftingRecipeIngredient ingredientBC;
    public CraftingRecipeIngredient ingredientBR;
}