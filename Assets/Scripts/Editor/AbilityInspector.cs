/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections.Generic;
using StormRend.Abilities;
using StormRend.Abilities.Effects;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
    [CustomEditor(typeof(Ability)), CanEditMultipleObjects]
    public class AbilityInspector : SmartEditor
    {
        /* Brainstorm
		----------- Layout
		Main
		- Ability Name in title font
		- Icon preview
		- Icon : Sprite
		- Animation Number : int
		- Level : int
		- Type : AbilityType
		- Description : string

		Casting
		- Glory Cost : int
		- Required Tiles : int
		- Target Tiles : Bitmask
		- AreaOfEffect : bool[7,7]

		- EffectInspectors
		 */

        //Members
        EffectInspector ei;
        Ability a;      //Target
        bool areaOfEffectFoldout = false;
        GUIStyle titleStyle;

        public override string[] propertiesToExclude => new[] { "m_Script" };

        void OnEnable()
        {
            //Inits
            a = target as Ability;
			ei = CreateInstance<EffectInspector>();
			ei.owner = a;
            // ei = new EffectInspector(a);

            titleStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
                fontSize = 16,
            };
        }

        public override void OnPreInspector()
        {
            GUILayout.Label(a.name, titleStyle);
            DrawIconPreview();
        }

        public override void OnPostInspector()
        {
            DrawCastAreaMatrix();
            DrawEffectItems();
            DrawAddEffectButton();
            // HandleContextMenu();
        }

        #region Assists
        void DrawIconPreview()
        {
            if (!a.icon) return;
            var rect = GUILayoutUtility.GetRect(0, 0, 75, 75);
            EditorGUI.DrawPreviewTexture(rect, a.icon.texture, null, ScaleMode.ScaleToFit);
            GUILayout.Space(5);
        }

        void DrawCastAreaMatrix()
        {
            // GUILayout.Space(5);
            areaOfEffectFoldout = EditorGUILayout.Foldout(areaOfEffectFoldout, "Cast Area");

            if (!areaOfEffectFoldout) return;

            Color oldGUICol = GUI.color;
            int sqrLen = Ability.castAreaSqrLen;

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.MinWidth(90)))
                {
                    if (GUILayout.Button("None", GUILayout.Height(39))) None();
                    if (GUILayout.Button("All", GUILayout.Height(39))) All();
                    if (GUILayout.Button("Invert", GUILayout.Height(39))) Invert();
                }
                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUIUtility.labelWidth = 1;
                    for (int i = 0; i < sqrLen; ++i)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            for (int j = 0; j < sqrLen; ++j)
                            {
                                //Highlight middle tile
                                if (i == sqrLen / 2 && j == sqrLen / 2)
                                {
                                    GUI.color = Color.red;
                                }

                                //Print "Tile"
                                a.castArea[i * sqrLen + j] = EditorGUILayout.Toggle(a.castArea[i * sqrLen + j]);

                                //Reset
                                GUI.color = oldGUICol;
                            }
                            GUILayout.FlexibleSpace();
                        }
                    }
                    EditorGUIUtility.labelWidth = 0;
                }
            }

            void None()
            {
                a.castArea = new bool[sqrLen * sqrLen];
            }
            void All()
            {
                for (int i = 0; i < a.castArea.Length; i++)
                    a.castArea[i] = true;
            }
            void Invert()
            {
                for (int i = 0; i < a.castArea.Length; i++)
                    a.castArea[i] = !a.castArea[i];
            }
        }

        void DrawEffectItems()
        {
            foreach (var e in a.effects)
            {
                if (ei.DrawHeader(e)) return;   //Break out if the close button is pressed

                if (e.isFoldOut)
                {
                    var effectInspector = CreateEditor(e);
                    effectInspector.OnInspectorGUI();
                }
            }
        }

        void DrawAddEffectButton()
        {
            GUILayout.Space(10);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                //This is the easiest way I could be bothered to calc the bottom left corner of the button
                var r = GUILayoutUtility.GetLastRect();
                var addEffectBottomLeftCorner = new Rect(r.xMax, r.yMax, 0, 20);
                if (GUILayout.Button("Add Effect", GUILayout.MaxWidth(150)))
                {
                    AddEffectDropdownMenu(addEffectBottomLeftCorner);
                }

                GUILayout.FlexibleSpace();
            }
        }

        void AddEffectDropdownMenu(Rect rect)
        {
            GenericMenu m = new GenericMenu();

            //Offense
            m.AddItem(new GUIContent("Damage"), false, () => a.AddEffect<DamageEffect>());
            m.AddItem(new GUIContent("Push"), false, () => a.AddEffect<PushEffect>());
            m.AddItem(new GUIContent("Summon"), false, () => a.AddEffect<SummonEffect>());
            m.AddItem(new GUIContent("Gain Glory"), false, () => a.AddEffect<GainGloryEffect>());

            //Defense
            m.AddSeparator("");
            m.AddItem(new GUIContent("Teleport"), false, () => a.AddEffect<TeleportEffect>());
            m.AddItem(new GUIContent("Swap Units"), false, () => a.AddEffect<SwapUnitEffect>());

            //Recovery
            m.AddSeparator("");
            m.AddItem(new GUIContent("Heal"), false, () => a.AddEffect<HealEffect>());
            m.AddItem(new GUIContent("Refresh"), false, () => a.AddEffect<RefreshEffect>());

            //Runes
            m.AddSeparator("");
            m.AddItem(new GUIContent("Protect"), false, () => a.AddEffect<ProtectEffect>());
            m.AddItem(new GUIContent("Taunt"), false, () => a.AddEffect<TauntEffect>());
            m.AddItem(new GUIContent("Gain Glory When Attacked"), false, () => a.AddEffect<GainGloryWhenAttackedEffect>());
			
            //Curses
            m.AddSeparator("");
            m.AddItem(new GUIContent("Blind"), false, () => a.AddEffect<BlindEffect>());
            m.AddItem(new GUIContent("Immobilise"), false, () => a.AddEffect<ImmobiliseEffect>());

			//Passive
			m.AddSeparator("");
			m.AddItem(new GUIContent("SoulReap"), false, () => a.AddEffect<SoulReapEffect>());

			//VFX
			m.AddSeparator("");
			m.AddItem(new GUIContent("Crater"), false, () => a.AddEffect<CraterEffect>());

            m.DropDown(rect);
        }
        #endregion
    }
}
