/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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
        [EnumFlags, SerializeField, Tooltip("ONLY THESE TYPES CAN BE PUSHED: Allies, Enemies")] TargetType unitTypesToPush = TargetType.Enemies | TargetType.Allies;
        [SerializeField] int pushAmount = 1;
        [SerializeField] int damage = 0;
        [SerializeField] bool canPushOffEdge = true;
		[SerializeField] bool causeCrater = true;
		[SerializeField] float craterAmount = 0.2f;

        List<Type> typesToCheck = new List<Type>();

        public override void Prepare(Ability ability = null, Unit owner = null)
        {
            //Populate unit type array
            typesToCheck.Clear();
            //Allies
            if ((unitTypesToPush & TargetType.Allies) == TargetType.Allies)
                typesToCheck.Add(typeof(AllyUnit));
            //Enemies
            if ((unitTypesToPush & TargetType.Enemies) == TargetType.Enemies)
                typesToCheck.Add(typeof(EnemyUnit));
        }

        public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
            //Foreach target tile
            foreach (var tt in targetTiles)
            {
                //Try getting tiles around it
                for (int i = 0; i < 360; i += 90)
                {
                    //Set directions
                    Vector2Int direction = new Vector2Int();
                    switch (i)
                    {
						case 360:
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
                            var au = unit as AnimateUnit;
                            //Do incremental pushes to avoid moving through obstacles
                            for (int j = 0; j < pushAmount; ++j)
                            {
                                //Push the unit in the vector direction from target tile to adjacent tile
                                var pushResult = au.Push(direction, canPushOffEdge);

								//UNABLE to push
                                if (pushResult == PushResult.HitUnit || pushResult == PushResult.HitBlockedTile)
								{
                                    break;  //Break out of loop; Cannot move anymore
								}
								//Pushed OVER THE EDGE!
								//Unit should now be in mid air... definitely should kill now because its in an invalid position
								else if (pushResult == PushResult.OverEdge)
								{
									//Add physics to let the unit fall
									var rb = au.gameObject.AddComponent<Rigidbody>();

									//Add knock back torque for nice effect
									float rand = UnityEngine.Random.Range(0.25f, 1.3f);
									rb.AddTorque(direction.y * rand, 0, -direction.x * rand, ForceMode.Impulse);

									//Unit to finish it's death sequence and automatically deactivate after a few seconds
									au.Kill(owner);		//This SHOULD let the system know that this unit is dead or dying, so don't let it do AI stuff for instance
								}
                            }		
                            //Do damage (where needed)
                            if (damage > 0) unit.TakeDamage(new HealthData(owner, damage));
                        }
                    }
                }

				//Cause crater
				if (causeCrater)
				{
					var pos = tt.transform.position;
					pos.y -= craterAmount;
					tt.transform.position = pos;
				}
            }
        }
    }
}