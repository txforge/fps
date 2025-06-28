using UnityEngine;
using System.Collections;

public class Hitscan : Weapon
{
    public Transform nuzzle_point;
    public bool IsLaser = false;
    public float laser_range = 1f; // Default range for laser weapons

    [Header("Bullet Trail")]
    public TrailRenderer bulletTrailPrefab; // Assign in inspector
    public float bulletTrailSpeed = 300f; // Units per second

    private bool isReloading = false;

    // Metodo principale per gestire il raycast
    private void PerformRaycast(Vector3 origin, Vector3 direction, float range)
    {
        // Disegna la traiettoria del ray sempre, per debug
        Debug.DrawRay(origin, direction * range, Color.red, 1.5f);

        anim.SetTrigger("shoot");
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        Vector3 endPoint = origin + direction * range;
        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;
            float distance = hit.distance;
            if (dmg_has_dmg_fall_off && distance > dmg_fall_of)
            {
                //Debug.Log("Hit (fall off): " + hit.collider.name + " at distance " + distance);
            }
            else
            {
                Debug.Log("Hit: " + hit.collider.name + " at distance " + distance);
            }
        }
        else
        {
            Debug.Log("Missed");
        }

        // Spawn bullet trail SOLO se non è un laser
        if (!IsLaser && bulletTrailPrefab != null)
        {
            TrailRenderer trail = Instantiate(bulletTrailPrefab, origin, Quaternion.identity);
            StartCoroutine(AnimateTrail(trail, origin, endPoint));
        }
    }

    private IEnumerator AnimateTrail(TrailRenderer trail, Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        float duration = distance / bulletTrailSpeed;
        float time = 0;
        while (time < duration)
        {
            trail.transform.position = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        trail.transform.position = end;
        yield return null;
        Destroy(trail.gameObject, trail.time);
    }

    protected override void Start()
    {
        base.Start();
        if (nuzzle_point == null)
        {
            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t != this.transform)
                {
                    nuzzle_point = t;
                    break;
                }
            }
        }
    }
    // Spara dal nuzzle verso la stessa direzione della camera (come Shoot())
    public void Shoot()
    {
        if (!CanShoot() || isReloading) return;
        if (nuzzle_point == null)
        {
            Debug.LogWarning("nuzzle_point non assegnato!");
            return;
        }
        Camera cam = Camera.main;
        // Calcola la direzione dal nuzzle verso il punto che la camera sta guardando (centro schermo)
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitInfo;
        Vector3 targetPoint;

        // Fai un raycast dalla camera per trovare il punto di impatto reale (es. muro, nemico)
        if (Physics.Raycast(ray, out hitInfo, 1000f))
            targetPoint = hitInfo.point;
        else
            targetPoint = ray.origin + ray.direction * 1000f;

        Vector3 direction = (targetPoint - nuzzle_point.position).normalized;

        Debug.DrawRay(nuzzle_point.position, direction * 1000f, Color.red, 1.5f);

        PerformRaycast(nuzzle_point.position, direction, 1000f);
        ConsumeAmmo();
        UpdateAmmoLabel();
    }

    public void Shoot(float range)
    {
        if (!CanShoot() || isReloading) return;
        if (nuzzle_point == null)
        {
            Debug.LogWarning("nuzzle_point non assegnato!");
            return;
        }
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitInfo;
        Vector3 targetPoint;

        // Fai un raycast dalla camera per trovare il punto di impatto reale (es. muro, nemico)
        if (Physics.Raycast(ray, out hitInfo, range))
            targetPoint = hitInfo.point;
        else
            targetPoint = ray.origin + ray.direction * range;

        Vector3 direction = (targetPoint - nuzzle_point.position).normalized;

        Debug.DrawRay(nuzzle_point.position, direction * range, Color.red, 1.5f);

        PerformRaycast(nuzzle_point.position, direction, range);
        ConsumeAmmo();
        UpdateAmmoLabel();
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public override void Reload()
    {
        if (!has_ammo) return;
        if (ammo == max_ammo) return;
        if (isReloading) return;

        isReloading = true;
        if (anim != null)
            anim.SetTrigger("reload");
        // ammo e UI verranno aggiornati da un Animation Event
    }

    // Da chiamare tramite Animation Event alla fine dell'animazione di reload
    public void OnReloadAnimationEnd()
    {
        ammo = max_ammo;
        UpdateAmmoLabel();
        isReloading = false;
    }

    private void UpdateAmmoLabel()
    {
        if (ui == null)
        {
            Debug.LogWarning("UI non assegnata! Assicurati di assegnare un UIDocument per l'interfaccia utente.");
            return;
        }
        var root = ui.rootVisualElement;
        if (UIammo == null)
        {
            Debug.LogWarning("Label 'UIammo' non trovata nel UIDocument.(Script Hitscan)");
            return;
        }

        if (has_ammo)
            UIammo.text = $"{ammo} / {max_ammo}";
        else
            UIammo.text = "∞";
    }
}

#region Deprecated Methods
// Questi metodi sono deprecati e non dovrebbero essere usati nei nuovi script
/* 
    //Spara dal centro della camera verso avanti (default)
    public void Shoot()
    {
        if (!CanShoot()) return;
        Camera cam = Camera.main;
        PerformRaycast(cam.transform.position, cam.transform.forward, 100f);
        ConsumeAmmo();
        UpdateAmmoLabel();
    }
*/
/*
    //Spara dal centro della camera verso il centro schermo, con range personalizzato
    public void Shoot(float range)
    {
        if (!CanShoot()) return;
        Camera cam = Camera.main;
        Vector3 direction = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)).direction;
        PerformRaycast(cam.transform.position, direction, range);
        ConsumeAmmo();
        UpdateAmmoLabel();
    }
*/
/*
    //Spara dal nuzzle verso il centro schermo, con range personalizzato
    public void ShootFromNuzzle(float range)
    {
        if (!CanShoot()) return;
        if (nuzzle_point == null)
        {
            Debug.LogWarning("nuzzle_point non assegnato!");
            Shoot(range); // fallback
            return;
        }
        Camera cam = Camera.main;
        Vector3 targetPoint = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)).GetPoint(range);
        Vector3 direction = (targetPoint - nuzzle_point.position).normalized;
        PerformRaycast(nuzzle_point.position, direction, range);
        ConsumeAmmo();
        UpdateAmmoLabel();
    }
*/
#endregion

