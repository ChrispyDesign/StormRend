using StormRend.Abilities;
using StormRend.Tags;
using StormRend.Units;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
    public class UIStatus : MonoBehaviour
    {
        //Enums
        public enum UnitType
        {
            Off,
            Berserker,
            Valkyrie,
            Sage,
            FrostHound,
            FrostTroll
        }
        public enum StatusType
        {
            Off,
            Protection,
            Immobilised,
            Blinded
        }

        //Inspector
        [SerializeField] UnitType unitType;
        [SerializeField] StatusType statusType;

        //Members
        Image icon = null;
        AnimateUnit unit;

        void Awake()
        {
			//Get Icon
            icon = GetComponentInChildren<Image>();

			//Determine type to find
            Type typeToFind = null;
            switch (unitType)
            {
                case UnitType.Berserker:
                    typeToFind = typeof(BerserkerTag);
                    break;
                case UnitType.Valkyrie:
                    typeToFind = typeof(ValkyrieTag);
                    break;
                case UnitType.Sage:
                    typeToFind = typeof(SageTag);
                    break;
                case UnitType.FrostHound:
                    typeToFind = typeof(FrostHoundTag);
                    // isEnemy = true;
                    break;
                case UnitType.FrostTroll:
                    typeToFind = typeof(FrostTrollTag);
                    // isEnemy = true;
                    break;
            }

			//First try getting unit from up the hierarchy
			var tag = GetComponentInParent(typeToFind) as Tag;
			unit = tag?.GetComponent<AnimateUnit>();
			
			//Second, this is probably a UI element. Just find
			if (!unit) unit = (FindObjectOfType(typeToFind) as Tag).GetComponent<AnimateUnit>();

			Debug.Assert(unit, "Unit could not be found for UI status");

            // var tag = FindObjectOfType(typeToFind) as Tag;
            // if (!isEnemy)
            //     unit = tag?.GetComponent<AnimateUnit>();
            // // else
            //     // CheckStatus();

            RegisterEvents();

			CheckStatus();
        }

        void RegisterEvents()
        {
            if (!unit) return;

            unit.onAddStatusEffect.AddListener(CheckStatus);
            unit.onBeginTurn.AddListener(CheckStatus);
        }
        void OnDestroy()
        {
            unit?.onAddStatusEffect.RemoveListener(CheckStatus);
            unit?.onBeginTurn.RemoveListener(CheckStatus);
        }

        void CheckStatus() => CheckStatus(null);    //Relay
        void CheckStatus(Effect effect = null)
        {
            switch (statusType)
            {
                case StatusType.Protection:
                    TurnIconOnAndOff(unit.isProtected);
                    break;
                case StatusType.Immobilised:
                    TurnIconOnAndOff(unit.isImmobilised);
                    break;
                case StatusType.Blinded:
                    TurnIconOnAndOff(unit.isBlind);
                    break;
            }
        }

        void TurnIconOnAndOff(bool _isOn) => icon.gameObject.SetActive(_isOn);
    }
}