using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        vertical = Input.GetAxis("Horizontal");
        horizontal = Input.GetAxis("Vertical");
        
        Vector3 velocity = (transform.forward * vertical) * moveSpeed * Time.fixedDeltaTime;
        float newSpeed = moveSpeed;
        if (newSpeed > 0)
        {
            if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") == 0)
            {
                velocity = (transform.forward * vertical) * (newSpeed * 100) * Time.fixedDeltaTime;
            }
            if (Input.GetAxis("Vertical") != 0 && Input.GetAxis("Horizontal") == 0)
            {
                velocity = (transform.right * horizontal) * (newSpeed * 100) * Time.fixedDeltaTime;
            }
            if (Input.GetAxis("Vertical") != 0 && Input.GetAxis("Horizontal") != 0)
            {
                velocity = (transform.right * horizontal + transform.forward * vertical) * (newSpeed * 100) * Time.fixedDeltaTime;
            }
        }
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
        */

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        Vector3 moveDirection = transform.forward * vertical;
        Vector3 rotationDirection = transform.up * horizontal;

        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);

 
        rb.velocity = moveDirection * moveSpeed;

    }
}
