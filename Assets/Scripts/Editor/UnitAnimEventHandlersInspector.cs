using StormRend.Anim.EventHandlers;
using UnityEditor;

namespace StormRend.Editors
{
	[CustomEditor(typeof(UnitAnimEventHandlers), true)]
	public class UnitAnimEventHandlersInspector : SmartEditor
	{
		string help =
			"[Callbacks]\n" +
			"PerformAbility()\n" +
			"Die()\n" +
			"PlayVFX(Object)\n" +
			"PlayOnboardVFX(string)";
		public override void OnPreInspector()
		{
			EditorGUILayout.HelpBox(help, MessageType.Info);
		}
		public override string[] propertiesToExclude => new[]{"m_Script"};
	}
}