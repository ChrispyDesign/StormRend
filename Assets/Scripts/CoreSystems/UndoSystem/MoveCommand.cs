using System.Collections;
using System.Collections.Generic;
using StormRend;
using StormRend.Defunct;
using UnityEngine;

public class MoveCommand : ICommand
{
    public xUnit m_unit;
    private Vector2Int m_coords;
    private Vector2Int m_origCoords;

    #region getterAndSetters

    public void SetCoordinates(Vector2Int _coords) { m_coords = _coords; }

    public Vector2Int GetOrigCoordinates() { return m_origCoords; }

    #endregion

    public MoveCommand(xUnit _unit, int _x, int _y)
    {
        m_unit = _unit;
        m_coords.x = _x;
        m_coords.y = _y;
        m_origCoords = m_unit.coords;
    }

    public MoveCommand(xUnit _unit, Vector2Int _coords)
    {
        m_unit = _unit;
        m_coords.x = _coords.x;
        m_coords.y = _coords.y;
        m_origCoords = m_unit.coords;
    }

    public void Execute()
    {
        m_unit.MoveTo(xGrid.CoordToTile(m_coords));
    }

    public void Undo()
    {
        m_unit.MoveTo(xGrid.CoordToTile(m_origCoords));
    }
}
