using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    InputManager input;

    public GameObject[] inventoryGO;
    public bool isOpened;
    private bool isOpening;

    public List<Slot> slots;

    public int selectedSlot;

    public TMP_Text descriptionTitle;
    public TMP_Text descriptionText;

    public float maxSlotChangeTime;
    private float slotChangeTimer;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        input = InputManager.Instance;
    }

    private void Update() {

        if(input.inventory_Input && !isOpening) OpenInventory(); 

        //Inventory Actions
        if(!isOpened) return;

        if(slotChangeTimer <= 0)
        {
            if(input.movementInput.x != 0){

                switch(input.movementInput.x){
                    case(1):
                        ChangeSlot(1);
                        return;
                    case(-1):
                        ChangeSlot(-1);
                        return;
                }

            }
        }
        else slotChangeTimer-=Time.deltaTime;
    }

    private void ChangeSlot(int selection){
        selectedSlot+=selection;
        if(selectedSlot > 6) selectedSlot = 0;
        else if(selectedSlot < 0) selectedSlot = 6;
        slotChangeTimer = maxSlotChangeTime;

        //slots hide

        int slot1ToHide = 0;
        int slot2ToHide = 0;

        slot1ToHide = selectedSlot + 3;
        slot2ToHide = selectedSlot + 4;

        if(slot1ToHide > 6) slot1ToHide = slot1ToHide-7;
        if(slot2ToHide > 6) slot2ToHide = slot2ToHide-7;

        for(int i = 0; i < slots.Count; i++) slots[i].gameObject.SetActive(true);

        Debug.Log(slot1ToHide);
        Debug.Log(slot2ToHide);
        
        slots[slot1ToHide].gameObject.SetActive(false);
        slots[slot2ToHide].gameObject.SetActive(false);
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

        for(int i = 0; i < inventoryGO.Length; i++) inventoryGO[i].SetActive(isOpened);
    }
    private void ResetOpening() {isOpening=false;}

    private void UpdateDescription(string _descriptionTitle, string _descriptionText){
        if(_descriptionTitle != null){
            descriptionTitle.text = _descriptionTitle;
            descriptionText.text = _descriptionText;
        }
        else{
            descriptionTitle.text = "";
            descriptionText.text = "";
        }
    }

    private void UpdateSlots(){
        for(int i = 0; i < slots.Count; i++){
            //slots[i].
        }
    }

    public void AddItem(WorldItem worldItem){

        ItemInfo newItem = worldItem.itemInfo;
        int _amount = worldItem.amount;

        for(int i = 0; i < slots.Count; i++){
            if(!slots[i].isEmpty && slots[i].item.itemName == newItem.itemName){

                worldItem.SetAmount(slots[i].CheckForFreeAmount(_amount));
                slots[i].AddItemToSlot(newItem, _amount);
                break;

            }
            else if(slots[i].isEmpty) {
                slots[i].AddItemToSlot(newItem, _amount);
                worldItem.SetAmount(0);
                break;
            }
        }

    }
}