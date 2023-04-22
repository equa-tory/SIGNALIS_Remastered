using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Use()
    {
        Debug.Log(info.itemName);
    }

    public override void Reload()
    {
        Debug.Log("Reloading: " + info.itemName);
    }
}
