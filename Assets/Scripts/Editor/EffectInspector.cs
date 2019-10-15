using System;
using System.Collections.Generic;
using StormRend.Abilities;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public class EffectInspector : SmartEditor
	{
		//Members
		Ability owner;
		List<Effect> effects;
		GUIStyle boldFoldoutStyle;

		public EffectInspector(Ability ability)
		{
			owner = ability;
			this.effects = ability.effects;
		}

		public EffectInspector(List<Effect> effects)    //Probably don't need this
		{
			this.effects = effects;
		}

		void OnEnable()
		{
			//Create a bold foldout GUI style
			boldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
			boldFoldoutStyle.fontStyle = FontStyle.Bold;
		}

		public void DrawGUI()
		{
			foreach (var e in effects)
			{
				DrawHeader(e);

				if (e && e.isFoldOut)
				{
					Editor ee = CreateEditor(e);
					ee.OnInspectorGUI();
				}
			}
		}

		void DrawHeader(Effect e)
		{
			EditorGUILayout.Space();
			using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
			{
				//Fold out
				e.isFoldOut = EditorGUILayout.Foldout(e.isFoldOut, e.name, true, boldFoldoutStyle);

				GUILayout.FlexibleSpace();

				//Close button
				if (GUILayout.Button("×")) RemoveEffect(e);
			}
		}

		/// <summary>
		/// Adds an effect of type T to the declared owner
		/// </summary>
		public void AddEffect<T>() where T : Effect
		{
			//Create
			var eNew = CreateInstance<T>();
			eNew.name = eNew.GetType().Name;
			owner.effects.Add(eNew);

			//Add to ability SO
			AssetDatabase.AddObjectToAsset(eNew, owner);

			//Hide
			eNew.hideFlags = HideFlags.HideInHierarchy;
			AssetDatabase.SaveAssets();
		}

		void RemoveEffect(Effect e)
		{
			owner.effects.Remove(e);
			DestroyImmediate(e, true);
			AssetDatabase.SaveAssets();
		}
	}
}