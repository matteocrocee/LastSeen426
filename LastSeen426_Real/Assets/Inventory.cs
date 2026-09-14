using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    [Header("INVENTARIO")]
    public List<string> items = new List<string>();

    [Header("HUD")]
    public UIDocument hudDocument;

    private VisualElement inventoryScreen;
    private Label inventoryItems;

    private bool inventoryOpen = false;

    private void Start()
    {
        if (hudDocument != null)
        {
            VisualElement root =
                hudDocument.rootVisualElement;

            inventoryScreen =
                root.Q<VisualElement>("InventoryScreen");

            inventoryItems =
                root.Q<Label>("InventoryItems");
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

    public void AddItem(string itemName)
    {
        items.Add(itemName);

        Debug.Log("OGGETTO RACCOLTO: " + itemName);

        UpdateInventoryUI();

        ShowPickupMessage(itemName);
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
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
        if (inventoryItems == null)
        {
            return;
        }

        if (items.Count == 0)
        {
            inventoryItems.text =
                "Inventario vuoto";

            return;
        }

        string text = "";

        for (int i = 0; i < items.Count; i++)
        {
            text += "• " + items[i];

            if (i < items.Count - 1)
            {
                text += "\n";
            }
        }

        inventoryItems.text = text;
    }

    private void ShowPickupMessage(string itemName)
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

        CancelInvoke(nameof(HidePickupMessage));

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