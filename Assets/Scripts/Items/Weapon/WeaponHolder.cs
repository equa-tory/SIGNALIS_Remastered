using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    public static WeaponHolder Instance;
    public AnimatorManager animManager;
    private InputManager input;
    public WeaponData currentWeapon;

    [Header("Scope")]
    public bool scoping;
    private bool scopingStarted;
    [SerializeField] private float defFov;
    [SerializeField] private float scopeFov;
    [SerializeField] private float scopeSpeed;

    public Image scope;
    public Sprite dotTexture;
    public Sprite scopeTexture;
    public float defSpread;
    public float currentSpread;
    private float scopeSpread;
    public float defScopeSpread;
    public float shootSpreadMult;
    public float scopeSpreadMult;
    public float maxScopeTimer;
    private float scopeTimer;

    private float shootCd;

    [Header("Init Ref")]
    public Camera cam;
    public Camera helpCam;
    public LayerMask hitMask;

    //Debug
    public WeaponData pistol;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        input = InputManager.Instance;

        if(transform.childCount>0) currentWeapon = transform.GetChild(0).GetComponent<WeaponData>();

        Invoke(nameof(Init),.01f);
    }

    void Update()
    {

        if(GameManager.Instance.gameIsPaused) return;

        //Debug
        //if (Input.GetKeyDown(KeyCode.G)) EquipWeapon(pistol);

        if (currentWeapon == null) return;
        Scope();
        
        animManager.animator.SetBool("Scoping", scoping);

        if (input.shoot_Input && !currentWeapon.isReloading && scoping && shootCd <= 0)
        {
            Vector3 _spread = cam.transform.forward;
            _spread += Random.Range(currentSpread, -currentSpread) * cam.transform.up;
            _spread += Random.Range(currentSpread, -currentSpread) * cam.transform.right;
            _spread.Normalize();
            currentWeapon.shootDir = _spread; 
            currentWeapon.Use();
            if(currentWeapon.currentAmmo > 0) shootCd = currentWeapon.info.maxShootCd;
        }

        if(input.reload_Input && currentWeapon.currentAmmo < currentWeapon.info.maxMagAmmo && !currentWeapon.isReloading)
            currentWeapon.Reload();

        if (shootCd > 0) shootCd -= Time.deltaTime;
    }

    private void Scope(){

        scoping = input.scope_Input;

        if(scoping){
            
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, scopeFov, scopeSpeed);
            helpCam.fieldOfView = Mathf.Lerp(cam.fieldOfView, scopeFov, scopeSpeed);

            scope.gameObject.SetActive(true);

            if (!scopingStarted)
            {
                //ScopeSet
                scopingStarted = true;
                scopeTimer = maxScopeTimer;
                scopeSpread = defScopeSpread;
                scope.sprite = scopeTexture;
                currentSpread = defSpread;
            }
            
        }
        else{
            scopingStarted = false;

            scope.sprite = dotTexture;
            scope.rectTransform.sizeDelta = new Vector2(32, 32);

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defFov, scopeSpeed);
            helpCam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defFov, scopeSpeed);
        }

        if(scopeTimer > 0 && scopingStarted)
        {
            currentSpread-=Time.deltaTime * shootSpreadMult;
            scopeTimer -= Time.deltaTime;
            scopeSpread -= Time.deltaTime * scopeSpreadMult;
            scope.rectTransform.sizeDelta = new Vector2(scopeSpread, scopeSpread);
        }
    }

    public void MomentumWeaponReload(){
        if(currentWeapon.currentAmmo < currentWeapon.info.maxMagAmmo){
            currentWeapon.currentAmmo = currentWeapon.info.maxMagAmmo;
            currentWeapon.reloadSound.Play();
        }
    }

    public void EquipWeapon(WeaponData _weapon)
    {
        WeaponData newWeapon = Instantiate(_weapon, transform);

        currentWeapon = newWeapon;

        Init();
    }

    public void UnequipWeapon()
    {

        Destroy(currentWeapon.gameObject);

        currentWeapon = null;

        Init();
    }

    //---------------------------------------------------------------

    private void Init(){
        if (currentWeapon != null)
        {
            currentWeapon.holder = this;
            currentWeapon.cam = cam;
        }
    }
}
