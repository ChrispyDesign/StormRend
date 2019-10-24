using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	public abstract class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[ReadOnlyField] public Tile currentTile;//{ get; set; }	//The tile this unit is currently/originally on
		[TextArea(0,2)] public string description = "";

		//Inspector
		[Header("Stats")]
		[ReadOnlyField, SerializeField] protected int _hp;        //Current HP
		[SerializeField] protected int _maxHP = 3;

		//Events
		[Header("Events")]
		public UnitEvent OnDeath;

        //Debug
        public KeyCode debugKey = KeyCode.O;
        public UnityEvent DebugEvent;

		//Properties
		public int HP
		{
			get => _hp;
			set => _hp = Mathf.Clamp(value, 0, maxHP);
		}
		public int maxHP => _maxHP;
		public bool isDead => HP <= 0;

	#region Startup
		protected virtual void Start()
		{
			//Reset health
			HP = maxHP;

			//TEMP Scan below
			float scanRange = 0.2f;
			foreach (var t in Map.current.tiles)
			{
				//If a tile is within a certain range then set it as the current tile
				if (Vector3.Distance(t.transform.position, transform.position) < scanRange)
				{
					currentTile = t;
					return;
				}
			}
			Debug.Assert(currentTile, name + "does not have a current tile!");
		}

        void Update()
        {
            if (Input.GetKeyDown(debugKey))
                DebugEvent.Invoke();
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