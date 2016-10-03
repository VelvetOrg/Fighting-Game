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

    //Allow th burst rate values depending on the state of the enum
    public override void OnInspectorGUI()
    {
        //Get gun class
        Gun g = (Gun)target;

        //Draw other values
        //Temporary
        g.end = EditorGUILayout.ObjectField("Barrel", g.end, typeof(Transform), true) as Transform;
        g.damage = EditorGUILayout.FloatField("Damage", g.damage);
        g.attackDuration = EditorGUILayout.FloatField("Attack Duration", g.attackDuration);
        g.range = EditorGUILayout.Slider("Range", g.range, 10, 100);
        g.fireRate = EditorGUILayout.Slider("Fire rate", g.fireRate, 0, 200);

        EditorGUILayout.PropertyField(layer);
        EditorGUI.showMixedValue = layer.hasMultipleDifferentValues;

        g.fireMode = (Gun.Mode)EditorGUILayout.EnumPopup("Fire mode", g.fireMode);
        
        //Get enum and draw burst count if active
        if (g.fireMode == Gun.Mode.Burst) { g.burstCount = EditorGUILayout.IntField("Burst count", g.burstCount); }
    }
}
