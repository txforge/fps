using UnityEngine;
using System.Collections;

public class Burst : Hitscan
{
    public int Nofbursts = 3;
    public float time_between_bullet = 0.1f; // secondi tra colpi della raffica
    public float burst_cooldown = 0.5f;      // tempo tra una raffica e l'altra

    private bool isFiring = false;

    protected override void Update()
    {
        base.Update(); // Assicurati di chiamare il metodo Update della classe base se necessario
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

            // Aggiorna ammo dopo ogni colpo
            // (già gestito in Shoot(), quindi non serve altro qui)

            if (i < Nofbursts - 1)
            {
                yield return new WaitForSeconds(time_between_bullet);
            }
        }
        yield return new WaitForSeconds(burst_cooldown);
        isFiring = false;
        // Aggiorna la UI delle munizioni dopo la raffica (utile se la UI non si aggiorna per qualche motivo)
        //UpdateAmmoLabel();
    }

    // AGGIUNGI IL MODIFICATORE 'new' PER SOPPRIMERE IL WARNING
    public new void OnReloadAnimationEnd()
    {
        base.OnReloadAnimationEnd();
    }
}
