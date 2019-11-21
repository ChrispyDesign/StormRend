using System;
using System.Collections;
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

namespace StormRend.Units
{
    [SelectionBase] //Avoid clicking on child objects
	public abstract class AnimateUnit : Unit //, IPointerEnterHandler, IPointerExitHandler
	{
		//Enums
		public enum LookSnap
		{
			RightAngle,
			// Diagonals_BUGGY,
			// Free_BUGGY,
		}

		//Inspector
		[ReadOnlyField] public Tile startTile = null;   //The tile this unit starts from at the beginning of each turn
		[ReadOnlyField, SerializeField] bool _canMove = true;
		[ReadOnlyField, SerializeField] bool _canAct = true;
		[SerializeField] LookSnap lookSnap = LookSnap.RightAngle;

		[Header("Abilities & Effects")]
		[SerializeField] protected int _moveRange = 4;
		[Tooltip("The unit types of that this unit cannot walk through ie. opponents")]
		[EnumFlags, SerializeField] TargetType pathBlockers = TargetType.Enemies | TargetType.InAnimates;
		[SerializeField] internal Ability[] abilities = new Ability[0];
		[ReadOnlyField, SerializeField] internal List<StatusEffect> statusEffects = new List<StatusEffect>();
		// [ReadOnlyField, SerializeField] internal HashSet<StatusEffect> statusEffects = new HashSet<StatusEffect>();

		[Header("Ghost")]
		[SerializeField] protected GameObject ghost = null;

		//Events
		[Header("Animate Unit Events")]
		public UnityEvent onBeginTurn = null;
		public EffectEvent onAddStatusEffect = null;
		public TileEvent onMoved = null;
		public AbilityEvent onActed = null;
		public UnityEvent onEndTurn = null;

		//Properties
		public Tile ghostTile { get; set; } = null;     //The tile the ghost is on
		public Tile[] possibleMoveTiles;// { get; set; } = new Tile[0];
		public Tile[] possibleTargetTiles;// { get; set; } = new Tile[0];
		public int moveRange => _moveRange;
		private float snapAngle
		{
			get
			{
				switch (lookSnap)
				{
					case LookSnap.RightAngle: return 90f;
					// case LookSnap.Diagonals_BUGGY: return 45f;
					// case LookSnap.Free_BUGGY: return 1f;
					default: return 90f;
				}
			}
		}

		//Status Effect Properties
		public bool isProvoking
		{
			get
			{
				foreach (var se in statusEffects)
					if (se is TauntEffect) return true;
				return false;
			}
		}
		public bool isImmobilised
		{
			get
			{
				foreach (var se in statusEffects)
					if (se is ImmobiliseEffect) return true;
				return false;
			}
		}
		public bool isBlind
		{
			get
			{
				foreach (var se in statusEffects)
					if (se is BlindEffect) return true;
				return false;
			}
		}
		public bool isProtected
		{
			get
			{
				foreach (var se in statusEffects)
					if (se is ProtectEffect) return true;
				return false;
			}
		}

		//Members
		// protected GameObject ghost = null;
		protected Tile[] currentTargetTiles = null;
		private Ability currentAbility;

		#region Can Move & Act
		public bool canMove => _canMove;
		public void SetCanMove(bool value, float delay = 0) => StartCoroutine(DelaySetMove(value, delay));
		IEnumerator DelaySetMove(bool value, float delay)       //Used for correct timing of refresh effect
		{
			//Set immediately so that the AllActionsUsedChecker logic runs correctly
			_canMove = value;
			yield return new WaitForSeconds(delay);
			//If move value true then reselect unit so that the move tiles will appear
			if (_canMove) uih.SelectUnit(this);
		}
		public bool canAct => _canAct;  //has performed an ability and hence this unit has completed it's turn and is locked until next turn
		public void SetCanAct(bool value, float delay = 0) => StartCoroutine(DelaySetAct(value, delay));
		IEnumerator DelaySetAct(bool value, float delay)        //Used for correct timing of refresh effect
		{
			//Set immediately so that the AllActionsUsedChecker logic runs correctly
			_canAct = value;
			yield return new WaitForSeconds(delay);
			//If can act again then show new possible act tiles
			if (_canAct)
			{
				uih.SelectUnit(this);
				uih.SelectAbility(currentAbility);
			}
		}
		#endregion

		#region Filtered Gets
		public List<Ability> GetAbilitiesByType(AbilityType type) => abilities.Where(x => x.type == type).ToList();
		#endregion

		#region Startup
		protected override void Awake() //This will not block base.Start()
		{
			base.Awake();   //This sets the current tile

			//Record origin tile
			startTile = currentTile;

			PrepGhost();
		}

