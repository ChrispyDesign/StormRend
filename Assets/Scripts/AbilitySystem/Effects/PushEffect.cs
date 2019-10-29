using System;
using System.Collections.Generic;
using StormRend.Defunct;
using StormRend.Enums;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Pushes adjacent units of the selected tiles away by push amount while dealing damage (or not)
	/// </summary>
    public class PushEffect : Effect
    {
        //[SerializeField] private EffectedTile m_direction;
        [SerializeField] int pushAmount = 1;
        [SerializeField] int damage = 1;
		[SerializeField, Tooltip("Choose unit types that can be pushed: Allies, Enemies, Inanimates")] TargetMask unitMask; 
		List<Type> unitTypes = new List<Type>();

		public override void Prepare(Unit owner)
		{
			//Populate unit type array
			//Allies
			if ((unitMask & TargetMask.Allies) == TargetMask.Allies)
				unitTypes.Add(typeof(AllyUnit));
			//Enemies
			if ((unitMask & TargetMask.Enemies) == TargetMask.Enemies)
				unitTypes.Add(typeof(EnemyUnit));
			// //Inanimates
			// if ((unitMask & TargetMask.InAnimates) == TargetMask.InAnimates)
			// 	listOfUnitTypes.Add(typeof(InAnimateUnit));
			//Animates
			if ((unitMask & TargetMask.Animates) == TargetMask.Animates)
				unitTypes.Add(typeof(AnimateUnit));
		}

		public override void Perform(Unit owner, Tile[] targetTiles)
        {
			//Foreach target tile
			foreach (var tt in targetTiles)
			{
				//Try getting tiles around it
				for (int i = 0; i <= 360; i += 90)
				{
					var deg = i * Mathf.Deg2Rad;
					var direction = new Vector2Int((int)Mathf.Cos(i), (int)Mathf.Sin(i));	//4 directions
					if (tt.TryGetTile(direction, out Tile t))
					{
						//NOTE: Only animate units can be pushed/moved
						//Try getting animate units on the tile
						if (UnitRegistry.TryGetUnitTypeOnTile<AnimateUnit>(t, out AnimateUnit au))
						{
							//Push the unit back in the vector direction from target to adjacent tile
							if (unitTypes.Contains(au.GetType()))	//Filter
								au.Move(direction, true);
						}
					}
				}
			}

			//vector = destination - position
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

        }
	}
}