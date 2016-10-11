/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using UnityEditor;

//This makes creating guns cleaner in the editor
[CustomEditor(typeof(Gun))]
public class GunEditor : Editor
{
    //Get Properties that are conrollable via an enum
    public SerializedProperty fireState;
    public SerializedProperty burstCount;
    public SerializedProperty layer;

    //Grab
    void OnEnable()
    {
        fireState  = serializedObject.FindProperty("fireMode");
        burstCount = serializedObject.FindProperty("burstCount");
        layer      = serializedObject.FindProperty("ignoreLayers");
    }

    //Allow the burst rate values depending on the state of the enum
    public override void OnInspectorGUI()
    {
        //Get gun class
        Gun gun = (Gun)target;

        //Draw other values
        //Temporary
        gun.end = EditorGUILayout.ObjectField("Barrel", gun.end, typeof(Transform), true) as Transform;
        gun.damageGiven = EditorGUILayout.FloatField("Damage", gun.damageGiven);
        gun.attackDuration = EditorGUILayout.FloatField("Attack Duration", gun.attackDuration);
        gun.range = EditorGUILayout.Slider("Range", gun.range, 10, 100);
        gun.physicsForce = EditorGUILayout.Slider("Physics force", gun.physicsForce, 0, 500);
        gun.fireRate = EditorGUILayout.Slider("Fire rate", gun.fireRate, 0, 200);
        gun.damageGiven = EditorGUILayout.FloatField("Damage", gun.damageGiven);
        EditorGUILayout.PropertyField(layer);
        EditorGUI.showMixedValue = layer.hasMultipleDifferentValues;

        gun.fireMode = (Gun.FireMode)EditorGUILayout.EnumPopup("Fire mode", gun.fireMode);
        gun.useShellEjection = EditorGUILayout.Toggle("Use Shell Ejection", gun.useShellEjection);

        //Get enum and draw burst count if active
        if (gun.fireMode == Gun.FireMode.Burst) { gun.burstCount = EditorGUILayout.IntField("Burst count", gun.burstCount); }
        if (gun.useShellEjection == true)
        {
            gun.shellEjectionPoint = EditorGUILayout.ObjectField("Shell Ejection", gun.shellEjectionPoint, typeof(Transform), true) as Transform;
            gun.shellPrefab = EditorGUILayout.ObjectField("Shell Prefab", gun.shellPrefab, typeof(GameObject), true) as GameObject;
        }
    }
}
