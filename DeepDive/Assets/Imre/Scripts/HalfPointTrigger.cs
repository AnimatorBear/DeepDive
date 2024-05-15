using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfPointTrigger : MonoBehaviour
{
    public GameObject lapCompleteTrigger;
    public GameObject halfPointTrigger;

    public GameObject miniMap;
    public GameObject HUD;
    public GameObject Finish;

    public void OnTriggerEnter()
    {
        LapComplete.lapsDone++;
        lapCompleteTrigger.SetActive(true);
        halfPointTrigger.SetActive(false);
        
    }

    public void Update()
    {
        if (LapComplete.lapsDone == 6)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            miniMap.SetActive(false);
            HUD.SetActive(false);
            Finish.SetActive(true);
        }
    }
}
