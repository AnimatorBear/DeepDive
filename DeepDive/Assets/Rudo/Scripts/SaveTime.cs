using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveTime : MonoBehaviour
{
    public static SaveTime instance;
    public TMP_Text bestText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if(bestText != null)
        {
            bestText.text = GetTime();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetTime()
    {
        return PlayerPrefs.GetString("BestTime");
    }
    public string GetName()
    {
        return PlayerPrefs.GetString("BestName");
    }

    public void SetTime(string time)
    {
        PlayerPrefs.SetString("BestTime", time);
    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("BestName", name);
    }
}
