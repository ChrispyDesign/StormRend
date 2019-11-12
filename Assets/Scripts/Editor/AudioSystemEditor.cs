using StormRend.Audio;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
    /// <summary>
    /// Logic to display menu item context menu
    /// </summary>
    [CustomEditor(typeof(AudioSystem))]
    public class AudioSystemEditor : SmartEditor
    {
		string help =
            "[Callbacks]\n" +
            "PlayOnce(AudioClip)\n" +
            "ChancePlayMagazine(AudioMagazine)";

        public override string[] propertiesToExclude => new[]{ "m_Script" };
		public override void OnPreInspector()
		{
			EditorGUILayout.HelpBox(help, MessageType.Info);
		}

        [MenuItem("GameObject/StormRend/AudioSystem", false, 10)]
        static void CreateGameObject(MenuCommand menuCommand)
        {
            var newo = new GameObject("AudioSystem", typeof(AudioSystem));
            GameObjectUtility.SetParentAndAlign(newo, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(newo, "Create StormRend AudioSystem");
            Selection.activeObject = newo;
        }
    }
}
