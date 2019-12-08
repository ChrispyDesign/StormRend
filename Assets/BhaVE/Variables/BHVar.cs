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
using pokoro.BhaVE.Core.Events;
using UnityEngine;
namespace pokoro.BhaVE.Core.Variables
{
	public abstract class BHVar<T> : ScriptableObject
	{
		[TextArea] public string description = "";
		[SerializeField] protected T _value;

		//Can utilise both custom BhaveEvents standard events that can use callbacks
		[SerializeField] protected BhaveEvent OnChanged = null;
		public virtual event Action onChanged = null;

		public virtual T value
		{
			get => _value;

			set
			{
				if (!_value.Equals(value))  //NOTE This might be problematic
				{
					_value = value;
					OnChanged?.Raise();
					onChanged?.Invoke();
				}
			}
		}

		protected bool Equals(BHVar<T> other)
			=> EqualityComparer<T>.Default.Equals(_value, other._value);

		public override bool Equals(object other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			if (this.GetType() != other.GetType()) return false;
			return Equals(other as BhaveVar<T>);
		}
		public override int GetHashCode()
		{
			unchecked
			{
				return EqualityComparer<T>.Default.GetHashCode(_value);
			}
		}
		public static bool operator ==(BHVar<T> left, BHVar<T> right)
			=> Equals(left, right);
		public static bool operator !=(BHVar<T> left, BHVar<T> right)
			=> !Equals(left, right);
	}
}
