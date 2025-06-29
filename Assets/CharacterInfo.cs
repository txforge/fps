using UnityEngine;
using UnityEngine.UIElements;

public class CharacterInfo : MonoBehaviour
{
    public Weapon weapon; // Only the one with tag "MainWeapon"
    public Animator weapon_anim;

    private void Awake()
    {
        SetUpWeapon();
    }

    private void SetUpWeapon()
    {
        // Find all weapons in children and grandchildren (recursive)
        weapon = null;
        Weapon[] allWeapons = GetComponentsInChildren<Weapon>(true);
        foreach (Weapon w in allWeapons)
        {
            if (w != null && w.CompareTag("MainWeapon"))
            {
                weapon = w;
                break;
            }
        }

        if (weapon == null)
        {
            Debug.LogError("Weapon with tag 'MainWeapon' not found! Make sure to assign the tag.");
        }
        else
        {
            weapon.ui = GetComponent<UIDocument>();
            weapon.anim = weapon_anim;
        }
    }
}