using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData : MonoBehaviour
{
    [Header("Data References")]
    public WeaponInfo info;
    public GameObject model;

    public bool isReloading;
    public int currentAmmo;
    public float ammoSetCd;

    public Vector3 shootDir;
    
    public Transform shootPos;
    public ParticleSystem muzzleFlash;
    public ParticleSystem bulletImpact;
    public ParticleSystem sleeve;

    [Header("Sound")]
    public AudioSource shootSound;
    public AudioSource lastShootSound;
    public AudioSource noAmmoSound;
    public AudioSource reloadSound;
    public AudioSource downSound;
    public AudioSource scopeSound;

    [Header("Init Data Ref")]
    [HideInInspector]public WeaponHolder holder;
    [HideInInspector]public Camera cam;
    
    public abstract void Use();
    public abstract void Reload();
}

public abstract class Weapon : WeaponData
{
    public abstract override void Use();
    public abstract override void Reload();
}
