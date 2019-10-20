using StormRend;

public class ProvokeEffect : Effect
{
	public int m_damageBack;
	public bool m_gainProtection;
	public override bool PerformEffect(Tile targetTile, Unit effectPerformer)
	{
		Unit unit = targetTile.GetUnitOnTop();
		unit.provokeDamage = m_damageBack;
		unit.isProvoking = true;

		if (m_gainProtection)
			unit.isProtected = true;

		return true;
	}
}
