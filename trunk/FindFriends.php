

<?php
	//get data for specified UserID
	include 'database.php';

	$UserID= $_GET["UserID"];
	//echo "UserID: ".$UserID."<br>";


	$con=connectToDatabase();
	$query="SELECT * FROM GameData WHERE UserID=".$UserID;
	$result = mysqli_query($con,$query);
	$numrows = mysqli_num_rows($result);

	if ($numrows == 0)
	{
		echo "Player";
	}
	else
	{
		echo "NonPlayer";
	}
  
	closeDatabase($con);

?> 
