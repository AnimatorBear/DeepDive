using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShakeOnTrigger : MonoBehaviour
{
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update() 
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Car")
        {
            RumbleManager.instance.RumblePulse(0f,0.5f, 99999999999999999f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Car")
        {
            RumbleManager.instance.currentDuration = 0f;
        }
    }
}
