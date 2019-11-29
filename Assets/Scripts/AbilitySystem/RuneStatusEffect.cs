using StormRend.Systems.StateMachines;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
    /// <summary>
    /// Positive status effect
    /// Rune Status Effects always expire or count down at the START of their unit's turn
    /// </summary>
    public abstract class RuneStatusEffect : StatusEffect
    {
        //Runes can never expire at the end of the turn
        public override bool OnEndTurn(AnimateUnit affectedUnit) => true;
        public override bool OnStartTurn(AnimateUnit affectedUnit)
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