using System;
using StormRend.Abilities;
using StormRend.Systems.Mapping;
using StormRend.Utility.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	[SelectionBase] //Avoid clicking on child objects
	[Serializable]
	public class Unit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[TextArea, SerializeField] string description = "";

		//Inspector
		[Header("Colors")]
		[SerializeField] protected Color unitColor = Color.red;	//Might not need this
		[SerializeField] protected Color ghostColor = Color.blue;

		[Header("Abilities")]
		[SerializeField] protected int moveRange = 4;
		[SerializeField] protected Ability[] abilities;

		[Header("Stats")]
		[ReadOnlyField, SerializeField] int _hp;		//Current HP
		[SerializeField] int _maxHP = 3;

		//Properties
		public bool isDead => _hp <= 0;
		public int HP
		{
			get => _hp;
			set => _hp = Mathf.Clamp(value, 0, _hp);
		}
		public int maxHP => _maxHP;

		//Events
		[Header("Events")]
		[SerializeField] public UnityEvent OnDeath;
		public static Action<Unit> onDeath;

		//Privates
		//TODO needs review
		protected bool hasMoved = false;
		protected bool hasPerformedAbility = false;
		Tile onTile;

		//Core
		public void TakeDamage(int damage)
		{
			if (isDead) return;		//Can't beat a dead horse :P
			HP -= damage;
			if (HP <= 0)
			{
				Die();
			}
		}

		public virtual void Die()
		{
			OnDeath.Invoke();
			onDeath?.Invoke(this);		//Register death here

			//Disable unit and whatever else this class needs to do
			gameObject.SetActive(false);
		}

		//Event system interface implementations
		public void OnPointerClick(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			//Tasks:
			//Invoke any events?
			//Update tile highlights for move
			//Update

			throw new System.NotImplementedException();
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}
	}
}