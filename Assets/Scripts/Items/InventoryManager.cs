using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemInfo> items;

    public GameObject inventoryGO;
    private bool isOpened;
    private bool isOpening;

    private void Update() {
        if(InputManager.Instance.inventory_Input && !isOpening) OpenInventory(); 
    }    

    private void OpenInventory(){

        isOpening = true;
        FadeManager.Instance.Fade();
        isOpened = !isOpened;
        Invoke(nameof(ShowInventory),.5f);
        Invoke(nameof(ResetOpening),1f);
    }
    private void ShowInventory(){
        GameManager.Instance.gameIsPaused = isOpened;
        inventoryGO.SetActive(isOpened);
    }
    private void ResetOpening() {isOpening=false;}
}
