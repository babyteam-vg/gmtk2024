using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public class CraftingManagement : MonoBehaviour
{
    private UIDocument _document;

    [Header("Settings")] [CanBeNull] public CraftingRecipeRegistry recipeRegistry;
    public string craftingButton = "CraftingButton";
    Button _craftingButton;

    [Header("Top Row")] [CanBeNull] public string slotTL = "CraftingTL";
    [CanBeNull] public string slotTC = "CraftingTC";
    [CanBeNull] public string slotTR = "CraftingTR";

    [Header("Center Row")] [CanBeNull] public string slotML = "CraftingML";
    [CanBeNull] public string slotMC = "CraftingMC";
    [CanBeNull] public string slotMR = "CraftingMR";

    [Header("Bottom Row")] [CanBeNull] public string slotBL = "CraftingBL";
    [CanBeNull] public string slotBC = "CraftingBC";
    [CanBeNull] public string slotBR = "CraftingBR";

    [Header("Preview")] [CanBeNull] public string previewImageId = "PreviewImage";

    private void Start()
    {
        _document = GetComponent<UIDocument>();

        _craftingButton = _document.rootVisualElement.Q<Button>(craftingButton);
        _craftingButton.clicked += Craft;

        foreach (DragTarget target in IterateTargets())
        {
            target.HeldObjectChange += OnHeldObjectChange;
        }
    }

    /// <summary>
    /// Handles when a crafting slot changes the item it's holding
    /// </summary>
    /// <param name="obj"></param>
    private void OnHeldObjectChange(DragTarget obj)
    {
        UpdatePreview();
    }

    private IEnumerable<DragTarget> IterateTargets()
    {
        List<string> targetIds = new()
        {
            slotTL, slotTC, slotTR,
            slotML, slotMC, slotMR,
            slotBL, slotBC, slotBR
        };

        foreach (string targetId in targetIds)
        {
            DragTarget target = GetTarget(targetId);
            if (target != null)
            {
                yield return target;
            }
        }
    }

    private CraftingItemSelection GetCurrentSelection()
    {
        Item itemTL = GetItemInTarget(slotTL);
        Item itemTC = GetItemInTarget(slotTC);
        Item itemTR = GetItemInTarget(slotTR);

        Item itemML = GetItemInTarget(slotML);
        Item itemMC = GetItemInTarget(slotMC);
        Item itemMR = GetItemInTarget(slotMR);

        Item itemBL = GetItemInTarget(slotBL);
        Item itemBC = GetItemInTarget(slotBC);
        Item itemBR = GetItemInTarget(slotBR);

        return new CraftingItemSelection(
            itemTL, itemTC, itemTR,
            itemML, itemMC, itemMR,
            itemBL, itemBC, itemBR
        );
    }

    [CanBeNull]
    private CraftingRecipe GetCompatibleRecipe(CraftingItemSelection selection)
    {
        if (recipeRegistry == null)
        {
            Debug.LogWarning("No crafting recipe registry defined in CraftingManagement");
            return null;
        }

        return recipeRegistry.GetCompatibleRecipe(selection);
    }

    private void Craft()
    {
        CraftingItemSelection selection = GetCurrentSelection();

        CraftingRecipe recipe = GetCompatibleRecipe(selection);

        if (recipe == null)
        {
            return;
        }

        List<Item> ingredients = selection.ListItems();
        Item resultItem = new(recipe.resultItem, ingredients);

        GameManager.Instance.playerData.inventory.Transmutate(ingredients, resultItem);
    }

    [CanBeNull]
    private DragTarget GetTarget(string targetQuery)
    {
        if (_document?.rootVisualElement == null)
        {
            Debug.LogWarning("Could not find root visual element");
            return null;
        }

        VisualElement target = _document.rootVisualElement.Q(targetQuery);
        return (DragTarget)target;
    }

    private Item GetItemInTarget(string targetQuery)
    {
        DragTarget target = GetTarget(targetQuery);
        if (target == null)
        {
            Debug.LogWarning($"Could not find target {targetQuery}");
            return null;
        }

        DraggableObject obj = target.GetHeldDraggable();

        ItemElement item = obj?.Q<ItemElement>();

        return item?.Item;
    }

    private void UpdatePreview()
    {
        VisualElement previewImage = _document.rootVisualElement.Q<VisualElement>(previewImageId);
        if (previewImage == null)
        {
            Debug.LogWarning($"Could not find element with id {previewImageId}");
            return;
        }

        CraftingItemSelection selection = GetCurrentSelection();
        CraftingRecipe recipe = GetCompatibleRecipe(selection);

        if (recipe == null)
        {
            previewImage.style.backgroundImage = null;
            return;
        }

        previewImage.style.backgroundImage = recipe.resultItem.Image;
    }

    private void OnDestroy()
    {
        if (_document == null || _document.rootVisualElement == null)
        {
            return;
        }

        foreach (DragTarget target in IterateTargets())
        {
            target.HeldObjectChange -= OnHeldObjectChange;
        }

        _craftingButton.clicked -= Craft;
    }
}