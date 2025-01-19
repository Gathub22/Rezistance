using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    private int lineIndex = 0;
    public float typingTime = 0.05f;
    private bool isTyping = false;

    [Header("Objects to Toggle")]
    [SerializeField] private List<GameObject> objectsToToggle; // Lista de objetos.
    [SerializeField] private List<int> toggleLineIndices;      // Índices del diálogo donde se activan/desactivan los objetos.

    void Start()
    {
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isTyping && dialogueText.text == dialogueLines[lineIndex])
            {
                NextLine();
            }
            else if (isTyping)
            {
                CompleteLine();
            }
        }
    }

    private IEnumerator ShowDialogue()
    {
        isTyping = true;
        dialogueText.text = string.Empty;

        foreach (char c in dialogueLines[lineIndex])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingTime);
        }

        isTyping = false;
    }

    private void NextLine()
    {
        UpdateGameObjectsState();

        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowDialogue());
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void CompleteLine()
    {
        StopAllCoroutines();
        dialogueText.text = dialogueLines[lineIndex];
        isTyping = false;
    }

    private void UpdateGameObjectsState()
    {
        for (int i = 0; i < objectsToToggle.Count; i++)
        {
            GameObject obj = objectsToToggle[i];
            if (obj != null)
            {
                // Activa/desactiva el objeto dependiendo de si el índice actual coincide con su línea asociada.
                obj.SetActive(toggleLineIndices[i] == lineIndex);
            }
        }
    }
}
