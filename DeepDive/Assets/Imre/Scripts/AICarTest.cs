using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICartTest : MonoBehaviour
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
    // max wheel turn angle
    public float maxWheelTurnAngle = 30f; // degrees
    public Vector3 centerOfMass = new Vector3(0f, -0.9f, 0f); // Lowered center of mass
    public float torquePower = 0f;
    public float steerAngle = 30f;

    public Rigidbody rgb;

    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    public float waypointThreshold = 3.0f; // Distance to waypoint before switching to next

    private bool isInBrakeZone = false;

    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        rgb.centerOfMass = centerOfMass;

        // Adjust suspension settings for stability
        SetupWheelCollider(wheelFL);
        SetupWheelCollider(wheelFR);
        SetupWheelCollider(wheelRL);
        SetupWheelCollider(wheelRR);
    }

    void SetupWheelCollider(WheelCollider col)
    {
        JointSpring suspensionSpring = col.suspensionSpring;
        suspensionSpring.spring = 50000f;
        suspensionSpring.damper = 4500f;
        col.suspensionSpring = suspensionSpring;
        col.suspensionDistance = 0.2f;

        WheelFrictionCurve forwardFriction = col.forwardFriction;
        forwardFriction.stiffness = 1.5f;
        col.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = col.sidewaysFriction;
        sidewaysFriction.stiffness = 1.5f;
        col.sidewaysFriction = sidewaysFriction;
    }

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

    void FixedUpdate()
    {
        if (waypoints.Count == 0)
        {
            return; // No waypoints, do nothing
        }

        // Get the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate the direction to the target waypoint
        Vector3 directionToWaypoint = (targetWaypoint.position - transform.position).normalized;

        // Calculate the angle to the target waypoint
        float angleToWaypoint = Vector3.Angle(transform.forward, directionToWaypoint);
        Vector3 cross = Vector3.Cross(transform.forward, directionToWaypoint);
        if (cross.y < 0) angleToWaypoint = -angleToWaypoint;

        // Adjust steering angle based on speed
        float speed = rgb.velocity.magnitude;
        float speedFactor = Mathf.Clamp01(speed / 50f); // Adjust 50f based on desired speed limit for full steering angle reduction
        float adjustedMaxSteerAngle = Mathf.Lerp(maxWheelTurnAngle, maxWheelTurnAngle * 0.5f, speedFactor); // Reduce to 50% at high speed
        steerAngle = Mathf.Clamp(angleToWaypoint, -adjustedMaxSteerAngle, adjustedMaxSteerAngle);
        wheelFL.steerAngle = steerAngle;
        wheelFR.steerAngle = steerAngle;

        // Check if the cart is close enough to the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }

        // Determine if we need to apply brakes when in a brake zone
        if (isInBrakeZone)
        {
            wheelRL.brakeTorque = brakeTorque;
            wheelRR.brakeTorque = brakeTorque;
            torquePower = 0f; // Optionally, reduce motor torque
        }
        else
        {
            // Apply motor torque
            torquePower = maxTorque;
            wheelRL.brakeTorque = 0f;
            wheelRR.brakeTorque = 0f;
            wheelRR.motorTorque = torquePower;
            wheelRL.motorTorque = torquePower;
        }

        // Anti-roll bars implementation
        ApplyAntiRoll(wheelFL, wheelFR);
        ApplyAntiRoll(wheelRL, wheelRR);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BrakeZone"))
        {
            isInBrakeZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BrakeZone"))
        {
            isInBrakeZone = false;
        }
    }

    void ApplyAntiRoll(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;

        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * 20000.0f;

        if (groundedL)
            rgb.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);
        if (groundedR)
            rgb.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);
    }
}
