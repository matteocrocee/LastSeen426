using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite itemImage;
    public int quantity;

    public InventoryItem(string name, Sprite image)
    {
        itemName = name;
        itemImage = image;
        quantity = 1;
    }
}