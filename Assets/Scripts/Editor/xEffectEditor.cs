using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StormRend.Abilities.Effects;

/// <summary>
/// Helper editor class used to display effects in the unity inspector.
/// Also handles the addition & removal of effects
/// </summary>
public class xEffectEditor : Editor
{
    // the effects to edit
    private List<xEffect> m_effects;

    /// <summary>
    /// Constructor which caches a reference to an ability's list of effects
    /// </summary>
    /// <param name="effects"></param>
    public xEffectEditor(List<xEffect> effects)
    {
        m_effects = effects;
    }

    /// <summary>
    /// displays the effect's fields on the unity inspector
    /// </summary>
    public void PrintEffects()
    {
        for (int i = 0; i < m_effects.Count; i++)
        {
            xEffect effect = m_effects[i];

            // print effect header (two lines, a title and a remove 'X' button)
            PrintEffectHeader(effect);

            // check if the effect still exists (can be deleted by header)
            if (effect != null && effect.m_isFoldOut)
            {
                // call the effect's OnInspectorGUI function to display it's serialized fields
                Editor effectEditor = CreateEditor(effect);
                effectEditor.OnInspectorGUI();
            }
        }
    }

    /// <summary>
    /// helper function which prints a header for an effect in a particular format, e.g. two
    /// lines, a title label and a remove 'X' button.
    /// </summary>
    /// <param name="effect">the effect to print the header of</param>
    public void PrintEffectHeader(xEffect effect)
    {
        EditorGUILayout.Space();

        // begin a horizontal group, for printing header and remove button on the same line
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        {
            // print bold foldout label for header
            GUIStyle boldFoldout = new GUIStyle(EditorStyles.foldout);
            boldFoldout.fontStyle = FontStyle.Bold;
            effect.m_isFoldOut = EditorGUILayout.Foldout(effect.m_isFoldOut, effect.name, true, boldFoldout);

            // right-align remove button using flexible space
            GUILayout.FlexibleSpace();

            // print remove button
            if (GUILayout.Button("X"))
                RemoveEffect(effect); // handle effect removal
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// adds an effect to an ability. Does this by creating an instance of the effect, adding it
    /// to the ability's list of effects, then parenting the effect asset to the ability asset
    /// </summary>
    /// <param name="effectType">the effect to add</param>
    public void AddEffect(System.Type effectType)
    {
        // instantiate the effect asset
        xEffect effect = CreateInstance(effectType) as xEffect;
        effect.name = effectType.Name;

        // add to ability
        m_effects.Add(effect);

        // parent effect to ability asset
        string path = AssetDatabase.GetAssetPath(xAbilityEditor.GetTarget());
        AssetDatabase.AddObjectToAsset(effect, path);

        // hide effect asset from unity project window
        effect.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.SaveAssets(); // save!
    }

    /// <summary>
    /// removes an effect from an ability. Does this by removing the effect from the ability's
    /// list of effects, then destroying the effect asset
    /// </summary>
    /// <param name="effect">the effect to remove</param>
    private void RemoveEffect(xEffect effect)
    {
        // remove from ability
        m_effects.Remove(effect);

        // destroy the asset!
        DestroyImmediate(effect, true);
        AssetDatabase.SaveAssets(); // save!
    }
}