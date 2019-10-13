using UnityEngine;

namespace StormRend.Defunct
{
	/// <summary>
	/// avatar selection script responsible for resizing the avatars on player selection
	/// </summary>
	public class xUIAvatarSelector : MonoBehaviour
	{
		//Renames suggestions:
		// - UIAvatarResizer

		// avatar image/transform elements
		[Header("Unit Avatars")]
		[SerializeField] RectTransform m_berserkerAvatar = null;
		[SerializeField] RectTransform m_valkyrieAvatar = null;
		[SerializeField] RectTransform m_sageAvatar = null;

		// the focused size of the avatar on selection
		[SerializeField] float m_focusedScalar = 1.5f;

		// helper variables
		Vector2 m_defaultAvatarSize;
		Vector2 m_focusedAvatarSize;

		/// <summary>
		/// store the default avatar size and determine the 
		/// </summary>
		void Start()
		{
			m_defaultAvatarSize = m_berserkerAvatar.rect.size;
			m_focusedAvatarSize = m_defaultAvatarSize * m_focusedScalar;
		}

		/// <summary>
		/// use this to resize the avatar based on the currently selected player unit!
		/// </summary>
		/// <param name="playerUnit">the currently selected player unit</param>
		public void SelectPlayerUnit(xPlayerUnit playerUnit)
		{
			// reset avatar sizes to default
			m_berserkerAvatar.sizeDelta = m_defaultAvatarSize;
			m_valkyrieAvatar.sizeDelta = m_defaultAvatarSize;
			m_sageAvatar.sizeDelta = m_defaultAvatarSize;

			// if no player unit is selected
			if (playerUnit == null)
				return; // don't do avatar focussing 

			switch (playerUnit.unitType)
			{
				case PlayerClass.BERSERKER:
					m_berserkerAvatar.sizeDelta = m_focusedAvatarSize; // resize berserker avatar
					break;

				case PlayerClass.VALKYRIE:
					m_valkyrieAvatar.sizeDelta = m_focusedAvatarSize; // resize valkyrie avatar
					break;

				case PlayerClass.SAGE:
					m_sageAvatar.sizeDelta = m_focusedAvatarSize; // resize sage avatar
					break;
			}
		}
	}
}