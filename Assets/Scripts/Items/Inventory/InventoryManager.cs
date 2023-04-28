using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    public Color[] healthColors;

    public Image currentHealthImage;
    public Image currentWeaponImage;
    public Image currentGadjetImage;

    public TMP_Text descriptionTitle;
    public TMP_Text descriptionText;
    public TMP_Text currentWeaponText;
    public TMP_Text currentWeaponAmmoText;
    public TMP_Text currentGadjetCountText;

    public float maxMoveTime;
    private float slotChangeTimer;
    private float varianceTimer;

    public Transform slotsList;
    public Transform varianceList;

    public Color defVarianceTextColor;
    public Color selectedVarianceTextColor;

    private bool varianceListIsOpened;
    private bool varianceListIsOpening;
    private bool isInspecting;

    private string selectedType;

    public GameObject previewPlaceHolder;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        input = InputManager.Instance;

        SetDesctiption();

        currentWeaponImage.color = new Color(0,0,0,0);
        currentWeaponText.text = "";
        currentWeaponAmmoText.text = "";
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
        if(input.use_Input && !slots[selectedSlot].isEmpty && !slots[selectedSlot].blocked && !varianceListIsOpened){
            varianceList.gameObject.SetActive(true);
            varianceListIsOpened = true;
            varianceListIsOpening = true;
            Invoke(nameof(ResetVarianceOpening),.1f);

            selectedType = slots[selectedSlot].item.itemType.ToString();

            UpdateVarianceButtons();
            varianceListIsOpening = true;
        }
        else if(input.cancel_Input && varianceListIsOpened){
            varianceList.gameObject.SetActive(false);
            varianceListIsOpened = false;
        }

        //UseItemButtons
        if(input.use_Input && varianceListIsOpened && !varianceListIsOpening){
            
            if (selectedType == "Weapon")
            {
                if(selectedVariance == 0) EquipWeapon();
                else if(selectedVariance == 1) ReloadWeapon();
                else if(selectedVariance == 2) InspectItem();
            }
            else if (selectedType == "Gadjet")
            {
                if(selectedVariance == 0) EquipGadjet();
                else if(selectedVariance == 1) ReloadWeapon();
                else if(selectedVariance == 2) InspectItem();
            }
            else
            {
                if(selectedVariance == 0) UseItem();
                else if(selectedVariance == 1) CombineItem();
                else if(selectedVariance == 2) InspectItem();
            }

            UpdateVarianceButtons();
        }

        //Scrolling Variances
        if(input.movementInput.y != 0 && varianceListIsOpened && !isInspecting){

            switch(input.movementInput.y){
                case(1):
                    ChangeVariance(1);
                    break;
                case(-1):
                    ChangeVariance(-1);
                    break;
            }
        }

        if(input.cancel_Input && isInspecting) InspectItem();
            
    }

    //-------------------------Variances Buttons-------------------------

    private void EquipWeapon(){

        if(!slots[selectedSlot].equipped){
            WeaponHolder.Instance.EquipWeapon(slots[selectedSlot].item.itemGO.GetComponent<WeaponData>());
            slots[selectedSlot].equipped = true;
            
            currentWeaponImage.color = new Color(255,255,255,1);
            currentWeaponImage.sprite = slots[selectedSlot].item.itemGO.GetComponent<WeaponData>().info.inventoryIcon;
            currentWeaponText.text = slots[selectedSlot].item.itemName;
            currentWeaponAmmoText.text = "7/" + slots[selectedSlot].item.itemGO.GetComponent<WeaponData>().info.maxMagAmmo;
        }
        else{
            WeaponHolder.Instance.UnequipWeapon();
            slots[selectedSlot].equipped = false;

            currentWeaponImage.color = new Color(0,0,0,0);
            currentWeaponText.text = "";
            currentWeaponAmmoText.text = "";
        }

    }

    private void ReloadWeapon(){

        WeaponHolder.Instance.MomentumWeaponReload();

    }

    private void EquipGadjet(){



    }

    private void CombineItem(){



    }

    private void UseItem(){



    }

    private void InspectItem(){

        isInspecting = !isInspecting;
        PreviewItem.Instance.isInspecting = isInspecting;  

    }

    //-------------------------Inventory Actions-------------------------

    public void UpdateHealthImage(int health){

        currentHealthImage.color = healthColors[health];

    }

    private void UpdateVarianceButtons(){
        //Buttons Text Set
            if(selectedType == "Default") {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Combine";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }

            if(selectedType == "Weapon" && !slots[selectedSlot].equipped) {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Equip";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Reload";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }
            else if(selectedType == "Weapon" && slots[selectedSlot].equipped){
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Unequip";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Reload";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }

            if(selectedType == "Gadjet" && !slots[selectedSlot].equipped) {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Equip";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }
            else if(selectedType == "Gadjet" && slots[selectedSlot].equipped){
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Unequip";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }

            if(selectedType == "Usable") {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Combine";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }

            if(selectedType == "Tool") {
                varianceList.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Use";
                varianceList.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "Combine";
                varianceList.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = "Inspect";
            }
    }

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

            for(int i = 0; i < slots.Count; i++) slotsList.GetChild(i).gameObject.SetActive(true);

            slotsList.GetChild(5).gameObject.SetActive(false);
            slotsList.GetChild(6).gameObject.SetActive(false);

            if(slots[selectedSlot].isEmpty || slots[selectedSlot].blocked) PreviewItem.Instance.RemoveModel();
            else if(slots[selectedSlot].item.inventoryPreviewModel != null) PreviewItem.Instance.SetModel(slots[selectedSlot].item.inventoryPreviewModel);
            else PreviewItem.Instance.SetModel(previewPlaceHolder);

            //Close other actions
            PreviewItem.Instance.model.rotation = Quaternion.identity;
            if(isInspecting) InspectItem();
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

        varianceList.gameObject.SetActive(false);
        varianceListIsOpened = false;

        Invoke(nameof(ShowInventory),.5f);
        Invoke(nameof(ResetOpening),1f);
    }
    private void ShowInventory(){
        GameManager.Instance.gameIsPaused = isOpened;
        PreviewItem.Instance.SetMaxTimer();
        WeaponHolder.Instance.ShowScope(!isOpened);
        for(int i = 0; i < inventoryGO.Length; i++) inventoryGO[i].SetActive(isOpened);
    }

    private void ResetOpening() {isOpening=false;}
    private void ResetVarianceOpening() {varianceListIsOpening=false;}

    private void UpdateDescriptionText(string _descriptionTitle, string _descriptionText){
        if(_descriptionTitle != null){
            descriptionTitle.text = _descriptionTitle;
            descriptionText.text = _descriptionText;
        }
        else{
            descriptionTitle.text = "";
            descriptionText.text = "";
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
            else if(slots[i].isEmpty && !slots[i].blocked) {
                slots[i].AddItemToSlot(newItem, _amount);
                worldItem.SetAmount(0);
                break;
            }
        }

    }

}