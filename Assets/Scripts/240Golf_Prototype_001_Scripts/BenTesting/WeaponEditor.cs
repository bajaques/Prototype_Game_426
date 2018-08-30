using UnityEngine;
using UnityEditor;

/**
	WARNING! DO NOT RUN WITHOUT NECESSARY KNOWLEDGE OF UNITY EDITOR INLINE SCRIPTS!!!!
 */
//[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{

    Weapon weapon;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        weapon = (Weapon)target;

        EditorGUILayout.LabelField("Weapon Helpers");

        if (GUILayout.Button("Save gun EQUIP location."))
        {
            Transform weaponT = weapon.transform;
            Vector3 weaponPos = weaponT.localPosition;
            Vector3 weaponRot = weaponT.localEulerAngles;
            weapon.weaponSettings.equipPosition = weaponPos;
            weapon.weaponSettings.equipRotation = weaponRot;
        }

        if (GUILayout.Button("Save gun UNEQUIP location."))
        {
            Transform weaponT = weapon.transform;
            Vector3 weaponPos = weaponT.localPosition;
            Vector3 weaponRot = weaponT.localEulerAngles;
            weapon.weaponSettings.unequipPosition = weaponPos;
            weapon.weaponSettings.unequipRotation = weaponRot;
        }

        EditorGUILayout.LabelField("Debug Positioning");

        if (GUILayout.Button("MOVE gun to EQUIP location"))
        {
            Transform weaponT = weapon.transform;
            weaponT.localPosition = weapon.weaponSettings.equipPosition;
            Quaternion eulerAngles = Quaternion.Euler(weapon.weaponSettings.equipRotation);
            weaponT.localRotation = eulerAngles;
        }

        if (GUILayout.Button("MOVE gun to UNEQUIP location"))
        {
            Transform weaponT = weapon.transform;
            weaponT.localPosition = weapon.weaponSettings.unequipPosition;
            Quaternion eulerAngles = Quaternion.Euler(weapon.weaponSettings.unequipRotation);
            weaponT.localRotation = eulerAngles;
        }
    }
}
