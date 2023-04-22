using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponHolder : MonoBehaviour
{
    public static WeaponHolder Instance;

    public  InputManager input;

    public List<WeaponData> weapons;
    public WeaponData currentWeapon;
    public int weaponIndex;
    public int previousWeaponIndex = -1;

    public LayerMask hitMask;

    private bool scoping;
    [SerializeField] private float defFov;
    [SerializeField] private float scopeFov;
    [SerializeField] private float scopeSpeed;

    [Header("Init Ref")]
    public Camera cam;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        input = InputManager.Instance;

        foreach(Transform c in transform){
            weapons.Add(c.GetComponent<WeaponData>());
        }

        Invoke(nameof(Init),.01f);
    }

    void Update()
    {
        Scope();

        if(input.shoot_Input && !currentWeapon.isReloading) currentWeapon.Use();
        //else  noAmmoSound;

        if(input.reload_Input && currentWeapon.currentAmmo < currentWeapon.info.maxMagAmmo && !currentWeapon.isReloading)
            currentWeapon.Reload();
    }

    private void Scope(){

        if(input.scope_Input){
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, scopeFov, scopeSpeed);
        }
        else{
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defFov, scopeSpeed);
        }

    }

    public void EquipWeapon(int _index)
    {
        if (_index == previousWeaponIndex) return;

        weaponIndex = _index;
        currentWeapon = weapons[weaponIndex];

        weapons[weaponIndex].model.SetActive(true);

        if (previousWeaponIndex != -1)
        {
            weapons[previousWeaponIndex].model.SetActive(false);
        }

        previousWeaponIndex = weaponIndex;
    }

    //---------------------------------------------------------------

    private void Init(){
        for(int i = 0; i < weapons.Count; i++){
            weapons[i].holder = this;
            weapons[i].cam = cam;
        }

        EquipWeapon(0);
    }
}
