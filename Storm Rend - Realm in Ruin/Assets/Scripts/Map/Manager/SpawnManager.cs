using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<SpawnPoints> m_spawns;
    
    private void Start()
    {
        foreach(SpawnPoints spawnPoint in m_spawns)
        {
            Node node = Grid.GetNodeFromCoords(
                        spawnPoint.m_spawnCoords.x, 
                        spawnPoint.m_spawnCoords.y);

            Vector3 pos = node.GetNodePosition();

            Transform go = Instantiate(
                spawnPoint.m_prefab,
                pos,
                Quaternion.identity,
                null).transform;
            Unit unit = go.GetComponent<Unit>();
            unit.m_coordinates = spawnPoint.m_spawnCoords;

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
