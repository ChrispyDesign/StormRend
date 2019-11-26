using System.Linq;
using StormRend.States;
using StormRend.Systems;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	public class AutoUnitSelector : MonoBehaviour
	{
		public enum SelectMethod
		{
			Random,
			LastSelected
		}

		//Inspector
		[SerializeField] SelectMethod selectMethod = SelectMethod.Random;

		//Members
		UnitRegistry ur = null;
		UserInputHandler uih = null;
		AnimateUnit lastSelected = null;

		//First run only
		bool isFirstRun = true;

		#region Inits
		void Awake()
		{
			ur = UnitRegistry.current;
			uih = UserInputHandler.current;
			isFirstRun = true;
		}
		#endregion

		#region Callbacks
		public void OnTurnEnter(State state)
		{
			//Ignore first run
			if (isFirstRun)
			{
				isFirstRun = false;
				return;
			}

			//Select a unit based on the settings
			switch (selectMethod)
			{
				case SelectMethod.LastSelected:
					//Reselect last selected unit if valid and still alive
					if (lastSelected && !lastSelected.isDead)
					{
						uih.SelectUnit(lastSelected, true);
						break;
					}
					//Else fall through
					goto case SelectMethod.Random;

				case SelectMethod.Random:
					{
						//Find a random unit based on the turn state type
						AnimateUnit randomAliveUnitToSelect = null;
						switch (state)
						{
							case AllyTurnState a:
								{
									var aliveUnits = ur.GetAliveUnitsByType<AllyUnit>();
									if (aliveUnits.Length <= 0) return;
									randomAliveUnitToSelect = aliveUnits[Random.Range(0, aliveUnits.Length)];
									break;
								}
							case EnemyTurnState e:
								{
									var aliveUnits = ur.GetAliveUnitsByType<EnemyUnit>();
									if (aliveUnits.Length <= 0) return;
									randomAliveUnitToSelect = aliveUnits[Random.Range(0, aliveUnits.Length)];
									break;
								}
							default:
								Debug.LogWarning("Invalid state passed in!");
								break;
						}

						// Debug.Log("Select unit: " + randomAliveUnitToSelect);
						uih.SelectUnit(randomAliveUnitToSelect, true);
					}
					break;
			}
		}

		public void OnTurnExit(State state)
		{
			//Record last selected unit and clear
			lastSelected = uih.selectedAnimateUnit;     //if its null its null

			uih.ClearSelectedUnit();
		}
		#endregion
	}
}