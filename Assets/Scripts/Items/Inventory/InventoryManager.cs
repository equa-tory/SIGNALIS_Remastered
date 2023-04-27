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
    public int selectedVariance;

    public TMP_Text descriptionTitle;
    public TMP_Text descriptionText;

    public float maxMoveTime;
    private float slotChangeTimer;
    private float varianceTimer;

    public Transform slotsList;
    public Transform varianceList;

    public Color defVarianceTextColor;
    public Color selectedVarianceTextColor;

    private bool varianceListIsOpened;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        input = InputManager.Instance;

        SetDesctiption();
    }

    void Update() {

        if(input.inventory_Input && !isOpening && !isOpened) OpenInventory(); 
        if((input.inventory_Input || (input.cancel_Input && !varianceListIsOpened)) && !isOpening && isOpened) CloseInventory(); 

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            slotsList.GetChild(6).SetSiblingIndex(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            slotsList.GetChild(0).SetSiblingIndex(6);
        }

        if(slotChangeTimer > 0) slotChangeTimer-=Time.deltaTime;
        if(varianceTimer > 0) varianceTimer-=Time.deltaTime;

        //Inventory Actions
        if(!isOpened) return;

        //Slots Scroll
        if(input.movementInput.x != 0 && !varianceListIsOpened){

            switch(input.movementInput.x){
                case(1):
                    ChangeSlot(1);
                    break;
                case(-1):
                    ChangeSlot(-1);
                    break;
            }
        }
        
        //Show and hide variance panel
        if(input.use_Input && !slots[selectedSlot].isEmpty && !varianceListIsOpened){
            varianceList.gameObject.SetActive(true);
            varianceListIsOpened = true;

            #region Buttons Text Set
            //Buttons Text Set
            if(slots[selectedSlot].item.itemType.ToString() == "Default") {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Combine";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }

            if(slots[selectedSlot].item.itemType.ToString() == "Weapon" && !slots[selectedSlot].equipped) {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Equip";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Reload";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }
            else if(slots[selectedSlot].item.itemType.ToString() == "Weapon" && slots[selectedSlot].equipped){
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Unequip";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Reload";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }

            if(slots[selectedSlot].item.itemType.ToString() == "Gadjet" && !slots[selectedSlot].equipped) {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Equip";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }
            else if(slots[selectedSlot].item.itemType.ToString() == "Gadjet" && slots[selectedSlot].equipped){
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Unequip";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }

            if(slots[selectedSlot].item.itemType.ToString() == "Usable") {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Combine";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }

            if(slots[selectedSlot].item.itemType.ToString() == "Tool") {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Combine";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }
            #endregion
        }
        else if(input.cancel_Input && varianceListIsOpened){
            varianceList.gameObject.SetActive(false);
            varianceListIsOpened = false;
        }

        //UseItemButtons
        if(input.use_Input && varianceListIsOpened){
            if(selectedVariance == 0)
            {
                if(slots[selectedSlot].item.itemType.ToString() == "Default" || slots[selectedSlot].item.itemType.ToString() == "Usable" || slots[selectedSlot].item.itemType.ToString() == "Tool")
                {

                }
                else if (slots[selectedSlot].item.itemType.ToString() == "Weapon" && slots[selectedSlot].equipped)
                {

                }
                else if (slots[selectedSlot].item.itemType.ToString() == "Gadjet" && slots[selectedSlot].equipped)
                {

                }
                else if (slots[selectedSlot].item.itemType.ToString() == "Weapon" && !slots[selectedSlot].equipped)
                {

                }
                else if (slots[selectedSlot].item.itemType.ToString() == "Gadjet" && !slots[selectedSlot].equipped)
                {

                }
            }
        }

        //Scrolling Variances
        if(input.movementInput.y != 0 && varianceListIsOpened){

            switch(input.movementInput.y){
                case(1):
                    ChangeVariance(1);
                    break;
                case(-1):
                    ChangeVariance(-1);
                    break;
            }
        }
            
    }

    //-------------------------Variances Buttons-------------------------

    private void EquipWeapon(){



    }

    private void EquipGadjet(){



    }

    private void CombineItem(){



    }

    private void UseItem(){



    }

    private void InspectItem(){



    }

    //-------------------------Inventory Actions-------------------------

    private void ChangeVariance(int selection){

        if(varianceTimer <= 0){

            selectedVariance-=selection;
            if(selectedVariance>2) selectedVariance = 0;
            else if(selectedVariance<0) selectedVariance = 2;

            for(int i = 0; i < varianceList.GetChild(0).childCount; i++){
                varianceList.GetChild(0).GetChild(i).GetComponent<TMP_Text>().color = defVarianceTextColor;
            }

            varianceList.GetChild(0).GetChild(selectedVariance).GetComponent<TMP_Text>().color = selectedVarianceTextColor;

            varianceTimer = maxMoveTime;

        }

    }

    private void SetDesctiption()
    {

        if(slots[selectedSlot].item == null)
        {
            descriptionTitle.text = "";
            descriptionText.text = "";
        }
        else
        {
            descriptionTitle.text = slots[selectedSlot].item.itemName;
            descriptionText.text = slots[selectedSlot].item.itemDescription;
        }

    }

    private void ChangeSlot(int selection){

        if(slotChangeTimer <= 0){

            selectedSlot+=selection;
            if(selectedSlot>6) selectedSlot = 0;
            else if(selectedSlot<0) selectedSlot = 6;

            SetDesctiption();

            if(selection == 1){
                slotsList.GetChild(0).SetSiblingIndex(6);
            }
            else if(selection == -1){
                slotsList.GetChild(6).SetSiblingIndex(0);
            }

            slotChangeTimer = maxMoveTime;
        }
    }

    private void OpenInventory(){
        isOpening = true;
        FadeManager.Instance.Fade();
        isOpened = true;
        Invoke(nameof(ShowInventory),.5f);
        Invoke(nameof(ResetOpening),1f);
    }
    private void CloseInventory(){
        isOpening = true;
        FadeManager.Instance.Fade();
        isOpened = false;
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