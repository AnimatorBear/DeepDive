using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterChange : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshes; 
    private int currentTextIndex = 0; 
    private char[] currentLetters = new char[3]; 
    private const float scrollSpeed = 0.5f; 
    private bool scrolling = false; 
    private bool nameConfirmed = false;

    void Start()
    {
        
        for (int i = 0; i < currentLetters.Length; i++)
        {
            currentLetters[i] = 'A';
            UpdateText(i);
        }
    }

    void Update()
    {
        if (nameConfirmed)
        {
            return; 
        }

        if (Input.GetKeyDown(KeyCode.W) && !scrolling)
        {
            ScrollLetters(1); 
        }
        else if (Input.GetKeyDown(KeyCode.S) && !scrolling)
        {
            ScrollLetters(-1); 
        }
        else if (Input.GetKeyDown(KeyCode.Return) && !nameConfirmed)
        {
            if (!scrolling)
            {
                if (currentTextIndex == textMeshes.Length - 1 && !nameConfirmed)
                {
                    ConfirmName();
                }
                else
                {
                    Debug.Log("Moved to next");
                    LockLetterAndMoveToNext();
                }
            }
            else if (nameConfirmed)
            {
                Debug.Log("Name confirmed: " + GetCurrentName());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) && !nameConfirmed)
        {
            Debug.Log("Moved to previous");
            MoveToPreviousLetter();
        }
        if (Input.GetKeyDown(KeyCode.Return) && nameConfirmed)
        {
            nameConfirmed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) && nameConfirmed)
        {
            Debug.Log("Name canceled!");
            nameConfirmed = false;
            currentTextIndex = 0;
        }
    }

    void ScrollLetters(int direction)
    {
        char targetLetter = (char)(currentLetters[currentTextIndex] + direction);

        if (targetLetter > 'Z')
        {
            targetLetter = 'A';
        }
        else if (targetLetter < 'A')
        {
            targetLetter = 'Z';
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
    }

    void MoveToPreviousLetter()
    {
        char previousLetter = currentLetters[currentTextIndex];

        if (previousLetter == 'A')
        {
            previousLetter = 'Z';
        }
        else
        {
            previousLetter--;
        }

        StartCoroutine(ScrollToLetter(previousLetter));
    }

    void UpdateText(int index)
    {
        textMeshes[index].text = currentLetters[index].ToString(); 
    }

    void ConfirmName()
    {
        nameConfirmed = true;
        Debug.Log("Name: " + GetCurrentName() + ". Press Enter again to confirm.");    
    }

    string GetCurrentName()
    {
        return string.Concat(currentLetters);
    }
}
