using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("OGGETTO")]
    [SerializeField] private string itemName = "Oggetto";

    [Header("IMMAGINE")]
    [SerializeField] private Sprite itemImage;

    public string ItemName
    {
        get { return itemName; }
    }

    public Sprite ItemImage
    {
        get { return itemImage; }
    }

    public void PickUp(Inventory inventory)
    {
        if (inventory == null)
        {
            Debug.LogError("PickupItem: Inventory non trovato!");
            return;
        }

        inventory.AddItem(itemName, itemImage);

        Destroy(gameObject);
    }
}