using UnityEngine;
using System;
using System.Collections;

public class Building : MonoBehaviour 
{
	// Map Data Saver
	MapDataSaver tileSave;
	// Building Data Saver
	BuildingDataSaver buildingSave;

	// Building Timer 
	BuildingTimer timerScript;

	// Options GUI Window
	BuildingOptionsGUIWindow optionsWindow;

	// Building Sprites
	public Sprite[] aSpriteTextures;

	// Tile Array Index Position
	int iTileX = 0;
	int iTileY = 0;

	// Building Size
	int iWidth;
	int iHeight;

	// World Position
	Vector3 v3WorldPosition;

	// Array of Positions of  Tiles Covered by the Building
	Vector2[] av2CoveredTiles;
	// Array of Covered Tile Scripts
	Tile[] aTileScripts;

	// Building Highlight flag
	bool bBuildingHighlight;

	// Object ID
	int iObjectID;

	// Initial Start Time
	DateTime initialStartTime;

	// Mouse Hovered Over flag
	bool bMousedHoveredOver = false;
	// Moving Building flag
	public static bool bMovingBuilding = false;

	// Dragging Building flag
	public static bool bDraggingBuilding = false;
	// Drag Tile Position
	public static Vector2 v2DragTilePosition = Vector2.zero;
	// Drag Tiles Filled flag
	public static bool bDragTilesFilled = false;

	// Tile Start Point Position
	Transform tTileStartPoint;

	// Default Sprite Material
	public Material regMaterial;
	// Highlight Material
	public Material selectMaterial;
	// Inactive Material
	public Material inactiveMaterial;

	// Use this for initialization
	void Awake () 
	{
		tileSave = GameObject.FindWithTag("World").GetComponent<MapDataSaver>();
		buildingSave = GameObject.FindWithTag("World").GetComponent<BuildingDataSaver>();

		optionsWindow = GameObject.Find ("BuildingOptionsGUIWindow").GetComponent<BuildingOptionsGUIWindow>();

		tTileStartPoint = GameObject.Find("TileStartPoint").transform;

		bBuildingHighlight = false;
	}

	// Initialize Values 
	public void Init(int tileX, int tileY, int width, int height,
	                 int id, Vector3 worldPosition, DateTime startTime, bool ready, bool inactive)
	{
		iTileX = tileX;
		iTileY = tileY;
		iWidth = width;
		iHeight = height;
		iObjectID = id;

		v3WorldPosition = worldPosition;

		gameObject.GetComponent<SpriteRenderer>().sprite = aSpriteTextures[iObjectID];

		timerScript = gameObject.GetComponent<BuildingTimer>();
		timerScript.SetResourceID(iObjectID);
		timerScript.SetStartTime (startTime);
		timerScript.bInActive = inactive;

		av2CoveredTiles = new Vector2[width * height];
		aTileScripts = new Tile[width * height];

		SetCoveredTiles();
	}

	// Get Tile Position
	public Vector2 v2GetTilePosition()
	{
		return new Vector2(iTileX, iTileY);
	}

	// Get World Position
	public Vector3 v3GetWorldPosition()
	{
		return v3WorldPosition;
	}

	// Get Covered Tiles
	public Vector2[] av2GetCoveredTiles()
	{
		return av2CoveredTiles;
	}

	// Get Object ID
	public int iGetObjectID()
	{
		return iObjectID;
	}

	// Get Building Timer 
	public BuildingTimer GetBuildingTimer()
	{
		return timerScript;
	}

	// Get Building Save Data 
	public string sGetObjectSaveData()
	{
		string dataString = "Building" + ","
			+ iTileX.ToString() + ","
			+ iTileY.ToString() + ","
			+ iWidth.ToString() + ","
			+ iHeight.ToString() + ","
			+ iObjectID.ToString() + ",";

		dataString += timerScript.sGetTimerSaveData();

		return dataString;
	}

	// Update
	void Update () 
	{
		// If the building is able to be moved
		if (bMovingBuilding)
		{
			// If the mouse is hovered over then allow the building to be dragged
			if (bMousedHoveredOver)
			{
				DragBuilding();
			}
		}
		else // Otherwise
		{
			// Make the building visible and set MouseHoveredOver to false
			gameObject.renderer.enabled = true;
			bMousedHoveredOver = false;
		}

		if (timerScript.bInActive)
		{
			renderer.material = inactiveMaterial;
		}
		else
		{
			if (!timerScript.bGetIsResourceReady())
			{
				renderer.material = regMaterial;
			}
		}
	}

	// Set Covered Tiles and Update Tile ID Array
	void SetCoveredTiles()
	{
		print ("setting tiles");

		for (int y = 0; y < iHeight; y++)
		{
			for (int x = 0; x < iWidth; x++)
			{
				av2CoveredTiles[(y * iWidth) + x] = new Vector2(iTileX + x, iTileY + y);
				aTileScripts[(y * iWidth) + x] = WorldManager.aTileArray[iTileY + y, iTileX + x];

				print (WorldManager.aiTileIDArray);

				WorldManager.aiTileIDArray[iTileY + y, iTileX + x] = 1;
			}
		}
	}

