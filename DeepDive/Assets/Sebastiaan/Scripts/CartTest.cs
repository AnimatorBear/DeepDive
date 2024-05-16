using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CartTest : MonoBehaviour
{
    // Front
    public Transform frontLeftWheelMesh;
    public Transform frontRightWheelMesh;
    // Rear
    public Transform rearLeftWheelMesh;
    public Transform rearRightWheelMesh;
    // Wheel Colliders
    // Front
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    // Rear
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public float maxTorque = 500f;
    public float brakeTorque = 1000f;
    // max wheel turn angle;
    public float maxWheelTurnAngle = 30f; // degrees
    public Vector3 centerOfMass = new Vector3(0f, 0f, 0f);
    public Vector3 eulertest;
    public float torquePower = 0f;
    public float steerAngle = 30f;



    public float maxspeed;
    public int[] shifts = { 0, 7, 13, 18, 24, 30, 44, 60 };
    public int currentShift = 1;


    public Rigidbody rgb;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float wheelHealth = 100f;
    public enum wheelTypes {Soft,Medium,Hard,Broken}
    public wheelTypes currentWheel;
    public float wheelSpeed = 0;
    public bool canMove = true;
    public bool forceBrake = false;

    [SerializeField] private float wheelHealthDivider = 50;

    public float wheelDamageMultiplier = 1;
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        if(playerInput == null)
        {
            playerInput = FindAnyObjectByType<PlayerInput>();
        }
        SwapWheels(wheelTypes.Medium);
    }
    // Visual updates
    void Update()
    {
        if (canMove)
        {
            shifting();
            Vector3 temp = frontLeftWheelMesh.localEulerAngles;
            Vector3 temp1 = frontRightWheelMesh.localEulerAngles;
            temp.y = wheelFL.steerAngle - (frontLeftWheelMesh.localEulerAngles.z);
            frontLeftWheelMesh.localEulerAngles = temp;
            temp1.y = wheelFR.steerAngle - (frontRightWheelMesh.localEulerAngles.z);
            frontRightWheelMesh.localEulerAngles = temp1;
            // Wheel rotation
            frontLeftWheelMesh.Rotate(wheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
            frontRightWheelMesh.Rotate(wheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
            rearLeftWheelMesh.Rotate(wheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
            rearRightWheelMesh.Rotate(wheelRR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        }
    }
    // Physics updates
    void FixedUpdate()
    {
        var temper = rgb.velocity.magnitude;
        wheelHealth -= ((temper*Time.deltaTime) / wheelHealthDivider) * wheelDamageMultiplier;
        //print(wheelHealth);
        if(wheelHealth < 0)
        {
            SwapWheels(wheelTypes.Broken);
        }
        var temperi = rgb.velocity.x;
        // CONTROLS - FORWARD & RearWARD
        float brake = playerInput.actions["Brake"].ReadValue<float>();
        //Makes the player brake 3x as much when being forced to brake (In a pitstop)
        if (forceBrake)
        {
            brake = 3;
        }

        if (brake != 0 && rgb.velocity.x > 1 || forceBrake)
        {
            // BRAKE
            wheelRL.brakeTorque = brakeTorque * brake;
            wheelRR.brakeTorque = brakeTorque * brake;
        }
        else
        {
            // SPEED
            torquePower = maxTorque * Mathf.Clamp(playerInput.actions["GasBrake"].ReadValue<float>(), -1, 1);
            wheelRL.brakeTorque = 0f;
            wheelRR.brakeTorque = 0f;
        }
        if (canMove)
        {
            // Apply torque
            wheelRR.motorTorque = torquePower;
            wheelRL.motorTorque = torquePower;
            // CONTROLS - LEFT & RIGHT
            // apply steering to front wheels
            steerAngle = maxWheelTurnAngle * playerInput.actions["SteeringWheel"].ReadValue<float>();
            wheelFL.steerAngle = steerAngle;
            wheelFR.steerAngle = steerAngle;


            float currentSpeed = rgb.velocity.magnitude;

            // Check if the current speed exceeds the max speed
            if (currentSpeed > (maxspeed + wheelSpeed))
            {
                // Calculate the velocity direction
                Vector3 velocityDirection = rgb.velocity.normalized;

                // Set the velocity to the max speed in the same direction
                rgb.velocity = velocityDirection * (maxspeed + wheelSpeed);
            }




            print(maxspeed + wheelSpeed);
            //Debug.LogWarning("x"+rgb.velocity.magnitude);

        }
    }

    public void shifting()
    {

        if (playerInput.actions["Manual"].triggered&& rgb.velocity.magnitude >= (maxspeed + wheelSpeed) - 5)
        {
            currentShift++;
            if (currentShift >= shifts.Length)
            {
                currentShift = shifts.Length - 1;
            }
            maxspeed = shifts[currentShift];
        }
        if (playerInput.actions["ReverseManual"].triggered)
        {
            currentShift--;
            if (currentShift < 0)
            {
                currentShift = 0;
            }
            maxspeed = shifts[currentShift];
        }

    }

    public void SwapWheels(wheelTypes wheel)
    {
        currentWheel = wheel;
        maxTorque = 500;
        maxWheelTurnAngle = 40;
        switch (currentWheel)
        {
            case wheelTypes.Soft:
                wheelHealth = 80;
                wheelSpeed = 5;
                break;
            case wheelTypes.Medium:
                wheelHealth = 100;
                wheelSpeed = 3;
                break;
            case wheelTypes.Hard:
                wheelHealth = 120;
                wheelSpeed = 1;
                break;
            case wheelTypes.Broken:
                wheelSpeed = -10;
                maxTorque = 100;
                maxWheelTurnAngle = 20;
                break;
        }
        if(maxspeed < -wheelSpeed) { maxspeed = -wheelSpeed; }
    }

}
