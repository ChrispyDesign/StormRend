using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	/// <summary>
	/// Class to automatically set a unit to a certain starting health
	/// </summary>
	public class StartingHealth : MonoBehaviour
	{
		[SerializeField] int startingHealth = 2;
		Unit unit = null;

		void Awake()
		{
			unit = GetComponent<Unit>();

			Debug.Assert(unit, "No unit found! Disabling..");
			if (!unit) enabled = false;
		}

		void Start() => unit.HP = startingHealth;
	}
}