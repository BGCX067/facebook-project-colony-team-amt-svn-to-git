using UnityEngine;
using System.Collections;
using System.IO;

public class DataReader : MonoBehaviour 
{

// Offline load and save variables (Development Testing Only)
#region Offline
	// File
	protected FileInfo file;
	// Stream reader
	protected StreamReader reader;

	// File path
	protected string sFilePath;
	// File name + extension
	public string sFileName;
#endregion

// Online load and save variables 
#region Online
	// Path to root folder on server
	protected string sDataLinkPath = "http://studentnet.cst.beds.ac.uk/~1201561/Project%20Colony/";
	// PHP File Name
	protected string sPHPFileName = "GetGameData.php";
#endregion

	// Online Load flag (Development Testing Only)
	public static bool bOnlineLoad = false;
	// Online Save flag (Development Testing Only)
	public static bool bOnlineSave = false;

	string dataTxt;
	
	void Start()
	{

	}

	protected string sLoadData(string columnName, string filename)
	{
		if (bOnlineLoad)
		{
			string linkPath = sDataLinkPath + sPHPFileName + "?UserID=" + FBManager.iFacebookID.ToString() + "&Column=" + columnName;
			print (linkPath);

			WWW webRequest = new WWW(linkPath);

			CheckDownloadProgress(webRequest);

			print (webRequest.text);
			dataTxt = webRequest.text.Replace("\n", "").Replace(" ", "");
		}
		else
		{
			sFilePath = Application.dataPath + "/Data/" + filename;

			StreamReader reader = new StreamReader(sFilePath);

			dataTxt = reader.ReadLine();

			reader.Close();
		}


		print (dataTxt);
		return dataTxt;
	}

	void CheckDownloadProgress(WWW request)
	{
		if (request.bytesDownloaded != request.bytes.Length)
		{
			CheckDownloadProgress(request);
		}
	}

}
