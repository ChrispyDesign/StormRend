/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Text.RegularExpressions;
using StormRend.Abilities;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public class EffectInspector : Editor
	{
		//Members
		public Ability owner;
		GUIStyle boldFoldoutStyle;

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
				boldFoldoutStyle = new GUIStyle(EditorStyles.foldout) 
					{ fontStyle = FontStyle.Bold };
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
				return false;
			}
		}
	}
}
