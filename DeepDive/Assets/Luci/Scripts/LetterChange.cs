using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LetterChange : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshes;
    private int currentTextIndex = 0;
    private char[] currentLetters = new char[3];
    private float scrollSpeed = 0.5f;
    public bool scrolling = false;
    public bool nameConfirmed = false;
    private bool alreadyBlinked = false;
    BlinkingLetter blscript;
    public GameObject confirmui;
    public TMP_Text confirmuitext;
    public bool isConfirming = false;
    PlayerInput playerInput;

    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        confirmui.SetActive(false);
        for (int i = 0; i < currentLetters.Length; i++)
        {
            currentLetters[i] = 'A';
            UpdateText(i);
        }
        blscript = GetComponent<BlinkingLetter>();
        blscript.currentText = textMeshes[currentTextIndex];
    }

    void Update()
    {
        if (!alreadyBlinked)
        {
            alreadyBlinked = true;
            blscript.PlayerActive(false);
        }

        if (nameConfirmed)
        {
            StopBlinking();
            PlayerPrefs.SetString("CurrentPlayerName", string.Concat(currentLetters));
            Debug.Log("Player Name: " + PlayerPrefs.GetString("CurrentPlayerName"));
            SceneManager.LoadScene("Merge");
            nameConfirmed = false;
            return;
        }

        float movin = playerInput.actions["GasBrake"].ReadValue<float>();
        if (movin > 0 && !scrolling)
        {
            blscript.PlayerActive(true);
            alreadyBlinked = false;
            ScrollLetters(1);
        }
        else if (movin < 0 && !scrolling)
        {
            blscript.PlayerActive(true);
            alreadyBlinked = false;
            ScrollLetters(-1);
        }
        else if (playerInput.actions["EnterName"].triggered && !nameConfirmed)
        {
            if (isConfirming)
            {
                nameConfirmed = true;
                isConfirming = false;
                return;
            }

            if (!scrolling)
            {
                if (currentTextIndex == textMeshes.Length - 1 && !nameConfirmed)
                {
                    ConfirmName();
                    StopBlinking();
                }
                else
                {
                    Debug.Log("Moved to next");
                    blscript.currentText.enabled = true;
                    LockLetterAndMoveToNext();
                }
            }
        }
        else if (playerInput.actions["BackName"].triggered && !nameConfirmed)
        {
            StopBlinking();

            if (isConfirming)
            {
                Debug.Log("Name canceled!");
                currentTextIndex = 0;
                currentLetters[currentTextIndex] = currentLetters[currentTextIndex];
                blscript.currentText = textMeshes[currentTextIndex];
                nameConfirmed = false;
                isConfirming = false;
                blscript.enabled = true;
                confirmui.SetActive(false);
                return;
            }

            MoveToPreviousLetter();
        }
    }

    void ScrollLetters(int direction)
    {
        char targetLetter = (char)(currentLetters[currentTextIndex] + direction);
        scrollSpeed = 0.5f;

        if (targetLetter > 'Z')
        {
            targetLetter = 'A';
            scrollSpeed = 0;
        }
        else if (targetLetter < 'A')
        {
            targetLetter = 'Z';
            scrollSpeed = 0;
        }
        StartCoroutine(ScrollToLetter(targetLetter));
    }

    IEnumerator ScrollToLetter(char targetLetter)
    {
        scrolling = true;

        while (currentLetters[currentTextIndex] != targetLetter)
        {
            if (currentLetters[currentTextIndex] < targetLetter)
            {
                currentLetters[currentTextIndex]++;
            }
            else
            {
                currentLetters[currentTextIndex]--;
            }

            UpdateText(currentTextIndex);

            yield return new WaitForSeconds(scrollSpeed);
        }

        scrolling = false;
    }

    void LockLetterAndMoveToNext()
    {
        currentTextIndex++;
        if (currentTextIndex >= textMeshes.Length)
        {
            currentTextIndex = 0;
        }

        blscript.currentText = textMeshes[currentTextIndex];
    }

    void MoveToPreviousLetter()
    {
        currentTextIndex--;
        if (currentTextIndex < 0)
        {
            currentTextIndex = textMeshes.Length - 1;
        }

        blscript.currentText = textMeshes[currentTextIndex];
    }

    void UpdateText(int index)
    {
        textMeshes[index].text = currentLetters[index].ToString();
    }

    void ConfirmName()
    {
        isConfirming = true;
        blscript.StopBlinking();
        confirmuitext.text = "Your name is " + string.Concat(currentLetters) + ". Are you sure you want this as your name?";
        confirmui.SetActive(true);
        Debug.Log("Name: " + GetCurrentName() + ". Press Enter again to confirm.");
        blscript.enabled = false;
        for (int i = 0; i < textMeshes.Length; i++)
        {
            textMeshes[i].enabled = true;
        }
    } 
    string GetCurrentName()
    {
        return string.Concat(currentLetters);
    }

    void StopBlinking()
    {
        if (blscript != null && blscript.currentText != null)
        {
            blscript.currentText.enabled = true;
            blscript.StopBlinking();
        }
    }
}