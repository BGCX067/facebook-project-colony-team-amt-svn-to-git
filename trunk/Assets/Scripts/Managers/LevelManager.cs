using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{
	// Level
	static int iLevel;
	// XP
	static int iXP;

	// Level Saver
	static LevelDataSaver levelSave;

	// AudioClip used when leveling up
	public AudioClip levelUpSound;

	// Use this for initialization
	void Start () 
	{
		levelSave = GameObject.Find ("LevelManager").GetComponent<LevelDataSaver>();
	}

	// Initialise Values
	public static void Init(int level, int xp)
	{
		iLevel = level;
		iXP = xp;
	}

	// Get Level
	public static int iGetLevel()
	{
		return iLevel;
	}

	// Get XP
	public static int iGetXP()
	{
		return iXP;
	}

	// Add XP 
	public static void iAddXP(int amount)
	{
		iXP += amount;

		levelSave.SaveData();
	}

	// Update
	void Update()
	{
		// Check XP Levels
		CheckXP();
	}

	// Checks the XP Levels
	void CheckXP()
	{
		// If the level is not the maximum level
		if (iLevel < XPLevelData.iNoOfLevels)
		{
			// If the current P is more than the XP needed for the next level then Level Up
			if (iXP >= XPLevelData.aiXPLevels[iLevel])
			{
				LevelUp();
			}
		}
	}

	// Level Up
	void LevelUp()
	{
		// Advance the level
		iLevel++;
		InventoryManager.AddGold(XPLevelData.aiGoldRewards[iLevel]);
		InventoryManager.AddCredits(XPLevelData.aiCreditRewards[iLevel]);
		LevelUpWindow.bWindowActive = true;
		WindowGUI.DisableControls();
		levelSave.SaveData();

		// Checks if muted
		if (SoundManager.bMute == false)
		{
			// Plays sound clip
			audio.volume = SoundManager.fVolume;
			audio.clip = levelUpSound;
			audio.Play();
		}
	}
}
