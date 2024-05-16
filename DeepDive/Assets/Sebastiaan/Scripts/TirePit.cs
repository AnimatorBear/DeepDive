using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TirePit : MonoBehaviour
{
    public GameObject TireUi;
    private CartTest player;
    float normalBrake;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            TireUi.SetActive(true);
            FindAnyObjectByType<EventSystem>().SetSelectedGameObject(TireUi.transform.GetChild(0).gameObject);
            player = other.gameObject.GetComponent<CartTest>();
            player.canMove = false;
            player.forceBrake = true;
            normalBrake = player.brakeTorque;
            player.brakeTorque = 9999999999999999;
        }
    }

    public void SelectTire(int tireChoice)
    {
        //1 soft, 2 med, 3 hard
        //change tire on car
        Debug.Log(tireChoice);
        switch (tireChoice)
        {
            case 1:
                player.SwapWheels(CartTest.wheelTypes.Soft);
                break;
            case 2:
                player.SwapWheels(CartTest.wheelTypes.Medium);
                break;
            case 3:
                player.SwapWheels(CartTest.wheelTypes.Hard);
                break;
        }
        TireUi.SetActive(false);
        player.canMove = true;
        player.forceBrake = false;
        player.brakeTorque = normalBrake;
    }
}
