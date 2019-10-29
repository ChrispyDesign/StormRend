using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	[SelectionBase]
	public abstract class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[ReadOnlyField] public Tile currentTile;//{ get; set; }	//The tile this unit is currently/originally on
		[TextArea(0,2)] public string description = "";

		//Inspector
		[Header("Stats")]
		[ReadOnlyField, SerializeField] protected int _hp;        //Current HP
		[SerializeField] protected int _maxHP = 3;

		//Events
		[Header("Events")]
		public UnitEvent onDeath;
		public DamageEvent onDamage;
		public UnityEvent onHeal;

		//Properties
		public int HP
		{
			get => _hp;
			set => _hp = Mathf.Clamp(value, 0, maxHP);
		}
		public int maxHP => _maxHP;
		public bool isDead => HP <= 0;

	#region Startup
		protected virtual void Awake()
		{
			//Reset health
			HP = maxHP;

			//Find a tile if it's not already set
			if (!currentTile) ScanTileBelow();
		}

		void ScanTileBelow()
		{
			//TEMP Scan below
			float scanRange = 0.2f;
			foreach (var t in Map.current.tiles)
			{
				//If a tile is within a certain range then set it as the current tile
				if (Vector3.Distance(t.transform.position, transform.position) < scanRange)
				{
					currentTile = t;
					return;
				}
			}
			Debug.Assert(currentTile, name + "does not have a current tile!");
		}
		#endregion

		#region Health
		public void TakeDamage(DamageData damageData)
		{
			if (isDead) return;     //Can't beat a dead horse :P
			HP -= damageData.amount;	
			if (HP <= 0) Die();

			onDamage.Invoke(damageData);
		}
		public void Heal(int amount)
		{
			HP += amount;

			onHeal.Invoke();
		}

		public virtual void Die()
		{
			onDeath.Invoke(this);
		}

	#endregion

	#region Event System
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
		}
	#endregion
	}
}

