using UnityEngine;
using System.Collections;

public class MapDataReader : DataReader 
{
	// Use this for initialization
	void Start () 
	{
		DecodeData();
	}
	
	void DecodeData()
	{
		// Tile Array
		int[,] tileIDArray = new int[WorldManager.iWorldHeight, WorldManager.iWorldWidth];
	
		// Read data
		string dataTxt = sLoadData("MapData", sFileName);

		// Check data to see if empty
		if (dataTxt == "" || dataTxt.Contains("NewUser") || dataTxt == null)
		{
			Debug.Log ("No User Map Data");

			// Loop through each tile space and set to empty
			for (int y = 0; y < tileIDArray.GetLength(0); y++)
			{
				for (int x = 0; x < tileIDArray.GetLength(0); x++)
				{
					tileIDArray[y, x] = 0;
				}
			}
		}
		else
		{
			// Split data into rows
			string[] rows = dataTxt.Split('|');
			
			// Current Row counter
			int currentRow = 0;
			
			// Check row data
			if (rows == null)
			{
				Debug.Log ("Could not split map data into rows");
			}
			else
			{
				// Loop through each row
				foreach (string row in rows)
				{
					// Split the row into columns
					string[] columns = row.Split (',');
					
					// Current Column counter
					int currentColumn = 0;
					
					// Check column data
					if (columns == null)
					{
						Debug.Log ("Could not split map data into columns");
					}
					else 
					{
						// Loop through each column
						foreach (string column in columns)
						{
							// Convert the Tile ID string into an int
							int tileID = int.Parse (column);
							
							// Set the Tile ID into the array
							tileIDArray[currentRow, currentColumn] = tileID;

							currentColumn++;
						}
					}

					currentRow++;
				}
			}
		}

		// Send Tile ID Array to World Manager
		WorldManager.aiTileIDArray = tileIDArray;

		Debug.Log ("Map Reading finished");
	}
}
