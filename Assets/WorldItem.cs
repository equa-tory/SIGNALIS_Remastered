using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemInfo itemInfo;

    public int amount;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.V)){
            PickUp();
        }
    }

    public void PickUp(){
        InventoryManager.Instance.AddItem(this);
    }

    public void SetAmount(int _amount) {
        if(_amount <= 0) {
            Destroy(gameObject);
        }
        else{
            amount = _amount;
        }
    }
}
