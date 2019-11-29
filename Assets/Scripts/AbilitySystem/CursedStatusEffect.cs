using StormRend.MapSystems.Tiles;
using StormRend.Systems.StateMachines;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
    /// <summary>
    /// Negative status effect
    /// Cursed status effects always expire or tick at the end of their unit's turn
    /// </summary>
    public abstract class CursedStatusEffect : StatusEffect
    {
        //Curses can never expire on start turn
        public override bool OnStartTurn(AnimateUnit affectedUnit) => true; 
        public override bool OnEndTurn(AnimateUnit affectedUnit)
		{
			//Increment number of turns this effect has operated
			++turnCount;

			//Check if this status effect has expired
			if (affectedTurns > 0   //NOTE: If affectedturns set to 0 then status effect will never expire
				&& turnCount >= affectedTurns)
			{
				//Expired. Flag to be removed in AnimateUnit
				return false;
			}
			return true;
		}
    }
}