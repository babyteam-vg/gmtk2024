using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ClientUI: MonoBehaviour
{
    [SerializeField] public string clientSpriteId = "client-";
    [SerializeField] public string requestImageId = "RequestImage";
    [SerializeField] public List<Client> clientSprites = new();

    [SerializeField] public VisualTreeAsset sellableItemTemplate;

    private UIDocument _document;

    private void Start()
    {
        _document = GetComponent<UIDocument>();

        UpdateClientSprites();
        ClientManager.Instance.clientsChanged += UpdateClientSprites;

        UpdateInventory();
    }

    private void UpdateInventory()
    {
        Inventory inventory = GameManager.Instance.playerData.inventory;
        VisualElement inventoryList = _document.rootVisualElement.Q<VisualElement>("InventoryList");
        inventoryList.Clear();

        foreach (ItemCount itemCount in inventory.ListItemsCount())
        {
            Item item = itemCount.Item;
            if (item == null) continue;

            if (itemCount.Item.MonetaryValue == 0) continue;

            for (int i = 0; i < itemCount.Amount; i++)
            {
                VisualElement itemElement = sellableItemTemplate.Instantiate();
                VisualElement child = itemElement.Q<VisualElement>();
                child.Q<VisualElement>("ItemIcon").style.backgroundImage = item.Description.Image;
                child.Q<Label>("ItemPrice").text = $"${itemCount.Item.MonetaryValue.ToString()}";

                itemElement.RegisterCallback<ClickEvent>(e =>
                {
                    if (ClientManager.Instance.SellItem(item))
                    {
                        UpdateInventory();
                    }
                });

                inventoryList.Add(child);
            }
        }
    }

    private void UpdateClientSprites()
    {
        for (int clientIndex = 0; clientIndex < ClientManager.Instance.CountClients(); clientIndex++)
        {
            Client client = ClientManager.Instance.GetClientAt(clientIndex);
            if (client == null)
            {
                Debug.LogWarning($"Client at index {clientIndex} not found");
                continue;
            }

            VisualElement clientImage = _document.rootVisualElement.Q<VisualElement>(clientSpriteId + clientIndex);
            if (clientImage == null)
            {
                continue;
            }

            clientImage.style.backgroundImage = client.sprite;
        }

        Client currentClient = ClientManager.Instance.CurrentClient;
        if (currentClient == null) return;

        VisualElement requestImage = _document.rootVisualElement.Q<VisualElement>(requestImageId);
        requestImage.style.backgroundImage = currentClient.neededItemType.sprite;
    }

    private void OnDestroy()
    {
        ClientManager.Instance.clientsChanged -= UpdateClientSprites;
    }
}