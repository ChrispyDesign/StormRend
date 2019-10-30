using System;
using UnityEngine;

namespace StormRend.Utility.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
    public class EnumFlagsAttribute : PropertyAttribute
    {
        //To display bitmasks in the inspector
    }
}