using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Systems;
using StormRend.Tags;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	[SelectionBase]
	[RequireComponent(typeof(UnitTag))]
	public abstract class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[TextArea(0,2)] public string description = "";

		//Inspector
		[Header("Stats")]
		[ReadOnlyField, SerializeField] protected int _hp;        //Current HP
		[SerializeField] protected int _maxHP = 3;

		//Events
		[Header("Unit Events")]
		public UnitEvent onDeath;
		public DamageEvent onDamage;
		public UnityEvent onHeal;

		[Header("Movement")]
		[ReadOnlyField] public Tile currentTile;//{ get; set; }	//The tile this unit is currently/originally on

		//Properties
		public int HP
		{
			get => _hp;
			set => _hp = Mathf.Clamp(value, 0, maxHP);
		}
		public int maxHP => _maxHP;
		public bool isDead => HP <= 0;
		public Animator animator { get; private set; }
		public new Tag tag { get; private set; }

		//Members
		protected UnitRegistry ur;
		protected UserInputHandler uih;

	#region Startup
		protected virtual void Awake()
		{
			//Reset health
			HP = maxHP;

			//Always scan the tile below to prevent previous tile value from locking unit on a tile
			ScanTileBelow();

			//Tag
			tag = GetComponent<UnitTag>();

			//Singletons
			ur = UnitRegistry.current;
			uih = UserInputHandler.current;
		}

		void Start()
		{
			//Get animator
			animator = GetComponentInChildren<Animator>();
		}

		void ScanTileBelow()
		{
			//TEMP Scan below
			float scanTolerance = 0.2f;
			foreach (var t in Map.current.tiles)
			{
				//If a tile is within a certain range then set it as the current tile
				if (Vector3.Distance(t.transform.position, transform.position) < scanTolerance)
				{
					currentTile = t;
					return;
				}
			}
			Debug.Assert(currentTile, name + " does not have a current tile!");
		}
		#endregion

	#region Health
		public virtual void TakeDamage(DamageData damageData)
		{
			if (isDead) return;     //Can't beat a dead horse :P
			HP -= damageData.amount;

			//Die() shouldn't be called immediately because the death sequence has timing complexities
			// if (HP <= 0) Die();		

			onDamage.Invoke(damageData);	//ie. Update health bar etc
		}
		public void Heal(int amount)
		{
			HP += amount;

			onHeal.Invoke();
		}

		public virtual void Die()
		{
			ur.RegisterUnitDeath(this);

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
