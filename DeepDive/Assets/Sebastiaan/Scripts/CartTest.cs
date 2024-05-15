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

    public Rigidbody rgb;

    [SerializeField] private PlayerInput playerInput;
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        if(playerInput == null)
        {
            playerInput = FindAnyObjectByType<PlayerInput>();
        }
    }
    // Visual updates
    void Update()
    {
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
    // Physics updates
    void FixedUpdate()
    {
        // CONTROLS - FORWARD & RearWARD
        float brake = playerInput.actions["Brake"].ReadValue<float>();
        if (brake != 0 && rgb.velocity.x > 1)
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
        // Apply torque
        wheelRR.motorTorque = torquePower;
        wheelRL.motorTorque = torquePower;
        // CONTROLS - LEFT & RIGHT
        // apply steering to front wheels
        steerAngle = maxWheelTurnAngle * playerInput.actions["SteeringWheel"].ReadValue<float>();
        wheelFL.steerAngle = steerAngle;
        wheelFR.steerAngle = steerAngle;
    }

}
