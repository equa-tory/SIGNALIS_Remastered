using UnityEngine;

public enum ItemType { Default, Weapon, Usable, Tool }
public class ItemInfo : ScriptableObject
{
    public string itemName;
    [TextArea(4,4)] public string itemDescription;
    [Space]
    public int maxItemAmount;
    public GameObject inventoryModel;
    public Sprite inventoryIcon;
}
