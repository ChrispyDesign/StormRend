using StormRend.Abilities;
using StormRend.Abilities.Effects;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
    [CustomEditor(typeof(Ability))]
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
		- Type : AbiltyType
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
            ei = new EffectInspector(a);

            titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
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
                                    // GUI.enabled = false;
                                }

                                //Print "Tile"
                                a.castArea[i * sqrLen + j] = EditorGUILayout.Toggle(a.castArea[i * sqrLen + j]);

                                //Reset
                                GUI.color = oldGUICol;
                                // GUI.enabled = true;
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

            m.AddItem(new GUIContent("Damage"), false, () => a.AddEffect<DamageEffect>());
            m.AddItem(new GUIContent("Push"), false, () => a.AddEffect<PushEffect>());
            m.AddItem(new GUIContent("Summon"), false, () => a.AddEffect<SummonEffect>());
            m.AddItem(new GUIContent("Gain Glory"), false, () => a.AddEffect<GainGloryEffect>());
            m.AddSeparator("");
            m.AddItem(new GUIContent("Teleport"), false, () => a.AddEffect<TeleportEffect>());
            m.AddItem(new GUIContent("Swap Units"), false, () => a.AddEffect<SwapUnitEffect>());
            m.AddSeparator("");
            m.AddItem(new GUIContent("Heal"), false, () => a.AddEffect<HealEffect>());
            m.AddItem(new GUIContent("Refresh?"), false, () => a.AddEffect<RefreshEffect>());
            m.AddSeparator("");
            // m.AddItem(new GUIContent("Protection"), false, () => a.AddEffect<ProtectEffect>());
            // m.AddItem(new GUIContent("Taunting"), false, () => a.AddEffect<TauntEffect>());
            m.AddSeparator("");
            m.AddItem(new GUIContent("Blinding"), false, () => a.AddEffect<BlindEffect>());
            m.AddItem(new GUIContent("Crippling"), false, () => a.AddEffect<CrippleEffect>());

            m.DropDown(rect);
        }
        #endregion
    }
}