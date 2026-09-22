using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("INVENTARIO PERSISTENTE")]
    public List<InventoryItem> savedItems =
        new List<InventoryItem>();

    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SaveInventory(Inventory inventory)
    {
        if (inventory == null)
        {
            return;
        }

        savedItems.Clear();

        foreach (InventoryItem item in inventory.items)
        {
            InventoryItem savedItem =
                new InventoryItem(
                    item.itemName,
                    item.itemImage
                );

            savedItem.quantity = item.quantity;

            savedItems.Add(savedItem);
        }
    }

    public void LoadInventory(Inventory inventory)
    {
        if (inventory == null)
        {
            return;
        }

        inventory.items.Clear();

        foreach (InventoryItem item in savedItems)
        {
            InventoryItem loadedItem =
                new InventoryItem(
                    item.itemName,
                    item.itemImage
                );

            loadedItem.quantity = item.quantity;

            inventory.items.Add(loadedItem);
        }
    }
}