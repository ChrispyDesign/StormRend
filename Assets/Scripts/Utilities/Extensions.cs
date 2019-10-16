using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Utility
{
    public static class UnityExtensions
    {
        /// <summary>
        /// Check if a layer is in a layermask
        /// </summary>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}