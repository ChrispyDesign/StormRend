using StormRend.Audio;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors.MenuItems
{
    public class AudioSystemMenuItem : Editor
    {
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
