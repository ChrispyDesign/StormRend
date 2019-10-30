using StormRend.MapSystems.Tiles;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	public abstract class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
		public Tile currentTile { get; set; }	//The tile this unit is currently/originally on

		//Events
		[Header("Events")]
		public UnitEvent OnDeath;

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
			OnDeath.Invoke(this);
		}
	#endregion

	#region Event System
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			// throw new NotImplementedException();
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			// throw new NotImplementedException();
		}
	#endregion
	}
}