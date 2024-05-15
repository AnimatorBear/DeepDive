using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public int currentCam;
    public GameObject cam1, cam2, cam3;
    // Start is called before the first frame update
    void Start()
    {
        cam2.SetActive(false);
        cam3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currentCam == 3)
            {
                currentCam = 1;
            }
            else
            {
                currentCam++;
            }
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);

            switch (currentCam)
            {
                case 1:
                    {
                        cam1.SetActive(true);
                        break;
                    }
                case 2:
                    {
                        cam2.SetActive(true);
                        break;
                    }
                case 3:
                    {
                        cam3.SetActive(true);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
