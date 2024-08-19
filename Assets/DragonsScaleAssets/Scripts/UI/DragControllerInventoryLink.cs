using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

/// <summary>
/// Handles keeping the DragController and the current inventory in sync. Adding/removing/updating items to the
/// drag controller when a change is detected in the inventory.
/// </summary>
[RequireComponent(typeof(DragController))]
public class DragControllerInventoryLink : MonoBehaviour
{
    [SerializeField] private ItemDescription debugItem;
    [SerializeField] private ItemDescription debugItem2;
    [SerializeField] private string containerId = "Inventory";
    [SerializeField] private VisualTreeAsset itemTemplate;
    [SerializeField] private string dragTargetTag = "craft";

    private DragController _dragController;

    /// <summary>
    /// The visual elements used to preview an item in the inventory by item.
    /// </summary>
    private Dictionary<Item, ItemElement> _itemElements = new();

    private void Start()
    {
        _dragController = GetComponent<DragController>();

        Inventory inventory = GameManager.Instance.playerData.inventory;

        LoadAllItems(inventory);
        inventory.ItemValueChanged += OnItemChanged;
    }

    private void LoadAllItems(Inventory inventory)
    {
        foreach (ItemCount item in inventory.ListItemsCount())
        {
            OnItemChanged(item);
        }
    }

    private void Update()
    {
        // FIXME: just for debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ItemDescription itemToCreate = Random.value < 0.5f ? debugItem : debugItem2;
            GameManager.Instance.playerData.inventory.AddItem(new Item(itemToCreate));
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GameManager.Instance.playerData.inventory.Clear();
        }
    }

    private void OnItemChanged(ItemCount count)
    {
        _itemElements.TryGetValue(count.Item, out ItemElement existingItemElement);
        if (existingItemElement != null)
        {
            if (count.Amount > 0)
            {
                // update
                existingItemElement.ChangeAmount(count.Amount);
            }
            else
            {
                // remove
                existingItemElement.parent.RemoveFromHierarchy();
                _itemElements.Remove(count.Item);
            }

            return;
        }

        // add
        VisualElement container = _dragController.document.rootVisualElement.Q<VisualElement>(containerId);
        if (container == null)
        {
            throw new Exception("Could not find container with id " + containerId);
        }

        TemplateContainer templateContainer = itemTemplate.Instantiate();

        DraggableObject draggableObject = templateContainer.Q<DraggableObject>();
        draggableObject.SetCompatibleTargetTags(dragTargetTag);
        if (draggableObject == null)
        {
            throw new Exception("Could not find a DraggableObject in given template");
        }

        ItemElement itemElement = draggableObject.Q<ItemElement>();
        if (itemElement == null)
        {
            throw new Exception("Could not find an ItemElement inside the draggable object in the given template");
        }

        itemElement.Init(count.Item, count.Amount);

        draggableObject.SetPreview(itemElement);
        _dragController.RegisterElement(draggableObject);

        _itemElements[count.Item] = itemElement;

        container.Add(draggableObject);
    }

    private void OnDestroy()
    {
        Inventory inventory = GameManager.Instance.playerData.inventory;

        inventory.ItemValueChanged -= OnItemChanged;
    }
}