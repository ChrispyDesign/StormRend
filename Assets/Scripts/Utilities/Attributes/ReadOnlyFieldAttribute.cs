using System;
using UnityEngine;

namespace StormRend.Utility.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class ReadOnlyFieldAttribute : PropertyAttribute
    {
        //Field will be displayed but cannot be modified in inspector
    }
}