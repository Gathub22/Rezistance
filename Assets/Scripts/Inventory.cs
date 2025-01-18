using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class Inventory : MonoBehaviour
{

	public int SelectedSoldier = -1;

	public int RemainingRifleman {
		get {
			return rifles;
		}
		set {
			rifles = value;
			RifleText.text = value.ToString();

			if (value == 0) {
				RifleText.transform.parent.GetComponent<Button>().enabled = false;
			}
		}
	}
	public int RemainingShotgun {
		get {
			return shotguns;
		}
		set {
			shotguns = value;
			ShotgunText.text = value.ToString();

			if (value == 0) {
				ShotgunText.transform.parent.GetComponent<Button>().enabled = false;
			}
		}
	}
	public int RemainingSniper {
		get {
			return snipers;
		}
		set {
			snipers = value;
			SniperText.text = value.ToString();

			if (value == 0) {
				SniperText.transform.parent.GetComponent<Button>().enabled = false;
			}
		}
	}

	public TMP_Text RifleText;
	public TMP_Text ShotgunText;
	public TMP_Text SniperText;

	private int rifles;
	private int shotguns;
	private int snipers;

	void Start()
	{
		RemainingRifleman = PlayerPrefs.GetInt("rifles", 2);
		RemainingShotgun = PlayerPrefs.GetInt("shotgun", 2);
		RemainingSniper = PlayerPrefs.GetInt("sniper", 0);

	}

	public void SelectUnit(int type) // 1 = rifle, 2 = shotgun, 3 = sniper
	{
		SelectedSoldier = type;
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		Square s = gm.GetBaseSquare();

		for (float i = s.Position.x-1; i <= s.Position.x+1; i++) {
			for (float j = s.Position.y-1; j <= s.Position.y+1; j++) {
				if (s.Position != new Vector3(i,j,0)) {
					gm.Map[(int) i][(int) j].EnableOverlay();
				}
			}
		}
	}

	public void UnselectUnit(int type) // 1 = rifle, 2 = shotgun, 3 = sniper
	{
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		Square s = gm.GetBaseSquare();

		for (float i = s.Position.x-1; i <= s.Position.x+1; i++) {
			for (float j = s.Position.y-1; j <= s.Position.y+1; j++) {
				gm.Map[(int) i][(int) j].DisableOverlay();
			}
		}
	}
}
