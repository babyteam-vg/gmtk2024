using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class CraftingItemSelection
{
    [CanBeNull] public readonly Item ingredientTL;
    [CanBeNull] public readonly Item ingredientTC;
    [CanBeNull] public readonly Item ingredientTR;

    [CanBeNull] public readonly Item ingredientML;
    [CanBeNull] public readonly Item ingredientMC;
    [CanBeNull] public readonly Item ingredientMR;

    [CanBeNull] public readonly Item ingredientBL;
    [CanBeNull] public readonly Item ingredientBC;
    [CanBeNull] public readonly Item ingredientBR;

    public CraftingItemSelection(
        [CanBeNull] Item ingredientTL,
        [CanBeNull] Item ingredientTC,
        [CanBeNull] Item ingredientTR,
        [CanBeNull] Item ingredientML,
        [CanBeNull] Item ingredientMC,
        [CanBeNull] Item ingredientMR,
        [CanBeNull] Item ingredientBL,
        [CanBeNull] Item ingredientBC,
        [CanBeNull] Item ingredientBR
    )
    {
        this.ingredientTL = ingredientTL;
        this.ingredientTC = ingredientTC;
        this.ingredientTR = ingredientTR;
        this.ingredientML = ingredientML;
        this.ingredientMC = ingredientMC;
        this.ingredientMR = ingredientMR;
        this.ingredientBL = ingredientBL;
        this.ingredientBC = ingredientBC;
        this.ingredientBR = ingredientBR;
    }

    public List<Item> ListItems()
    {
        return new List<Item>
        {
            ingredientTL, ingredientTC, ingredientTR,
            ingredientML, ingredientMC, ingredientMR,
            ingredientBL, ingredientBC, ingredientBR
        }.Where(item => item != null).ToList();
    }

    public override string ToString()
    {
        return $"TL: {ingredientTL}, TC: {ingredientTC}, TR: {ingredientTR}, ML: {ingredientML}, MC: {ingredientMC}, MR: {ingredientMR}, BL: {ingredientBL}, BC: {ingredientBC}, BR: {ingredientBR}";
    }
}