		void PrepGhost()
		{
			//No ghost, create a default placeholder ghost
			if (!ghost)
			{
				ghost = new GameObject("[GHOST]");
				ghost.transform.position = transform.position;
				ghost.transform.SetParent(transform);
				var mesh = GameObject.CreatePrimitive(PrimitiveType.Capsule);
				mesh.GetComponent<Renderer>().material.color = Color.magenta;
				mesh.transform.position = ghost.transform.position;
				mesh.transform.Translate(0, 1, 0, Space.World);
				mesh.transform.SetParent(ghost.transform);
			}
			//Prefab passed in, instantiate and setup
			else
			{
				ghost = Instantiate(ghost, transform.position, transform.rotation, transform);
			}

			//Hide ghost
			ghost.SetActive(false);
		}
		#endregion

		#region Core
		//------------------ CALLBACKS
		/// <summary>
		/// Kills the unit. No need to send in a huge value etc.
		/// </summary>
		public void Kill(Unit vendor = null)
		{
			//Just set the damage amount to match unit's current health
			TakeDamage(new HealthData(vendor, HP));
		}

		public override void TakeDamage(HealthData damageData)
		{
			base.TakeDamage(damageData);

			//Face attacker if available
			if (damageData.vendor)
				transform.rotation = GetSnappedRotation(damageData.vendor.transform.position, snapAngle);

			//Animate
			animator.SetTrigger("HitReact");

			//Status effect
			foreach (var se in statusEffects)
				se.OnTakeDamage(this, damageData);
		}

		//------------------- STATS
		//State machine / game director / Unit registry to run through all these on ally turn enter?
		public void BeginTurn()     //Reset necessary stats and get unit ready for the next turn
		{
			//Can take action again (This doesn't reselect the units)
			_canMove = true;
			_canAct = true;

			//Other resets
			hasKilledThisTurn = false;

			//Calculate new move tiles
			startTile = currentTile;
			CalculateMoveTiles();       //Doesn't work anyways

			//Prep effects (reset counts etc)
			foreach (var a in abilities)
				foreach (var e in a.effects)
					e.Prepare(a, this);

			//Run Begin Status effects (ie. blind, cripple, etc) 
			for (int i = statusEffects.Count - 1; i >= 0; --i)
				if (!statusEffects[i].OnBeginTurn(this))    //Also auto remove expired status effects
					statusEffects.RemoveAt(i);

			onBeginTurn.Invoke();
		}
		public void EndTurn()           //Run before next turn
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
			base.Die();     //onDeath will invoke; Should register death on this unityevent

			//Status effect
			foreach (var se in statusEffects)
				se.OnDeath(this);

			//Finally hide the body
			gameObject.SetActive(false);
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
				if (restrictToPossibleMoveTiles && !possibleMoveTiles.Contains(destination)) return false;		//Filter
				ghostTile = destination;	//Set
				ghost?.SetActive(true);		//Activate
				ghost.transform.rotation = GetSnappedRotation(ghostTile.transform.position, snapAngle);		//Look
				// StartCoroutine(LerpMove(ghost.transform, ghostTile.transform.position));		//Lerp
				ghost.transform.position = ghostTile.transform.position;		//Move Ghost
			}
			//Move the actual unit
			else
			{
				//Ghost was probably just active so deactivate ghost ??? Should this be here?
				if (ghost != null) ghost.SetActive(false);
				if (restrictToPossibleMoveTiles && !possibleMoveTiles.Contains(destination)) return false;		//Filter
				currentTile = destination;		//Set
				transform.rotation = GetSnappedRotation(currentTile.transform.position, snapAngle);		//Look
				// StartCoroutine(LerpMove(transform, currentTile.transform.position));		//Lerp
				transform.position = currentTile.transform.position;		//Move
				onMoved.Invoke(currentTile);	//Events
			}


