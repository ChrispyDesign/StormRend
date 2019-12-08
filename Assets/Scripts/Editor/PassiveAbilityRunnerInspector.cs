/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Abilities.Utilities;
using UnityEditor;

namespace StormRend.Editors
{
    [CustomEditor(typeof(PassiveAbilityRunner))]
    public class PassiveAbilityRunnerInspector : SmartEditor
    {
        string help = "Caches all passive abilities and runs them where appropriate\n" +
                    	"ie. on unit creation, unit killed or unit moved";
        public override string[] propertiesToExclude => new[] { "m_Script" };
        public override void OnPreInspector()
        {
            EditorGUILayout.HelpBox(help, MessageType.Info);
        }
    }
}