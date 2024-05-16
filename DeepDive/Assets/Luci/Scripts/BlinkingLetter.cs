using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingLetter : MonoBehaviour
{
    public TextMeshProUGUI currentText;
    private float blinkspeed = 0.5f;
    LetterChange letterchangescript;
    void Start()
    {
        letterchangescript = GetComponent<LetterChange>();
        StartCoroutine(Blink());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Blink()
    {
        while (!letterchangescript.nameConfirmed)
        {
            currentText.enabled = false;
            yield return new WaitForSeconds(blinkspeed);
            Debug.Log("ayo where the food at");
            currentText.enabled = true;
            yield return new WaitForSeconds(blinkspeed);
            StartCoroutine(Blink());
            yield return null;
        }
    }
}
