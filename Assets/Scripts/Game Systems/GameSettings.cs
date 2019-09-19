using System.Collections;
using System.Collections.Generic;
using BhaVE.Patterns;
using UnityEngine;

namespace StormRend.Settings
{
    [CreateAssetMenu(menuName = "StormRend/Settings")]
    public class GameSettings : ScriptableSingleton<GameSettings>
    {
        public float tileSize;
    }
}
