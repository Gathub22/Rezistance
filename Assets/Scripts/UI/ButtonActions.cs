using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonActions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent == null)
        {
            Debug.LogError("No se encontró un componente Text como hijo del botón.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (textComponent != null)
        {
            textComponent.color = Color.white;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (textComponent != null)
        {
            textComponent.color = Color.black;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("Camp");
    }
}
