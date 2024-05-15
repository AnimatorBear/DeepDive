using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class DriveController : MonoBehaviour
{
    public enum WheelTypes {Soft,Medium,Hard};
    public WheelTypes currentType = WheelTypes.Medium;


    [SerializeField] private Transform[] wheels;
    public WheelCollider[] wheel_col;

    Rigidbody rb;

    private float horizontal;
    private float vertical;

    public float maxspeed;
    public int[] shifts = { 0, 7, 13, 18, 24 };
    public int currentShift = 1;

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

        for (int i = 0; i < wheel_col.Length; i++)
        {
            wheel_col[i].motorTorque = vertical * moveSpeed;
            if (i == 0 || i == 2)
            {
                wheel_col[i].steerAngle = horizontal * rotationSpeed;
            }
            var pos = transform.position;
            var rot = transform.rotation;
            wheel_col[i].GetWorldPose(out pos, out rot);
            wheels[i].position = pos;
            wheels[i].rotation = rot;
        }


        if (playerInput.actions["Manual"].triggered)
        {
            currentShift++;
            if (currentShift >= shifts.Length)
            {
                currentShift = 0;
            }
            maxspeed = shifts[currentShift];
        }
        if (playerInput.actions["ReverseManual"].triggered)
        {
            currentShift--;
            if (currentShift < 0)
            {
                currentShift = shifts.Length - 1;
            }
            maxspeed = shifts[currentShift];
        }
    }
}
