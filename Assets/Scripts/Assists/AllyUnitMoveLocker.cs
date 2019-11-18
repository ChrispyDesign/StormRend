using StormRend.Abilities;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists 
{ 
	[RequireComponent(typeof(UnitRegistry))]
	public class AllyUnitMoveLocker : MonoBehaviour
	{
		UnitRegistry ur;

		void Awake()
		{
			ur = GetComponent<UnitRegistry>();
		}

		void Start()
		{
			foreach (var au in ur.GetAliveUnitsByType<AllyUnit>())
				au.onActed.AddListener(Lock);
		}

		/// <summary>
		/// Lock all ally unit movement 
		/// </summary>
		public void Lock(Ability a)
		{
			Debug.Log("Locking ally units!");
			foreach (var au in ur.GetAliveUnitsByType<AllyUnit>())
				au.SetCanMove(false);
		}
   	}
}