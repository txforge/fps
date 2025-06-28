using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public bool dmg_has_dmg_fall_off = false;
    public float dmg_fall_of = 100f;
    public float fire_rate = 0.15f; // tempo tra i colpi

    public bool has_ammo = true; // Indica se l'arma ha munizioni
    public int max_ammo = 30; // Indica il numero massimo di munizioni
    public int ammo; // Indica il numero corrente di munizioni
    public Animator anim = null; // Animatore per le animazioni di sparo

    public UIDocument ui;
    protected Label UIammo; // Label per mostrare le munizioni
    

    protected virtual void Awake()
    {
        if (has_ammo && ammo == 0)
            ammo = max_ammo;
    }

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("Animator non assegnato! Assicurati di assegnare un Animator al GameObject.");
        }
        if (ui == null)
        {
            Debug.LogWarning("UI non assegnata! Assicurati di assegnare un UIDocument per l'interfaccia utente.");
        }
        else
        {
            UIammo = ui.rootVisualElement.Q<Label>("UIammo");
            if (UIammo != null)
            {
                if (has_ammo)
                    UIammo.text = $"{ammo} / {max_ammo}";
                else
                    UIammo.text = "∞";
            }
            else
            {
                Debug.LogWarning("Label 'UIammo' non trovata nel UIDocument.(Script Weapon)");
            }
        }
    }

    // Controlla se puoi sparare (override nelle armi se serve)
    public virtual bool CanShoot()
    {
        if (!has_ammo) return true;
        return ammo > 0;
    }

    // Consuma una munizione se necessario
    public virtual void ConsumeAmmo()
    {
        if (has_ammo && ammo > 0)
            ammo--;
    }

    // Ricarica l'arma
    public virtual void Reload()
    {
        if (has_ammo)
            ammo = max_ammo;
    }
}
