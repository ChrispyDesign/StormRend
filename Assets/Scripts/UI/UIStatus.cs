/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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
        //Rename to status effect icon controller
        //This should do only one thing not handle both UI status effect icons AND unit status effect icons

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
        [SerializeField] UnitType unitType = UnitType.Off;     //Unit type is required for UI
        [SerializeField] StatusType statusType = StatusType.Off;

        //Members
        Image icon = null;
        AnimateUnit unit = null;

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
                    break;
                case UnitType.FrostTroll:
                    typeToFind = typeof(FrostTrollTag);
                    break;
            }

            if (TrySetUnit(typeToFind))
            {
                RegisterEvents();
                CheckStatus();
            }
        }

        bool TrySetUnit(Type typeToFind)
        {
            //First try getting unit from up the hierarchy
            var tag = GetComponentInParent(typeToFind) as Tag;
            unit = tag?.GetComponent<AnimateUnit>();
            if (unit) return true;

            //Second, this is probably a UI element. Just find
            tag = (FindObjectOfType(typeToFind) as Tag);
            unit = tag?.GetComponent<AnimateUnit>();
            if (unit) return true;

            return false;
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