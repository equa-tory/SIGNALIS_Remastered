using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weapon_", menuName = "Items/Weapon/New Weapon")]
public class WeaponInfo : ItemInfo
{
    public float damage;
    public float critDamage;
    public int maxMagAmmo;
    public float reloadTime;
    public float maxShootCd;
}
