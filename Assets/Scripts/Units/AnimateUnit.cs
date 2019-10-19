using System.Collections.Generic;
using StormRend.Abilities;
using StormRend.MapSystems.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	[SelectionBase] //Avoid clicking on child objects
	public abstract class AnimateUnit : Unit, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		//Inspector
		[Header("Color")]
		[SerializeField] protected Color ghostColor = Color.blue;

		[Header("Abilities")]
		[SerializeField] protected int moveRange = 4;
		[SerializeField] protected Ability[] abilities;

		//Properties
		public Ability currentAbility { get; set; } = null;

		//Members
		protected bool hasMoved = false;
		public Tile[] possibleMoveTiles { get; set; }
		//has performed an ability and hence this unit has completed it's turn and is locked until next turn
		protected bool hasCasted = false;	
		protected GameObject ghostMesh;

	#region Startup
		void Start()	//This will not block base.Start()
		{
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
		public void MoveTo(Tile tile, bool useGhost = false)
		{

		}
		public void MoveTo(Vector2Int direction, bool useGhost = false)
		{
			//Where should the push effect kill logic be implemented?
		}

		public override void Die()
		{
			base.Die();

			//TEMP
			gameObject.SetActive(false);
		}

	#endregion

	#region Event System Interface Implementations
		public void OnPointerClick(PointerEventData eventData)
		{
			//If unit is movable, show move highlights for the tile this unit is on
			//Set this unit as current selected unit >> which will move the camera etc
			Debug.LogFormat("{0}.Click", name);
		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			//Show hover highlights for the tile this unit is on
			Debug.LogFormat("{0}.Hover", name);
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			//Hide hover highlights for the tile this unit is on
			Debug.LogFormat("{0}.UnHover", name);
		}
	#endregion
	}
}