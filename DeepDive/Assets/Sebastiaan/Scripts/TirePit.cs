using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirePit : MonoBehaviour
{
    public GameObject TireUi;

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
        }
    }

    public void SelectTire(int tireChoice)
    {
        //1 soft, 2 med, 3 hard
        //change tire on car
        Debug.Log(tireChoice);
        TireUi.SetActive(false);
    }
}
