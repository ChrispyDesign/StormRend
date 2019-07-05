using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static List<ICommand> m_moves;

    private void Start()
    {
        m_moves = new List<ICommand>();
    }
}
