using StormRend.Defunct;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public enum EffectedTile
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        COUNT
    }

	/// <summary>
	/// Pushes adjacent units of the selected tiles away by push amount while dealing damage (or not)
	/// </summary>
    public class PushEffect : Effect
    {
        //[SerializeField] private EffectedTile m_direction;
        [SerializeField] int pushAmount = 1;
        [SerializeField] int damage = 1;
		public override bool Perform(Unit owner, Tile[] targetTiles)
        {
            // base.PerformEffect(_effectedNode, _thisUnit);

            // if (!m_isTileAllowed)
            //     return false;

            // Vector2Int nodeCoords = _effectedNode.GetCoordinates();
            // Vector2Int unitCoords = _thisUnit.coords;

            // Vector2Int tempCoords = new Vector2Int(0, 0);
            // xTile tempNode;

            // // Process Left
            // tempCoords = nodeCoords - m_left;
            // tempNode = xGrid.CoordToTile(tempCoords);

            // if (tempNode.GetUnitOnTop() != null)
            // {
            //     xUnit unit = tempNode.GetUnitOnTop();
            //     xTile newNode = xGrid.CoordToTile(tempCoords - m_left);

            //     if (m_doDamage)
            //         xGrid.CoordToTile(tempCoords).GetUnitOnTop().TakeDamage(m_damage);

            //     if (newNode.GetUnitOnTop() == null
            //         && newNode.m_nodeType != NodeType.BLOCKED)
            //         unit.MoveTo(newNode);
            // }

            // // Process Up
            // tempCoords = nodeCoords - m_up;
            // tempNode = xGrid.CoordToTile(tempCoords);

            // if (tempNode.GetUnitOnTop() != null)
            // {
            //     xUnit unit = tempNode.GetUnitOnTop();
            //     xTile newNode = xGrid.CoordToTile(tempCoords - m_up);

            //     if (m_doDamage)
            //         xGrid.CoordToTile(tempCoords).GetUnitOnTop().TakeDamage(m_damage);

            //     if (newNode.GetUnitOnTop() == null
            //         && newNode.m_nodeType != NodeType.BLOCKED)
            //         unit.MoveTo(newNode);
            // }

            // // Process Right
            // tempCoords = nodeCoords - m_right;
            // tempNode = xGrid.CoordToTile(tempCoords);

            // if (tempNode.GetUnitOnTop() != null)
            // {
            //     xUnit unit = tempNode.GetUnitOnTop();
            //     xTile newNode = xGrid.CoordToTile(tempCoords - m_right);

            //     if (m_doDamage)
            //         xGrid.CoordToTile(tempCoords).GetUnitOnTop().TakeDamage(m_damage);

            //     if (newNode.GetUnitOnTop() == null
            //         && newNode.m_nodeType != NodeType.BLOCKED)
            //         unit.MoveTo(newNode);
            // }

            // // Process Down
            // tempCoords = nodeCoords - m_down;
            // tempNode = xGrid.CoordToTile(tempCoords);

            // if (tempNode.GetUnitOnTop() != null)
            // {
            //     xUnit unit = tempNode.GetUnitOnTop();
            //     xTile newNode = xGrid.CoordToTile(tempCoords - m_down);

            //     if (m_doDamage)
            //         xGrid.CoordToTile(tempCoords).GetUnitOnTop().TakeDamage(m_damage);

            //     if (newNode.GetUnitOnTop() == null
            //         && newNode.m_nodeType != NodeType.BLOCKED)
            //         unit.MoveTo(newNode);
            // }

            return true;
        }
	}
}