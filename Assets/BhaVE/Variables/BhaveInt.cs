using System;
using UnityEngine;
namespace pokoro.BhaVE.Core.Variables
{
	[Serializable, CreateAssetMenu(menuName = "BhaVE/Variable/Int", fileName = "BhaveInt")]
	public sealed class BhaveInt : BhaveVar<int>
	{
		//Implicit convert definition to convert from int to BhaveInt
		//ie. Allows this:
		//BhaveInt bi = 10;		//Don't need bi.value = 10;
		public static implicit operator BhaveInt(int rhs)
		{
			return new BhaveInt { value = rhs };
		}

		//Implicit convert definition to convert from BhaveInt to Int
		//ie. Allows this:
		//BhaveInt bi = 5;
		//int i = bi;	//Don't need int i = bi.value;
		public static implicit operator int(BhaveInt self)
		{
			return self.value;
		}

		public void Increment(int amount) => value += amount;
		public void Decrement(int amount) => value -= amount;
	}
}
