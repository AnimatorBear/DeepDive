using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveTime : MonoBehaviour
{
    public static SaveTime instance;
    public TMP_Text bestText;
    public TMP_Text nameText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if(bestText != null)
        {
            string text = GetTime();
            print(text);
            if(text == null || text == "")
            {
                bestText.text = "Unknown";
            }
            else
            {
                print(float.Parse(text.Split(':')[2]).ToString("###,#"));
                print(text.Split(":")[2]);
                string fullText = "";
                if (float.Parse(text.Split(':')[0]) < 9)
                {
                    fullText += "0" + text.Split(':')[0] + ":";
                }
                else
                {
                    fullText += text.Split(':')[0] + ":";
                }
                if (float.Parse(text.Split(':')[1]) < 9)
                {
                    fullText += "0" + text.Split(':')[1] + ":";
                }
                else
                {
                    fullText += text.Split(':')[1] + ":";
                }
                fullText += float.Parse(text.Split(':')[2]).ToString("###,#");
                bestText.text = fullText;
            }
            string name = GetName();
            if(name != null && name != "")
            {
                nameText.text = name + ":";
            }
            else
            {
                nameText.text = "Unknown:";
            }
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
