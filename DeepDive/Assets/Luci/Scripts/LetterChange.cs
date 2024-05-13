using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class LetterChange : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshes; 
    private int currentTextIndex = 0; 
    private char[] currentLetters = new char[3]; 
    private float scrollSpeed = 0.5f; 
    private bool scrolling = false; 
    public bool nameConfirmed = false;
    BlinkingLetter blscript;

    void Start()
    {
        
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
                    blscript.currentText.enabled = true;
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
            blscript.currentText.enabled = true;
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
        nameConfirmed = true;
        Debug.Log("Name: " + GetCurrentName() + ". Press Enter again to confirm.");    
    }

    string GetCurrentName()
    {
        return string.Concat(currentLetters);
    }
}
