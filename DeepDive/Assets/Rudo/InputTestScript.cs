using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTestScript : MonoBehaviour
{
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        //playerInput = transform.parent.gameObject.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        //float minus = 5;
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + (playerInput.actions["CamUp"].ReadValue<float>() / minus), transform.localEulerAngles.y + (playerInput.actions["CamLeft"].ReadValue<float>() / minus), transform.localEulerAngles.z);
    }
}
