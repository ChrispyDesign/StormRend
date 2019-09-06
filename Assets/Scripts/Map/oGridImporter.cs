using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// helper class that's responsible for importing csv grid data
/// </summary>
namespace StormRend.Defunct
{
	public class oGridImporter : MonoBehaviour
	{
		public string m_path;

		[Header("Legend")]
		[SerializeField] private string m_walkableTiles = null;
		[SerializeField] private string m_emptyTiles = null;
		[SerializeField] private string m_blockedTiles = null;
		[SerializeField] private string m_playerSpawnTiles = null;
		[SerializeField] private string m_enemySpawnTiles = null;

		/// <summary>
		/// use this function to import a grid! Reads a .csv file found in the given path and converts it to a 2D
		/// array of node types
		/// </summary>
		/// <param name="path">the path of the csv</param>
		/// <returns>the array of NodeTypes</returns>
		public NodeType[,] ImportGrid(string path)
		{
			// open file for reading
			FileStream fileStream = File.OpenRead(path);
			StreamReader streamReader = new StreamReader(fileStream);

			// create a list of strings arrays (oh boy) for each element. List = rows, arrays = column
			List<string[]> csv = new List<string[]>();

			// perform reading
			while (!streamReader.EndOfStream)
			{
				// split by line then by comma
				string line = streamReader.ReadLine();
				string[] characters = line.Split(',');

				// add characters to the csv data for later use
				csv.Add(characters);
			}

			// initialize NodeType 2D array
			NodeType[,] grid = new NodeType[csv.Count, csv[0].Length];
			int x = 0;

			// assign node types based off the csv data
			for (int i = grid.GetLength(0) - 1; i >= 0; i--)
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
}