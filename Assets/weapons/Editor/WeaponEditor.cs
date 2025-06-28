#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Burst))]
public class WeaponEditor : Editor
{
    private readonly string[] tabs = { "Weapon Info","Damage", "Fire Rate", "Components", "VFX" };

    private int Tab
    {
        get => EditorPrefs.GetInt("WeaponEditor_tab", 0);
        set => EditorPrefs.SetInt("WeaponEditor_tab", value);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Tab = GUILayout.Toolbar(Tab, tabs);

        // Ordine: Weapon Info, Damage, Fire Rate, Components
        SerializedProperty IsLaser = serializedObject.FindProperty("IsLaser");
        SerializedProperty laser_range = serializedObject.FindProperty("laser_range");
        SerializedProperty has_ammo = serializedObject.FindProperty("has_ammo");
        SerializedProperty max_ammo = serializedObject.FindProperty("max_ammo");
        SerializedProperty ammo = serializedObject.FindProperty("ammo");

        SerializedProperty damage = serializedObject.FindProperty("damage");
        SerializedProperty dmgHasFallOff = serializedObject.FindProperty("dmg_has_dmg_fall_off");
        SerializedProperty dmgFallOf = serializedObject.FindProperty("dmg_fall_of");

        SerializedProperty fireRate = serializedObject.FindProperty("fire_rate");
        SerializedProperty time_between_bullet = serializedObject.FindProperty("time_between_bullet");

        SerializedProperty anim = serializedObject.FindProperty("anim");
        SerializedProperty ui = serializedObject.FindProperty("ui");
        SerializedProperty nuzzle = serializedObject.FindProperty("nuzzle_point"); // CORRETTO: il campo si chiama nuzzle_point nelle armi, non "nuzzle"
        SerializedProperty bulletTrailPrefab = serializedObject.FindProperty("bulletTrailPrefab");
        SerializedProperty bulletTrailSpeed = serializedObject.FindProperty("bulletTrailSpeed");

        GUILayout.Space(10);

        switch (Tab)
        {
            case 0: // Weapon Info
                EditorGUILayout.PropertyField(IsLaser);
                if (IsLaser.boolValue)
                {
                    EditorGUILayout.PropertyField(laser_range);
                }
                EditorGUILayout.PropertyField(has_ammo);
                if (!has_ammo.hasMultipleDifferentValues && has_ammo.boolValue)
                {
                    EditorGUILayout.PropertyField(max_ammo);
                    EditorGUILayout.PropertyField(ammo);
                }
                break;
            case 1: // Damage
                EditorGUILayout.PropertyField(damage);
                EditorGUILayout.PropertyField(dmgHasFallOff);
                if (!dmgHasFallOff.hasMultipleDifferentValues && dmgHasFallOff.boolValue)
                {
                    EditorGUILayout.PropertyField(dmgFallOf);
                }
                break;
            case 2: // Fire Rate
                EditorGUILayout.PropertyField(fireRate);
                EditorGUILayout.PropertyField(time_between_bullet);
                break;
            case 3: // Components
                EditorGUILayout.PropertyField(anim);
                EditorGUILayout.PropertyField(ui);
                EditorGUILayout.PropertyField(nuzzle); // ora mostra il campo corretto
                break;
            case 4: // VFX
                // Mostra i campi bulletTrailPrefab e bulletTrailSpeed solo se non è laser
                if (!IsLaser.boolValue)
                {
                    EditorGUILayout.PropertyField(bulletTrailPrefab);
                    EditorGUILayout.PropertyField(bulletTrailSpeed);
                }
                else
                {
                    EditorGUILayout.HelpBox("Bullet Trail is disabled for Laser weapons.", MessageType.Info);
                }
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(Fullauto))]
public class FullautoEditor : Editor
{
    private readonly string[] tabs = { "Weapon Info", "Damage", "Fire Rate", "Components", "VFX" };

