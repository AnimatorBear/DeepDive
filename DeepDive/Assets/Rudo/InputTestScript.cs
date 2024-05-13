using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTestScript : MonoBehaviour
{
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions["TestAction"].triggered)
        {
            print("Hi");
        }
        if (playerInput.actions["TestPosNeg"].ReadValue<float>() != 0)
        {
            print(playerInput.actions["TestPosNeg"].ReadValue<float>());
        }
    }
}
