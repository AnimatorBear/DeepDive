using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingLetter : MonoBehaviour
{
    public TextMeshProUGUI currentText;
    private float blinkspeed = 0.5f;
    private Coroutine blinkingCoroutine;
    private bool playerActive = false;

    void Start()
    {
        StartBlinking();
    }

    void Update()
    {
        if (playerActive)
        {
            StopBlinking();
        }
        else if (blinkingCoroutine == null)
        {
            StartBlinking();
        }
    }

    public void StopBlinking()
    {
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine);
            blinkingCoroutine = null;
            currentText.enabled = true;
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            currentText.enabled = !currentText.enabled;
            yield return new WaitForSeconds(blinkspeed);
        }
    }

    public void PlayerActive(bool isActive)
    {
        playerActive = isActive;
        if (playerActive)
        {
            StopBlinking();
        }
    }

    void StartBlinking()
    {
        if (blinkingCoroutine == null)
        {
            blinkingCoroutine = StartCoroutine(Blink());
        }
    }
}