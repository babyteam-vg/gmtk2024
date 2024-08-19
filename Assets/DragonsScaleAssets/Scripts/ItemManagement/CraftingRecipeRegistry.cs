using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable, CreateAssetMenu]
public class CraftingRecipeRegistry: ScriptableObject
{
    public List<CraftingRecipe> recipes = new();

    [CanBeNull]
    public CraftingRecipe GetCompatibleRecipe(
        [CanBeNull] Item itemTL,
        [CanBeNull] Item itemTM,
        [CanBeNull] Item itemTR,
        [CanBeNull] Item itemCL,
        [CanBeNull] Item itemCM,
        [CanBeNull] Item itemCR,
        [CanBeNull] Item itemBL,
        [CanBeNull] Item itemBM,
        [CanBeNull] Item itemBR
        )
    {
        foreach (CraftingRecipe recipe in recipes)
        {
            if (recipe.ingredientTL.IsCompatible(itemTL) &&
                recipe.ingredientTM.IsCompatible(itemTM) &&
                recipe.ingredientTR.IsCompatible(itemTR) &&
                recipe.ingredientCL.IsCompatible(itemCL) &&
                recipe.ingredientCM.IsCompatible(itemCM) &&
                recipe.ingredientCR.IsCompatible(itemCR) &&
                recipe.ingredientBL.IsCompatible(itemBL) &&
                recipe.ingredientBM.IsCompatible(itemBM) &&
                recipe.ingredientBR.IsCompatible(itemBR))
            {
                return recipe;
            }
        }

        return null;
    }
}