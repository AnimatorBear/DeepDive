using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LapComplete : MonoBehaviour
{

	public GameObject lapCompleteTrig;
	public GameObject halfLapTrig;

	public GameObject minuteDisplay;
	public GameObject secondDisplay;
	public GameObject milliDisplay;

	public GameObject lapTimeBox;

	public GameObject lapCounter;

	//public GameObject miniMap;
	//public GameObject HUD;
	//public GameObject Finish;

	public static int lapsDone = 1;

	private string bestLap;

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && halfLapTrig.activeSelf == false)
        {
			float bmin = 0;
			float bsec = 0;
			float bmil = 0;

            if (bestLap != null)
			{
				print(bestLap);
                bmin = float.Parse(bestLap.Split(':')[0]);
                bsec = float.Parse(bestLap.Split(':')[1]);
                bmil = float.Parse(bestLap.Split(':')[2]);
            }
			lapsDone++;
			if (bmin <= LapTimeManager.minuteCount)
			{
				if (bsec <= LapTimeManager.secondCount)
				{
					if (bmil <= LapTimeManager.milliCount)
					{
                        if (LapTimeManager.secondCount <= 9)
                        {
                            secondDisplay.GetComponent<Text>().text = "0" + LapTimeManager.secondCount + ".";
                        }
                        else
                        {
                            secondDisplay.GetComponent<Text>().text = "" + LapTimeManager.secondCount + ".";
                        }

                        if (LapTimeManager.minuteCount <= 9)
                        {
                            minuteDisplay.GetComponent<Text>().text = "0" + LapTimeManager.minuteCount + ".";
                        }
                        else
                        {
                            minuteDisplay.GetComponent<Text>().text = "" + LapTimeManager.minuteCount + ".";
                        }

                        milliDisplay.GetComponent<Text>().text = "" + LapTimeManager.milliCount;
						bestLap = LapTimeManager.minuteCount + ":" + LapTimeManager.secondCount + ":" + LapTimeManager.milliCount;
                    }
				}
			}
			string best = SaveTime.instance.GetTime();

            if (best != "")
			{
                float min = float.Parse(best.Split(':')[0]);
                float sec = float.Parse(best.Split(':')[1]);
                float mil = float.Parse(best.Split(':')[2]);
				print(min + ":" + sec + ":" + mil + " !Best");
				print(LapTimeManager.minuteCount + ":" + LapTimeManager.secondCount + ":" + LapTimeManager.milliCount + " !New");
                if (min >= LapTimeManager.minuteCount)
                {
                    if (sec >= LapTimeManager.secondCount)
                    {
                        if (mil >= LapTimeManager.milliCount)
                        {
                            SaveTime.instance.SetTime(LapTimeManager.minuteCount + ":" + LapTimeManager.secondCount + ":" + LapTimeManager.milliCount);
							SaveTime.instance.SetName("Unnamed");
							print("Set new best!");

                        }
						else
						{
                        }
					}
					else
					{
                    }
				}
				else
				{
                }
			}
			else
			{
				//PlayerPrefs.SetString("CurrentTime", LapTimeManager.minuteCount + ":" + LapTimeManager.secondCount + ":" + LapTimeManager.milliCount);
				SaveTime.instance.SetTime(LapTimeManager.minuteCount + ":" + LapTimeManager.secondCount + ":" + LapTimeManager.milliCount);
            }

            LapTimeManager.minuteCount = 0;
			LapTimeManager.secondCount = 0;
			LapTimeManager.milliCount = 0;

			lapCounter.GetComponent<Text>().text = "" + lapsDone;

            halfLapTrig.SetActive(true);
			lapCompleteTrig.SetActive(false);
		}		
	}
}