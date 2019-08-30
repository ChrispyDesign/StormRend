using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States.UI
{
    public abstract class UIState : StackState
    {
        [SerializeField] GameObject[] UI_items;

        //Auto hide UI panel
        internal override void OnCover()
        {
            foreach (var i in UI_items)
                i.SetActive(false);
        }
        internal override void OnUncover()
        {
            foreach (var i in UI_items)
                i.SetActive(true);
        }
    }
}
