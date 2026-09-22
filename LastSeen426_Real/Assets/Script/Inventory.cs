using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    [Header("INVENTARIO")]
    public List<InventoryItem> items = new List<InventoryItem>();

    [Header("HUD")]
    public UIDocument hudDocument;

    private VisualElement inventoryScreen;
    private VisualElement inventoryGrid;

    private bool inventoryOpen = false;

    private void Start()
    {
        if (hudDocument != null)
        {
            VisualElement root =
                hudDocument.rootVisualElement;

            inventoryScreen =
                root.Q<VisualElement>("InventoryScreen");

            inventoryGrid =
                root.Q<VisualElement>("InventoryGrid");
        }

        // Carica gli oggetti raccolti nei livelli precedenti
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadInventory(this);
        }

        CloseInventory();

        UpdateInventoryUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void AddItem(string itemName, Sprite itemImage)
    {
        InventoryItem existingItem = null;

        foreach (InventoryItem item in items)
        {
            if (item.itemName == itemName)
            {
                existingItem = item;
                break;
            }
        }

        if (existingItem != null)
        {
            existingItem.quantity++;
        }
        else
        {
            InventoryItem newItem =
                new InventoryItem(
                    itemName,
                    itemImage
                );

            items.Add(newItem);
        }

        Debug.Log("OGGETTO RACCOLTO: " + itemName);

        // Salva l'inventario nel GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveInventory(this);
        }

        UpdateInventoryUI();

        ShowPickupMessage(itemName);
    }

    public bool HasItem(string itemName)
    {
        foreach (InventoryItem item in items)
        {
            if (item.itemName == itemName &&
                item.quantity > 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == itemName)
            {
                items[i].quantity--;

                if (items[i].quantity <= 0)
                {
                    items.RemoveAt(i);
                }

                // Aggiorna il salvataggio
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SaveInventory(this);
                }

                UpdateInventoryUI();

                Debug.Log(
                    "OGGETTO UTILIZZATO: " + itemName
                );

                return true;
            }
        }

        return false;
    }

    private void ToggleInventory()
    {
        if (inventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    private void OpenInventory()
    {
        inventoryOpen = true;

        if (inventoryScreen != null)
        {
            inventoryScreen.style.display =
                DisplayStyle.Flex;
        }

        UpdateInventoryUI();
    }

    private void CloseInventory()
    {
        inventoryOpen = false;

        if (inventoryScreen != null)
        {
            inventoryScreen.style.display =
                DisplayStyle.None;
        }
    }

    private void UpdateInventoryUI()
    {
        if (inventoryGrid == null)
        {
            return;
        }

        inventoryGrid.Clear();

        if (items.Count == 0)
        {
            Label emptyLabel =
                new Label("Inventario vuoto");

            emptyLabel.AddToClassList(
                "inventory-empty"
            );

            inventoryGrid.Add(emptyLabel);

            return;
        }

        foreach (InventoryItem item in items)
        {
            CreateInventorySlot(item);
        }
    }

    private void CreateInventorySlot(
        InventoryItem item
    )
    {
        VisualElement slot =
            new VisualElement();

        slot.AddToClassList(
            "inventory-slot"
        );

        Image image =
            new Image();

        image.AddToClassList(
            "inventory-image"
        );

        if (item.itemImage != null)
        {
            image.image =
                item.itemImage.texture;
        }

        slot.Add(image);

        Label nameLabel =
            new Label(item.itemName);

        nameLabel.AddToClassList(
            "inventory-name"
        );

        slot.Add(nameLabel);

        Label quantityLabel =
            new Label("×" + item.quantity);

        quantityLabel.AddToClassList(
            "inventory-quantity"
        );

        slot.Add(quantityLabel);

        inventoryGrid.Add(slot);
    }

    private void ShowPickupMessage(
        string itemName
    )
    {
        if (hudDocument == null)
        {
            return;
        }

        Label pickupMessage =
            hudDocument.rootVisualElement
            .Q<Label>("PickupMessage");

        if (pickupMessage == null)
        {
            return;
        }

        pickupMessage.text =
            "HAI RACCOLTO: " + itemName;

        pickupMessage.style.display =
            DisplayStyle.Flex;

        CancelInvoke(
            nameof(HidePickupMessage)
        );

        Invoke(
            nameof(HidePickupMessage),
            3f
        );
    }

    private void HidePickupMessage()
    {
        if (hudDocument == null)
        {
            return;
        }

        Label pickupMessage =
            hudDocument.rootVisualElement
            .Q<Label>("PickupMessage");

        if (pickupMessage != null)
        {
            pickupMessage.style.display =
                DisplayStyle.None;
        }
    }
}