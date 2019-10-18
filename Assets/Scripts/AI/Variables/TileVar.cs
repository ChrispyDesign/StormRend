using pokoro.BhaVE.Core.Variables;
using StormRend.Systems.Mapping;
using UnityEngine;

namespace StormRend.Variables
{
    [CreateAssetMenu(menuName = "StormRend/Variables/TileVar", fileName = "TileVar")]
    public class TileVar : BhaveVar<Tile>
    {
        public static implicit operator TileVar(Tile rhs)
        {
            return new TileVar { value = rhs };
        }

        public static implicit operator Tile(TileVar self)
        {
            return self.value;
        }
    }
}