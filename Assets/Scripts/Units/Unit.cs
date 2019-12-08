/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Anim.EventHandlers;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Systems;
using StormRend.Tags;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;

namespace StormRend.Units
{
	[SelectionBase]
	[RequireComponent(typeof(UnitTag))]
	public abstract class Unit : MonoBehaviour, ITaggable  //, IPointerEnterHandler, IPointerExitHandler
	{
		[TextArea(0, 2)] public string description = null;

		//Inspector
		[Header("Stats")]
		[ReadOnlyField, SerializeField] protected int _hp;        //Current HP
		[SerializeField] protected int _maxHP = 3;

		//Events
		[Header("Unit Events")]
		public UnitEvent onDeath = null;
		public HealthEvent onTakeDamage = null;
		public UnitEvent onEnemyKilled = null;
		public HealthEvent onHeal = null;

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
		public Animator animator { get; private set; } = null;
		public UnitAnimEventHandlers animEventHandlers { get; private set; } = null;
		public new Tag tag { get; private set; } = null;
		internal bool hasJustKilled { get; set; } = false;

		//Members
		protected UnitRegistry ur = null;
		protected UserInputHandler uih = null;


		#region Startup
		protected virtual void Awake()
		{
			//Reset health
			HP = maxHP;

			//Always scan the tile below to prevent previous tile value from locking unit on a tile
			ScanTileBelow();

			//Tag
			tag = GetComponent<Tag>();

			//Singletons
			ur = UnitRegistry.current;
			uih = UserInputHandler.current;
		}

		void Start()
		{
			//Get animator
			animator = GetComponentInChildren<Animator>();
			animEventHandlers = GetComponentInChildren<UnitAnimEventHandlers>();
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
		/// <summary>
		/// Kills the unit. No need to send in a huge value etc.
		/// </summary>
		public virtual void Kill(Unit vendor = null)
		{
			TakeDamage(new HealthData(vendor, HP));
		}
		public virtual void TakeDamage(HealthData healthData)
		{
			if (isDead) return;     //Can't beat a dead horse :P
			HP -= healthData.amount;

			//Die() shouldn't be called immediately because the death sequence has timing complexities
			//Die() needs to be called at the end of the death animation using an animation event
			// if (HP <= 0) Die();

			onTakeDamage.Invoke(healthData);    //ie. Update health bar etc
		}
		public void Heal(HealthData healthData)
		{
			HP += healthData.amount;

			onHeal.Invoke(healthData);
		}

		public virtual void Die()
		{
			ur.RegisterUnitDeath(this);

			onDeath.Invoke(this);
		}

		public void OnOpponentKilled(Unit opponent)
		{
			//Invoke onOpponentKilled once the opponent has died for correct timing
			
		}

		#endregion

		#region Event System
		// public virtual void OnPointerEnter(PointerEventData eventData)
		// {
		// }
		// public virtual void OnPointerExit(PointerEventData eventData)
		// {
		// }
		#endregion
	}
}
