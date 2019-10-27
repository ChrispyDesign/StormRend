using System;
using System.Collections.Generic;
using System.Linq;
using StormRend.Abilities;
using StormRend.Abilities.Effects;
using StormRend.Enums;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Utility;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	[SelectionBase] //Avoid clicking on child objects
	public abstract class AnimateUnit : Unit, IPointerEnterHandler, IPointerExitHandler
	{
		//Inspector
		[Header("Abilities")]
		[SerializeField] protected int moveRange = 4;
		[Tooltip("The unit types of that this unit cannot walk through ie. opponents")]
		[EnumFlags, SerializeField] TargetMask pathblockingUnitTypes = TargetMask.Enemies;
		[SerializeField] internal Ability[] abilities;

		[Header("Color")]
		[SerializeField] protected Color ghostColor = Color.blue;

		//Properties
		public Tile originTile { get; set; } = null;  	//The tile this unit was originally on at the beginning of each turn; Used to set a different texture to that tile so the user knows where he originated from
		public Tile ghostTile { get; set; } = null;		//The tile the ghost is on
		public Tile[] possibleMoveTiles { get; set; } = new Tile[0];
		public Tile[] possibleTargetTiles { get; set; } = new Tile[0];
		public List<StatusEffect> statusEffects { get; set; } = new List<StatusEffect>();

		//Members
		bool _hasActed = false;
		public bool hasActed => _hasActed;	//has performed an ability and hence this unit has completed it's turn and is locked until next turn
		public void SetActed(bool value) => _hasActed = value;
		protected GameObject ghostMesh;

		//Events
		[SerializeField] EffectEvent onAddStatusEffect;
		[SerializeField] UnityEvent onBeginTurn;
		[SerializeField] AbilityEvent onActed;
		[SerializeField] UnityEvent onEndTurn;

	#region Filtered Gets
		public List<Ability> GetAbilitiesByType(AbilityType type) => abilities.Where(x => x.type == type).ToList();
	#endregion

	#region Startup
		protected override void Awake()	//This will not block base.Start()
		{
			base.Awake();

			//Init origin tile
			originTile = currentTile;

			CreateGhostMesh();
		}

		/// <summary>
		///  Semi-auto create a tinted ghost mesh for moving etc
		/// </summary>
		void CreateGhostMesh()
		{
			//Find
			var mesh = transform.Find("Mesh");
			//Assert
			Debug.Assert(mesh, "'Mesh' child object not found! Cannot create ghost mesh for this unit!");
			//Create
			ghostMesh = Instantiate(mesh.gameObject, transform.position, transform.rotation);
			ghostMesh.transform.SetParent(transform);
			//Tint all renderer materials
			var ghostRenderers = ghostMesh.GetComponentsInChildren<Renderer>();
			List<Material> ghostMaterials = new List<Material>();
			foreach (var r in ghostRenderers)
				foreach (var m in r.materials)
					m.SetColor("_Color", ghostColor);
			//Hide
			ghostMesh.SetActive(false);
		}
		#endregion

	#region Core
		//------------------- STATS
		//State machine / game director / Unit registry to run through all these on ally turn enter?
		public void BeginTurn()		//Reset necessary stats and get unit ready for the next turn
		{
			SetActed(false);		//Be able to move again

			//Status effects
			foreach (var se in statusEffects)
				se.OnBeginTurn(this);
			
			onBeginTurn.Invoke();
		}
		public void EndTurn()			//Run before 
		{
			//Status effects
			foreach (var se in statusEffects)
				se.OnEndTurn(this);

			onEndTurn.Invoke();
		}

		public void AddStatusEffect(StatusEffect statusEffect)
		{
			statusEffects.Add(statusEffect);

			onAddStatusEffect.Invoke(statusEffect);
		}

		public override void Die()
		{
			base.Die();     //OnDeath will invoke

			//TEMP
			gameObject.SetActive(false);
		}

		//------------------ MOVE
		public bool Move(Tile destination, bool useGhost = false, bool limitToMoveTiles = true)
		{
			//Only set the position of the ghost
			if (useGhost)
			{
				//Filter out non-moving tiles
				if (limitToMoveTiles && !possibleMoveTiles.Contains(destination)) return false;
				//Set
				ghostTile = destination;
				//Move ghost
				ghostMesh.SetActive(true);
				ghostMesh.transform.position = ghostTile.transform.position;
			}
			//Move the actual unit
			//TODO as soon as the unit has acted, 
			else
			{
				//Ghost was probably just active so deactivate ghost ??? Should this be here?
				ghostMesh.SetActive(false);
				//Filter
				if (limitToMoveTiles && !possibleMoveTiles.Contains(destination)) return false;
				//Set
				currentTile = destination;
				//Move
				transform.position = destination.transform.position;
			}
			return true;	//Successful move
		}

		/// <summary>
		/// Move Unit by direction ie. Move({2, 1}) means the unit to move right 2 and forward 1.
		/// Returns false if the unit moved onto an empty space.
		/// Can set to kill unit if it does move onto an empty space.
		/// </summary>
		public bool Move(Vector2Int direction, bool kill = true)
		{
			if (currentTile.TryGetTile(direction, out Tile t))
			{
				//Pushed
				Move(t, false, false);
				return true;
			}
			else
			{
				//Pushed off the edge
				if (kill) Die();
				return false;
			}
		}

		//------------------- PERFORM ABILITY
		public void Act(Ability ability, params Tile[] targetTiles)
		{
			//Lock in movement and new position
			SetActed(true);
			originTile = currentTile;

			//Perform Ability
			ability.Perform(this, targetTiles);

			//Run status effects
			foreach (var se in statusEffects)
				se.OnActed(this);

			onActed.Invoke(ability);
		}
		public void Act(Ability ability, params Unit[] targetUnits)
			=> Act(ability, targetUnits.Select(x => x.currentTile).ToArray());

		//------------------- CALCULATE TILES
		/// <summary>
		/// Calculate the tiles that this unit can currently move to for this turn and point in game time.
		/// Filters based on which unit type cannot be traversed through.
		/// Returns the list of tiles if needed.
		/// </summary>
		public Tile[] CalculateMoveTiles()
		{
			var pathblockers = new List<Type>();

			//Allies
			if ((pathblockingUnitTypes & TargetMask.Allies) == TargetMask.Allies)
				pathblockers.Add(typeof(AllyUnit));

			//Enemies
			if ((pathblockingUnitTypes & TargetMask.Enemies) == TargetMask.Enemies)
				pathblockers.Add(typeof(EnemyUnit));

			//Crystals
			if ((pathblockingUnitTypes & TargetMask.Crystals) == TargetMask.Crystals)
				pathblockers.Add(typeof(CrystalUnit));

			//InAnimates
			if ((pathblockingUnitTypes & TargetMask.InAnimates) == TargetMask.InAnimates)
				pathblockers.Add(typeof(InAnimateUnit));

			//Animates
			if ((pathblockingUnitTypes & TargetMask.Animates) == TargetMask.Animates)
				pathblockers.Add(typeof(AnimateUnit));

			return possibleMoveTiles = Map.GetPossibleTiles(currentTile.owner, originTile, moveRange, pathblockers.ToArray());
		}

		/// <summary>
		/// Get the tiles that can be currently acted upon by this ability
		/// </summary>
		public Tile[] CalculateTargetTiles(Ability a)
		{
			var result = new List<Tile>();
			var sqrLen = Ability.caSize;
			
			//Find the center of the cast area
			Vector2Int center = new Vector2Int(sqrLen / 2, sqrLen / 2);

			//Go through castArea
			for (int row = 0; row < sqrLen; row++)  //rows
			{
				for (int col = 0; col < sqrLen; col++)  //columns
				{
					if (a.castArea[row * sqrLen + col])
					{
						Vector2Int offset = new Vector2Int(row, col) - center;

						if (currentTile.TryGetTile(offset, out Tile t))
						{
							if (!(t is UnWalkableTile))
								result.Add(t);
						}
					}
				}
			}
			// SRDebug.PrintCollection(possibleTargetTiles);
			possibleTargetTiles = result.ToArray();
			return possibleTargetTiles;
		}

		//------------------ OTHER
		public void ClearGhost()
		{
			ghostMesh.SetActive(false);
			ghostMesh.transform.position = transform.position;
		}
	#endregion

	#region Event System Interface Implementations
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
		}
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
		}
	#endregion
	}
}