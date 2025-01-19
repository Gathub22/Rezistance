using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Square[][] Map;
	public int Points;
	public int Round;
	public bool IsPlayerTurn = false;
	public GameObject SelectedSoldier;

	void Start()
	{
		GameStats gs = GameObject.Find("GameStats").GetComponent<GameStats>();
		gs.roundText.text = Round.ToString();
		gs.pointsText.text = 0.ToString();

	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(r, out hit)) {
				Square s = hit.collider.GetComponent<Square>();
				if (s != null) {
					if (s.IsUsable){

						// If it wants to select a unit
						if (s.Child != null && SelectedSoldier == null) {
							Soldier sol;
							if ((sol = s.Child.GetComponent<Soldier>()) != null) {
								SelectedSoldier = sol.gameObject;
								s.DetectNeighbourSquares();
							}
						} else {

							// Spawning a unit
							Inventory inv = GameObject.Find("TroopsInventory").GetComponent<Inventory>();
							if (s.Child == null && s.IsEnabled && inv.SelectedSoldier != null) {
								s.Child = Instantiate(inv.SelectedSoldier);
								s.IsUsable = false;
								inv.SelectedSoldier = null;
								RestartPlayerMouseData();
								return;
							}

							// Moving a unit
							if (s.Child == null && s.IsEnabled) {
								GetSquareFromUnit(SelectedSoldier).Child = null;
								s.Child = SelectedSoldier;
								s.IsUsable = false;
								RestartPlayerMouseData();
								return;
							}

							Zombie z;
							if ((z = s.Child.GetComponent<Zombie>()) != null && s.IsEnabled) {
								GetSquareFromUnit(SelectedSoldier).IsUsable = false;
								z.Health -= SelectedSoldier.GetComponent<Soldier>().damage;
								s.HealthText.text = z.Health.ToString();
								RestartPlayerMouseData();
								return;
							}

							RestartPlayerMouseData();
						}
					}
				} else {
					RestartPlayerMouseData();
				}
			} else {
				RestartPlayerMouseData();
			}
		}
	}

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

	public Square GetSquareFromUnit(GameObject unit)
	{
		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				if (Map[i][j].Child == unit) {
					return Map[i][j];
				}
			}
		}
		return null;
	}

	public void RestartPlayerMouseData()
	{
		SelectedSoldier = null;
		GameObject.Find("TroopsInventory").GetComponent<Inventory>().SelectedSoldier = null;
		ClearEnabledSquares();
	}

	public void ClearEnabledSquares()
	{
		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				Map[i][j].DisableOverlay();
			}
		}
	}

	public void ClearUsedSquares()
	{
		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				Map[i][j].IsUsable = true;
			}
		}
	}

	public void LoseRound()
	{
		print("Lost round");
		PlayerPrefs.DeleteKey("points");
		PlayerPrefs.DeleteKey("round");
		PlayerPrefs.DeleteKey("rifles");
		PlayerPrefs.DeleteKey("shotgun");
		PlayerPrefs.DeleteKey("sniper");
		SceneManager.LoadScene("Menu");
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
		PlayerPrefs.SetInt("rounds", ++Round);

	}
}