    private int Tab
    {
        get => EditorPrefs.GetInt("FullautoEditor_tab", 0);
        set => EditorPrefs.SetInt("FullautoEditor_tab", value);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Tab = GUILayout.Toolbar(Tab, tabs);

        SerializedProperty IsLaser = serializedObject.FindProperty("IsLaser");
        SerializedProperty laser_range = serializedObject.FindProperty("laser_range");
        SerializedProperty has_ammo = serializedObject.FindProperty("has_ammo");
        SerializedProperty max_ammo = serializedObject.FindProperty("max_ammo");
        SerializedProperty ammo = serializedObject.FindProperty("ammo");

        SerializedProperty damage = serializedObject.FindProperty("damage");
        SerializedProperty dmgHasFallOff = serializedObject.FindProperty("dmg_has_dmg_fall_off");
        SerializedProperty dmgFallOf = serializedObject.FindProperty("dmg_fall_of");

        SerializedProperty fireRate = serializedObject.FindProperty("fire_rate");
        SerializedProperty anim = serializedObject.FindProperty("anim");
        SerializedProperty ui = serializedObject.FindProperty("ui");
        SerializedProperty nuzzle = serializedObject.FindProperty("nuzzle_point"); // CORRETTO: il campo si chiama nuzzle_point nelle armi, non "nuzzle"
        SerializedProperty bulletTrailPrefab = serializedObject.FindProperty("bulletTrailPrefab");
        SerializedProperty bulletTrailSpeed = serializedObject.FindProperty("bulletTrailSpeed");

        GUILayout.Space(10);

        switch (Tab)
        {
            case 0: // Weapon Info
                EditorGUILayout.PropertyField(IsLaser);
                if (IsLaser.boolValue)
                {
                    EditorGUILayout.PropertyField(laser_range);
                }
                EditorGUILayout.PropertyField(has_ammo);
                if (!has_ammo.hasMultipleDifferentValues && has_ammo.boolValue)
                {
                    EditorGUILayout.PropertyField(max_ammo);
                    EditorGUILayout.PropertyField(ammo);
                }
                break;
            case 1: // Damage
                EditorGUILayout.PropertyField(damage);
                EditorGUILayout.PropertyField(dmgHasFallOff);
                if (!dmgHasFallOff.hasMultipleDifferentValues && dmgHasFallOff.boolValue)
                {
                    EditorGUILayout.PropertyField(dmgFallOf);
                }
                break;
            case 2: // Fire Rate
                EditorGUILayout.PropertyField(fireRate);
                break;
            case 3: // Components
                EditorGUILayout.PropertyField(anim);
                EditorGUILayout.PropertyField(ui);
                EditorGUILayout.PropertyField(nuzzle); // ora mostra il campo corretto
                break;
            case 4: // VFX
                // Mostra i campi bulletTrailPrefab e bulletTrailSpeed solo se non è laser
                if (!IsLaser.boolValue)
                {
                    EditorGUILayout.PropertyField(bulletTrailPrefab);
                    EditorGUILayout.PropertyField(bulletTrailSpeed);
                }
                else
                {
                    EditorGUILayout.HelpBox("Bullet Trail is disabled for Laser weapons.", MessageType.Info);
                }
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(Semiauto))]
public class SemiautoEditor : Editor
{
    private readonly string[] tabs = { "Weapon Info", "Damage", "Fire Rate", "Components", "VFX" };

    private int Tab
    {
        get => EditorPrefs.GetInt("SemiautoEditor_tab", 0);
        set => EditorPrefs.SetInt("SemiautoEditor_tab", value);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Tab = GUILayout.Toolbar(Tab, tabs);

        SerializedProperty IsLaser = serializedObject.FindProperty("IsLaser");
        SerializedProperty laser_range = serializedObject.FindProperty("laser_range");
        SerializedProperty has_ammo = serializedObject.FindProperty("has_ammo");
        SerializedProperty max_ammo = serializedObject.FindProperty("max_ammo");
        SerializedProperty ammo = serializedObject.FindProperty("ammo");

        SerializedProperty damage = serializedObject.FindProperty("damage");
        SerializedProperty dmgHasFallOff = serializedObject.FindProperty("dmg_has_dmg_fall_off");
        SerializedProperty dmgFallOf = serializedObject.FindProperty("dmg_fall_of");

        SerializedProperty fireRate = serializedObject.FindProperty("fire_rate");
        SerializedProperty anim = serializedObject.FindProperty("anim");
        SerializedProperty ui = serializedObject.FindProperty("ui");
        SerializedProperty nuzzle = serializedObject.FindProperty("nuzzle_point"); // CORRETTO: il campo si chiama nuzzle_point nelle armi, non "nuzzle"
        SerializedProperty bulletTrailPrefab = serializedObject.FindProperty("bulletTrailPrefab");
        SerializedProperty bulletTrailSpeed = serializedObject.FindProperty("bulletTrailSpeed");

        GUILayout.Space(10);

        switch (Tab)
        {
            case 0: // Weapon Info
                EditorGUILayout.PropertyField(IsLaser);
                if (IsLaser.boolValue)
                {
                    EditorGUILayout.PropertyField(laser_range);
                }
                EditorGUILayout.PropertyField(has_ammo);
                if (!has_ammo.hasMultipleDifferentValues && has_ammo.boolValue)
                {
                    EditorGUILayout.PropertyField(max_ammo);
                    EditorGUILayout.PropertyField(ammo);
                }
                break;
            case 1: // Damage
                EditorGUILayout.PropertyField(damage);
                EditorGUILayout.PropertyField(dmgHasFallOff);
                if (!dmgHasFallOff.hasMultipleDifferentValues && dmgHasFallOff.boolValue)
                {
                    EditorGUILayout.PropertyField(dmgFallOf);
                }
                break;
            case 2: // Fire Rate
                EditorGUILayout.PropertyField(fireRate);
                break;
            case 3: // Components
                EditorGUILayout.PropertyField(anim);
                EditorGUILayout.PropertyField(ui);
                EditorGUILayout.PropertyField(nuzzle); // ora mostra il campo corretto
                break;
            case 4: // VFX
                // Mostra i campi bulletTrailPrefab e bulletTrailSpeed solo se non è laser
                if (!IsLaser.boolValue)
                {
                    EditorGUILayout.PropertyField(bulletTrailPrefab);
                    EditorGUILayout.PropertyField(bulletTrailSpeed);
                }
                else
                {
                    EditorGUILayout.HelpBox("Bullet Trail is disabled for Laser weapons.", MessageType.Info);
                }
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif