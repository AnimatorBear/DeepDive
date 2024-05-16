using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HalfPointTrigger : MonoBehaviour
{
    public GameObject lapCompleteTrigger;
    public GameObject halfPointTrigger;

    public GameObject miniMap;
    public GameObject HUD;
    public GameObject Finish;

    public GameObject Player;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            lapCompleteTrigger.SetActive(true);
            halfPointTrigger.SetActive(false);
        }              
    }

    public void Update()
    {
        if (LapComplete.lapsDone == 3)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            miniMap.SetActive(false);
            HUD.SetActive(false);
            Finish.SetActive(true);
            Player.GetComponent<CartTest>().enabled = false;
        }
    }
    public void Start()
    {
        HUD.SetActive(true);
        Finish.SetActive(false);
    }
}
