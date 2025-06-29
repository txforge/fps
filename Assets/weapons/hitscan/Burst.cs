using UnityEngine;
using System.Collections;

public class Burst : Hitscan
{
    public int Nofbursts = 3;
    public float time_between_bullet = 0.1f; // Seconds between burst shots
    public float burst_cooldown = 0.5f;      // Time between bursts

    private bool isFiring = false;

    protected override void Update()
    {
        base.Update();
        if (Input.GetButton("Fire1") && !isFiring)
        {
            StartCoroutine(BurstFire());
        }
    }

    IEnumerator BurstFire()
    {
        isFiring = true;
        for (int i = 0; i < Nofbursts; i++)
        {
            if (!IsLaser) Shoot();
            else Shoot(laser_range);

            // Ammo/UI update is handled in Shoot()

            if (i < Nofbursts - 1)
            {
                yield return new WaitForSeconds(time_between_bullet);
            }
        }
        yield return new WaitForSeconds(burst_cooldown);
        isFiring = false;
    }

    // Makes the function callable from Animation Event (like Semiauto)
    public new void OnReloadAnimationEnd()
    {
        base.OnReloadAnimationEnd();
    }
}