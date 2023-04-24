using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    public ItemInfo item;

    public bool isEmpty = true;

    public TMP_Text slotNameText;
    public Image slotIcon;
    public TMP_Text slotItemsCount;

    public int amount;

    private void Init(string itemName, Sprite itemIcon, int itemsCount){

        slotNameText.text = itemName;
        slotIcon.sprite = itemIcon;
        slotIcon.color = new Color(1,1,1,1);

        if(itemsCount > -1) slotItemsCount.text = itemsCount.ToString();
        else slotItemsCount.text = "";
    }

    public int CheckForFreeAmount(int _amount){
        int tmpAmount = amount;
        if((tmpAmount += _amount) > item.maxItemAmount) return _amount - (item.maxItemAmount - amount);
        else return 0;
    }

    public void AddItemToSlot(ItemInfo newItem, int _amount){

        item = newItem;
        
        int tmpAmount = amount;
        amount += _amount;
        Init(item.itemName, item.inventoryIcon, amount);

    }
}
