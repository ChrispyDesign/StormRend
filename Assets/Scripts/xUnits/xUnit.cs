using System;
using System.Collections.Generic;
using StormRend.Abilities;
using StormRend.Utility.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Defunct
{
    //Review and Refactor
    [SelectionBase]
    public abstract class xUnit : MonoBehaviour, xISelectable, xIHoverable
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

        [Header("Mesh")]
        [SerializeField] GameObject m_duplicateMesh = null;

        [Header("Abilities")]
        [SerializeField] protected xAbility m_passiveAbility;
        [SerializeField] protected xAbility[] m_firstAbilities;
        [SerializeField] protected xAbility[] m_secondAbilities;

        [Header("Unit Stats")]
        [SerializeField] int m_maxHP = 4;
        [SerializeField] int m_maxMoveRange = 4;

        protected bool m_hasMoved;
        protected bool m_hasAttacked;
        protected bool m_isSelected;
        protected xAbility m_selectedAbility;
        List<xTile> m_availableTiles;		//This gets the tiles this unit is able to move to. Rename: tilesAbleToMoveTo
        List<xTile> m_attackTiles;

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
        public List<xTile> GetAvailableTiles() { return m_availableTiles; }
        public xAbility GetSelectedAbility() { return m_selectedAbility; }
        public List<xTile> GetAttackTiles() { return m_attackTiles; }
        public xTile GetTile() { return xGrid.CoordToTile(m_coordinates); }
        public int GetMoveRange() { return m_maxMoveRange; }
        public bool GetIsSelected() { return m_isSelected; }
        public bool GetHasMoved() { return m_hasMoved; }
        public bool GetHasAttacked() { return m_hasAttacked; }
        public void GetAbilities(ref xAbility passive,
            ref xAbility[] first, ref xAbility[] second)
        {
            passive = m_passiveAbility;
            first = m_firstAbilities;
            second = m_secondAbilities;
        }
        public void SetHasMoved(bool moveFlag) { m_hasMoved = moveFlag; }
        public void SetHasAttacked(bool attackFlag) { m_hasAttacked = attackFlag; }
        public void SetAttackNodes(List<xTile> tiles) { m_attackTiles = tiles; }
        public void SetSelectedAbility(xAbility ability) { m_selectedAbility = ability; }
        public void SetIsSelected(bool isSelected) { m_isSelected = isSelected; }
        public void SetDuplicateMeshVisibilty(bool _isOff) { m_duplicateMesh.SetActive(_isOff); }

        public bool isDead => m_HP <= 0;

	#endregion

        void Start()
        {
            m_HP = m_maxHP;
            m_attackTiles = new List<xTile>();
        }

        public void MoveTo(xTile tile)
        {
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
            foreach (xTile node in m_attackTiles)
			{
				if (node.m_nodeType == NodeType.EMPTY
					|| node.m_nodeType == NodeType.BLOCKED)
					continue;

				node.m_attackCover.SetActive(true);
				node.m_moveCover.SetActive(false);
				node.m_selected = true;
            }
        }
        public void UnShowAttackTiles()
        {
            foreach (xTile node in m_attackTiles)
			{
				if (node.m_nodeType == NodeType.EMPTY)
					continue;

				node.m_attackCover.SetActive(false);
				node.m_moveCover.SetActive(false);
				node.m_selected = false;
            }
        }

        public void MoveDuplicateTo(xTile _moveToNode)
        {
            m_duplicateMesh.transform.position = _moveToNode.GetNodePosition();
        }

        public virtual void OnSelect()
        {
			//Highlight Tile Move
            if (xGameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.MOVE &&
                !m_afterClear)
            {

                m_availableTiles = xDijkstra.Instance.m_validMoves;

                foreach (xTile tile in m_availableTiles)
                {
                    if (tile.GetUnitOnTop())
                        continue;

					tile.m_attackCover.SetActive(false);
					tile.m_moveCover.SetActive(true);
					tile.m_selected = true;
                }
            }

            if (xGameManager.singleton.GetPlayerController().GetCurrentMode() == PlayerMode.ATTACK)
            {
                xTile node = GetTile();
                node.OnSelect();
            }

            FindObjectOfType<Camera>().GetComponent<CameraMove>().MoveTo(transform.position, 1.0f);
        }

        public virtual void OnDeselect()
        {
            xGrid.CoordToTile(m_coordinates).OnDeselect();
        }

        public virtual void OnHover()
        {
			xTile tile = xGrid.CoordToTile(m_coordinates);
			if (tile.m_nodeType == NodeType.WALKABLE && tile.GetUnitOnTop() != null)
			{
				tile.m_onHoverCover.SetActive(true);
			}
		}

        public virtual void OnUnhover()
        {
			xTile tile = xGrid.CoordToTile(m_coordinates);
			tile.m_onHoverCover.SetActive(false);
		}

        public void TakeDamage(int damage)
        {
			if (isDead) return;     //Can't beat a dead horse :P

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
            xGrid.CoordToTile(this.m_coordinates).SetUnitOnTop(null);

            //This should work for any unit regardless of type
            xGameManager.singleton.RegisterUnitDeath(this);
        }
    }
}