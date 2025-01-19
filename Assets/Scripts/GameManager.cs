using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Square[][] Map;
	public int Round;
	public int Zombies {
		get {
			return _zombies;
		}
		set {
			_zombies = value;
			GameObject.Find("GameStats").GetComponent<GameStats>().zombiesText.text = value.ToString();

			if (value == 0) {
				WinRound();
			}
		}
	}

	public int Points {
		get => _points;
		set {
			_points = value;
			GameObject.Find("GameStats").GetComponent<GameStats>().pointsText.text = value.ToString();
		}
	}


	public bool IsPlayerTurn = false;
	public GameObject SelectedSoldier;

	[SerializeField] private int _zombies;
	[SerializeField] private int _points;
	[SerializeField] private AudioSource soldierStepSound;
	[SerializeField] private Shop shop;
	[SerializeField] private GameObject zombiePanel;

	void Start()
	{
		Round = PlayerPrefs.GetInt("round", 1);
		GameStats gs = GameObject.Find("GameStats").GetComponent<GameStats>();
		gs.roundText.text = Round.ToString();
		gs.pointsText.text = 0.ToString();
		zombiePanel.SetActive(false);
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

								string s_type = inv.SelectedSoldier.GetComponent<Soldier>().soldierType;
								switch(s_type) {
									case "Rifleman":
										inv.RemainingRifleman--;
										break;
									case "Shotgun":
										inv.RemainingShotgun--;
										break;
									case "Sniper":
										inv.RemainingSniper--;
										break;
								}

								s.Child = Instantiate(inv.SelectedSoldier);
								s.IsUsable = false;
								inv.SelectedSoldier = null;
								RestartPlayerMouseData();
								return;
							}

							// Moving a unit
							if (s.Child == null && s.IsEnabled) {
								GetSquareFromUnit(SelectedSoldier).Child = null;
								soldierStepSound.Play();
								s.Child = SelectedSoldier;
								s.IsUsable = false;
								RestartPlayerMouseData();
								return;
							}

							Zombie z;
							if ((z = s.Child.GetComponent<Zombie>()) != null && s.IsEnabled) {
								GetSquareFromUnit(SelectedSoldier).IsUsable = false;
								s.ApplyDamage(SelectedSoldier.GetComponent<Soldier>().damage);
								if(SelectedSoldier.GetComponent<AudioSource>() != null) {
									SelectedSoldier.GetComponent<AudioSource>().Play();
								}
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
		ClearUsedSquares();
		ClearEnabledSquares();

		IsPlayerTurn = !IsPlayerTurn;

		if (!IsPlayerTurn)
			GameObject.Find("Bot").GetComponent<Bot>().Calculate();
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
    zombiePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"Your brains have been eaten; you survived {Round - 1} nights.";
    zombiePanel.SetActive(true);

    StartCoroutine(WaitForDialogueAndLoadScene());
}

private IEnumerator WaitForDialogueAndLoadScene()
{
    yield return new WaitForSeconds(5f);

    PlayerPrefs.DeleteKey("points");
    PlayerPrefs.DeleteKey("round");
    PlayerPrefs.DeleteKey("rifles");
    PlayerPrefs.DeleteKey("shotgun");
    PlayerPrefs.DeleteKey("sniper");

    SceneManager.LoadScene("MainMenu");
}


	public void WinRound()
	{
		print("Round won!");
		shop.openShop();
		PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points", 0) + Points);
		PlayerPrefs.SetInt("round", ++Round);

		int[] remainingSoldiers = {0,0,0}; // rifleman, shotgunner, sniper
		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				GameObject c = Map[i][j].Child;
				try {
					switch (c.GetComponent<Soldier>().soldierType) {
						case "Rifleman":
							remainingSoldiers[0]++;
							break;

						case "Shotgun":
							remainingSoldiers[1]++;
							break;

						case "Sniper":
							remainingSoldiers[2]++;
							break;
					}
				} catch{}
			}
		}

		Inventory inv = GameObject.Find("TroopsInventory").GetComponent<Inventory>();
		PlayerPrefs.SetInt("rifles", remainingSoldiers[0] + inv.RemainingRifleman);
		PlayerPrefs.SetInt("shotgun", remainingSoldiers[1] + inv.RemainingShotgun);
		PlayerPrefs.SetInt("sniper", remainingSoldiers[2] + inv.RemainingSniper);
	}
}
