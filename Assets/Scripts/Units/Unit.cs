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
		// - Refactor get/setters to properties where appropriate
		// - Shwo/Unshow attack tiles should be handled by a different utility class
		// - Decouple Camera in OnSelect()
		// - Decouple/reduce coupling of game manager

        [SerializeField][ReadOnlyField] int m_HP;
        static bool m_isDead;

        [SerializeField] Vector2Int m_coordinates;
		public Vector2Int coords {
			get => m_coordinates;
			set => m_coordinates = value; }
			
        public bool m_afterClear;
		public bool m_protected;
		public bool m_blind;
		public bool m_haste;
		public bool m_crippled;

        [Header("Mesh")]
        [SerializeField] GameObject m_duplicateMesh = null;

        [Header("Abilities")]
        [SerializeField] protected Ability m_passiveAbility;
        [SerializeField] protected Ability[] m_firstAbilities;
        [SerializeField] protected Ability[] m_secondAbilities;

        [Header("Unit Stats")]
        [SerializeField] int m_maxHP = 4;
        [SerializeField] int m_maxMoveRange = 4;


        [Space]
        [Header("Unit Interaction")]
        [SerializeField] UnityEvent m_onSelect;
        [SerializeField] UnityEvent m_onDeselect;
        [SerializeField] UnityEvent m_onHover;
        [SerializeField] UnityEvent m_onUnhover;


        protected bool m_hasMoved;
        protected bool m_hasAttacked;
        protected bool m_isSelected;
        protected Ability m_selectedAbility;
        List<Tile> m_availableTiles;
        List<Tile> m_attackTiles;

        public Action OnDie = delegate
        {
            m_isDead = false;
        };

		//Properties
		public int HP {
			get => m_HP;
			set => m_HP = Mathf.Clamp(value, 0, m_maxHP); }
		public int maxHP => m_maxHP;

    #region Properties
        public List<Tile> GetAvailableTiles() { return m_availableTiles; }
        public Ability GetSelectedAbility() { return m_selectedAbility; }
        public List<Tile> GetAttackTiles() { return m_attackTiles; }
        public Tile GetTile() { return Grid.CoordToTile(m_coordinates); }
        public int GetMoveRange() { return m_maxMoveRange; }
        public bool GetIsSelected() { return m_isSelected; }
        public bool GetHasMoved() { return m_hasMoved; }
        public bool GetHasAttacked() { return m_hasAttacked; }
        public void GetAbilities(ref Ability passive,
            ref Ability[] first, ref Ability[] second)
        {
            passive = m_passiveAbility;
            first = m_firstAbilities;
            second = m_secondAbilities;
        }
        public void SetHasMoved(bool moveFlag) { m_hasMoved = moveFlag; }
        public void SetHasAttacked(bool attackFlag) { m_hasAttacked = attackFlag; }
        public void SetAttackNodes(List<Tile> tiles) { m_attackTiles = tiles; }
        public void SetSelectedAbility(Ability ability) { m_selectedAbility = ability; }
        public void SetIsSelected(bool isSelected) { m_isSelected = isSelected; }
        public void SetDuplicateMeshVisibilty(bool _isOff) { m_duplicateMesh.SetActive(_isOff); }

        public bool isDead => m_HP <= 0;

	#endregion

        void Start()
        {
            m_HP = m_maxHP;
            m_attackTiles = new List<Tile>();
        }

        public void MoveTo(Tile tile)
        {
			if (m_crippled)
				return;

			//PROBABLY BAD
			var oldPos = transform.position;	//Record old position to change

            GetTile().SetUnitOnTop(null);
            tile.SetUnitOnTop(this);

            m_coordinates = tile.GetCoordinates();
            transform.position = tile.GetNodePosition();

			//Rotate unit accordingly
			var moveDir = Vector3.Normalize(transform.position - oldPos);
			if (moveDir != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);
        }

	
        public void ShowAttackTiles()
        {
            foreach (Tile node in m_attackTiles)
			{
				if (node.m_nodeType == NodeType.EMPTY
					|| node.m_nodeType == NodeType.BLOCKED)
					continue;

				Unit unit = node.GetUnitOnTop();
				if (unit != null)
				{
					unit.GetComponent<BoxCollider>().enabled = false;
				}

				node.m_attackCover.SetActive(true);
				node.m_moveCover.SetActive(false);
				node.m_selected = true;
            }
        }
        public void UnShowAttackTiles()
        {
            foreach (Tile node in m_attackTiles)
			{
				if (node.m_nodeType == NodeType.EMPTY)
					continue;

				Unit unit = node.GetUnitOnTop();
				if (unit != null)
				{
					unit.GetComponent<BoxCollider>().enabled = true;
				}


				node.m_attackCover.SetActive(false);
				node.m_moveCover.SetActive(false);
				node.m_selected = false;
            }
        }

        public void MoveDuplicateTo(Tile _moveToNode)
        {
            m_duplicateMesh.transform.position = _moveToNode.GetNodePosition();
        }

        public virtual void OnSelect()
        {
            m_onSelect.Invoke();

            if (GameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.MOVE &&
                !m_afterClear)
            {

                m_availableTiles = Dijkstra.Instance.m_validMoves;

                foreach (Tile node in m_availableTiles)
                {
                    if (node.GetUnitOnTop())
                        continue;

					node.m_attackCover.SetActive(false);
					node.m_moveCover.SetActive(true);
					node.m_selected = true;
                }
            }

            if (GameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.ATTACK)
            {
                Tile node = GetTile();
                node.OnSelect();
            }

            FindObjectOfType<Camera>().GetComponent<CameraMove>().MoveTo(transform.position, 1.0f);
        }

        public virtual void OnDeselect()
        {
            m_onDeselect.Invoke();

            Grid.CoordToTile(m_coordinates).OnDeselect();
        }

        public virtual void OnHover()
        {
            m_onHover.Invoke();

			Tile tile = Grid.CoordToTile(m_coordinates);
			if (tile.m_nodeType == NodeType.WALKABLE && tile.GetUnitOnTop())
			{
				tile.m_onHoverCover.SetActive(true);
			}
		}

        public virtual void OnUnhover()
        {
            m_onUnhover.Invoke();

			Tile tile = Grid.CoordToTile(m_coordinates);
			tile.m_onHoverCover.SetActive(false);
		}

        public void TakeDamage(int damage)
        {
			if (isDead) return;     //Can't beat a dead horse :P

			if (!m_protected)
				m_HP -= damage;

            if (m_HP <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            OnDie.Invoke();

            //Temp
            gameObject.SetActive(false);
            Grid.CoordToTile(this.m_coordinates).SetUnitOnTop(null); 

            //This should work for any unit regardless of type
            GameManager.singleton.RegisterUnitDeath(this);
        }
    }
}