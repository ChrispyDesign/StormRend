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

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        m_ability = (Ability)target;
        m_effectEditor = new EffectEditor(m_ability.GetEffects());
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnInspectorGUI()
    {
        // update the ability
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        {
            PrintAbilityInfo();
            PrintAbilityCasting();
        }
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        m_effectEditor.PrintEffects();

        ProcessContextMenu();

        // allow ability serialization
        EditorUtility.SetDirty(m_ability);
    }

    /// <summary>
    /// 
    /// </summary>
    private void PrintAbilityInfo()
    {
        AbilityEditorUtility.PrintHeader("Ability Info");

        // 
        AbilityEditorUtility.PropertyField(serializedObject, "m_name");
        AbilityEditorUtility.PropertyField(serializedObject, "m_icon");
        AbilityEditorUtility.PropertyField(serializedObject, "m_description");
    }

    /// <summary>
    /// 
    /// </summary>
    private void PrintAbilityCasting()
    {
        AbilityEditorUtility.PrintHeader("Casting");
        AbilityEditorUtility.PropertyField(serializedObject, "m_gloryRequirement");
        AbilityEditorUtility.PropertyField(serializedObject, "m_tilesToSelect");

        m_foldOutAOE = EditorGUILayout.Foldout(m_foldOutAOE, "Area Of Effect");

        if (m_foldOutAOE)
            AbilityEditorUtility.ToggleMatrix(ref m_ability.m_castArea); // 2D boolean array for area of effect

        AbilityEditorUtility.PropertyField(serializedObject, "m_targetableTiles", true);
    }

    /// <summary>
    /// use right-click context menu for adding new effects
    /// </summary>
    private void ProcessContextMenu()
    {
        switch (Event.current.type)
        {
            // context click = right-click
            case EventType.ContextClick:
                GenericMenu genericMenu = new GenericMenu();

                genericMenu.AddItem(new GUIContent("Add Effect/Offense/Damage"), false, () => m_effectEditor.AddEffect(typeof(DamageEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Offense/Push"), false, () => m_effectEditor.AddEffect(typeof(PushEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Offense/Summon"), false, () => m_effectEditor.AddEffect(typeof(SummonEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Glory Gain"), false, () => m_effectEditor.AddEffect(typeof(GloryEffect)));

				//Defensive
                genericMenu.AddItem(new GUIContent("Add Effect/Defensive/Teleport"), false, () => m_effectEditor.AddEffect(typeof(TeleportEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Defensive/Swap Units"), false, () => m_effectEditor.AddEffect(typeof(SwapUnitEffect)));

                // recovery
                genericMenu.AddItem(new GUIContent("Add Effect/Recovery/Heal"), false, () => m_effectEditor.AddEffect(typeof(HealEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Recovery/Refresh"), false, () => m_effectEditor.AddEffect(typeof(RefreshEffect)));

                // runes
                genericMenu.AddItem(new GUIContent("Add Effect/Runes/Protection"), false, () => m_effectEditor.AddEffect(typeof(ProtectEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Runes/Taunting"), false, () => m_effectEditor.AddEffect(typeof(TauntEffect)));

                // curses
                genericMenu.AddItem(new GUIContent("Add Effect/Curses/Blinding"), false, () => m_effectEditor.AddEffect(typeof(BlindEffect)));
                genericMenu.AddItem(new GUIContent("Add Effect/Curses/Crippling"), false, () => m_effectEditor.AddEffect(typeof(CrippleEffect)));

                genericMenu.ShowAsContext();

                break;
        }
    }
}

#endif