using UnityEngine;
using System.Collections;

public class Hitscan : Weapon
{
    public Transform nuzzle_point;
    public GameObject nuzzle_flash_prefab;
    public bool IsLaser = false;
    public float laser_range = 1f; // Default range for laser weapons

    [Header("Bullet Trail")]
    public TrailRenderer bulletTrailPrefab; // Assign in inspector
    public float bulletTrailSpeed = 300f; // Units per second

    public LineRenderer laserLinePrefab; // Assign in inspector for laser
    private LineRenderer activeLaserLine;
    private bool isReloading = false;

    // Main method for handling raycast
    private void PerformRaycast(Vector3 origin, Vector3 direction, float range)
    {
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
                // Debug.Log("Hit (fall off): " + hit.collider.name + " at distance " + distance);
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

        // Spawn bullet trail ONLY if not a laser
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

    // Shoot from nuzzle toward the direction aimed by the camera (center screen)
    public void Shoot()
    {
        if (!CanShoot() || isReloading) return;
        if (nuzzle_point == null)
        {
            Debug.LogWarning("nuzzle_point not assigned!");
            return;
        }
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitInfo;
        Vector3 targetPoint;

        // Raycast from camera to find the real hit point (wall, enemy, etc.)
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
            Debug.LogWarning("nuzzle_point not assigned!");
            return;
        }
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitInfo;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hitInfo, range))
            targetPoint = hitInfo.point;
        else
            targetPoint = ray.origin + ray.direction * range;

        Vector3 direction = (targetPoint - nuzzle_point.position).normalized;

        Debug.DrawRay(nuzzle_point.position, direction * range, Color.red, 1.5f);

        // If laser, create LineRenderer if not exists, and activate it (but do NOT update position here)
        if (IsLaser && laserLinePrefab != null)
        {
            if (activeLaserLine == null)
            {
                activeLaserLine = Instantiate(laserLinePrefab, nuzzle_point.position, Quaternion.identity, nuzzle_point);
            }
            activeLaserLine.gameObject.SetActive(true);
            _laserActive = true;
        }

        PerformRaycast(nuzzle_point.position, direction, range);
        ConsumeAmmo();
        UpdateAmmoLabel();
    }

    private bool _laserActive = false;

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // Update laser position every frame ONLY if laser is active (i.e., firing)
        if (IsLaser && activeLaserLine != null && nuzzle_point != null && _laserActive)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hitInfo;
            Vector3 targetPoint;
            float range = laser_range;

            // Calculate the point aimed by the camera (center screen)
            Vector3 cameraTarget;
            if (Physics.Raycast(ray, out hitInfo, 1000f))
                cameraTarget = hitInfo.point;
            else
                cameraTarget = ray.origin + ray.direction.normalized * 1000f;

            // Calculate direction from nuzzle to camera target
            Vector3 direction = (cameraTarget - nuzzle_point.position).normalized;
            Ray nuzzleRay = new Ray(nuzzle_point.position, direction);
            // The laser stops where it hits from the nuzzle, or goes to the aimed point
            if (Physics.Raycast(nuzzleRay, out hitInfo, range))
                targetPoint = hitInfo.point;
            else
                targetPoint = nuzzle_point.position + direction * range;

            activeLaserLine.SetPosition(0, nuzzle_point.position);
            activeLaserLine.SetPosition(1, targetPoint);
        }
        else if (activeLaserLine != null)
        {
            // If not firing, hide the laser
            activeLaserLine.gameObject.SetActive(false);
        }

        // Reset flag for next frame ONLY if Fire1 is not held
        if (!Input.GetButton("Fire1"))
            _laserActive = false;
    }

    public override void Reload()
    {
        if (!has_ammo) return;
        if (ammo == max_ammo) return;
        if (isReloading) return;

        isReloading = true;
        if (anim != null)
            anim.SetTrigger("reload");
        // ammo and UI will be updated by Animation Event
    }

    // To be called by Animation Event at the end of reload animation
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
            Debug.LogWarning("UI not assigned! Make sure to assign a UIDocument for the UI.");
            return;
        }
        var root = ui.rootVisualElement;
        if (UIammo == null)
        {
            Debug.LogWarning("Label 'UIammo' not found in UIDocument. (Script Hitscan)");
            return;
        }

        if (has_ammo)
            UIammo.text = $"{ammo} / {max_ammo}";
        else
            UIammo.text = "∞";
    }

    // Hide the laser when you stop firing (call from Fullauto/Semiauto/Burst if needed)
    public void HideLaser()
    {
        if (activeLaserLine != null)
            activeLaserLine.gameObject.SetActive(false);
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

