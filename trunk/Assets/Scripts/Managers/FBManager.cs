using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Facebook.MiniJSON;

public class FBManager : MonoBehaviour
{
	// Profile Name
	public static string sProfileName;
	// Profile Image
	public static Texture2D t2ProfileImage;
	// List of Friends
	public static List <object> lProfileFriends;

	// Facebook User ID
	public static int iFacebookID;

	// JSON String
	string jsonString;

	// Name Loaded flag
	bool bNameLoaded = false;
	// Friends Loaded flag
	bool bFriendsLoaded = false;
	// Image Loaded flag
	bool bImageLoaded = false;

	// Game Loaded flag
	bool bGameLoaded = false;

	// Game Activated flag
	bool bGameActivated = false;

	// Game Controller object
	GameObject oGameController;

	// Initialisation
	void Start()
	{
		Debug.Log("FB Init");

		// Find the Game Controller object and deactivate it
		oGameController = GameObject.FindWithTag ("GameController");
		//oGameController.SetActive (false);
	
		// Initialise Facebook Connection
		//InitFB();
	}

	// Update
	void Update()
	{
		// If the game is not activated
		if (!bGameActivated)
		{
			// If the Name, Friends and Image are loaded then activate the game
			if (bNameLoaded && bFriendsLoaded && bImageLoaded)
			{
				ActivateGame();
			}
		}
	}
			
	// Initialise Facebook Connection
	void InitFB()
	{
		FB.Init(OnInitComplete, HideUnity);
	}

	// Init Complete Function
	void OnInitComplete()
	{
		Debug.Log("Initiated");

		// Login
		LoginButton();
	}

	// Hide Unity Function
	void HideUnity(bool gameShown)
	{
		// If the game is not shown then stop the timescale
		if (!gameShown) 
		{
			Debug.Log("Hide Unity");
			Time.timeScale = 0;
		} 
		else  // Otherwise, set the timescale to normal
		{
			Debug.Log("Show Unity");
			Time.timeScale = 1;
		}
	}

	// Login to Facebook
	void LoginButton()
	{
		FB.Login("email", CheckLogIn);
	}

	// Check the user is logged into Facebook
	void CheckLogIn(FBResult result)
	{
		// If the user is logged in, retrieve their id and other details
		if (FB.IsLoggedIn)
		{
			Debug.Log(FB.UserId);
			iFacebookID = int.Parse(FB.UserId);
			RetrieveDetails();
		}
		else
		{
			Debug.Log("User couldn't log in");
		}
	}
	
	// Retrieves User Data, Friend Data and Profile Picture
	void RetrieveDetails()
	{
		RetrieveUserData();
		RetrieveUserFriends();
		RetrieveUserPic();
	}

	// Retrieves User Data
	void RetrieveUserData()
	{
		FB.API ("/me", Facebook.HttpMethod.GET, HandleFBDetails);
		Debug.Log ("Me");
	}

	// Retrieves Friend Data
	void RetrieveUserFriends()
	{
		FB.API ("/me/friends/", Facebook.HttpMethod.GET, HandleFBFriends);
	}

	// Retrieves Profile Picture
	void RetrieveUserPic()
	{
		FB.API("/me/picture?type=large", Facebook.HttpMethod.GET, HandleFBPic);
	}

	// Handles the User Data
	void HandleFBDetails(FBResult result)
	{
		// Get the User Data
		var json = JSONNode.Parse (result.Text);

		// Get the Name from the User Data
		sProfileName = json ["name"];
		Debug.Log (sProfileName);

		// Set Name Loaded to true
		bNameLoaded = true;
	}

	// Handles the Friend Data
	void HandleFBFriends(FBResult result)
	{
		// Get the Friend Data
		var json = Json.Deserialize(result.Text) as Dictionary<string, object>;

		// Retrieve Friends from the Friend Data
		var friends = (List<object>)(((Dictionary<string, object>)json)["data"]);

		Debug.Log(friends.Count);

		// Store the friends into a local list
		lProfileFriends = friends;
		Debug.Log (lProfileFriends);

		// Set Friends Loaded to true
		bFriendsLoaded = true;
	}

	// Handles the Profile Picture
	void HandleFBPic(FBResult result)
	{
		// Retrieve the Profile Picture
		t2ProfileImage = result.Texture;
		Debug.Log (t2ProfileImage);

		// Set Image Loaded to true
		bImageLoaded = true;
	}

	// Activates the Game
	void ActivateGame()
	{
		Debug.Log("Activate Game");
		oGameController.SetActive (true);

		// Set Game Activated to true
		bGameActivated = true;
	}
}
