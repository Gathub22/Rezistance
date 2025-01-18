using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressEffect : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;   
    [SerializeField] private Sprite pressedSprite;  

    [Header("Animation Settings")]
    [SerializeField] private float pressDuration = 0.1f;

    private Image buttonImage;
    private Button button;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();

        if (buttonImage == null)
        {
            Debug.LogError("Este objeto no tiene un componente Image.");
        }

        if (button == null)
        {
            Debug.LogError("Este objeto no tiene un componente Button.");
        }

        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }

        if (button != null)
        {
            button.onClick.AddListener(() => StartCoroutine(PressEffect()));
        }
    }

    private IEnumerator PressEffect()
    {
        // Cambia al sprite presionado
        if (buttonImage != null && pressedSprite != null)
        {
            buttonImage.sprite = pressedSprite;
        }

        // Espera el tiempo configurado
        yield return new WaitForSeconds(pressDuration);

        // Regresa al sprite normal
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }
}
