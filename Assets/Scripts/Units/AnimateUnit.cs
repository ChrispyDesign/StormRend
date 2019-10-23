using System;
using System.Collections.Generic;
using System.Linq;
using StormRend.Abilities;
using StormRend.Enums;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Utility.Attributes;
using UnityEngine;
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
		[EnumFlags, SerializeField] TargetUnitMask pathblockingUnitTypes = TargetUnitMask.Enemies;
		[SerializeField] protected Ability[] abilities;

		[Header("Color")]
		[SerializeField] protected Color ghostColor = Color.blue;

		//Properties
		public Tile ghostTile { get; set; } = null;	//The tile this unit wants to move to
		public Ability currentAbility { get; set; } = null;

		//Members
		protected bool hasActed = false;	//has performed an ability and hence this unit has completed it's turn and is locked until next turn
		public Tile[] possibleMoveTiles;
		public Tile[] possibleTargetTiles { get; set; }

		protected GameObject ghostMesh;

	#region Startup
		protected override void Start()	//This will not block base.Start()
		{
			base.Start();

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
		public void Move(Tile destination, bool useGhost = false)
		{
			//Only set the position of the ghsot
			if (useGhost)
			{
				Debug.Log("Use Ghost");
				//Filter
				// if (!possibleMoveTiles.Contains(destination)) return;

				//Move
				ghostTile = destination;

				//Activate ghost
				ghostMesh.SetActive(true);
				ghostMesh.transform.position = ghostTile.transform.position;
			}
			//Move the actual unit (permanent)
			else
			{
				//Filter
				if (!possibleMoveTiles.Contains(destination)) return;

				//Move
				currentTile = destination;

				//Deactivate ghost ??? Should this be here?
				ghostMesh.SetActive(false);
			}

			// //Set the new tile
			// currentTile = destination;

			// //Move/Position logic	- NEED REVIEW
			// transform.position = currentTile.transform.position;
		}
		/// <summary>
		/// Move Unit by direction ie. Move({2, 1}) means the unit to move right 2 and forward 1.
		/// Returns false if the unit moved onto an empty space
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="useGhost"></param>
		/// <returns></returns>
		public bool Move(Vector2Int vector, bool useGhost = false)
		{
			//Where should the push effect kill logic be implemented?
			throw new NotImplementedException();
		}

		public void PerformAbility(Ability ability, params Tile[] targetTiles)
		{
			ability.Perform(this, targetTiles);
		}
		public void PerformAbility(Ability ability, params Unit[] targetUnits)
		{
			ability.Perform(this, targetUnits.Select(x => x.currentTile).ToArray());
		}

		/// <summary>
		/// Calculate the tiles that this unit can currently move to for this turn and point in game time.
		/// Filters based on which unit type cannot be traversed through.
		/// Returns the list of tiles if needed.
		/// </summary>
		public Tile[] CalculateMoveTiles()
		{
			var pathblockers = new List<Type>();

			//Allies
			if ((pathblockingUnitTypes & TargetUnitMask.Allies) == TargetUnitMask.Allies)
				pathblockers.Add(typeof(AllyUnit));

			//Enemies
			if ((pathblockingUnitTypes & TargetUnitMask.Enemies) == TargetUnitMask.Enemies)
				pathblockers.Add(typeof(EnemyUnit));

			//Crystals
			if ((pathblockingUnitTypes & TargetUnitMask.Crystals) == TargetUnitMask.Crystals)
				pathblockers.Add(typeof(CrystalUnit));

			//InAnimates
			if ((pathblockingUnitTypes & TargetUnitMask.InAnimates) == TargetUnitMask.InAnimates)
				pathblockers.Add(typeof(InAnimateUnit));

			//Animates
			if ((pathblockingUnitTypes & TargetUnitMask.Animates) == TargetUnitMask.Animates)
				pathblockers.Add(typeof(AnimateUnit));

			return possibleMoveTiles = Map.GetPossibleTiles(currentTile.owner, currentTile, moveRange, pathblockers.ToArray());
		}

		public override void Die()
		{
			base.Die();

			//TEMP
			gameObject.SetActive(false);
		}

	#endregion

	#region Filtered Gets
		public List<Ability> GetAbilitiesByType(AbilityType type) => abilities.Where(x => x.type == type).ToList();
	#endregion

	#region Event System Interface Implementations
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			//Show hover highlights for the tile this unit is on
			// Debug.LogFormat("{0}.Hover", name);
		}
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			//Hide hover highlights for the tile this unit is on
			// Debug.LogFormat("{0}.UnHover", name);
		}
	#endregion
	}
}