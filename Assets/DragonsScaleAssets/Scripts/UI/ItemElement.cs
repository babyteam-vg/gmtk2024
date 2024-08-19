using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ItemElement, UxmlTraits>
    {
    }

    private VisualElement Image => this.Q<VisualElement>("image");
    private Label QuantityBadge => this.Q<Label>("quantity");
    private Label DisplayNameBadge => this.Q<Label>("displayName");
    private static Color ItemColor => Color.white;

    public void Init(
        Item item,
        int quantity
    )
    {

        if (item.Description.Image)
        {
            Image.style.backgroundImage = new StyleBackground(item.Description.Image);
        }

        DisplayNameBadge.text = item.Description.displayName;
        QuantityBadge.text = quantity.ToString();
    }

    public ItemElement() {}

    public void ChangeAmount(int amount)
    {
        QuantityBadge.text = amount.ToString();
    }
}