using StormRend.Systems;
using StormRend.UI;
using UnityEngine;

namespace StormRend.Defunct
{
	/// <summary>
	/// singleton UI manager which houses relevant UI scripts
	/// </summary>
	public class xUIManager : MonoBehaviour
	{
		// singleton (uh oh)
		private static xUIManager m_instance;

		// an assortment of different UI scripts/managers
		[Header("UI Components")]
		[SerializeField] private xGloryManager m_gloryManager = null;
		[SerializeField] private BlizzardController m_blizzardManager = null;
		[SerializeField] private xUIAvatarSelector m_avatarSelector = null;
		[SerializeField] private xUIAbilitySelector m_abilitySelector = null;

		#region getters

		public xGloryManager GetGloryManager() { return m_gloryManager; }
		public BlizzardController GetBlizzardManager() { return m_blizzardManager; }
		public xUIAvatarSelector GetAvatarSelector() { return m_avatarSelector; }
		public xUIAbilitySelector GetAbilitySelector() { return m_abilitySelector; }

		#endregion

		/// <summary>
		/// error handling
		/// </summary>
		void Start()
		{
			// ensure all of the relevant UI scripts are assigned
			Debug.Assert(m_blizzardManager, "Blizzard Manager not assigned to UI Manager!");
			Debug.Assert(m_gloryManager, "Glory Manager not assigned to UI Manager!");
			Debug.Assert(m_avatarSelector, "Avatar Manager not assigned to UI Manager!");
			Debug.Assert(m_abilitySelector, "Ability Selector not assigned to UI Manager!");
		}

		/// <summary>
		/// single getter
		/// </summary>
		/// <returns>instance of this singleton class</returns>
		public static xUIManager GetInstance()
		{
			// if no instance is assigned...
			if (!m_instance)
				m_instance = FindObjectOfType<xUIManager>(); // find the instance

			// error handling
			Debug.Assert(m_instance, "UI Manager not found!");

			// done
			return m_instance;
		}
	}
}