using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

[CustomEditor(typeof(Ability))]
public class AbilityEditor : Editor
{
    /// <summary>
    /// 
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Ability ability = (Ability)target;
        
        // 2D boolean array for area of effect
        AreaOfEffect("Area Of Effect", ref ability.m_castArea);

        // tile targetting selection
        string[] targetableTiles = Enum.GetNames(typeof(TargetableTiles));
        ability.m_targetableTileMask = EditorGUILayout.MaskField("Targetable Tiles", ability.m_targetableTileMask, targetableTiles);

        Header("Effects");
        string[] effects = Enum.GetNames(typeof(AbilityEffects));
        ability.m_effectMask = EditorGUILayout.MaskField("Ability Effects", ability.m_effectMask, effects);

        List<int> effectIndexes = ConvertMaskToEnumIndex(ability.m_effectMask);
        
        for (int i = 0; i < effectIndexes.Count; i++)
        {
            switch ((AbilityEffects)effectIndexes[i])
            {
                case AbilityEffects.Damage:
                    EditorGUILayout.LabelField("Damage");
                    break;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="area"></param>
    private void AreaOfEffect(string label, ref RowData[] area)
    {
        Header("Area Of Effect");
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>
    private List<int> ConvertMaskToEnumIndex(int mask)
    {
        List<int> output = new List<int>();

        // nothing
        if (mask == 0)
            return output;

        // everything
        if (mask == -1)
            return null;

        string binary = Convert.ToString(mask, 2);

        int j = 0;

        for (int i = binary.Length - 1; i >= 0; i--)
        {
            if (binary[i] != '0')
                output.Add(j);

            j++;
        }

        return output;
    }
}

#endif