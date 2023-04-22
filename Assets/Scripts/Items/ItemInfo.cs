using UnityEngine;

public enum ItemType { Default, Weapon, Usable, Tool }
public class ItemInfo : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int maxItemAmount;
    public GameObject inventoryModel;
    public Sprite inventoryIcon;
}
