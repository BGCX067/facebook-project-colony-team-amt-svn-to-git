using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour 
{
	// Tile Array
	public static Tile[,] aTileArray;
	// Building Array
	public static List<Building> lBuildingList;

	// Tile ID Array
	public static int[,] aiTileIDArray;

	// Tile Start Point
	public Transform tTileStartPoint;

	public GameObject oBackgroundPrefab;
	// Tile Prefab Object
	public GameObject oTilePrefab;
	// Building Prefab Object
	public GameObject oBuildingPrefab;

	// World Size
	public static int iWorldWidth = 10;
	public static int iWorldHeight = 10;
	
	// Tile Size
	public static float fTileWidth = 1.28f;
	public static float fTileHeight = 1.28f;
	
	// Building Y Offset
	public static float fBuildingOffset = 0.64f;

	// Left Most Tile X in the world
	float leftTileX;
	// Right Most Tile X in the world
	float rightTileX;
	// Top Tile Y of the world
	float topTileY;
	// Bottom Tile Y of the world
	float bottomTileY;
	
	// Use this for initialization
	void Awake () 
	{
		aiTileIDArray = new int[iWorldHeight, iWorldWidth];

		for (int y = 0; y < iWorldHeight; y++)
		{
			for (int x = 0; x < iWorldWidth; x++)
			{
				aiTileIDArray[y, x] = 0;
			}
		}

		lBuildingList = new List<Building>();

		CreateBackground();
		CreateTiles ();
	}

	// Update is called once per frame
	void Update () 
	{
	}

	public void CreateBackground()
	{
		for (int y = 0; y < 34; y++)
		{
			for (int x = 0; x < 34; x++)
			{
				Vector3 position;

				position.x = tTileStartPoint.position.x 
						+ (x * (fTileWidth / 2)) 
						- (y * (fTileWidth / 2));

				position.y = (tTileStartPoint.position.y + 7.66f)
						- ((x + y) * (fTileHeight / 4));

				position.z = tTileStartPoint.position.z - (x + y) + 25;

				Instantiate(oBackgroundPrefab, position, Quaternion.identity);
			}
		}
	}

	// Creates the game world 
	public void CreateTiles()
	{
		// Create the Tile Component Array
		aTileArray = new Tile[iWorldHeight, iWorldWidth];
		
		Debug.Log("--- Creating World");
		
		// Loop through each tile space
		for (int y = 0; y < iWorldHeight; y++) // Rows
		{
			for (int x = 0; x < iWorldWidth; x++) // Columns
			{
				// Create the base tile
				CreateTile (x, y);
			}
		}

		//StartCoroutine(this.gameObject.GetComponent<WorldSaver>().SaveWorldData());
		
		Debug.Log("--- World Creation Complete");
	}

	// Creates a base tile
	void CreateTile(int x, int y)
	{
		// Tile Position
		Vector3 tilePosition;

		tilePosition.x = tTileStartPoint.position.x 
			+ (x * (fTileWidth / 2)) 
				- (y * (fTileWidth / 2));

		tilePosition.y = tTileStartPoint.position.y
			- ((x + y) * (fTileHeight / 4));

		tilePosition.z = tTileStartPoint.position.z - (x + y);
		
		// Create the tile
		GameObject tile = (GameObject)Instantiate (oTilePrefab, tilePosition, Quaternion.identity);;
		// Find the Tile Component on the Tile Object
		Tile tileScript = tile.GetComponent<Tile>();
		
		// Give the Tile Component it's array position
		tileScript.iTileIndexX = x;
		tileScript.iTileIndexY = y;
		
		// Add the tile to the Tile Object array
		aTileArray[y, x] = tileScript;
	}

	public void CreateBuildings(List<BuildingData> buildingDataList)
	{
		foreach (BuildingData building in buildingDataList)
		{
			int x = building.iTileX;
			int y = building.iTileY;
			int width = building.iWidth;
			int height = building.iHeight;
			int id = building.iObjectID;
			DateTime startTime = building.resourceStartTime;
			bool ready = building.bResourceReady;
			bool inactive = building.bInActive;

			print ("CreateBuilding " + startTime.ToString());

			CreateBuilding (x, y, width, height, id, startTime, ready, inactive);
		}
	}

	public void ClickCreateBuilding(int x, int y)
	{
		CreateBuilding (x, y, Tile.iBuildingWidth, Tile.iBuildingHeight, Tile.iBuildingID, DateTime.Now, true, false);

		QuestManager.CheckQuestCondition("Building", Tile.iBuildingID);

		this.gameObject.GetComponent<MapDataSaver>().SaveData();
		this.gameObject.GetComponent<BuildingDataSaver>().SaveData();
	}

	// Creates a building
	void CreateBuilding(int tileX, int tileY, int width, int height,
	                    int id, DateTime startTime, bool ready, bool inactive)
	{
		if (lBuildingList == null)
		{
			lBuildingList = new List<Building>();
		}

		// Tile Position
		Vector3 buildingPosition;

		buildingPosition.x = tTileStartPoint.position.x 
			+ (tileX * (fTileWidth / 2)) 
				- (tileY * (fTileWidth / 2));

		buildingPosition.y = tTileStartPoint.position.y
			- ((tileX + tileY) * (fTileHeight / 4))
				+ fBuildingOffset;

		buildingPosition.z = tTileStartPoint.position.z - ((tileX + width - 1) + (tileY + height - 1)) - 0.01f;

		// Create the tile
		GameObject building = (GameObject)Instantiate(oBuildingPrefab, buildingPosition, Quaternion.identity);

		// Find the Building Component on the Building Object
		Building buildingScript = building.GetComponent<Building>();
		BuildingTimer timerScript = building.GetComponent<BuildingTimer>();

		// Add the tile to the Tile Object array
		lBuildingList.Add(buildingScript);

		buildingScript.Init(tileX, tileY, width, height, id, buildingPosition, startTime, ready, inactive);
		Debug.Log ("Instantiate: " + startTime.ToString());

		timerScript.SetTimeInterval(BuildingTypeData.aBuildingTypes[id].resourceTime);
	}
}