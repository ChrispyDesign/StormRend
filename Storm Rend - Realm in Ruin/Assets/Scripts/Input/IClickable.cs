using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    void OnHover();
    void OnUnhover();
    void OnSelect();
    void OnDeselect();
}
