using UnityEngine;
using System.Collections;

public class Semiauto : Hitscan
{
    private bool can_fire = true; // Flag to control shooting (non usiamo can_shoot per evitare confusione con eventuali eventi di stun)
    protected override void Update()
    {
        base.Update(); // Assicurati di chiamare il metodo Update della classe base se necessario
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

    // Rende la funzione richiamabile dall'Animation Event come per Burst
    public new void OnReloadAnimationEnd()
    {
        base.OnReloadAnimationEnd();
    }
}