using System;
using System.Collections.Generic;
using StormRend.Enums;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    /// <summary>
    /// Pushes adjacent units of the selected tiles away by push amount while dealing damage (or not)
    /// </summary>
    public class PushEffect : Effect
    {
        [EnumFlags, SerializeField, Tooltip("ONLY THESE TYPES CAN BE PUSHED: Allies, Enemies")] TargetMask unitTypesToPush;
        [SerializeField] int pushAmount = 1;
        [SerializeField] int damage = 0;
        [SerializeField] bool canPushOffEdge = true;
        List<Type> typesToCheck = new List<Type>();

        void OnValidate()
        {
            Prepare();
        }

        public override void Prepare(Unit owner = null)
        {
            //Populate unit type array
            typesToCheck.Clear();
            //Allies
            if ((unitTypesToPush & TargetMask.Allies) == TargetMask.Allies)
                typesToCheck.Add(typeof(AllyUnit));
            //Enemies
            if ((unitTypesToPush & TargetMask.Enemies) == TargetMask.Enemies)
                typesToCheck.Add(typeof(EnemyUnit));
        }

        public override void Perform(Unit owner, Tile[] targetTiles)
        {
            //Foreach target tile
            foreach (var tt in targetTiles)
            {
                //Try getting tiles around it
                for (int i = 0; i < 360; i += 90)
                {
                    // var deg = i * Mathf.Deg2Rad;
                    // var direction = new Vector2Int(Mathf.RoundToInt(Mathf.Cos(i)), -Mathf.RoundToInt(Mathf.Sin(i)));	//4 directions
                    Vector2Int direction = new Vector2Int();
                    switch (i)
                    {
                        case 0: direction = Vector2Int.up; break;
                        case 90: direction = Vector2Int.right; break;
                        case 180: direction = Vector2Int.down; break;
                        case 270: direction = Vector2Int.left; break;
                    }

                    if (tt.TryGetTile(direction, out Tile t))
                    {
                        //NOTE: Only animate units can be pushed/moved
                        //Try getting animate units on the tile
                        if (UnitRegistry.TryGetUnitTypeOnTile(t, out Unit unit, typesToCheck.ToArray()))
                        {
                            //Push the unit in the vector direction from target tile to adjacent tile
                            (unit as AnimateUnit).Move(direction * pushAmount, canPushOffEdge);
                            
                            //Do damage (where needed)
                            unit.TakeDamage(new DamageData(owner, damage));
                        }
                    }
                }
            }
        }
    }
}