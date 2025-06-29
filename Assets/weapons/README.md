# FPS Weapons System - Documentation

## Overview

This system manages weapons for a first-person shooter in Unity.  
It supports multiple weapon types (Fullauto, Semiauto, Burst), ammo management, UI integration, and VFX (bullet trails and laser lines).

---

## Main Classes

- **Weapon**: Base class for all weapons. Handles damage, ammo, UI, and animator references.
- **Hitscan**: Inherits from Weapon. Handles hitscan shooting logic, bullet trails, laser lines, and reload logic.
- **Fullauto, Semiauto, Burst**: Inherit from Hitscan. Implement specific firing behaviors.

---

## Key Features

- **Ammo System**:  
  - `has_ammo`, `max_ammo`, `ammo` fields.
  - `Reload()` logic with animation trigger ("reload").
  - Prevents shooting while reloading.

- **UI Integration**:  
  - Uses a `UIDocument` and a `Label` named "UIammo" to display ammo count.
  - UI updated automatically after each shot and reload.

- **Bullet Trail**:  
  - Assign a `TrailRenderer` prefab to `bulletTrailPrefab`.
  - Set trail speed with `bulletTrailSpeed`.
  - Bullet trails are only used for non-laser weapons.

- **Laser Line**:  
  - Assign a `LineRenderer` prefab to `laserLinePrefab`.
  - The laser is visible only while firing and updates its position in real time.
  - The laser starts from the nuzzle and goes toward the point aimed by the camera.

- **Weapon Selection**:  
  - The main weapon is selected by searching for a child with the tag **"MainWeapon"**.
  - **Important:** You must assign the tag "MainWeapon" to the GameObject of your main weapon in the Unity Inspector.  
    Only the weapon with this tag will be used as the active weapon by the `CharacterInfo` script.

---

## Setup Instructions

1. **Prefab Setup**:  
   - Add the desired weapon script (e.g., Fullauto, Semiauto, Burst) to your weapon GameObject.
   - **Assign the "MainWeapon" tag to the main weapon GameObject.**
   - Assign the `Animator` and `UIDocument` references.

2. **UI**:  
   - The UI document must have a `Label` with the name "UIammo" for ammo display.

3. **Bullet Trail**:  
   - Create a `TrailRenderer` prefab and assign it to `bulletTrailPrefab` in the inspector.

4. **Laser Line**:  
   - Create a `LineRenderer` prefab and assign it to `laserLinePrefab` in the inspector.
   - The laser will be shown only for weapons with `IsLaser = true`.

5. **Reload Animation**:  
   - The reload animation must call the method `OnReloadAnimationEnd()` via Animation Event at the end of the reload.

---

## Example: CharacterInfo.cs

```csharp
// ...existing code...
private void SetUpWeapon()
{
    Weapon[] allWeapons = GetComponentsInChildren<Weapon>(true);
    foreach (Weapon w in allWeapons)
    {
        if (w != null && w.CompareTag("MainWeapon"))
        {
            weapon = w;
            break;
        }
    }
    if (weapon != null)
    {
        weapon.ui = GetComponent<UIDocument>();
        weapon.anim = weapon_anim;
    }
}
// ...existing code...
```

---

## Notes

- **The "MainWeapon" tag is required**: Only the weapon GameObject with this tag will be used as the player's active weapon.
- Always assign the correct tag and references in the inspector.
- For custom VFX, use the "VFX" tab in the custom inspector.
- Bullet trails are automatically hidden for laser weapons.
- The laser line is visible only while firing and updates every frame.
- The system is modular: you can extend or override methods for custom behaviors.

---