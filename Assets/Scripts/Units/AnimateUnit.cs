using System;
using System.Collections.Generic;
using StormRend.Abilities;
using StormRend.Systems.Mapping;
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

		//Privates
		protected bool hasMoved = false;
		protected bool hasFinishedTurn = false;		//has peformed an ability and henced been locked and completed its turn
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
		public void MoveTo(Tile tile)
		{

		}
		public void MoveTo(Vector2Int direction)
		{
			//Where should the push effect kill logic be implemented?
		}
	#endregion

	#region Event System Interface Implementations
		public void OnPointerClick(PointerEventData eventData)
		{
			//If unit is movable, show move highlights for the tile this unit is on
			//Set this unit as current selected unit >> which will move the camera etc
			Debug.LogFormat("{0}.OnPointerClick()");
		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			//Show hover highlights for the tile this unit is on
			Debug.LogFormat("{0}.OnPointerEnter()");
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			//Hide hover highlights for the tile this unit is on
			Debug.LogFormat("{0}.OnPointerExit()");
		}
	#endregion
	}
}