			//NOTE: Unit can still move
			return true;    //Successful move
		}

		/// <summary>
		/// Hmmm... lerps don't particularly look that good
		IEnumerator LerpMove(Transform root, Vector3 destination, float lerpTime = 0.3f)
		{
			float time = 0;
			float rate = time / lerpTime;
			while (time < lerpTime)
			{
				time += Time.deltaTime;
				root.position = Vector3.Lerp(root.position, destination, time);
				yield return null;
			}
			root.position = destination;
		}

		/// <summary>
		/// Force Move Unit by direction ie. (0, 1) means the unit to moves forward 1 tile.
		/// Can set to kill unit pushed over the edge.
		/// </summary>
		public PushResult Push(Vector2Int direction, bool pushOverEdge = true, bool faceBackward = true)
		{
			if (currentTile.TryGetTile(direction, out Tile t))
			{
				var originalTile = currentTile;

				//Face backward Part 1: faces the attacker even if pushed into an obstacle
				if (faceBackward) SnappedLookAt(new Vector3(transform.position.x - direction.x, 0, transform.position.z - direction.y));

				//Check for any units or obstacles
				if (UnitRegistry.IsAnyUnitOnTile(t))
					return PushResult.HitUnit;      //Don't push

				//Check if pushed onto an unwalkable tile
				if (t is UnWalkableTile)
					return PushResult.HitBlockedTile;   //Don't push

				//Push unit back (facing toward the pusher)
				Move(t, false, false, true);
				//Correct the facing
				if (faceBackward) SnappedLookAt(new Vector3(transform.position.x - direction.x, 0, transform.position.z - direction.y));
				return PushResult.Nothing;
			}
			//PUSHED OF THE EDGE
			else
			{
				//Face backward
				if (faceBackward) SnappedLookAt(new Vector3(-direction.x, 0, -direction.y));

				//Pushed out of bounds
				if (pushOverEdge) transform.position = currentTile.GetProjectedTilePos(direction);
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
		/// BEGINS execution of the ability
		/// </summary>
		public void Act(Ability ability, params Tile[] targetTiles)
		{
			//Only take action if able to ie. not affected by status effects
			if (!canAct) return;

			//Cache
			currentAbility = ability;
			currentTargetTiles = targetTiles;

			//Lock in movement and action
			_canAct = false;
			_canMove = false;

			//Face the the last tile that was passed in
			if (targetTiles.Length > 0)
				SnappedLookAt(targetTiles[targetTiles.Length - 1].transform.position);

			//Launch the Ability's animation triggering a series of
			//animations events to be executed with precision timing
			animator.SetTrigger(ability.animationTrigger);

			//Status effects
			foreach (var se in statusEffects)
				se.OnActed(this);
		}

		/// <summary>
		/// Perform the actual logic of the current ability
		/// </summary>
		public void Act()
		{
			//Targets calculated
			if (currentTargetTiles.Length == 0 || currentAbility == null) return;
			currentAbility.Perform(this, currentTargetTiles);

			//Event (This needs to go here in case refresh effect)
			onActed.Invoke(currentAbility);
		}

		/// <summary>
		/// Performs a specific effect in the current ability; Use to time effects with animation
		/// </summary>
		public void Act<T>() where T : Effect
		{
			if (currentTargetTiles.Length == 0 || currentAbility == null) return;
			currentAbility.Perform<T>(this, currentTargetTiles);

			//Event (This needs to go here in case refresh effect)
			onActed.Invoke(currentAbility);
		}

		//------------------- CALCULATE TILES
		/// <summary>
		/// Calculate the tiles that this unit can currently move to for this turn and point in game time.
		/// Filters based on which unit type cannot be traversed through.
		/// Returns the list of tiles if needed.
		/// </summary>
		public Tile[] CalculateMoveTiles(int range = 0)
		{
			//Default to this unit's move range if nothing passed in
			if (range == 0) range = moveRange;

			return possibleMoveTiles = Map.GetPossibleTiles(startTile.owner, startTile, range, pathBlockingUnitTypes);
		}

		public Type[] pathBlockingUnitTypes
		{
			get
			{
				var results = new List<Type>();

				//Allies
				if ((pathBlockers & TargetType.Allies) == TargetType.Allies)
					results.Add(typeof(AllyUnit));

				//Enemies
				if ((pathBlockers & TargetType.Enemies) == TargetType.Enemies)
					results.Add(typeof(EnemyUnit));

				//Crystals
				if ((pathBlockers & TargetType.Crystals) == TargetType.Crystals)
					results.Add(typeof(CrystalUnit));

				//InAnimates
				if ((pathBlockers & TargetType.InAnimates) == TargetType.InAnimates)
					results.Add(typeof(InAnimateUnit));

				//Animates
				if ((pathBlockers & TargetType.Animates) == TargetType.Animates)
					results.Add(typeof(AnimateUnit));

				return results.ToArray();
			}
		}

		/// <summary>
		/// Get the tiles that can be currently acted upon by this ability
		/// </summary>
		public Tile[] CalculateTargetTiles(Ability a, Tile startTile = null, bool onlyGetResults = false)
		{
			//Defaults
			if (startTile == null) startTile = currentTile;

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

						if (startTile.TryGetTile(offset, out Tile t))
						{
							if (!(t is UnWalkableTile))
								result.Add(t);
						}
					}
				}
			}

			//Cache
			var resultToArray = result.ToArray();
			if (!onlyGetResults) possibleTargetTiles = resultToArray;
			return resultToArray;
		}

		//------------------ OTHER
		public void ClearGhost()
		{
			ghost.SetActive(false);
			ghost.transform.position = transform.position;
		}
		#endregion

		#region Event System Interface Implementations
		// public override void OnPointerEnter(PointerEventData eventData)
		// {
		// 	base.OnPointerEnter(eventData);
		// 	//InfoPanel.current.SetText()
		// }
		// public override void OnPointerExit(PointerEventData eventData)
		// {
		// 	base.OnPointerExit(eventData);
		// }
		#endregion
	}
}