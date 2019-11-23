using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.Abilities;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Systems
{
	public partial class UserInputHandler : Singleton<UserInputHandler>
	{
		#region Sets
		//Public; can be called via unity events
		public void SelectUnit(AnimateUnit au, bool moveCamera = false)
		{
			//Clear tile highlights if a unit was already selected
			if (isUnitSelected)
			{
				selectedAnimateUnit.ClearGhost();
				ClearSelectedUnitTileHighlights();
				selectedAbility = null;
			}

			//Set the selected unit
			selectedUnit = au;

			//Show move tile if unit is able to move
			ShowMoveTiles();

			//Move camera
			if (moveCamera) camMover.MoveTo(au, cameraSmoothTime);

			onUnitSelected.Invoke(au);  //ie. Update UI, Play sounds,
		}

		public void SelectAbility(Ability a)    //aka. OnAbilityChanged()
		{
			//Checks
			if (!isUnitSelected)
			{
				Debug.LogWarning("No unit selected! Cannot select ability");
				return;
			}
			if (!selectedAnimateUnit.canAct)
			{
				Debug.LogWarning("Unit cannot perform any more abilities this turn");
				return;
			}

			//Set
			selectedAbility = a;

			//Recalculate target tiles
			selectedAnimateUnit.CalculateTargetTiles(selectedAbility);

			//Clear move tiles + Show target tiles + clear ghosts
			selectedAnimateUnit.ClearGhost();
			ClearAllTileHighlights();
			ShowActionTiles();

			//Auto perform ability on self if required tiles set to 0
			if (selectedAbility.requiredTiles == 0)
				AddTargetTile(selectedAnimateUnit.currentTile);

			//Raise
			onAbilitySelected.Invoke(a);
		}

		/// <summary>
		/// Add a target tile to the casting stack and if the selected ability required target input is reached then perform the ability
		/// </summary>
		void AddTargetTile(Tile t)
		{
			if (selectedAbility.IsAcceptableTileType(selectedAnimateUnit, t) &&       //Check ability can accept this tile type
				selectedAnimateUnit.possibleTargetTiles.Contains(t) &&          	  //Check tile is within possible target tiles
					!targetTileStack.Contains(t))                      	         //Can't select the same tile twice
					{
						//VALID
						targetTileStack.Push(t);
						ShowTargetTile(t);
						onTargetTileAdd.Invoke(t);
					} 
					// else onTargetTileInvalid.Invoke();   							//ALREADY BEEN SELECTED 
				// else onTargetTileInvalid.Invoke();   								//OUT OF BOUNDS
			else 
				onTargetTileInvalid.Invoke();   									//UNACCEPTABLE

			//Perform ability once required number of tiles reached
			if (targetTileStack.Count >= selectedAbility.requiredTiles) SelectedUnitPerformAbility();
		}

		//Redirect because sometimes the raycast can only hit a unit
		void AddTargetTile(Unit u) => AddTargetTile(u.currentTile);

		//Pop and send through the event
		void PopTargetTile() => onTargetTileCancel.Invoke(ClearTargetTile(targetTileStack.Pop()));

		//Enough tile targets chosen by user. Execute the selected ability
		void SelectedUnitPerformAbility()
		{
			//Check there's enough glory
			if (!EnoughGlory())
			{
				onNotEnoughGlory.Invoke();
				return;
			}
			//Spend glory
			else
			{
				glory.value -= selectedAbility.gloryCost;
			}

			//Perform
			selectedAnimateUnit.Act(selectedAbility, targetTileStack.ToArray());

			//Focus camera (on the target tile or last target tile input)
			Vector3 averageTarget = Vector3.zero;
			foreach (var t in targetTileStack)
				averageTarget += t.transform.position;
			averageTarget /= (float)targetTileStack.Count;
			camMover.MoveTo(averageTarget, cameraSmoothTime);

			//Clear target stack
			targetTileStack.Clear();

			//clear ability
			ClearSelectedAbility(selectedAnimateUnit.canMove);

			//Events
			onAbilityPerformed.Invoke(selectedAbility);
		}
		#endregion
	}
}