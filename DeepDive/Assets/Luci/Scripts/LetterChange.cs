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
    private bool AlreadyBlinked = false;
    BlinkingLetter blscript;
    public GameObject confirmui;
    public TMP_Text confirmuitext;
    public bool isConfirming = false;
    PlayerInput playerInput;

    void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
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
        if (!AlreadyBlinked)
        {
            AlreadyBlinked = true;
            blscript.PlayerActive(false);
        }
        if (nameConfirmed)
        {
            print("Yo!");
            PlayerPrefs.SetString("CurrentPlayerName", string.Concat(currentLetters));
            print(PlayerPrefs.GetString("CurrentPlayerName"));
            SceneManager.LoadScene("Merge");
            nameConfirmed = false;
            return;
        }
        float movin = playerInput.actions["GasBrake"].ReadValue<float>();
        if (movin > 0 && !scrolling)
        {
            blscript.PlayerActive(true);
            AlreadyBlinked = false;
            ScrollLetters(1);
        }
        else if (movin < 0 && !scrolling)
        {
            blscript.PlayerActive(true);
            AlreadyBlinked = false;
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
            else if (nameConfirmed)
            {
                Debug.Log("Name confirmed: " + GetCurrentName());
            }
        }
        else if (playerInput.actions["BackName"].triggered && !nameConfirmed)
        {
            StopBlinking();
            MoveToPreviousLetter();
            if (isConfirming)
            {
                Debug.Log("Name canceled!");
                nameConfirmed = false;
                isConfirming = false;
                confirmui.SetActive(false);
                currentTextIndex = 0;
                return;
            }
        }
        
    }

    void ScrollLetters(int direction)
    {
        char targetLetter = (char)(currentLetters[currentTextIndex] + direction);

        if (targetLetter > 'Z')
        {
            print("switch to a");
            scrollSpeed = 0;
            targetLetter = 'A';
        }
        else if (targetLetter < 'A')
        {
            print("Switch to z");
            scrollSpeed = 0;
            targetLetter = 'Z';
        }
        else
        {
            scrollSpeed = 0.5f;
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
            print(targetLetter);
            UpdateText(currentTextIndex); 

            yield return new WaitForSeconds(scrollSpeed); 
        }

        scrolling = false;
    }

    void LockLetterAndMoveToNext()
    {
        currentTextIndex++;
        blscript.currentText = textMeshes[currentTextIndex];
        if (currentTextIndex >= textMeshes.Length)
        {
            currentTextIndex = 0; 
        }
    }

    void MoveToPreviousLetter()
    {
        char previousLetter = currentLetters[currentTextIndex];

        if (previousLetter == 'A')
        {
            previousLetter = 'Z';
        }
        else if (previousLetter == 'Z')
        {
            previousLetter = 'A';
        }

        StartCoroutine(ScrollToLetter(previousLetter));
    }

    void UpdateText(int index)
    {
        textMeshes[index].text = currentLetters[index].ToString();
    }

    void ConfirmName()
    {
        isConfirming = true;
        confirmuitext.text = "Your name is " + string.Concat(currentLetters) + " Are you sure you want this as your name?";
        confirmui.SetActive(true);
        Debug.Log("Name: " + GetCurrentName() + ". Press Enter again to confirm.");    
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
