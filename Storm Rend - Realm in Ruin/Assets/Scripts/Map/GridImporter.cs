using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// helper class that's responsible for importing csv grid data
/// </summary>
public class GridImporter : MonoBehaviour
{
    public string m_path;

    [Header("Legend")]
    [SerializeField] private string m_walkableTiles = null;
    [SerializeField] private string m_emptyTiles = null;
    [SerializeField] private string m_blockedTiles = null;
    [SerializeField] private string m_playerSpawnTiles = null;
    [SerializeField] private string m_enemySpawnTiles = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public NodeType[,] ImportGrid(string path)
    {
        FileStream fileStream = File.OpenRead(path);
        StreamReader streamReader = new StreamReader(fileStream);

        //
        List<string[]> csv = new List<string[]>();
        
        while (!streamReader.EndOfStream)
        {
            string line = streamReader.ReadLine();
            string[] characters = line.Split(',');

            csv.Add(characters);
        }

        // initialize grid
        NodeType[,] grid = new NodeType[csv.Count, csv[0].Length];
        int x = 0;

        for (int i = grid.GetLength(0) - 1; i >= 0 ; i--)
        {
            int y = 0;

            for (int j = grid.GetLength(1) - 1; j >= 0; j--)
            {
                if (csv[x][y] == m_walkableTiles)
                    grid[i, j] = NodeType.WALKABLE;

                else if (csv[x][y] == m_emptyTiles)
                    grid[i, j] = NodeType.EMPTY;

                else if (csv[x][y] == m_blockedTiles)
                    grid[i, j] = NodeType.BLOCKED;

                else if (csv[x][y] == m_playerSpawnTiles)
                    grid[i, j] = NodeType.PLAYER;

                else if (csv[x][y] == m_enemySpawnTiles)
                    grid[i, j] = NodeType.ENEMY;

                y++;
            }

            x++;
        }

        // output
        return grid;
    }
}
