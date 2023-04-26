using UnityEngine;

public enum ItemType { Default, Weapon, Gadjet, Usable, Tool }
public class ItemInfo : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    [TextArea(4,4)] public string itemDescription;
    [Space]
    public int maxItemAmount;
    public GameObject inventoryPreviewModel;
    public Sprite inventoryIcon;
}
