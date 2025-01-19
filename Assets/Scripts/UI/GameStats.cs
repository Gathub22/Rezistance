using UnityEngine;
using TMPro;

public class GameStats : MonoBehaviour
{
    private GameManager gameManager;

    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI zombiesText;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        pointsText.text = gameManager.Points.ToString();
        roundText.text = gameManager.Round.ToString();
        // zombiesText.text = gameManager.Zombies.ToString();
    }

    public void UpdateStats()
    {
        pointsText.text = gameManager.Points.ToString();
        roundText.text = gameManager.Round.ToString();
        // zombiesText.text = gameManager.Zombies.ToString();
    }

    void Update()
    {

    }
}