	// Mouse Over Object
	void OnMouseOver()
	{
		// Change the render material to the highlight material
		if (timerScript.bGetIsResourceReady())
		{
			renderer.material = selectMaterial;
		}

		// If the Build Window is showing then disable the building highlight
		if (BuildButtonGUI.bButtonPressed)
		{
			foreach (Tile tile in aTileScripts)
			{
				tile.SetHighlight(true, false);
			}

			bBuildingHighlight = true;
		}

		// If the Right Mouse Button is clicked
		if (Input.GetMouseButtonDown(1))
		{
			// If the building is able to be moved then destroy the building
			if (bMovingBuilding)
			{
				DestroyBuilding();
			}
		}

		// Set MouseHoveredOver to true
		bMousedHoveredOver = true;
	}

	// Drags the building to a new tile position
	void DragBuilding()
	{
		// If the Left Mouse Button is held
		if (Input.GetMouseButton(0))
		{
			// If a building is not being dragged
			if (!bDraggingBuilding)
			{
				// Empty all the original tiles the building covered
				foreach (Vector2 tile in av2CoveredTiles)
				{
					WorldManager.aiTileIDArray[(int)tile.y, (int)tile.x] = 0;
				}

				bDraggingBuilding = true;

				// Make the building invisible
				gameObject.renderer.enabled = false;
			}
		}
		else // Otherwise
		{
			// If a building is being dragged
			if (bDraggingBuilding)
			{
				// If the new position has already been filled then refill the original tiles
				if (bDragTilesFilled)
				{
					foreach (Vector2 tile in av2CoveredTiles)
					{
						WorldManager.aiTileIDArray[(int)tile.y, (int)tile.x] = 1;
					}
				}
				else // Otherwise
				{
					// Set the new building tile position
					iTileX = (int)v2DragTilePosition.x;
					iTileY = (int)v2DragTilePosition.y;

					// New Building World Position
					Vector3 buildingPosition;

					// X
					buildingPosition.x = tTileStartPoint.position.x 
						+ (iTileX * (WorldManager.fTileWidth / 2)) 
							- (iTileY * (WorldManager.fTileWidth / 2));

					// Y
					buildingPosition.y = tTileStartPoint.position.y
						- ((iTileX + iTileY) * (WorldManager.fTileHeight / 4))
							+ WorldManager.fBuildingOffset;

					// Z
					buildingPosition.z = tTileStartPoint.position.z - ((iTileX + iWidth - 1) + (iTileY + iHeight - 1)) - 0.01f;

					// Set the gameobject and world position to the new position
					gameObject.transform.position = buildingPosition;
					v3WorldPosition = buildingPosition;

					// Save the new building data
					buildingSave.SaveData();

					// Cover the new tiles
					SetCoveredTiles();
				}
			}

			// Reset DraggingBuilding, MousedHovererOver and Building Visibility
			bDraggingBuilding = false;
			bMousedHoveredOver = false;
			gameObject.renderer.enabled = true;
		}
	}

	// Mouse Button Pressed
	void OnMouseDown()
	{
		// If a building is not being dragged
		if (!bDraggingBuilding)
		{
			// If button press is true then allow click deletion
			if (!timerScript.bGetIsResourceReady() && !bMovingBuilding)
			{
				optionsWindow.ActivateWindow(this);
			}

			bMousedHoveredOver = false;
		}
	}

	// Mouse Off Object
	void OnMouseExit()
	{
		// Change the render material to the highlight material
		renderer.material = regMaterial;

		// If the building highlight is active then set the highlight
		if (bBuildingHighlight)
		{
			foreach (Tile tile in aTileScripts)
			{
				tile.SetHighlight(false);
			}

			bBuildingHighlight = false;
		}
	}

	// Destroys the building and reset the Tile ID for the grid space
	void DestroyBuilding()
	{
		Debug.Log("--- Destroying Building at: (" + iTileX.ToString() + ", " + iTileY.ToString() + ")");

		// Reset the Tile ID
		foreach (Vector2 tilePos in av2CoveredTiles)
		{
			WorldManager.aiTileIDArray[(int)tilePos.y, (int)tilePos.x] = 0;
		}

		// Save the world
		tileSave.SaveData();

		// Remove the building and give the player the value in gold
		WorldManager.lBuildingList.Remove(this);
		InventoryManager.AddGold(ResourceTypeData.aResourceTypes[iObjectID].iValue / 2);

		// Save the new building data
		buildingSave.SaveData();

		Debug.Log ("Building Removed");
		Debug.Log ("Destruction saved");

		// Destroy the game object
		Destroy(this.gameObject);
	}
}
