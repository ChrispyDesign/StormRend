using StormRend;
using StormRend.Defunct;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Abilities.Effects
{
	public enum Target
	{
		SelectedTiles,
		SelectedTilesWithBreadth,
		AdjacentTiles,
		Self
	}

	[System.Serializable]
	public class Effect : ScriptableObject
	{
		public Target m_target;
		// public UnityEvent OnPeformEffect;

		protected bool m_isTileAllowed;

		public bool m_isFoldOut { get; set; } = true;

		public virtual bool PerformEffect(oTile targetTile, Unit effectPerformer)
		{
			// OnPeformEffect.Invoke();

			Ability ability = effectPerformer.GetSelectedAbility();
			TargetableTiles tileInfo = ability.GetTargetableTiles();

			if (tileInfo.m_empty == (targetTile.GetUnitOnTop() == null))
				m_isTileAllowed = true;

			if (targetTile.GetUnitOnTop() != null)
			{
				if (tileInfo.m_enemies &&
					tileInfo.m_enemies == (targetTile.GetUnitOnTop().GetComponent<EnemyUnit>() != null))
					m_isTileAllowed = true;

				if (tileInfo.m_players &&
					tileInfo.m_players == (targetTile.GetUnitOnTop().GetComponent<PlayerUnit>() != null))
					m_isTileAllowed = true;

				if (tileInfo.m_self &&
					tileInfo.m_self == (targetTile.GetUnitOnTop().GetComponent<Unit>() == effectPerformer))
					m_isTileAllowed = true;
			}

			if (!m_isTileAllowed)
			{
				effectPerformer.SetHasAttacked(false);
				return false;
			}

			//Effect successfully performed???
			effectPerformer.SetHasMoved(true);
			effectPerformer.SetHasAttacked(true);

			//TEMP Orient performer
			var effectDir = Vector3.Normalize(targetTile.transform.position - effectPerformer.transform.position);
			if (effectDir != Vector3.zero)
				effectPerformer.transform.rotation = Quaternion.LookRotation(effectDir, Vector3.up);

			//Set glory
			UIManager.GetInstance().GetGloryManager().SpendGlory(ability.GetGloryRequirement());

			return true;
		}
	}
}