using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Square[][] Map;
	public int Points;
	public int Round;
	public bool IsPlayerTurn = false;

	public void SetMap(Square[][] map)
	{
		Map = map;
	}

	public void LoseRound()
	{
		print("Lost round");
		PlayerPrefs.SetInt("points", 0);
		PlayerPrefs.SetInt("round", 0);
	}

	public void WinRound()
	{
		print("Round won!");
		PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points", 0) + Points);
		PlayerPrefs.SetInt("round", ++Round);

		// TODO: Save inventory

	}
}
