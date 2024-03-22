using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "HealthModifier", menuName = "2Gether/Entities/HealthModifier")]
public class HealthModifier : ScriptableObject
{
    public float _maxHealth = 100f;
    public float _damageCooldown = 0.25f;
    public float[] damageModifiers = Enumerable.Repeat(1f, 7).ToArray();

    public float ApplyDamage(IDamagable target, ref float damageCooldownTime, Damage damage)
    {
        if (Time.time < damageCooldownTime)
        {
            return 0f;
        }

        damageCooldownTime = Time.time + _damageCooldown;
        float finalDmg = ApplyModifiers(damage);
        target.Health -= finalDmg;
        return finalDmg;
    }

    private float ApplyModifiers(Damage damage)
    {
        return damage.value * damageModifiers[(byte)damage.type];
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(HealthModifier))]
public class HealthModifierEditor : Editor
{
    private HealthModifier HealthModifier;
    private SerializedProperty healthModifierProp;
    private bool showDamageModifiers = false;

    private void OnEnable()
    {
        HealthModifier = (HealthModifier)target;
        healthModifierProp = serializedObject.FindProperty("damageModifiers");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxHealth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_damageCooldown"));

        showDamageModifiers = EditorGUILayout.Foldout(showDamageModifiers, "Damage Modifiers");
        if (showDamageModifiers)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < HealthModifier.damageModifiers.Length; i++)
            {
                HealthModifier.damageModifiers[i] = EditorGUILayout.Slider(((DamageType)i).ToString(), HealthModifier.damageModifiers[i], 0f, 4f);
            }
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif