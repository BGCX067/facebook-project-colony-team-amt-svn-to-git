using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
	// Sprite Renderer Component
	SpriteRenderer renderer;

	// Default Sprite Material
	public Material regMaterial;
	// Highlight Empty Tile Material
	public Material selectEmptyMaterial;
	// Highlight Full Tile Material
	public Material selectFullMaterial;

	// Tile ID
	int iTileID;

	// Array Position
	public int iTileIndexX;
	public int iTileIndexY;
	
	public static int iBuildingID = 1;
	public static int iBuildingWidth = 1;
	public static int iBuildingHeight = 1;

	public static int iBuildingCost = 100;

	public static bool bReadyToBuild = false;

	// AudioClip used when placing buildings
	public AudioClip PlaceBuildingSound;

	// World Manager Component
	WorldManager world;

	InventoryDataSaver inventorySaver;

	// Use this for initialization
	void Awake() 
	{
		// Find the Sprite Renderer Component
		renderer = GetComponent<SpriteRenderer>();

		// Find the World Manager component
		world = GameObject.FindWithTag("World").GetComponent<WorldManager>();

		inventorySaver = GameObject.Find ("InventoryManager").GetComponent<InventoryDataSaver>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	// Mouse Click on Object
	void OnMouseDown()
	{
		// If button press is true then allow click creation
		if (bReadyToBuild)
		{
			Debug.Log (WorldManager.aiTileIDArray [iTileIndexY, iTileIndexX]);

			// If there is not a building on this tile, then create one.
			if (!bCheckHighlightTiles())
			{
				// If the player has enough money then take the money, create the building
				// and save the new money value
				if (InventoryManager.TakeGold(iBuildingCost))
				{
					Debug.Log ("Creating Building at (" + iTileIndexX.ToString ()
								+ ", " + iTileIndexY.ToString () + ")");
				
					world.ClickCreateBuilding (iTileIndexX, iTileIndexY);

					inventorySaver.SaveData();

					// Checks if muted
					if (SoundManager.bMute == false)
					{
						// Plays sound clip
						audio.volume = SoundManager.fVolume;
						audio.clip = PlaceBuildingSound;
						audio.Play();
					}
				}
			}
		}
	}

	// Mouse Over Object
	void OnMouseOver()
	{
		// If the Build Button is pressed, set the highlight to true
		if (bReadyToBuild || Building.bDraggingBuilding)
		{
			// Check the surrounding tile contents
			bool tileFilled = bCheckHighlightTiles();

			// Highlight the tiles from the TileFilled flag
			HighlightTileGroup(tileFilled);

			// Set the new Building DragPosition to this tile
			Building.v2DragTilePosition = new Vector2(iTileIndexX, iTileIndexY);
			// Set Building DragTilesFilled to TileFilled
			Building.bDragTilesFilled = tileFilled;
		}
	}

	// Mouse Off Object
	void OnMouseExit()
	{
		// De-Highlight Tiles 
		DeHighlightTileGroup();
		// Set Building DragTilesFiled to true
		Building.bDragTilesFilled = true;
	}

	// Checks to see if the highlighted tiles are filled or out-of-bounds
	bool bCheckHighlightTiles()
	{
		// Tiles Filled flag
		bool tileFilled = false;

		// Loop through each covered tile and check for building contents or edges of the map
		for (int y = 0; y < iBuildingHeight; y++)
		{
			if (tileFilled)
			{
				break;
			}

			if (y + iTileIndexY >= WorldManager.aiTileIDArray.GetLength(0))
			{
				tileFilled = true;
				break;
			}

			for (int x = 0; x < iBuildingWidth; x++)
			{
				if (x + iTileIndexX >= WorldManager.aiTileIDArray.GetLength(1))
				{
					tileFilled = true;
					break;
				}

				if (WorldManager.aiTileIDArray[iTileIndexY + y, iTileIndexX + x] == 1)
				{
					tileFilled = true;
					break;
				}
			}
		}

		return tileFilled;
	}

	// Sets the highlight material
	public void SetHighlight(bool highlight, bool validTile = true)
	{
		if (renderer == null)
		{
			// Find the Sprite Renderer Component
			renderer = GetComponent<SpriteRenderer>();
		}

		// If highlighted
		if (highlight)
		{
			if (validTile)
			{
				// Change the render material to the highlight empty material
				renderer.material = selectEmptyMaterial;
			}
			else
			{
				// Change the render material to the highlight full material
				renderer.material = selectFullMaterial;
			}
		}
		else // Otherwise
		{
			// Change the render material to the regular material
			renderer.material = regMaterial;
		}
	}

	// Highlights a group of tiles within the range of the map
	void HighlightTileGroup(bool tileFilled)
	{
		for (int y = 0; y < iBuildingHeight; y++)
		{
			for (int x = 0; x < iBuildingWidth; x++)
			{
				// If the highlight the tile based on whether its empty or not
				if ((iTileIndexY + y < WorldManager.aiTileIDArray.GetLength(0)) &&
				    (iTileIndexX + x < WorldManager.aiTileIDArray.GetLength(1)))
				{
					WorldManager.aTileArray[iTileIndexY + y, iTileIndexX + x].SetHighlight(true, !tileFilled);
				}
			}
		}
	}

	// Dehighlights a group of tiles within the range of the map
	void DeHighlightTileGroup()
	{
		for (int y = 0; y < iBuildingHeight; y++)
		{
			for (int x = 0; x < iBuildingWidth; x++)
			{
				// If the highlight the tile based on whether its empty or not
				if ((iTileIndexY + y < WorldManager.aiTileIDArray.GetLength(0)) &&
				    (iTileIndexX + x < WorldManager.aiTileIDArray.GetLength(1)))
				{
					// Remove the highlight
					WorldManager.aTileArray[iTileIndexY + y, iTileIndexX + x].SetHighlight(false);
				}
			}
		}
	}
}
