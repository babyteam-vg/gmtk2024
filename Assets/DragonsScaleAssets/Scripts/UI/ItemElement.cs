using UnityEngine;
using UnityEngine.UIElements;

public class ItemElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ItemElement, UxmlTraits>
    {
    }

    public Item Item { get; private set; }
    public int Quantity { get; private set; }

    private VisualElement Image => this.Q<VisualElement>("image");
    private Label QuantityBadge => this.Q<Label>("quantity");
    private Label DisplayNameBadge => this.Q<Label>("displayName");

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

        Item = item;
        Quantity = quantity;
    }

    public ItemElement()
    {
    }

    public void ChangeAmount(int amount)
    {
        QuantityBadge.text = amount.ToString();
    }

    public VisualElement CreatePreview()
    {
        VisualElement element = new();
        element.Add(
            new VisualElement
            {
                style =
                {
                    backgroundImage = Item.Description.Image
                },
                name = "image"
            });
        return element;
    }
}