using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("OGGETTO")]
    [SerializeField] private string itemName = "Oggetto";

    public string ItemName
    {
        get { return itemName; }
    }

    public void PickUp(Inventory inventory)
    {
        if (inventory == null)
        {
            Debug.LogError("PickupItem: Inventory non trovato!");
            return;
        }

        inventory.AddItem(itemName);

        Destroy(gameObject);
    }
}