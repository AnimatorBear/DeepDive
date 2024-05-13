using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class DriveController : MonoBehaviour
{
    float x;
    float y;
    float z;

    Rigidbody rb;
    public GameObject player;

    private float horizontal;
    private float vertical;
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    PlayerInput playerInput;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontal = playerInput.actions["SteeringWheel"].ReadValue<float>();
        vertical = playerInput.actions["GasBrake"].ReadValue<float>();


        Vector3 moveDirection = transform.forward * vertical;
        Vector3 rotationDirection = transform.up * horizontal;

        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);

 
        rb.velocity = moveDirection * moveSpeed;

    }
}
