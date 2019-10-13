using System.Collections;
using System.Collections.Generic;
using StormRend;
using StormRend.Defunct;
using UnityEngine;

public class xSpawnManager : MonoBehaviour
{
    public List<SpawnPoints> m_spawns;

    private void Start()
    {

    }

    public void spawnPlayers()
    {
        foreach (SpawnPoints spawnPoint in m_spawns)
        {
            xTile node = xGrid.GetNodeFromCoords(
                        spawnPoint.m_spawnCoords.x,
                        spawnPoint.m_spawnCoords.y);

            Vector3 pos = node.GetNodePosition();

            Transform go = Instantiate(
                spawnPoint.m_prefab,
                pos,
                Quaternion.identity,
                null).transform;
            xUnit unit = go.GetComponent<xUnit>();
            unit.coords = spawnPoint.m_spawnCoords;

            node.SetUnitOnTop(unit);
        }
    }
}



[System.Serializable]
public class SpawnPoints
{
    public Vector2Int m_spawnCoords;
    public GameObject m_prefab;
}
