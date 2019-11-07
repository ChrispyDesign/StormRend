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
		//Enums
		public enum LookSnap
		{
			RightAngle,
			Diagonals_BUGGY,
			Free_BUGGY,
		}

		//Inspector
		[ReadOnlyField] public Tile beginTurnTile = null;   //The tile this unit starts from at the beginning of each turn
		[ReadOnlyField] public bool _canMove = true;
		[ReadOnlyField] public bool _canAct = true;
		[SerializeField] LookSnap lookSnap = LookSnap.RightAngle;

		[Header("Abilities & Effects")]
		[SerializeField] protected int moveRange = 4;
		[Tooltip("The unit types of that this unit cannot walk through ie. opponents")]
		[EnumFlags, SerializeField] TargetType pathBlockers = TargetType.Enemies | TargetType.InAnimates;
		[SerializeField] internal Ability[] abilities = new Ability[0];
		[ReadOnlyField, SerializeField] internal List<StatusEffect> statusEffects = new List<StatusEffect>();

		[Header("Ghost")]
		[SerializeField] protected Color ghostColor = Color.blue;
		[SerializeField] protected Material tbcGhostMaterial = null;

		//Events
		[Header("Animate Unit Events")]
		[SerializeField] EffectEvent onAddStatusEffect = null;
		[SerializeField] UnityEvent onBeginTurn = null;
		[SerializeField] AbilityEvent onActed = null;
		[SerializeField] UnityEvent onEndTurn = null;

		//Properties
		public Tile ghostTile { get; set; } = null;		//The tile the ghost is on
		public Tile[] possibleMoveTiles;// { get; set; } = new Tile[0];
		public Tile[] possibleTargetTiles;// { get; set; } = new Tile[0];
		public bool canMove => _canMove;
		public void SetCanMove(bool value) => _canMove = value;
		public bool canAct => _canAct;	//has performed an ability and hence this unit has completed it's turn and is locked until next turn
		public void SetCanAct(bool value) => _canAct = value;
		float snapAngle
		{
			get
			{
				switch (lookSnap)
				{
					case LookSnap.RightAngle: return 90f;
					case LookSnap.Diagonals_BUGGY: return 45f;
					case LookSnap.Free_BUGGY: return 1f;
					default: return 90f;
				}
			}
		}

		//Members
		protected GameObject ghostMesh;


	#region Filtered Gets
		public List<Ability> GetAbilitiesByType(AbilityType type) => abilities.Where(x => x.type == type).ToList();
	#endregion

	#region Startup
		protected override void Awake()	//This will not block base.Start()
		{
			base.Awake();   //This sets the current tile

			//Record origin tile
			beginTurnTile = currentTile;

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
		//------------------ CALLBACKS
		public override void TakeDamage(DamageData damageData)
		{
			base.TakeDamage(damageData);

			//Face attack
			transform.rotation = GetSnappedRotation(damageData.attacker.transform.position, snapAngle);

			//Animate
			animator.SetTrigger("HitReact");

			//Status effect
			foreach (var se in statusEffects)
				se.OnTakeDamage(this, damageData);
		}

		//------------------- STATS
		//State machine / game director / Unit registry to run through all these on ally turn enter?
		public void BeginTurn()		//Reset necessary stats and get unit ready for the next turn
		{
			Debug.Log("Begin turn: " + this.name);

			//Can take action again
			SetCanAct(true);
			SetCanMove(true);

			//Calculate new move tiles
			beginTurnTile = currentTile;
			CalculateMoveTiles();

			//Prep effects (reset counts etc)
			foreach (var a in abilities)
				foreach (var e in a.effects)
					e.Prepare(a, this);

			//Begin Status effects (ie. blind, cripple, etc)
			foreach (var se in statusEffects)
				se.OnBeginTurn(this);

			onBeginTurn.Invoke();
		}
		public void EndTurn()			//Run before next turn
		{
			//Status effects (Nothing so far)
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
			base.Die();     //onDeath will invoke

			//Status effect
			foreach (var se in statusEffects)
				se.OnDeath(this);

			//TEMP
			// gameObject.SetActive(false);
		}

		//------------------ MOVE
		/// <summary>
		/// Move unit to the destination tile. Option to move the ghost only and to restrict movement within confines of current possible move tiles.
		/// </summary>
		/// <param name="destination">The destination tile to move this unit to</param>
		/// <param name="useGhost">Move the unit's ghost instead</param>
		/// <param name="restrictToPossibleMoveTiles">Only move if the destination tile is within this unit's list of possible move tiles (Optional for teleport)</param>
		/// <param name="forcedMove">Overrides moved status or any immobilising status effects</param>
		/// <returns>return true if successfully moved (Required for the camera)</returns>
		public bool Move(Tile destination, bool useGhost = false, bool restrictToPossibleMoveTiles = true, bool forcedMove = false)
		{
			//Check can move
			if (!forcedMove && !canMove) return false;

			//Only set the position of the ghost
			if (useGhost)
			{
				//Filter out non-moving tiles
				if (restrictToPossibleMoveTiles && !possibleMoveTiles.Contains(destination)) return false;
				//Set
				ghostTile = destination;
				//Move and look
				ghostMesh?.SetActive(true);
				ghostMesh.transform.rotation = GetSnappedRotation(ghostTile.transform.position, snapAngle);
				ghostMesh.transform.position = ghostTile.transform.position;
			}
			//Move the actual unit
			else
			{
				//Ghost was probably just active so deactivate ghost ??? Should this be here?
				if (ghostMesh != null) ghostMesh.SetActive(false);
				//Filter
				if (restrictToPossibleMoveTiles && !possibleMoveTiles.Contains(destination)) return false;
				//Set
				currentTile = destination;
				//Move and look
				transform.rotation = GetSnappedRotation(currentTile.transform.position, snapAngle);
				transform.position = currentTile.transform.position;
			}
			//NOTE: Unit can still move
			return true;	//Successful move

		}

		/// <summary>
		/// Force Move Unit by direction ie. (0, 1) means the unit to moves forward 1 tile.
		/// Can set to kill unit pushed over the edge.
		/// </summary>
		public PushResult Push(Vector2Int direction, bool kill = true)
		{
			if (currentTile.TryGetTile(direction, out Tile t))
			{
				//Check for any units or obstacles
				if (UnitRegistry.IsAnyUnitOnTile(t))
					return PushResult.HitUnit;		//Don't push

				//Check if pushed onto an unwalkable tile
				if (t is UnWalkableTile)
					return PushResult.HitBlockedTile;	//Don't push

				//Push unit
				Move(t, false, false, true);
				return PushResult.Nothing;
			}
			else
			{
				//Pushed out of bounds
				if (kill) Die();
				return PushResult.OverEdge;
			}
		}

		public void SnappedLookAt(Vector3 lookAt) => transform.rotation = GetSnappedRotation(lookAt, snapAngle);
		Quaternion GetSnappedRotation(Vector3 lookTarget, float snapAng)
		{
			var dir = lookTarget - transform.position;
			var angle = -Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + snapAng;
			angle = Mathf.Round(angle / snapAng) * snapAng;
			return Quaternion.AngleAxis(angle, Vector3.up);
			
			// var angle = Vector3.Angle(dir, Vector3.up);
			// if (angle < snapAngle / 2f) 
			// 	return Quaternion.LookRotation(Vector3.up * dir.magnitude);
			// if (angle > 180f - snapAngle / 2f)
			// 	return Quaternion.LookRotation(Vector3.down * dir.magnitude);

			// float t = Mathf.Round(angle / snapAngle);
			// float deltaAngle = (t * snapAngle) - angle;
			// return Quaternion.AngleAxis(deltaAngle, Vector3.Cross(Vector3.up, dir));
		}


		//------------------- PERFORM ABILITY
		/// <summary>
		/// Override for when raycasts hits units instead of tiles
		/// </summary>
		public void Act(Ability ability, params Unit[] targetUnits)
			=> Act(ability, targetUnits.Select(x => x.currentTile).ToArray());
		public void FilteredAct(Ability ability, params Unit[] targetUnits)
			=> FilteredAct(ability, targetUnits.Select(x => x.currentTile).ToArray());
		
		/// <summary>
		/// Filter target tiles based on ability's tile type settings before performing ability
		/// </summary>
		public void FilteredAct(Ability ability, params Tile[] targetTiles)
		{
			var filteredTargetTiles = new List<Tile>();
			foreach (var t in targetTiles)
			{
				if (ability.IsAcceptableTileType(this, t))
					filteredTargetTiles.Add(t);
			}
			Act(ability, filteredTargetTiles.ToArray());
		}

		/// <summary>
		/// Perform the ability and lock unit for this turn
		/// </summary>
		public void Act(Ability ability, params Tile[] targetTiles)
		{
			//Only take action if able to ie. not affected by status effects
			if (!canAct) return;

			//Lock in movement and action
			SetCanAct(false);
			SetCanMove(false);

			//Face the the last tile that was passed in
			if (targetTiles.Length > 0)
				SnappedLookAt(targetTiles[targetTiles.Length-1].transform.position);

			//Perform Ability
			ability.Perform(this, targetTiles);

			//Status effects
			foreach (var se in statusEffects)
				se.OnActed(this);

			onActed.Invoke(ability);
		}

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
			if ((pathBlockers & TargetType.Allies) == TargetType.Allies)
				pathblockers.Add(typeof(AllyUnit));

			//Enemies
			if ((pathBlockers & TargetType.Enemies) == TargetType.Enemies)
				pathblockers.Add(typeof(EnemyUnit));

			//Crystals
			if ((pathBlockers & TargetType.Crystals) == TargetType.Crystals)
				pathblockers.Add(typeof(CrystalUnit));

			//InAnimates
			if ((pathBlockers & TargetType.InAnimates) == TargetType.InAnimates)
				pathblockers.Add(typeof(InAnimateUnit));

			//Animates
			if ((pathBlockers & TargetType.Animates) == TargetType.Animates)
				pathblockers.Add(typeof(AnimateUnit));

			possibleMoveTiles = Map.GetPossibleTiles(beginTurnTile.owner, beginTurnTile, moveRange, pathblockers.ToArray());
			Debug.LogFormat("Calculated move tiles for: {0}, Count: {1}", this.name, possibleMoveTiles.Length);
			return possibleMoveTiles;
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

			//InfoPanel.current.SetText()
		}
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
		}
	#endregion
	}
}