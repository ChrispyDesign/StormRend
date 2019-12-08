/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Anim.EventHandlers;
using UnityEditor;

namespace StormRend.Editors
{
	[CustomEditor(typeof(UnitAnimEventHandlers), true)]
	public class UnitAnimEventHandlersInspector : SmartEditor
	{
		string help =
			"PerformAbility()\n" +
			"Die()\n" +
			"ActivateInbuiltVFX(string)\n" +
			"CycleInbuiltVFX(string)\n" +
			"DeactivateInbuiltVFX(string)\n" +
			"PlayVFX(VFX)\n" +
			"MountVFX(VFX)";
		public override void OnPreInspector()
		{
			EditorGUILayout.HelpBox(help, MessageType.Info);
		}
		public override string[] propertiesToExclude => new[]{"m_Script"};
	}
}