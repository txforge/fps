using UnityEngine;
using UnityEngine.UIElements;

public class CharacterInfo : MonoBehaviour
{
    public Weapon weapon; // Solo quella con tag "MainWeapon"
    public Animator weapon_anim;

    

    private void Awake()
    {
        SetUpWeapon();
    }
    private void SetUpWeapon()
    {
        // Trova tutte le armi nei figli e nei figli dei figli (ricorsivo)
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
            Debug.LogError("Arma con tag 'MainWeapon' non trovata! Assicurati di assegnare il tag.");
        }
        else
        {
            weapon.ui = GetComponent<UIDocument>();
            weapon.anim = weapon_anim;
        }
    }
}