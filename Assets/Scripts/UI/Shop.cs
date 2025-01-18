using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    private GameManager gameManager;
    private GameStats gameStats;
    public TextMeshProUGUI zolarsText;
    public TextMeshProUGUI riflesText;
    public TextMeshProUGUI shotgunText;
    public TextMeshProUGUI sniperText;

    public GameObject buyRifleButton;
    public GameObject buyShotgunButton;
    public GameObject buySniperButton;

    public int rifleCost = 10;
    public int shotgunCost = 20;
    public int sniperCost = 30;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameStats = GameObject.Find("GameStats").GetComponent<GameStats>();

        riflesText.text = rifleCost.ToString();
        shotgunText.text = shotgunCost.ToString();
        sniperText.text = sniperCost.ToString();
    }

    public void buySoldier(int soldierType)
    {
        int cost = 0;
        switch (soldierType)
        {
            case 0:
                cost = rifleCost;
                break;
            case 1:
                cost = shotgunCost;
                break;
            case 2:
                cost = sniperCost;
                break;
        }
        if (gameManager.Points >= cost)
        {
            switch (soldierType)
            {
                case 0:
                    PlayerPrefs.SetInt("rifles", PlayerPrefs.GetInt("rifles", 0) + 1);
                    gameManager.Points -= rifleCost;
                    break;
                case 1:
                    PlayerPrefs.SetInt("shotgun", PlayerPrefs.GetInt("shotgun", 0) + 1);
                    gameManager.Points -= shotgunCost;
                    break;
                case 2:
                    PlayerPrefs.SetInt("sniper", PlayerPrefs.GetInt("sniper", 0) + 1);
                    gameManager.Points -= sniperCost;
                    break;
            }
            gameStats.UpdateStats();
        }
        else
        {
            Debug.Log("Not enough points to buy this soldier.");
        }
    }

    public void openShop()
    {
        this.gameObject.SetActive(false);
    }

    public void closeShop()
    {
        this.gameObject.SetActive(false);
    }
}
