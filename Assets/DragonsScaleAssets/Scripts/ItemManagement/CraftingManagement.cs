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

    [Header("Settings")] 
    [CanBeNull] public CraftingRecipeRegistry recipeRegistry;
    public string craftingButton = "CraftingButton";
    Button _craftingButton;

    [Header("Top Row")] 
    [CanBeNull] public string slotTL = "CraftingTL";
    [CanBeNull] public string slotTC = "CraftingTC";
    [CanBeNull] public string slotTR = "CraftingTR";

    [Header("Center Row")] 
    [CanBeNull] public string slotML = "CraftingML";
    [CanBeNull] public string slotMC = "CraftingMC";
    [CanBeNull] public string slotMR = "CraftingMR";

    [Header("Bottom Row")] 
    [CanBeNull] public string slotBL = "CraftingBL";
    [CanBeNull] public string slotBC = "CraftingBC";
    [CanBeNull] public string slotBR = "CraftingBR";
    
    [Header("Scene Management")]
    public string leftSceneButton = "LeftBar";
    Button _leftSceneButton;
    public string rightSceneButton = "RightBar";
    Button _rightSceneButton;

    

    private void Start()
    {
        _document = GetComponent<UIDocument>();

        _craftingButton = _document.rootVisualElement.Q<Button>(craftingButton);
        _craftingButton.clicked += Craft;
        _leftSceneButton = _document.rootVisualElement.Q<Button>(leftSceneButton);
        _leftSceneButton.clicked += LoadLeftScene;
        _rightSceneButton = _document.rootVisualElement.Q<Button>(rightSceneButton);
        _rightSceneButton.clicked += LoadRightScene;
    }

    private void Craft()
    {
        Item itemTL = GetItemInTarget(slotTL);
        Item itemTM = GetItemInTarget(slotTC);
        Item itemTR = GetItemInTarget(slotTR);

        Item itemML = GetItemInTarget(slotML);
        Item itemMC = GetItemInTarget(slotMC);
        Item itemMR = GetItemInTarget(slotMR);

        Item itemBL = GetItemInTarget(slotBL);
        Item itemBC = GetItemInTarget(slotBC);
        Item itemBR = GetItemInTarget(slotBR);

        if (recipeRegistry == null)
        {
            Debug.LogWarning("No crafting recipe registry defined in CraftingManagement");
            return;
        }

        CraftingRecipe recipe = recipeRegistry.GetCompatibleRecipe(
            itemTL, itemTM, itemTR,
            itemML, itemMC, itemMR,
            itemBL, itemBC, itemBR
        );

        if (recipe != null)
        {
            List<Item> ingredients = new List<Item>()
            {
                itemTL, itemTM, itemTR,
                itemML, itemMC, itemMR,
                itemBL, itemBC, itemBR
            }.Where(item => item != null).ToList();

            Item resultItem = new(recipe.resultItem, ingredients);
            GameManager.Instance.playerData.inventory.Transmutate(ingredients, resultItem);
        }
    }

    private Item GetItemInTarget(string targetQuery)
    {
        DragTarget target = (DragTarget)_document.rootVisualElement.Q(targetQuery);
        if (target == null)
        {
            Debug.LogWarning($"Could not find target {targetQuery}");
            return null;
        }

        DraggableObject obj = target.GetHoldDraggable();

        ItemElement item = obj?.Q<ItemElement>();

        return item?.Item;
    }

    private void LoadLeftScene()
    {
        AudioManager.Instance.SetActivePlayerByIndex(0,true);
        TransitionManager.Instance.LoadScene("SampleScene",true);
    }

    private void LoadRightScene()
    {
        AudioManager.Instance.SetActivePlayerByIndex(2,true);
        TransitionManager.Instance.LoadScene("Shop",true);
    }
}