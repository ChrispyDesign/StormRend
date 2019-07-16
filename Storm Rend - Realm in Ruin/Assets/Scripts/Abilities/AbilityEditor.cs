using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

[CustomEditor(typeof(Ability))]
public class AbilityEditor : Editor
{
    private EffectEditor m_effectEditor;

    // the target ability object
    private static Ability m_ability;

    private bool m_foldOutAOE = false;

    #region getters

    public static Ability GetAbility() { return m_ability; }

    #endregion

    private void OnEnable()
    {
        m_ability = (Ability)target;
        m_effectEditor = new EffectEditor(m_ability.m_effects);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PrintAbilityInfo();

        Header("Casting");
        m_ability.m_gloryRequirement = EditorGUILayout.IntField("Glory Requirement", m_ability.m_gloryRequirement);
        m_ability.m_tilesToSelect = EditorGUILayout.IntField("Tiles To Select", m_ability.m_tilesToSelect);

        m_foldOutAOE = EditorGUILayout.Foldout(m_foldOutAOE, "Area Of Effect");

        if (m_foldOutAOE)
            AreaOfEffect(ref m_ability.m_castArea); // 2D boolean array for area of effect

        EditorGUI.BeginChangeCheck();
        {
            SerializedProperty targetableTiles = serializedObject.FindProperty("m_targetableTiles");
            EditorGUILayout.PropertyField(targetableTiles, true);
        }
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        m_effectEditor.PrintEffects();

        ProcessContextMenu();

        EditorUtility.SetDirty(m_ability);
    }

    private void ProcessContextMenu()
    {
        switch (Event.current.type)
        {
            case EventType.ContextClick:
                GenericMenu genericMenu = new GenericMenu();

                genericMenu.AddItem(new GUIContent("Add Effect/Damage"), false, () => m_effectEditor.AddEffect(typeof(DamageEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Heal"), false, () => m_effectEditor.AddEffect(typeof(HealEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Glory Gain"), false, () => m_effectEditor.AddEffect(typeof(GloryEffect)));

                genericMenu.ShowAsContext();

                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void PrintAbilityInfo()
    {
        Header("Ability Info");

        // 
        m_ability.m_name = EditorGUILayout.TextField("Name", m_ability.m_name);
        m_ability.m_icon = EditorGUILayout.ObjectField("Icon", m_ability.m_icon, typeof(Sprite), false) as Sprite;
        EditorGUILayout.LabelField("Description");
        m_ability.m_description = EditorGUILayout.TextArea(m_ability.m_description);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="area"></param>
    private void AreaOfEffect(ref RowData[] area)
    {
        EditorGUILayout.BeginVertical();
        EditorGUIUtility.labelWidth = 1;

        // columns
        for (int i = 0; i < area.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // rows
            for (int j = 0; j < area.Length; j++)
            {
                // make middle boolean "tile" green
                if (i == area.Length / 2 && j == area.Length / 2)
                {
                    // make middle boolean "tile" green
                    GUI.color = Color.green;
                    area[i].elements[j] = EditorGUILayout.Toggle(area[i].elements[j]);
                    GUI.color = Color.white;
                }
                else
                    area[i].elements[j] = EditorGUILayout.Toggle(area[i].elements[j]);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUIUtility.labelWidth = 0;
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    /// <summary>
    /// helper function which prints a space followed by a bold label
    /// </summary>
    /// <param name="label">the header label</param>
    private void Header(string label)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
    }

}

#endif