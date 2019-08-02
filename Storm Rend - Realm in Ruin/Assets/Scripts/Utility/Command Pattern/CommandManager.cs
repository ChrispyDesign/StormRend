using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public List<ICommand> m_moves;

    private void Start()
    {
        m_moves = new List<ICommand>();
    }
}
