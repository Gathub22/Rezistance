using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonActions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textComponent;
    [SerializeField] private RawImage iconImage;
    [SerializeField] private Image image;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    [SerializeField] private Sprite normalButton;
    [SerializeField] private Sprite highlightedButton;

    void Start()
    {
        image = GetComponent<Image>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        iconImage = GetComponentInChildren<RawImage>();

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
            image.sprite = normalButton;
            if(iconImage != null) iconImage.texture = normalSprite.texture;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (textComponent != null)
        {
            textComponent.color = Color.black;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (iconImage != null && highlightedSprite != null)
        {
            image.sprite = highlightedButton;
            iconImage.texture = highlightedSprite.texture;
        }

        if (textComponent != null)
        {
            textComponent.color = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (iconImage != null && normalSprite != null)
        {
            image.sprite = normalButton;
            iconImage.texture = normalSprite.texture;
        }

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
