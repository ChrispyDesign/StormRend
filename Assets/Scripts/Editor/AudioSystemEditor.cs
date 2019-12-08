/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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
			"*ChancePlay()\n" +
			"*ChancePlay(AudioClip)\n" +
            "PlayOnce(AudioClip)\n" +
			"ChancePlayClip(AudioClip)\n" +
            "ChancePlayMagazine(AudioMagazine)";

        public override string[] propertiesToExclude => new[]{ "m_Script" };
		public override void OnPreInspector()
		{
			EditorGUILayout.HelpBox(help, MessageType.Info);
		}

        [MenuItem("GameObject/StormRend/AudioSystem", false, 10)]
        static void CreateGameObject(MenuCommand menuCommand)
        {
            var go = new GameObject("AudioSystem", typeof(AudioSystem));
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create StormRend AudioSystem");
            Selection.activeObject = go;
        }
    }
}
