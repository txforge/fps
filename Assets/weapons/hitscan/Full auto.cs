using UnityEngine;
using System.Collections;

public class Fullauto : Hitscan
{
    private bool isFiring = false;

    protected override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Fire1") && !isFiring)
        {
            StartCoroutine(AutoFire());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }
    }

    IEnumerator AutoFire()
    {
        isFiring = true;
        while (Input.GetButton("Fire1"))
        {
            if (!IsLaser)
                Shoot();
            else
                Shoot(laser_range);

            yield return new WaitForSeconds(fire_rate);
        }
        isFiring = false;
    }
}
