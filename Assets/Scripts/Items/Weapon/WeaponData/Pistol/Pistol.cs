using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Use()
    {
        if (currentAmmo <= 0)
        {

            noAmmoSound.Stop();
            noAmmoSound.Play();

        }
        else if (Physics.Raycast(cam.transform.position, shootDir, out RaycastHit hit, Mathf.Infinity, holder.hitMask))
        {
            //FX
            muzzleFlash.Play();
            Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up) * bulletImpact.transform.rotation);

            sleeve.Play();

            if (currentAmmo == 1) lastShootSound.Play();
            else shootSound.Play();

            currentAmmo--;
        }
    }

    public override void Reload()
    {
        reloadSound.Play();

        isReloading = true;

        Invoke(nameof(SetAmmo),ammoSetCd);
    }

    private void SetAmmo()
    {
        isReloading = false;
        currentAmmo = info.maxMagAmmo;
    }
}
