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

	public void EndTurn()
	{
		IsPlayerTurn = !IsPlayerTurn;

		// TODO: Complete
	}

	public GameObject GetBase()
	{
		for (int i = 0; i < Map.Length; i++)
		{
			for (int j = 0; j < Map[i].Length; j++)
			{
				Square square = Map[i][j];
				if (square.Child != null)
				{
					if (square.Child.GetComponent<Base>() != null)
					{
						return square.Child;
					}
				}
			}
		}
		return null;
	}

	public Square GetBaseSquare()
	{
		for (int i = 0; i < Map.Length; i++)
		{
			for (int j = 0; j < Map[i].Length; j++)
			{
				Square square = Map[i][j];
				if (square.Child != null)
				{
					if (square.Child.GetComponent<Base>() != null)
					{
						return square;
					}
				}
			}
		}
		return null;
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

		int[] remainingSoldiers = {0,0,0}; // rifleman, shotgunner, sniper
		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				switch (Map[i][j].name) {
					case "RifleSoldier":
						remainingSoldiers[0]++;
						break;

					case "ShotgunSoldier":
						remainingSoldiers[1]++;
						break;

					case "SniperSoldier":
						remainingSoldiers[2]++;
						break;
				}
			}
		}

		// TODO: Save inventory

		PlayerPrefs.SetInt("rifles", remainingSoldiers[0]);
		PlayerPrefs.SetInt("shotgun", remainingSoldiers[1]);
		PlayerPrefs.SetInt("sniper", remainingSoldiers[2]);

	}
}
