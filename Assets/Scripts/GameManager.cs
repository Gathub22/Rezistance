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

	public GameObject GetBase()
	{
		for (int i = 0; i < Map.Length; i++)
		{
			for (int j = 0; j < Map[i].Length; j++)
			{
				Square square = Map[i][j];
				if (square.Child != null)
				{
					if (square.Child.getComponent<Base>() != null)
					{
						return square.Child;
					}
				}
			}
		}
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
