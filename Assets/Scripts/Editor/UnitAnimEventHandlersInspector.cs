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