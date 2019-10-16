using StormRend.Abilities;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	[CustomEditor(typeof(Ability))]
	public class AbilityInspector : SmartEditor
	{
		EffectInspector ei;
		Ability a;
		bool areaOfEffectFoldout = false;

		public override string[] propertiesToExclude => new[]{ "m_Script" };

		void OnEnable()
		{
			a = target as Ability;
			ei = new EffectInspector(a);
		}
   	}
}