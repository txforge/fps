using UnityEngine;
using System.Collections;

public class Semiauto : Hitscan
{
    private bool can_fire = true; // Flag to control shooting (do not use CanShoot to avoid confusion with stun events)
    protected override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Fire1") && can_fire)
        {
            if (!IsLaser) Shoot();
            else Shoot(laser_range);
            StartCoroutine(Sinleshot());
        }
    }
    IEnumerator Sinleshot()
    {
        can_fire = false;
        yield return new WaitForSeconds(fire_rate);
        can_fire = true;
    }

    // Makes the function callable from Animation Event (like Burst)
    public new void OnReloadAnimationEnd()
    {
        base.OnReloadAnimationEnd();
    }
}