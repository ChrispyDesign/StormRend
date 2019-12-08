/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System;
using pokoro.BhaVE.Core.Variables;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Variables
{
    [Serializable, CreateAssetMenu(menuName = "StormRend/Variables/Unit", fileName = "UnitVar")]
    public sealed class UnitVar : BhaveVar<Unit>
    {
        public override event Action onChanged;
        public override Unit value
        {
            get => _value;
            set
            {
                //Doesn't matter... just invoke OnChange every time
                _value = value;
                OnChanged?.Raise();
                onChanged?.Invoke();

                // //NOTE: this apparently prevents null ref exception on startup. Not 100% trustworthy though
                // // if (value && _value != value)
                // if (value && _value != value)
                // {
                //     _value = value;
                //     OnChanged?.Raise();
                //     onChanged?.Invoke();
                // }
                // //If the value is null
                // else
                // {
                //     //Check if the current value is not null
                //     if (_value != null)
                //     {
                //         //If _value is already null and 
                //         _value = value;
                //         OnChanged?.Raise();
                //         onChanged?.Invoke();
                //     }
                //     //Current value is already null, do nothing
                // }
            }
        }
    }
}