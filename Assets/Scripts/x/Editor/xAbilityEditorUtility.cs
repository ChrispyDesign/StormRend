using UnityEngine;
using UnityEditor;
using StormRend.Abilities;

//RENAME THIS TO SREditorUtility


/// <summary>
/// container class which provides helper functions for the AbilityEditor class
/// </summary>
public class AbilityEditorUtility : Editor
{
    /// <summary>
    /// helper function which prints a space followed by a bold label
    /// </summary>
    /// <param name="label">the header label</param>
    public static void PrintHeader(string label)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
    }

    /// <summary>
    /// helper function which prints a field in the same format that the default unity inspector does
    /// </summary>
    /// <param name="serializedObject">the ability as a serialized object</param>
    /// <param name="property">the variable name</param>
    /// <param name="includeChildren">does the property have children?</param>
    public static void PropertyField(SerializedObject serializedObject, string property, bool includeChildren = false)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(property), includeChildren);
    }

    /// <summary>
    /// helper function which prints a 2D array of toggles to the editor
    /// </summary>
    /// <param name="area">a 2D array of booleans</param>
    public static void ToggleMatrix(ref RowData[] area)
    {
        // begin verticle group for rows
        EditorGUILayout.BeginVertical();
        {
            // minimize label width, as these toggles won't have labels
            EditorGUIUtility.labelWidth = 1;

            for (int i = 0; i < area.Length; i++)
            {
                // begin horizontal group for columns
                EditorGUILayout.BeginHorizontal();
                {
                    for (int j = 0; j < area.Length; j++)
                    {
                        // make middle toggle "tile" green
                        if (i == area.Length / 2 && j == area.Length / 2)
                            GUI.color = Color.green;

                        // print toggle "tile"
                        area[i].elements[j] = EditorGUILayout.Toggle(area[i].elements[j]);
                        GUI.color = Color.white;
                    }

                    // left-align matrix using flexible space
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
            }

            // revert label width to unity default
            EditorGUIUtility.labelWidth = 0;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }
}