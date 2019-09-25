using System;
using System.Collections.Generic;
using StormRend.Utility.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend
{
	//Review and Refactor
	[SelectionBase]
	public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
	{
		//This class is doing WAY TOO MUCH!
		// - Refactor get/setters to properties where appropriate
		// - Shwo/Unshow attack tiles should be handled by a different utility class
		// - Decouple Camera in OnSelect()
		// - Decouple/reduce coupling of game manager

	#region Inspector
		[SerializeField] [ReadOnlyField] int m_HP;
		static bool m_isDead;

		[SerializeField] Vector2Int m_coordinates;
		public Vector2Int coords
		{
			get => m_coordinates;
			set => m_coordinates = value;
		}

		[Header("Unit Properties")]
        [Tooltip("A unit is closed when it has moved but not attacked and another unit has moved and ")]
		public bool isLocked;
		public bool isProtected;
		public bool isBlind;
		public bool isProvoking;
		public bool isCrippled;

		[Header("Mesh")]
		[SerializeField] GameObject m_duplicateMesh = null;

		[Header("Abilities")]
		[SerializeField] protected Ability m_passiveAbility;
		[SerializeField] protected Ability[] m_firstAbilities;
		[SerializeField] protected Ability[] m_secondAbilities;

		[Header("Unit Stats")]
		[SerializeField] int m_maxHP = 4;
		[SerializeField] int m_maxMoveRange = 4;
		// [Space]
		// [Header("Unit Interaction")]
		// [SerializeField] UnityEvent m_onSelect;
		// [SerializeField] UnityEvent m_onDeselect;
		// [SerializeField] UnityEvent m_onHover;
		// [SerializeField] UnityEvent m_onUnhover;
	#endregion Inspector
	#region Properties
		public List<Tile> GetAvailableTiles() { return availableTiles; }
		public Ability GetSelectedAbility() { return selectedAbility; }
		public List<Tile> GetAttackTiles() { return attackTiles; }
		public Tile GetTile() { return Grid.CoordToTile(m_coordinates); }
		public int GetMoveRange() { return m_maxMoveRange; }
		public bool GetIsSelected() { return isSelected; }
		public bool GetHasMoved() { return hasMoved; }
		public bool GetHasAttacked() { return hasAttacked; }
		public void GetAbilities(ref Ability passive, ref Ability[] first, ref Ability[] second)
		{
			passive = m_passiveAbility;
			first = m_firstAbilities;
			second = m_secondAbilities;
		}
		public void SetHasMoved(bool moveFlag) { hasMoved = moveFlag; }
		public void SetHasAttacked(bool attackFlag) { hasAttacked = attackFlag; }
		public void SetAttackNodes(List<Tile> tiles) { attackTiles = tiles; }
		public void SetSelectedAbility(Ability ability) { selectedAbility = ability; }
		public void SetIsSelected(bool isSelected) { this.isSelected = isSelected; }
		public void SetDuplicateMeshVisibilty(bool _isOff) { m_duplicateMesh.SetActive(_isOff); }

		public bool isDead => m_HP <= 0;
		public int HP
		{
			get => m_HP;
			set => m_HP = Mathf.Clamp(value, 0, m_maxHP);
		}
		public int maxHP => m_maxHP;
	#endregion Properties

		//----------- Protected ---------------
		protected bool hasMoved;
		protected bool hasAttacked;
		protected bool isSelected;
		protected Ability selectedAbility;

		//----------- Privates ---------------
		List<Tile> availableTiles;
		List<Tile> attackTiles;
		new Camera camera;
		CameraMove cameraMover;

		//---------- Others ----------------
		public Action OnDie = delegate  { m_isDead = false; };
		[HideInInspector] public int provokeDamage;


	#region Core
		void Awake()
		{
			camera = FindObjectOfType<Camera>();
			cameraMover = camera.GetComponent<CameraMove>();
		}

		void Start()
		{
			m_HP = m_maxHP;
			attackTiles = new List<Tile>();
		}

		void Update()
		{

		}
	#endregion Core

	#region Select and Hover
		public virtual void OnSelect()
		{
			// m_onSelect.Invoke();

            if (GameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.MOVE && !isLocked)
            {
				availableTiles = Dijkstra.Instance.validMoves;

				foreach (Tile t in availableTiles)
				{
					if (t.GetUnitOnTop())
						continue;

					t.attackHighlight.SetActive(false);
					t.moveHighlight.SetActive(true);
					t.m_selected = true;
				}
			}

			//Do on select for tile that this unti is on
			if (GameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.ATTACK)
			{
				GetTile().OnSelect();
			}

			//Move camera to just selected unit
			cameraMover.MoveTo(transform.position, 1.0f);
		}

		public virtual void OnDeselect()
		{
			// m_onDeselect.Invoke();
			Grid.CoordToTile(m_coordinates).OnDeselect();
			UIManager.GetInstance().GetAbilitySelector().GetButtonPanel().SetActive(false);
		}

		public virtual void OnHover()
		{
			// m_onHover.Invoke();

			Tile tile = Grid.CoordToTile(m_coordinates);
			if (tile.m_nodeType == NodeType.WALKABLE && tile.GetUnitOnTop())
			{
				tile.hoverHighlight.SetActive(true);
			}
		}

		public virtual void OnUnhover()
		{
			// m_onUnhover.Invoke();

			Tile tile = Grid.CoordToTile(m_coordinates);
			tile.hoverHighlight.SetActive(false);
		}
	#endregion Select and Hover

	#region Helpers
        public void MoveTo(Tile destTile)
        {
			if (isCrippled)     //TODO temp don't move if crippled
				return;

			//PROBABLY BAD
			var oldPos = transform.position;    //Record old position to change

			GetTile().SetUnitOnTop(null);
			destTile.SetUnitOnTop(this);

			m_coordinates = destTile.GetCoordinates();
			transform.position = destTile.GetNodePosition();

			//Rotate unit accordingly
			var moveDir = Vector3.Normalize(transform.position - oldPos);
			if (moveDir != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);

			//Move camera to move destination
			cameraMover.MoveTo(destTile.transform.position, 1f);
		}

		public void ShowAttackTiles()
		{
			foreach (Tile t in attackTiles)
			{
				if (t.m_nodeType == NodeType.EMPTY || t.m_nodeType == NodeType.BLOCKED)
					continue;

				Unit unit = t.GetUnitOnTop();
				if (unit) unit.GetComponent<BoxCollider>().enabled = false;

				t.attackHighlight.SetActive(true);
				t.moveHighlight.SetActive(false);
				t.m_selected = true;
			}
		}
		public void UnShowAttackTiles()
		{
			foreach (Tile t in attackTiles)
			{
				if (t.m_nodeType == NodeType.EMPTY)
					continue;

				Unit unit = t.GetUnitOnTop();
				if (unit != null)
				{
					unit.GetComponent<BoxCollider>().enabled = true;
				}


				t.attackHighlight.SetActive(false);
				t.moveHighlight.SetActive(false);
				t.m_selected = false;
			}
		}

		public void MoveDuplicateTo(Tile _moveToNode)
		{
			m_duplicateMesh.transform.position = _moveToNode.GetNodePosition();
		}

		public void TakeDamage(int damage)
		{
			if (isDead) return;     //Can't beat a dead horse :P

			if (!isProtected)
				m_HP -= damage;

			if (m_HP <= 0)
			{
				Die();
			}
		}

		public virtual void Die()
		{
			OnDie.Invoke();

			GameManager.singleton.sage.CheckSoulCommune(this);

			//Temp
			gameObject.SetActive(false);
			Grid.CoordToTile(this.m_coordinates).SetUnitOnTop(null);

			//This should work for any unit regardless of type
			GameManager.singleton.RegisterUnitDeath(this);
		}
	#endregion
	}
}