using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ClientNeed
{
    public ItemType itemType;
    public Texture2D sprite;
}

[Serializable]
public class Client
{
    public ClientNeed neededItemType;
    public Texture2D sprite;
}


public class ClientManager : MonoBehaviour
{
    [SerializeField] private List<Texture2D> clientTextures = new();
    [SerializeField] public List<ClientNeed> clientNeeds = new();
    [SerializeField] private int maxClients = 5;

    private List<Client> _incomingClients = new();

    public Action clientsChanged;
    public static ClientManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < maxClients; i++)
        {
            _incomingClients.Add(GenerateRandomClient());
        }
    }

    private Client GenerateRandomClient()
    {
        return new Client
        {
            neededItemType = clientNeeds[Random.Range(0, clientNeeds.Count)],
            sprite = clientTextures[Random.Range(0, clientTextures.Count)]
        };
    }

    public Client CurrentClient => _incomingClients[0];

    /// <summary>
    /// Sells an item to the current item by the price defined by "MonetaryValue"
    /// </summary>
    /// <param name="item">
    /// The item you are attempting to sell
    /// </param>
    /// <returns>
    /// true if the item is successfully sold, false otherwise
    /// </returns>
    public bool SellItem(Item item)
    {
        if (CurrentClient.neededItemType.itemType != item.Description.itemType)
            return false;

        if (!GameManager.Instance.playerData.inventory.SpendItem(item))
            return false;

        _incomingClients.RemoveAt(0);
        _incomingClients.Add(GenerateRandomClient());
        GameManager.Instance.playerData.AddCoins(item.MonetaryValue);
        clientsChanged?.Invoke();

        return true;
    }

    /// <summary>
    /// Removes the first client on the list.
    /// </summary>
    public void KickClient()
    {
        _incomingClients.RemoveAt(0);
        _incomingClients.Add(GenerateRandomClient());
        clientsChanged?.Invoke();
    }

    public int CountClients()
    {
        return _incomingClients.Count;
    }

    public Client GetClientAt(int index)
    {
        return index >= _incomingClients.Count ? null : _incomingClients[index];
    }
}