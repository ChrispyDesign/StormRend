using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridImporter : MonoBehaviour
{
    [SerializeField] private string m_path;

    [Header("Legend")]
    [SerializeField] private string m_walkableTiles;
    [SerializeField] private string m_emptyTiles;
    [SerializeField] private string m_blockedTiles;
    [SerializeField] private string m_playerSpawnTiles;
    [SerializeField] private string m_enemySpawnTiles;

    public NodeType[,] ImportGrid(string path)
    {
        FileStream fileStream = File.OpenRead(path);
        StreamReader streamReader = new StreamReader(fileStream);
        
        //
        string csv = streamReader.ReadToEnd();
        
        //
        string[] lines = csv.Split('\n');
        string[][] characters = new string[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
            characters[i] = lines[i].Split(',');

        // initialize grid
        NodeType[,] grid = new NodeType[characters.Length, characters[0].Length];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (characters[i][j] == m_walkableTiles)
                    grid[i, j] = NodeType.WALKABLE;

                if (characters[i][j] == m_emptyTiles)
                    grid[i, j] = NodeType.EMPTY;

                if (characters[i][j] == m_blockedTiles)
                    grid[i, j] = NodeType.BLOCKED;

                if (characters[i][j] == m_playerSpawnTiles)
                    grid[i, j] = NodeType.PLAYER;

                if (characters[i][j] == m_enemySpawnTiles)
                    grid[i, j] = NodeType.ENEMY;
            }
        }

        // output
        return grid;
    }

    // Start is called before the first frame update
    void Start()
    {
        NodeType[,] grid = ImportGrid(m_path);

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Debug.Log(grid[i, j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
