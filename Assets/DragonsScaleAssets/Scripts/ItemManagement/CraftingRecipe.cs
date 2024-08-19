using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

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
    [Tooltip("Item means it will check if the item is allowed. ItemType means it will check if the item type is allowed.")]
    public CraftingRecipeIngredientType type = CraftingRecipeIngredientType.Empty;
    [Space]
    public ItemDescription allowedItem;
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
public class CraftingRecipe: ScriptableObject
{
    public ItemDescription resultItem;

    [Header("Top Row")]
    [CanBeNull] public CraftingRecipeIngredient ingredientTL;
    [CanBeNull] public CraftingRecipeIngredient ingredientTM;
    [CanBeNull] public CraftingRecipeIngredient ingredientTR;
    
    [Header("Center Row")]
    [CanBeNull] public CraftingRecipeIngredient ingredientCL;
    [CanBeNull] public CraftingRecipeIngredient ingredientCM;
    [CanBeNull] public CraftingRecipeIngredient ingredientCR;
    
    [Header("Bottom Row")]
    [CanBeNull] public CraftingRecipeIngredient ingredientBL;
    [CanBeNull] public CraftingRecipeIngredient ingredientBM;
    [CanBeNull] public CraftingRecipeIngredient ingredientBR;
}