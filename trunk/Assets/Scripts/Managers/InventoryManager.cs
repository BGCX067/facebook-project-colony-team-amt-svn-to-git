using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour 
{
	// Gold
	static int iGold;
	// Credits
	static int iCredits;

	// Resource Array
	static int[] iResources;

	static InventoryDataSaver inventorySave;

	// Use this for initialization
	void Start () 
	{
		inventorySave = gameObject.GetComponent<InventoryDataSaver> ();
	}

	// Initialize values
	public static void Init(int gold, int credits, int[] resources)
	{
		iGold = gold;
		iCredits = credits;
		iResources = resources;
	}

	// Get Current Gold
	public static int GetGold()
	{
		return iGold;
	}

	// Get Current Credits
	public static int GetCredits()
	{
		return iCredits;
	}

	// Get Current Resources
	public static int[] GetResources()
	{
		return iResources;
	}

	// Get Current Resource Amount 
	public static int GetResource(int id)
	{
		return iResources[id];
	}

	// Add Gold
	public static void AddGold(int amount)
	{
		iGold += amount;
		inventorySave.SaveData ();
	}

	// Add Credits
	public static void AddCredits(int amount)
	{
		iCredits += amount;
		inventorySave.SaveData ();
	}

	// Add to a Resource 
	public static void AddResource(int id, int amount)
	{
		iResources[id] += amount;
		inventorySave.SaveData ();
	}

	// Subtract Gold
	public static bool TakeGold(int amount)
	{
		if (CheckSubtraction(iGold, amount))
		{
			iGold -= amount;
			inventorySave.SaveData ();
			return true;
		}
		else
		{
			return false;
		}
	}

	// Subtract Credits
	public static bool TakeCredits(int amount)
	{
		if (CheckSubtraction(iCredits, amount))
		{
			iCredits -= amount;
			inventorySave.SaveData ();
			return true;
		}
		else
		{
			return false;
		}
	}

	// Subtract from a Resource
	public static bool TakeResource(int id, int amount)
	{
		if (CheckSubtraction(iResources[id], amount))
		{
			iResources[id] -= amount;
			inventorySave.SaveData ();
			return true;
		}
		else
		{
			return false;
		}
	}

	// Checks to see if a subtraction can be made
	static bool CheckSubtraction(int value, int amount)
	{
		if ((value - amount) < 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
