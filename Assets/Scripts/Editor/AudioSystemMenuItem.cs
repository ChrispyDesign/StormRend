using StormRend.Audio;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors.MenuItems
{
    /// <summary>
    /// Logic to display menu item context menu
    /// </summary>
    [CustomEditor(typeof(AudioSystem))]
    public class AudioSystemMenuItem : SmartEditor
    {
		string help = "Animation Event Handlers: \nPlayOnce(AudioClip)\nChancePlayMagazine(AudioMagazine)";
        public override string[] propertiesToExclude => new[]{ "m_Script" };

        [MenuItem("GameObject/StormRend/AudioSystem", false, 10)]
        static void CreateGameObject(MenuCommand menuCommand)
        {
            var newo = new GameObject("AudioSystem", typeof(AudioSystem));
            GameObjectUtility.SetParentAndAlign(newo, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(newo, "Create StormRend AudioSystem");
            Selection.activeObject = newo;
        }

		public override void OnPreInspector()
		{
			EditorGUILayout.HelpBox(help, MessageType.Info);
		}
    }
}
