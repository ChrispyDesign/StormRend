using System;
using StormRend.Systems.Mapping;
using StormRend.Utility.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Units
{
	public abstract class Unit : MonoBehaviour 
	{
		[TextArea(0,2), SerializeField] string description = "";

		//Inspector
		[Header("Stats")]
		[ReadOnlyField, SerializeField] protected int _hp;        //Current HP
		[SerializeField] protected int _maxHP = 3;

		//Properties
		public int HP
		{
			get => _hp;
			set => _hp = Mathf.Clamp(value, 0, _hp);
		}
		public int maxHP => _maxHP;
		public bool isDead => HP <= 0;
		public Tile currentTile { get; set; }

		//Events
		[Header("Events")]
		[SerializeField] public UnityEvent OnDeath;
		public static Action<Unit> onDeath;

		//Privates

		#region Startup
		void Start()
		{
			//Reset health
			HP = maxHP;
		}
		#endregion

		#region Health
		public void TakeDamage(int damage)
		{
			if (isDead) return;     //Can't beat a dead horse :P
			HP -= damage;
			if (HP <= 0) Die();
		}

		public virtual void Die()
		{
			OnDeath.Invoke();           //UnityEvent
			onDeath?.Invoke(this);      //System.Action
		}
		#endregion
	}
}