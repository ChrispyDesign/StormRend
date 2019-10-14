using System;
using System.Collections.Generic;
using System.Linq;
using StormRend.Abilities;
using StormRend.Systems.Mapping;
using StormRend.Utility.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	[SelectionBase] //Avoid clicking on child objects
	[Serializable]
	public abstract class Unit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[TextArea, SerializeField] string description = "";

		//Inspector
		[Header("Colors")]
		[SerializeField] protected Color ghostColor = Color.blue;
		// [Tooltip("Inject this unit's mesh child object here. No need to create a separate mesh.")]
		// [SerializeField] GameObject ghostMesh;

		[Header("Abilities")]
		[SerializeField] protected int moveRange = 4;
		[SerializeField] protected Ability[] abilities;

		[Header("Stats")]
		[ReadOnlyField, SerializeField] int _hp;        //Current HP
		[SerializeField] int _maxHP = 3;

		//Properties
		public int HP
		{
			get => _hp;
			set => _hp = Mathf.Clamp(value, 0, _hp);
		}
		public int maxHP => _maxHP;
		public bool isDead => HP <= 0;
		public Tile currentTile
		{
			get => _currentTile;
			set => _currentTile = value;
		}

		//Events
		[Header("Events")]
		[SerializeField] public UnityEvent OnDeath;
		public static Action<Unit> onDeath;

		//Privates
		Tile _currentTile = null;		//The tile unit is currently on
		protected bool hasMoved = false;
		protected bool hasFinishedTurn = false;		//has peformed an ability and henced been locked and completed its turn
		protected GameObject ghostMesh;

	#region Startup
		void Start()
		{
			//Reset health
			HP = maxHP;

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

		}
	#endregion

	#region Health
		public void TakeDamage(int damage)
		{
			if (isDead) return;     //Can't beat a dead horse :P
			HP -= damage;
			if (HP <= 0)
			{
				Die();
			}
		}

		public virtual void Die()
		{
			OnDeath.Invoke();
			onDeath?.Invoke(this);      //Register death here

			//Disable unit and whatever else this needs to do upon death
			gameObject.SetActive(false);
		}
	#endregion

	#region Event System Interface Implementations
		public void OnPointerClick(PointerEventData eventData)
		{
			//If unit is movable, show move highlights for the tile this unit is on
			//Set this unit as current selected unit >> which will move the camera etc
			throw new System.NotImplementedException();
		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			//Show hover highlights for the tile this unit is on
			throw new System.NotImplementedException();
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			//Hide hover highlights for the tile this unit is on
			throw new System.NotImplementedException();
		}
	#endregion
	}
}