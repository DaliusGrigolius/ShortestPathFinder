using GameRunner;

IGame game = new Game();

var path = @"..\..\..\..\Repo\TestData\map2.txt";

try
{
	var result = game.Run(path);
}
catch (Exception ex)
{
	throw new Exception("Unhandled error occured: " + ex.Message);
}

