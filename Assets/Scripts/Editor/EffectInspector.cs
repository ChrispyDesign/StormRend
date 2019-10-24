using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StormRend.Abilities;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public class EffectInspector : Editor
	{
		//Members
		Ability owner;
		GUIStyle boldFoldoutStyle;

		public EffectInspector(Ability ability)
		{
			owner = ability;
		}

		void OnEnable()
		{
			//Create a bold foldout GUI style
			boldFoldoutStyle = new GUIStyle()
			{
				fontStyle = FontStyle.Bold
			};
		}

		public void DrawGUI()
		{
			foreach (var e in owner.effects)
			{
				DrawHeader(e);

				if (e && e.isFoldOut)
				{
					Editor ee = CreateEditor(e);
					ee.OnInspectorGUI();
				}
			}
		}

		/// <summary>
		/// Draws the header for the effect.
		/// </summary>
		/// <returns>Returns true if the close button is pressed</returns>
		public bool DrawHeader(Effect e)
		{
			GUILayout.Space(3);
			using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
			{
				var formattedName = Regex.Replace(e.name, "[E-e]ffect", "");
				// var formattedName = Regex.Replace(e.name, "[([a-z])([A-Z])]", "$1 $2");
				e.isFoldOut = EditorGUILayout.Foldout(e.isFoldOut, formattedName, true, boldFoldoutStyle);

				GUILayout.FlexibleSpace();

				//Close button
				if (GUILayout.Button("×"))
				{
					owner.RemoveEffect(e);
					return true;
				} 
			}
			return false;
		}
	}
}