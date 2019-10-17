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
        [MenuItem("GameObject/StormRend/AudioSystem", false, 10)]
        static void CreateGameObject(MenuCommand menuCommand)
        {
            var newo = new GameObject("AudioSystem", typeof(AudioSystem));
            GameObjectUtility.SetParentAndAlign(newo, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(newo, "Create StormRend AudioSystem");
            Selection.activeObject = newo;
        }

        public override string[] propertiesToExclude => new[]{ "m_Script" };
    }
}
