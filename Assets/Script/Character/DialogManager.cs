using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public float typingSpeed = 0.03f;

    public AudioClip typingSound;
    private AudioSource audioSource;

    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;
    private bool inputEnabled = true;

    private string currentSentence = "";
    private Action onDialogComplete;

    void Start()
    {
        dialogPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialog(string[] lines, Action onComplete = null)
    {
        dialogPanel.SetActive(true);
        Time.timeScale = 0f;
        GameManager.isDialogActive = true;

        onDialogComplete = onComplete;
        sentences.Clear();

        foreach (string line in lines)
            sentences.Enqueue(line);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogText.text = currentSentence; // tampilkan langsung
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogText.text = "";

        int counter = 0;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;

            if (typingSound != null && audioSource != null && counter % 2 == 0)
                audioSource.PlayOneShot(typingSound);

            counter++;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        StartCoroutine(ResumeGameAfterDelay());
        onDialogComplete?.Invoke();
    }

    IEnumerator ResumeGameAfterDelay()
    {
        inputEnabled = false;
        yield return new WaitForSecondsRealtime(0.2f); // buffer agar input spasi tidak diteruskan
        Time.timeScale = 1f;
        inputEnabled = true;
        GameManager.isDialogActive = false;
    }

    void Update()
    {
        if (dialogPanel.activeSelf && inputEnabled && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }
}
