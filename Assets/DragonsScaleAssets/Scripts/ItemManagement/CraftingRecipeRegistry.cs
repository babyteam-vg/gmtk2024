using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable, CreateAssetMenu]
public class CraftingRecipeRegistry: ScriptableObject
{
    public List<CraftingRecipe> recipes = new();

    [CanBeNull]
    public CraftingRecipe GetCompatibleRecipe(CraftingItemSelection selection)
    {
        foreach (CraftingRecipe recipe in recipes)
        {
            if (recipe.ingredientTL.IsCompatible(selection.ingredientTL) &&
                recipe.ingredientTC.IsCompatible(selection.ingredientTC) &&
                recipe.ingredientTR.IsCompatible(selection.ingredientTR) &&
                recipe.ingredientML.IsCompatible(selection.ingredientML) &&
                recipe.ingredientMC.IsCompatible(selection.ingredientMC) &&
                recipe.ingredientMR.IsCompatible(selection.ingredientMR) &&
                recipe.ingredientBL.IsCompatible(selection.ingredientBL) &&
                recipe.ingredientBC.IsCompatible(selection.ingredientBC) &&
                recipe.ingredientBR.IsCompatible(selection.ingredientBR))
            {
                return recipe;
            }
        }

        return null;
    }
}